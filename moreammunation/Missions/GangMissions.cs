using GTA;
using GTA.Math;
using GTA.Native;
using System.Collections.Generic;

namespace moreammunation
{
    /// <summary>Gang "skins" so the ground missions don't all look the same.</summary>
    internal sealed class GangSet
    {
        public string Name;
        public string[] Peds;
        public string[] Vehicles;

        public static readonly GangSet[] All =
        {
            new GangSet { Name = "The Lost MC",   Peds = new[] { "g_m_y_lost_01", "g_m_y_lost_02", "g_m_y_lost_03" },            Vehicles = new[] { "gburrito" } },
            new GangSet { Name = "the Vagos",     Peds = new[] { "g_m_y_mexgoon_01", "g_m_y_mexgoon_02", "g_m_y_mexgoon_03" },  Vehicles = new[] { "buccaneer" } },
            new GangSet { Name = "the Ballas",    Peds = new[] { "g_m_y_ballaeast_01", "g_m_y_ballaorig_01", "g_m_y_ballasout_01" }, Vehicles = new[] { "baller" } },
            new GangSet { Name = "the Armenians", Peds = new[] { "g_m_m_armgoon_01", "g_m_m_armlieut_01", "g_m_m_armboss_01" }, Vehicles = new[] { "schafter2" } }



            //new GangSet { Name = "the Families",      Peds = new[] { "g_m_y_famca_01", "g_m_y_famdnf_01", "g_m_y_famfor_01" },           Vehicles = new[] { "voodoo" } },
            //new GangSet { Name = "Marabunta Grande",  Peds = new[] { "g_m_y_salvagoon_01", "g_m_y_salvagoon_02", "g_m_y_salvaboss_01" }, Vehicles = new[] { "chino" } },
            //new GangSet { Name = "the Aztecas",       Peds = new[] { "g_m_y_azteca_01" },                                                 Vehicles = new[] { "primo" } },
            //new GangSet { Name = "the Kkangpae",      Peds = new[] { "g_m_y_korean_01", "g_m_y_korean_02", "g_m_y_korlieut_01" },        Vehicles = new[] { "sultan" } },
            //new GangSet { Name = "the Triads",        Peds = new[] { "g_m_m_chigoon_01", "g_m_m_chigoon_02", "g_m_m_chicold_01" },       Vehicles = new[] { "fugitive" } },
            //new GangSet { Name = "the Madrazo Cartel",Peds = new[] { "g_m_y_mexgang_01", "g_m_m_mexboss_01", "g_m_m_mexboss_02" },       Vehicles = new[] { "cavalcade" } },
            //new GangSet { Name = "the Hillbillies",   Peds = new[] { "a_m_m_hillbilly_01", "a_m_m_hillbilly_02" },                        Vehicles = new[] { "rebel" } },


        };

        public static GangSet Random() { return Spawner.Pick(All); }
    }

    /// <summary>
    /// A gang is guarding a weapons stash near the player.
    /// Kill the guards, grab the stash, then bring it to the nearest drop-off to get paid.
    /// Grabbing the stash also fills the ammo of the weapon in your hands.
    /// </summary>
    internal sealed class StashRaidMission : Mission
    {
        public const int BaseReward = 45000;
        private const float MinDropDistance = 400f;
        private static readonly string[] StashProps = { "prop_box_ammo03a", "prop_box_ammo04a", "prop_box_guncase_01a" };

        private enum Phase { Approach, Clear, Grab, Deliver }

        private readonly GangSet gang = GangSet.Random();
        private readonly List<Ped> guards = new List<Ped>();
        private Phase phase = Phase.Approach;
        private Vector3 location, dropOff;
        private Blip areaBlip, stashBlip;
        private Prop stash;

        public StashRaidMission(Settings settings, MissionData data) : base(settings, data)
        {
            Reward = BaseReward;
        }

        public override string Title { get { return "Raid a Gun Stash"; } }

        public override bool Start(Vector3 origin)
        {
            location = LocationHelper.StreetPointNear(origin, Settings.MinMissionDistance + 100f, Settings.MinMissionDistance + 450f);
            if (location == Vector3.Zero)
            {
                CannotStart("Nothing nearby. Try again somewhere more populated.");
                return false;
            }
            areaBlip = LocationBlip(location, "Gun Stash", BlipColor.Red, BlipSprite.WeaponSupplies);
            SetObjective(gang.Name + " are sitting on a stash near ~r~" + LocationHelper.AreaName(location) + "~w~. Go take it.");
            return true;
        }

