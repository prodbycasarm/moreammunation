using GTA;
using GTA.Math;
using System.Collections.Generic;

namespace moreammunation
{
    /// <summary>
    /// NEW: Defend the Ammu-Nation + store nearest to the player.
    /// Waves of attackers drive in from nearby roads. The store's guards fight
    /// on your side (they hate the attackers' relationship group).
    /// Leaving the area for too long fails the job.
    /// </summary>
    internal sealed class StoreDefenseMission : Mission
    {
        public const int RewardPerWave = 20000;
        private const float StartRadius = 45f;
        private const float LeaveRadius = 180f;

        private enum Phase { GoToStore, Countdown, Fighting, Between }

        private readonly ArmoryZone zone;
        private readonly GangSet gang = GangSet.Random();
        private readonly List<Ped> waveEnemies = new List<Ped>();
        private Phase phase = Phase.GoToStore;
        private int wave;
        private int timer;
        private int leftAreaSince;
        private Blip storeBlip;

        public StoreDefenseMission(Settings settings, MissionData data, ArmoryZone zone) : base(settings, data)
        {
            this.zone = zone;
            Reward = RewardPerWave * settings.DefenseWaves;
        }

        public override string Title { get { return "Defend the Store"; } }

        public override bool Start(Vector3 origin)
        {
            storeBlip = LocationBlip(zone.Position, "Ammu-Nation", BlipColor.Blue, BlipSprite.Shield);
            SetObjective("Head to the ~b~Ammu-Nation~w~ before " + gang.Name + " get there.");
            return true;
        }

        protected override void OnUpdate()
        {
            float distance = Player.Position.DistanceTo(zone.Position);

            if (phase == Phase.GoToStore)
            {
                if (distance < 60f) DrawCheckpoint(zone.Position, StartRadius * 0.15f);
                if (distance > StartRadius) return;

                storeBlip.ShowRoute = false;
                phase = Phase.Countdown;
                timer = Game.GameTime + 8000;
                SetObjective("Get ready. They'll be here any second.");
                return;
            }

            // Don't abandon the store
            if (distance > LeaveRadius)
            {
                if (leftAreaSince == 0)
                {
                    leftAreaSince = Game.GameTime;
                    storeBlip.ShowRoute = true;
                }
                int left = 15 - (Game.GameTime - leftAreaSince) / 1000;
                SetObjective("Get back to the ~b~store~w~! (" + left + "s)");
                if (left <= 0) Fail("You abandoned the store.");
                return;
            }
            if (leftAreaSince != 0)
            {
                leftAreaSince = 0;
                storeBlip.ShowRoute = false;
            }

            switch (phase)
            {
                case Phase.Countdown:
                case Phase.Between:
                    if (Game.GameTime >= timer) SpawnWave();
                    break;

                case Phase.Fighting:
                    // Attackers that parked up go on foot and push the store
                    foreach (Ped p in waveEnemies)
                        if (Alive(p) && p.IsInVehicle() && p.CurrentVehicle.Speed < 1f && p.Position.DistanceTo(zone.Position) < 90f)
                            AttackPlayer(p);

                    int alive = AliveCount(waveEnemies);
                    SetObjective("Wave " + wave + "/" + Settings.DefenseWaves + ": kill the ~r~attackers~w~ (" + alive + " left).");
                    if (alive > 0) break;

                    if (wave >= Settings.DefenseWaves)
                    {
                        Succeed("The store's still standing. The owners won't forget this.");
                        return;
                    }
                    phase = Phase.Between;
                    timer = Game.GameTime + 7000;
                    SetObjective("Wave cleared. Reload, more are coming.");
                    break;
            }
        }

        private void SpawnWave()
        {
            wave++;
            waveEnemies.Clear();
            phase = Phase.Fighting;

            int vehicles = 1 + (wave + 1) / 2; // 1, 2, 2, 3, 3...
            for (int i = 0; i < vehicles; i++)
            {
                Vector3 spawn = LocationHelper.StreetPointNear(zone.Position, 160f, 260f);
                if (spawn == Vector3.Zero) continue;

                Vehicle v = SpawnVehicle(Spawner.Pick(gang.Vehicles), spawn, LocationHelper.HeadingTo(spawn, zone.Position));
                if (v == null) continue;

                int crew = Spawner.Range(2, 4);
                Ped driver = null;
                for (int seat = 0; seat < crew; seat++)
                {
                    Ped p = SpawnEnemyInVehicle(v, seat, Spawner.Pick(gang.Peds), 35 + wave * 5, wave > 1 ? 50 : 0);
                    if (p == null) continue;
                    waveEnemies.Add(p);
                    if (seat == 0) driver = p;
                }
                DriveTo(driver, v, zone.Position, 25f, true);
            }

            // Guarantee at least a few attackers on foot, even if no road was found
            if (waveEnemies.Count == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    Ped p = SpawnEnemy(Spawner.Pick(gang.Peds), Spawner.OnFootPositionNear(zone.Position, 40f, 60f), 35 + wave * 5);
                    if (p == null) continue;
                    AttackPlayer(p);
                    waveEnemies.Add(p);
                }
            }

            Notify.Agent("~r~Wave " + wave + "/" + Settings.DefenseWaves + "~w~ incoming!", "Defend the Store");
        }
    }
}
