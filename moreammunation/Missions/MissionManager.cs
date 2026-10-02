using GTA;
using GTA.Math;
using LemonUI.Menus;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace moreammunation
{
    /// <summary>A job type shown on Agent 16's board.</summary>
    internal sealed class MissionType
    {
        public string Title;
        public string Description;
        public Func<string> Unavailable;   // null/empty = available, otherwise the reason
        public Func<string> Preview;       // extra line: where it will happen
        public Func<string> RewardLabel;
        public Func<Mission> Create;
    }

    internal sealed class MissionManager
    {
        private readonly Settings settings;
        private readonly MissionData data;
        private readonly Random rng = new Random();
        private readonly List<MissionType> types = new List<MissionType>();
        private readonly List<NativeMenu> boards = new List<NativeMenu>();

        private Mission active;
        private int cooldownUntil;

        private int statCompleted, statFailed, statEarned;
        private static readonly string StatsPath = Settings.DataFolder + "\\stats.txt";

        public ArmoryZoneManager Zones { get; set; }

        public MissionManager(Settings settings, MissionData data)
        {
            this.settings = settings;
            this.data = data;
            LoadStats();
            RegisterTypes();
        }

        public bool IsActive { get { return active != null; } }
        public Mission Active { get { return active; } }

        // ------------------------------------------------------------------ job types

        private void RegisterTypes()
        {
            types.Add(new MissionType
            {
                Title = "Destroy the Target",
                Description = "A guarded vehicle is moving hardware for a rival. Blow it up.",
                Unavailable = () => data.ContactLocations.Count == 0 ? "No ContactMissions\\*.xml files found." : null,
                Preview = () =>
                {
                    if (settings.Selection != MissionSelectionMode.Nearest) return "~n~The location is picked when you accept.";
                    HeistLocation loc = PickContactLocation();
                    if (loc == null) return "";
                    return "~n~Nearest: ~y~" + loc.Name + "~w~ (" + Util.Distance(loc.Anchor.DistanceTo2D(Game.Player.Character.Position)) + ")";
                },
                RewardLabel = () =>
                {
                    if (settings.Selection != MissionSelectionMode.Nearest) return "Varies";
                    HeistLocation loc = PickContactLocation();
                    return loc == null ? "" : Util.Money(Scaled(loc.Reward));
                },
                Create = () =>
                {
                    HeistLocation loc = PickContactLocation();
                    return loc == null ? null : new DestroyTargetMission(settings, data, loc);
                }
            });

            types.Add(new MissionType
            {
                Title = "Hijack the Convoy",
                Description = "An escorted weapons truck is driving through the area. Steal it and bring it to the drop-off.",
                Preview = () => "~n~Starts on a road near you.",
                RewardLabel = () => Util.Money(Scaled(ConvoyHijackMission.BaseReward)),
                Create = () => new ConvoyHijackMission(settings, data)
            });

            types.Add(new MissionType
            {
                Title = "Raid a Gun Stash",
                Description = "A gang is sitting on a weapons stash nearby. Clear them out, grab it and bring it to the drop-off.",
                Preview = () => "~n~Starts near you.",
                RewardLabel = () => Util.Money(Scaled(StashRaidMission.BaseReward)),
                Create = () => new StashRaidMission(settings, data)
            });

            types.Add(new MissionType
            {
                Title = "Eliminate the Dealer",
                Description = "A rival arms dealer is meeting his bodyguards nearby. He will run if he spots trouble.",
                Preview = () => "~n~Starts near you.",
                RewardLabel = () => Util.Money(Scaled(AssassinationMission.BaseReward)),
                Create = () => new AssassinationMission(settings, data)
            });

            types.Add(new MissionType
            {
                Title = "Defend the Store",
                Description = "Word is a crew is coming to hit an Ammu-Nation. Hold the line.",
                Unavailable = () => Zones == null || Zones.Zones.Count == 0 ? "No Ammu-Nation + locations are configured." : null,
                Preview = () =>
                {
                    ArmoryZone z = Zones == null ? null : Zones.NearestZone(Game.Player.Character.Position);
                    if (z == null) return "";
                    return "~n~Nearest store: ~y~" + z.BlipName + "~w~ (" + Util.Distance(z.Position.DistanceTo2D(Game.Player.Character.Position)) + ")";
                },
                RewardLabel = () => Util.Money(Scaled(StoreDefenseMission.RewardPerWave * settings.DefenseWaves)),
                Create = () =>
                {
                    ArmoryZone z = Zones == null ? null : Zones.NearestZone(Game.Player.Character.Position);
                    return z == null ? null : new StoreDefenseMission(settings, data, z);
                }
            });
        }

        private HeistLocation PickContactLocation()
        {
            return LocationHelper.Pick(data.ContactLocations, l => l.Anchor, Game.Player.Character.Position,
                                       settings.MinMissionDistance, settings.Selection);
        }

        private int Scaled(int reward)
        {
            return (int)Math.Round(reward * settings.RewardMultiplier);
        }

        // ------------------------------------------------------------------ lifecycle

        public bool Start(Mission mission)
        {
            if (mission == null) return false;
            if (active != null)
            {
                Notify.Agent("~w~Finish the job you're on first.");
                return false;
            }

            Groups.Refresh();

            bool ok;
            try
            {
                ok = mission.Start(Game.Player.Character.Position);
            }
            catch (Exception ex)
            {
                Log.Error("starting " + mission.Title, ex);
                ok = false;
            }

            if (!ok)
            {
                mission.Cleanup(true);
                Notify.Agent("~r~" + (mission.ResultMessage ?? "I couldn't set that one up. Try again from somewhere else."));
                return false;
            }

            active = mission;
            return true;
        }

        public void Update()
        {
            if (active == null) return;
            if (settings.DisableWantedLevel)
            {
                Game.MaxWantedLevel = 0;
                if (Game.Player.WantedLevel > 0) Game.Player.WantedLevel = 0;
            }
            if (Game.Player.IsDead)
            {
                active.Fail("You got wasted.");
            }
            else
            {
                try
                {
                    active.Update();
                }
                catch (Exception ex)
                {
                    Log.Error("updating " + active.Title, ex);
                    active.Fail("Something went wrong with the job (details in log.txt).");
                }
            }

            if (active.Status == MissionStatus.Running) return;

            if (active.Status == MissionStatus.Succeeded)
            {
                int pay = Scaled(active.Reward);
                Game.Player.Money += pay;
                statCompleted++;
                statEarned += pay;
                Notify.Agent("~g~Job done! ~w~Your cut: ~g~" + Util.Money(pay) +
                             (string.IsNullOrEmpty(active.ResultMessage) ? "" : "~n~~w~" + active.ResultMessage), "Important");
            }
            else
            {
                statFailed++;
                Notify.Agent("~r~Job failed. ~w~" + active.ResultMessage, "Important");
            }

            active.Cleanup(false);
            active = null;
            cooldownUntil = Game.GameTime + settings.CooldownSeconds * 1000;
            SaveStats();
        }
        private void RestoreWantedLevel()
        {
            if (settings.DisableWantedLevel) Game.MaxWantedLevel = 5;
        }
        public void Abort(bool silent)
        {
            if (active == null) return;
            active.Cleanup(true);
            active = null;
            if (!silent) Notify.Agent("~w~Alright, job's off. Let me know when you're ready again.");
        }

        private int CooldownSecondsLeft
        {
            get { return Math.Max(0, (cooldownUntil - Game.GameTime + 999) / 1000); }
        }

        // ------------------------------------------------------------------ board

        public NativeMenu CreateBoardMenu(ShopMenu shop)
        {
            NativeMenu board = shop.CreateMenu("AGENT 16 - JOBS", "Special operations.");
            board.Shown += (s, e) => PopulateBoard(board);
            boards.Add(board);
            return board;
        }

        private void PopulateBoard(NativeMenu board)
        {
            board.Clear();

            if (active != null)
            {
                board.Add(new NativeItem("Current: " + active.Title, active.Objective ?? "") { Enabled = false });
                var abort = new NativeItem("Abort Job", "Call it off. Everything the job spawned is removed.");
                abort.Activated += (s, e) =>
                {
                    Abort(false);
                    board.Visible = false;
                };
                board.Add(abort);
            }
            else if (CooldownSecondsLeft > 0)
            {
                board.Add(new NativeItem("~c~Lying low...", "The heat needs to die down first.")
                {
                    Enabled = false,
                    AltTitle = CooldownSecondsLeft + "s"
                });
            }
            else
            {
                foreach (MissionType type in types)
                {
                    string reason = type.Unavailable == null ? null : type.Unavailable();
                    bool available = string.IsNullOrEmpty(reason);

                    string description = type.Description;
                    if (available && type.Preview != null) description += type.Preview();
                    if (!available) description = "~c~" + reason;

                    var item = new NativeItem(type.Title, description)
                    {
                        Enabled = available,
                        AltTitle = available && type.RewardLabel != null ? type.RewardLabel() : ""
                    };

                    MissionType captured = type;
                    item.Activated += (s, e) =>
                    {
                        board.Visible = false;
                        Mission m = captured.Create();
                        if (m == null)
                        {
                            Notify.Agent("~r~Nothing available for that job right now.");
                            return;
                        }
                        if (Start(m))
                            Notify.Agent("~w~" + captured.Title + ". I've marked it on your GPS. Move fast.");
                    };
                    board.Add(item);
                }

                var surprise = new NativeItem("Surprise Me", "A random job from the list above.");
                surprise.Activated += (s, e) =>
                {
                    var available = types.Where(t => t.Unavailable == null || string.IsNullOrEmpty(t.Unavailable())).ToList();
                    if (available.Count == 0) return;
                    MissionType pick = available[rng.Next(available.Count)];
                    board.Visible = false;
                    if (Start(pick.Create()))
                        Notify.Agent("~w~" + pick.Title + ". Check your GPS.");
                };
                board.Add(surprise);
            }

            board.Add(new NativeSeparatorItem("Record"));
            board.Add(new NativeItem("Jobs completed") { Enabled = false, AltTitle = statCompleted.ToString(CultureInfo.InvariantCulture) });
            board.Add(new NativeItem("Jobs failed") { Enabled = false, AltTitle = statFailed.ToString(CultureInfo.InvariantCulture) });
            board.Add(new NativeItem("Total earned") { Enabled = false, AltTitle = Util.Money(statEarned) });
        }

        // ------------------------------------------------------------------ stats

        private void LoadStats()
        {
            try
            {
                if (!File.Exists(StatsPath)) return;
                foreach (string line in File.ReadAllLines(StatsPath))
                {
                    string[] kv = line.Split('=');
                    if (kv.Length != 2) continue;
                    int value;
                    if (!int.TryParse(kv[1].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value)) continue;
                    switch (kv[0].Trim())
                    {
                        case "Completed": statCompleted = value; break;
                        case "Failed": statFailed = value; break;
                        case "Earned": statEarned = value; break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("loading stats", ex);
            }
        }

        private void SaveStats()
        {
            try
            {
                Directory.CreateDirectory(Settings.DataFolder);
                File.WriteAllLines(StatsPath, new[]
                {
                    "Completed=" + statCompleted.ToString(CultureInfo.InvariantCulture),
                    "Failed=" + statFailed.ToString(CultureInfo.InvariantCulture),
                    "Earned=" + statEarned.ToString(CultureInfo.InvariantCulture)
                });
            }
            catch (Exception ex)
            {
                Log.Error("saving stats", ex);
            }
        }
    }
}
