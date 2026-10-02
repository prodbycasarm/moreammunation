using GTA;
using GTA.Math;
using GTA.Native;
using System.Collections.Generic;

namespace moreammunation
{
    /// <summary>
    /// NEW: An escorted weapons truck drives through the area near the player.
    /// Stop it, steal it, and deliver it to the nearest drop-off.
    /// Fails if the truck is destroyed or reaches its destination.
    /// </summary>
    internal sealed class ConvoyHijackMission : Mission
    {
        public const int BaseReward = 85000;

        private static readonly string[] TruckModels = { "mule4", "pounder2", "stockade" };
        private static readonly string[] EscortModels = { "baller5", "mesa3", "xls2" };
        private static readonly string[] CrewModels = { "s_m_m_armoured_02", "mp_m_securoguard_01", "s_m_y_blackops_01" };

        private enum Phase { Approach, Intercept, Deliver }

        private Phase phase = Phase.Approach;
        private Vector3 startPoint, convoyDestination, dropOff;
        private Vehicle truck;
        private Ped truckDriver;
        private readonly List<Ped> enemies = new List<Ped>();
        private Blip startBlip, truckBlip, dropBlip;
        private bool alerted;

        public ConvoyHijackMission(Settings settings, MissionData data) : base(settings, data)
        {
            Reward = BaseReward;
        }

        public override string Title { get { return "Hijack the Convoy"; } }

        public override bool Start(Vector3 origin)
        {
            startPoint = LocationHelper.StreetPointNear(origin, Settings.MinMissionDistance + 150f, Settings.MinMissionDistance + 550f);
            if (startPoint == Vector3.Zero)
            {
                CannotStart("No roads nearby. Try again from somewhere in the city or on a highway.");
                return false;
            }

            startBlip = LocationBlip(startPoint, "Convoy", BlipColor.Red, BlipSprite.ArmoredTruck);
            SetObjective("A weapons convoy is passing through ~r~" + LocationHelper.AreaName(startPoint) + "~w~. Intercept it.");
            return true;
        }

        protected override void OnUpdate()
        {
            switch (phase)
            {
                case Phase.Approach:
                    if (Player.Position.DistanceTo(startPoint) < 260f) SpawnConvoy();
                    break;
                case Phase.Intercept:
                    UpdateIntercept();
                    break;
                case Phase.Deliver:
                    UpdateDeliver();
                    break;
            }
        }

        private void SpawnConvoy()
        {
            Tracked.RemoveBlip(startBlip);

            // Put the convoy on the road, facing away from the player, and give it somewhere far to go
            convoyDestination = LocationHelper.StreetPointNear(startPoint, 1400f, 2000f);
            float heading = LocationHelper.HeadingTo(startPoint, convoyDestination);

            truck = SpawnVehicle(Spawner.Pick(TruckModels), startPoint, heading);
            if (truck == null)
            {
                Fail("The convoy never showed up.");
                return;
            }
            truck.LockStatus = VehicleLockStatus.Unlocked;
            truckBlip = EntityBlip(truck, "Weapons Truck", BlipColor.Red, BlipSprite.ArmoredTruck, true);

            truckDriver = SpawnEnemyInVehicle(truck, 0, Spawner.Pick(CrewModels), 45, 50, false);
            Ped guard = SpawnEnemyInVehicle(truck, 1, Spawner.Pick(CrewModels), 45, 50);
            if (guard != null) enemies.Add(guard);

            // Escorts: one ahead, one behind
            Vector3 forward = truck.ForwardVector;
            SpawnEscort(startPoint + forward * 14f, heading, 0);
            SpawnEscort(startPoint - forward * 14f, heading, -1);

            DriveTo(truckDriver, truck, convoyDestination, 18f, false);

            phase = Phase.Intercept;
            SetObjective("Stop the ~r~weapons truck~w~ and steal it. Don't blow it up.");
        }