        protected override void OnUpdate()
        {
            switch (phase)
            {
                case Phase.Approach:
                    if (Player.Position.DistanceTo(location) < 200f) Spawn();
                    break;

                case Phase.Clear:
                    int alive = AliveCount(guards);
                    if (alive > 0)
                    {
                        SetObjective("Take out ~r~" + gang.Name + "~w~ (" + alive + " left).");
                        break;
                    }
                    stashBlip = stash != null && stash.Exists()
                        ? EntityBlip(stash, "Stash", BlipColor.Green, BlipSprite.WeaponSupplies, true)
                        : LocationBlip(location, "Stash", BlipColor.Green, BlipSprite.WeaponSupplies);
                    phase = Phase.Grab;
                    SetObjective("Area clear. Grab the ~g~stash~w~.");
                    break;

                case Phase.Grab:
                    UpdateGrab();
                    break;

                case Phase.Deliver:
                    UpdateDeliver();
                    break;
            }
        }

        private void UpdateGrab()
        {
            Vector3 stashPos = stash != null && stash.Exists() ? stash.Position : location;
            if (Player.Position.DistanceTo(stashPos) < 12f) DrawCheckpoint(stashPos, 1.5f);
            if (!Player.IsOnFoot || Player.Position.DistanceTo(stashPos) > 2.2f) return;

            // Bonus: top up the weapon in your hands
            Weapon held = Player.Weapons.Current;
            if (held != null && held.Hash != WeaponHash.Unarmed) held.Ammo = held.MaxAmmo;

            Tracked.RemoveBlip(stashBlip);
            stashBlip = null;
            if (stash != null && stash.Exists()) stash.Delete();

            // Nearest delivery point that isn't right next to the stash
            Vector3? nearest = LocationHelper.Nearest(Data.DeliveryPoints, Player.Position, MinDropDistance);
            dropOff = nearest ?? LocationHelper.StreetPointNear(Player.Position, MinDropDistance, MinDropDistance + 500f);
            LocationBlip(dropOff, "Drop-off", BlipColor.Yellow);

            phase = Phase.Deliver;
            Notify.Agent("~w~Got it. Bring the stash to ~y~" + LocationHelper.AreaName(dropOff) + "~w~ (" +
                         Util.Distance(dropOff.DistanceTo2D(Player.Position)) + ").");
            SetObjective("Bring the stash to the ~y~drop-off~w~.");
        }

        private void UpdateDeliver()
        {
            float d = Player.Position.DistanceTo(dropOff);
            if (d < 60f) DrawCheckpoint(dropOff, 5f);
            if (d < 4f)
                Succeed("Stash delivered. I topped up your piece when you grabbed it.");
        }

        private void Spawn()
        {
            Tracked.RemoveBlip(areaBlip);

            // Stash sits on the sidewalk next to the road point
            Vector3 stashPos = Spawner.OnFootPositionNear(location, 3f, 7f);
            location = stashPos;
            foreach (string model in StashProps)
            {
                var m = new Model(model);
                if (!Spawner.LoadModel(m, 2000)) continue;
                stash = Tracked.Add(World.CreateProp(m, stashPos, true, true));
                m.MarkAsNoLongerNeeded();
                if (stash != null) break;
            }

            // A parked gang car for cover
            Vector3 carPos = World.GetNextPositionOnStreet(location, true);
            SpawnVehicle(Spawner.Pick(gang.Vehicles), carPos == Vector3.Zero ? location.Around(6f) : carPos, Spawner.Range(0, 359));

            int count = Spawner.Range(6, 9);
            for (int i = 0; i < count; i++)
            {
                Ped p = SpawnEnemy(Spawner.Pick(gang.Peds), Spawner.OnFootPositionNear(location, 3f, 16f), 35, 0);
                if (p == null) continue;
                p.Task.GuardCurrentPosition();
                guards.Add(p);
            }

            phase = Phase.Clear;
            if (guards.Count == 0) Fail("Looks like someone got there first. Stash is gone.");
        }
    }
    /// <summary>
    /// NEW: A rival arms dealer with bodyguards is near the player.
    /// When he spots you (or shots are fired) he runs for his car.
    /// Kill him before he gets away (~600 m).
    /// </summary>
    internal sealed class AssassinationMission : Mission
    {
        public const int BaseReward = 60000;
        private static readonly string[] DealerModels = { "g_m_m_armboss_01", "g_m_m_mexboss_01", "g_m_m_chiboss_01" };
        private static readonly string[] GuardModels = { "s_m_m_highsec_01", "s_m_m_highsec_02" };
        private static readonly string[] CarModels = { "baller6", "cognoscenti", "schafter2" };

