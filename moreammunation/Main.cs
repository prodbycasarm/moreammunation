using GTA;
using iFruitAddon2;
using LemonUI;
using LemonUI.Menus;
using System;
using System.Windows.Forms;

namespace moreammunation
{
    /// <summary>
    /// Entry point. This file only wires things together; the actual logic lives in:
    ///   Shop/      - the Ammu-Nation + menu
    ///   World/     - store locations, trucks and guards
    ///   Missions/  - Agent 16's jobs
    ///   Core/      - settings, spawning, helpers
    /// </summary>
    public class Main : Script
    {
        private readonly Settings settings;
        private readonly ObjectPool pool = new ObjectPool();
        private readonly MissionData data = new MissionData();
        private readonly MissionManager missions;
        private readonly ShopMenu shop;
        private readonly ArmoryZoneManager zones;
        private readonly NativeMenu phoneBoard;
        private readonly CustomiFruit phone;

        private bool initialized;
        private int lastErrorLog;

        public Main()
        {
            settings = moreammunation.Settings.Load();

            try { data.Load(); }
            catch (Exception ex) { Log.Error("loading mission data", ex); }

            missions = new MissionManager(settings, data);
            shop = new ShopMenu(pool, settings, missions);
            zones = new ArmoryZoneManager(settings, missions, data, shop);
            missions.Zones = zones;
            phoneBoard = missions.CreateBoardMenu(shop);

            phone = CreatePhone();

            Tick += OnTick;
            KeyUp += OnKeyUp;
            Aborted += OnAborted;
        }

        private CustomiFruit CreatePhone()
        {
            var p = new CustomiFruit();
            p.SetWallpaper(Wallpaper.Orange8Bit);
            p.SetWallpaper("prop_screen_dctl");
            p.LeftButtonColor = System.Drawing.Color.LimeGreen;
            p.CenterButtonColor = System.Drawing.Color.Orange;
            p.RightButtonColor = System.Drawing.Color.Purple;
            p.LeftButtonIcon = SoftKeyIcon.Police;
            p.CenterButtonIcon = SoftKeyIcon.Fire;
            p.RightButtonIcon = SoftKeyIcon.Website;

            var agent = new iFruitContact("Agent 16")
            {
                DialTimeout = 4000,
                Active = true,
                Icon = ContactIcon.MP_ArmyContact
            };
            agent.Answered += contact =>
            {
                phone.Close(2000);
                pool.HideAll();
                // The board now opens even during a job, so you can check the objective or abort
                phoneBoard.Visible = true;
            };
            p.Contacts.Add(agent);
            return p;
        }

        private void Initialize()
        {
            initialized = true;
            Groups.Refresh();
            zones.Initialize();

            if (settings.ShowStartupNotifications)
            {
                Notify.Shop("~w~Loaded ~b~" + zones.Zones.Count + "~w~ Ammu-Nation + locations and ~b~" +
                            data.ContactLocations.Count + "~w~ contact missions.");
                Notify.Agent("~w~ Hey, Agent 16 here. Call me when you want work. Jobs start wherever you are.~n~" +
                             "The Ammu-Nation trucks on the map can be robbed for extra cash too.");
            }
            if (data.LoadErrors.Count > 0)
                Notify.Shop("~r~" + data.LoadErrors.Count + " mission file(s) failed to load.~w~ See MoreAmmunationsMod\\log.txt.");
        }

        private void OnTick(object sender, EventArgs e)
        {
            try
            {
                phone.Update();
                if (!initialized) Initialize();

                zones.Update();
                missions.Update();

                // Gamepad support: D-pad right = shop, D-pad left = rob (same as before)
                if (!pool.AreAnyVisible && Game.LastInputMethod == InputMethod.GamePad)
                {
                    if (Game.IsControlJustPressed(GTA.Control.Context)) zones.TryOpenShop();
                    else if (Game.IsControlJustPressed(GTA.Control.ContextSecondary)) zones.TryRob();
                }

                pool.Process();
            }
            catch (Exception ex)
            {
                // Don't flood the log if something breaks every frame
                if (Game.GameTime - lastErrorLog > 5000)
                {
                    lastErrorLog = Game.GameTime;
                    Log.Error("OnTick", ex);
                }
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == settings.MenuKey) zones.TryOpenShop();
                else if (e.KeyCode == settings.RobKey && !pool.AreAnyVisible) zones.TryRob();
                else if (settings.JobBoardKey != Keys.None && e.KeyCode == settings.JobBoardKey)
                {
                    if (phoneBoard.Visible) phoneBoard.Visible = false;
                    else if (!pool.AreAnyVisible) phoneBoard.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("OnKeyUp", ex);
            }
        }

        private void OnAborted(object sender, EventArgs e)
        {
            try
            {
                pool.HideAll();
                missions.Abort(true);
                zones.Cleanup();
            }
            catch (Exception ex)
            {
                Log.Error("OnAborted", ex);
            }
        }
    }
}