        private void SpawnEscort(Vector3 position, float heading, int escortMode)
        {
            Vector3 street = World.GetNextPositionOnStreet(position, true);
            Vehicle v = SpawnVehicle(Spawner.Pick(EscortModels), street == Vector3.Zero ? position : street, heading);
            if (v == null) return;

            Ped driver = SpawnEnemyInVehicle(v, 0, Spawner.Pick(CrewModels), 40, 50);
            if (driver != null) enemies.Add(driver);
            for (int seat = 1; seat <= 3; seat++)
            {
                Ped p = SpawnEnemyInVehicle(v, seat, Spawner.Pick(CrewModels), 40, 50);
                if (p != null) enemies.Add(p);
            }
            Escort(driver, v, truck, escortMode);
        }

        private void UpdateIntercept()
        {
            if (!Alive(truck))
            {
                Fail("The truck got destroyed. The weapons are scrap.");
                return;
            }

            if (truck.Position.DistanceTo(convoyDestination) < 25f && Alive(truckDriver) && truckDriver.IsInVehicle(truck))
            {
                Fail("The convoy got away.");
                return;
            }

            // First shot fired / truck hit: the truck floors it and the escorts fight
            if (!alerted && (truck.Health < truck.MaxHealth || AnyInCombat() || Player.Position.DistanceTo(truck.Position) < 25f))
            {
                alerted = true;
                if (Alive(truckDriver)) DriveTo(truckDriver, truck, convoyDestination, 32f, true);
                Game.Player.WantedLevel = System.Math.Max(Game.Player.WantedLevel, Settings.ConvoyWantedLevel);
            }

            if (Player.IsInVehicle(truck) && Player.SeatIndex == VehicleSeat.Driver)
                BeginDelivery();
        }

        private bool AnyInCombat()
        {
            foreach (Ped p in enemies)
                if (Alive(p) && Function.Call<bool>(Hash.IS_PED_IN_COMBAT, p.Handle, Player.Handle)) return true;
            return false;
        }

        private void BeginDelivery()
        {
            phase = Phase.Deliver;
            if (truckBlip != null && truckBlip.Exists()) truckBlip.Delete();
            truckBlip = null;

            Vector3? nearest = LocationHelper.Nearest(Data.DeliveryPoints, truck.Position, Settings.MinDeliveryDistance * 0.6f);
            dropOff = nearest ?? LocationHelper.StreetPointNear(truck.Position, 600f, 1200f);
            dropBlip = LocationBlip(dropOff, "Drop-off", BlipColor.Yellow);

            Notify.Agent("~w~Nice work! Bring the truck to ~y~" + LocationHelper.AreaName(dropOff) + "~w~ and lose anyone on your tail.");
        }

        private void UpdateDeliver()
        {
            if (!Alive(truck))
            {
                Fail("The truck got destroyed. The weapons are scrap.");
                return;
            }

            if (!Player.IsInVehicle(truck))
            {
                if (truckBlip == null || !truckBlip.Exists())
                    truckBlip = EntityBlip(truck, "Weapons Truck", BlipColor.Blue, BlipSprite.ArmoredTruck, true);
                dropBlip.ShowRoute = false;
                SetObjective("Get back in the ~b~truck~w~.");
                return;
            }

            if (truckBlip != null && truckBlip.Exists()) truckBlip.Delete();
            truckBlip = null;
            dropBlip.ShowRoute = true;
            SetObjective("Deliver the truck to the ~y~drop-off~w~.");

            float d = truck.Position.DistanceTo(dropOff);
            if (d < 60f) DrawCheckpoint(dropOff, 6f);
            if (d < 8f)
            {
                Game.Player.WantedLevel = 0;
                truck.Speed = 0f;
                Player.Task.LeaveVehicle(truck, LeaveVehicleFlags.None);
                truck.LockStatus = VehicleLockStatus.PlayerCannotEnter;
                Succeed("That's a lot of hardware off the street.");
            }
        }
    }
}
