using GTA;
using GTA.Math;
using System.Collections.Generic;

namespace moreammunation
{
    /// <summary>
    /// The original Agent 16 mission, driven by ContactMissions\*.xml.
    /// Changes from the old version:
    ///  - uses the location nearest to the player (see LocationHelper),
    ///  - enemies spawn when you get close (~300 m) instead of instantly across the map,
    ///  - ground troops work even when the XML has no AreaVehicles,
    ///  - the target flees once it takes damage, and planes/boats don't try to "cruise".
    /// </summary>
    internal sealed class DestroyTargetMission : Mission
    {
        private const float SpawnDistance = 300f;

        private readonly HeistLocation loc;
        private Blip areaBlip;
        private Vehicle target;
        private Ped driver;
        private readonly List<Ped> enemies = new List<Ped>();
        private readonly List<KeyValuePair<Ped, Vehicle>> escortDrivers = new List<KeyValuePair<Ped, Vehicle>>();
        private bool spawned, fleeing, targetCanDrive;

        public DestroyTargetMission(Settings settings, MissionData data, HeistLocation location) : base(settings, data)
        {
            loc = location;
            Reward = location.Reward;
        }

        public override string Title { get { return "Destroy the Target: " + loc.Name; } }

        public override bool Start(Vector3 origin)
        {
            areaBlip = LocationBlip(loc.Anchor, loc.Name, BlipColor.Red, BlipSprite.BountyHit);
            string description = string.IsNullOrWhiteSpace(loc.Description) ? "Destroy the target." : loc.Description.Trim();
            SetObjective("Go to ~r~" + loc.Name + "~w~. " + description);
            return true;
        }

        protected override void OnUpdate()
        {
            if (!spawned)
            {
                if (Player.Position.DistanceTo(loc.Anchor) > SpawnDistance) return;
                SpawnEverything();
                if (target == null)
                {
                    Fail("The target never showed up (couldn't load model '" + loc.VehicleModel + "').");
                    return;
                }
                return;
            }

            if (target == null || !target.Exists() || target.IsDead)
            {
                Succeed("Target destroyed. I've got other targets for you, stay sharp.");
                return;
            }

            // Once the target is hit, it makes a run for it
            if (!fleeing && targetCanDrive && Alive(driver) &&
                (target.Health < target.MaxHealth || target.EngineHealth < 1000f || Player.Position.DistanceTo(target.Position) < 40f))
            {
                fleeing = true;
                Vector3 escape = LocationHelper.StreetPointNear(target.Position, 1200f, 1800f);
                DriveTo(driver, target, escape, 35f, true);
                SetObjective("The target is running! Destroy the ~r~vehicle~w~.");
            }
        }

        private void SpawnEverything()
        {
            spawned = true;
            Tracked.RemoveBlip(areaBlip);

            // --- target vehicle + crew ---
            target = SpawnVehicle(loc.VehicleModel, loc.VehiclePosition, loc.TargetRotation);
            if (target == null) return;

            Model model = target.Model;
            targetCanDrive = model.IsCar || model.IsBike || model.IsQuadBike;
            target.LockStatus = VehicleLockStatus.PlayerCannotEnter;
            EntityBlip(target, "Target", BlipColor.Red, BlipSprite.BountyHit, true);

            for (int i = 0; i < loc.TargetPedModels.Count; i++)
            {
                Ped p;
                if (targetCanDrive)
                    p = SpawnEnemyInVehicle(target, i, loc.TargetPedModels[i], 45, 0, i > 0);
                else
                    p = SpawnEnemy(loc.TargetPedModels[i], Spawner.OnFootPositionNear(target.Position, 4f, 9f), 45);
                if (p == null) continue;
                if (i == 0 && targetCanDrive) driver = p; else enemies.Add(p);
            }

            // --- escort / parked vehicles ---
            foreach (AreaVehicle av in loc.AreaVehicles)
            {
                Vehicle v = SpawnVehicle(av.ModelName, av.Position, av.Rotation);
                if (v == null) continue;
                for (int i = 0; i < av.PedModels.Count; i++)
                {
                    Ped p = SpawnEnemyInVehicle(v, i, av.PedModels[i], 40);
                    if (p == null) continue;
                    enemies.Add(p);
                    if (i == 0 && p.IsInVehicle(v)) escortDrivers.Add(new KeyValuePair<Ped, Vehicle>(p, v));
                }
            }

            // --- ground troops (positions picked now that the area is loaded) ---
            foreach (string troop in loc.GroundTroops)
            {
                Ped p = SpawnEnemy(troop, Spawner.OnFootPositionNear(loc.VehiclePosition, 6f, 14f), 45, 50);
                if (p != null) enemies.Add(p);
            }

            // --- AI: target cruises, escorts follow it ---
            if (targetCanDrive && Alive(driver))
            {
                driver.BlockPermanentEvents = true;
                driver.Task.CruiseWithVehicle(target, 20f, DrivingStyle.Normal);

                int mode = -1; // behind
                foreach (var pair in escortDrivers)
                {
                    Escort(pair.Key, pair.Value, target, mode);
                    mode = mode == -1 ? 0 : -1; // alternate behind/in front
                }
            }

            SetObjective("Destroy the ~r~target~w~. Watch out for its escort.");
        }
    }
}