        private enum Phase { Approach, Meeting, Escaping }

        private Phase phase = Phase.Approach;
        private Vector3 location;
        private Ped dealer;
        private Vehicle car;
        private readonly List<Ped> guards = new List<Ped>();
        private Blip areaBlip;
        private bool driving;

        public AssassinationMission(Settings settings, MissionData data) : base(settings, data)
        {
            Reward = BaseReward;
        }

        public override string Title { get { return "Eliminate the Dealer"; } }

        public override bool Start(Vector3 origin)
        {
            location = LocationHelper.StreetPointNear(origin, Settings.MinMissionDistance + 150f, Settings.MinMissionDistance + 500f);
            if (location == Vector3.Zero)
            {
                CannotStart("Couldn't find the meeting spot. Try again somewhere else.");
                return false;
            }
            areaBlip = LocationBlip(location, "Meeting", BlipColor.Red, BlipSprite.BountyHit);
            SetObjective("An arms dealer is meeting his crew near ~r~" + LocationHelper.AreaName(location) + "~w~. Take him out.");
            return true;
        }

        protected override void OnUpdate()
        {
            switch (phase)
            {
                case Phase.Approach:
                    if (Player.Position.DistanceTo(location) < 200f) Spawn();
                    break;

                case Phase.Meeting:
                    if (!Alive(dealer)) { Succeed("Clean work. His buyers will come to us now."); return; }
                    bool spotted = Player.Position.DistanceTo(dealer.Position) < 45f
                                   || dealer.Health < dealer.MaxHealth
                                   || AnyGuardFighting();
                    if (spotted) Escape();
                    break;

                case Phase.Escaping:
                    if (!Alive(dealer)) { Succeed("He didn't get far. Nice."); return; }
                    if (Player.Position.DistanceTo(dealer.Position) > 600f) { Fail("The dealer got away."); return; }

                    if (!driving && Alive(car) && dealer.IsInVehicle(car))
                    {
                        driving = true;
                        Vector3 escape = LocationHelper.StreetPointNear(dealer.Position, 1500f, 2000f);
                        DriveTo(dealer, car, escape, 40f, true);
                    }
                    SetObjective(driving ? "The ~r~dealer~w~ is driving off. Stop him!" : "The ~r~dealer~w~ is running to his car!");
                    break;
            }
        }

        private bool AnyGuardFighting()
        {
            foreach (Ped g in guards)
                if (Alive(g) && Function.Call<bool>(Hash.IS_PED_IN_COMBAT, g.Handle, Player.Handle)) return true;
            return false;
        }

        private void Spawn()
        {
            Tracked.RemoveBlip(areaBlip);

            Vector3 street = World.GetNextPositionOnStreet(location, true);
            car = SpawnVehicle(Spawner.Pick(CarModels), street == Vector3.Zero ? location : street, Spawner.Range(0, 359));
            if (car != null) car.LockStatus = VehicleLockStatus.Unlocked;

            Vector3 standPos = Spawner.OnFootPositionNear(car != null ? car.Position : location, 4f, 7f);
            dealer = Tracked.Add(Spawner.CreatePed(Spawner.Pick(DealerModels), standPos));
            if (dealer == null)
            {
                Fail("The dealer never showed up.");
                return;
            }
            dealer.RelationshipGroup = Groups.Hostiles;
            dealer.Weapons.Give(WeaponHash.Pistol, 200, false, true);
            dealer.Armor = 50;
            EntityBlip(dealer, "Dealer", BlipColor.Red, BlipSprite.BountyHit, true);

            int count = Spawner.Range(3, 5);
            for (int i = 0; i < count; i++)
            {
                Ped g = SpawnEnemy(Spawner.Pick(GuardModels), Spawner.OnFootPositionNear(dealer.Position, 2f, 8f), 50, 100);
                if (g == null) continue;
                g.Task.GuardCurrentPosition();
                guards.Add(g);
            }

            phase = Phase.Meeting;
            SetObjective("Kill the ~r~dealer~w~. He'll run if he spots you.");
        }

        private void Escape()
        {
            phase = Phase.Escaping;
            dealer.BlockPermanentEvents = true;
            if (Alive(car))
                Function.Call(Hash.TASK_ENTER_VEHICLE, dealer.Handle, car.Handle, 10000, -1, 2.0f, 1, 0);
            else
                dealer.Task.FleeFrom(Player);

            foreach (Ped g in guards) AttackPlayer(g);
        }
    }
}
