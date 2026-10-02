using GTA;
using GTA.Math;
using GTA.Native;

namespace moreammunation
{
    /// <summary>
    /// Rob an Ammu-Nation delivery truck and bring it to a drop-off.
    /// The drop-off is now the nearest delivery point that is at least
    /// MinDeliveryDistance away (the old code picked a random one, which
    /// could be on the other side of the map).
    /// If you leave the truck, the GPS points you back to it.
    /// </summary>
    internal sealed class TruckRobberyMission : Mission
    {
        private readonly Vehicle truck;
        private readonly ArmoryZone zone;
        private Vector3 dropOff;
        private Blip dropBlip, truckBlip;

        public TruckRobberyMission(Settings settings, MissionData data, ArmoryZone zone, Vehicle truck) : base(settings, data)
        {
            this.zone = zone;
            this.truck = truck;
            Reward = zone.HeistReward;
        }

        public override string Title { get { return "Truck Robbery"; } }
        public ArmoryZone Zone { get { return zone; } }

        public override bool Start(Vector3 origin)
        {
            if (!Alive(truck))
            {
                CannotStart("The truck is gone.");
                return false;
            }

            Tracked.Add(truck);

            Vector3? nearest = LocationHelper.Nearest(Data.DeliveryPoints, truck.Position, Settings.MinDeliveryDistance);
            dropOff = nearest ?? LocationHelper.StreetPointNear(truck.Position, Settings.MinDeliveryDistance, Settings.MinDeliveryDistance + 700f);

            truck.LockStatus = VehicleLockStatus.Unlocked;
            truck.IsDriveable = true;
            Function.Call(Hash.SET_VEHICLE_DOOR_SHUT, truck.Handle, 5, false);
            Function.Call(Hash.TASK_ENTER_VEHICLE, Player.Handle, truck.Handle, -1, -1, 2.0f, 1, 0);

            Game.Player.WantedLevel = Settings.RobberyWantedLevel;

            dropBlip = LocationBlip(dropOff, "Delivery Point", BlipColor.Yellow);
            dropBlip.ShowRoute = false;

            Notify.Agent("~w~Damn it! The stolen weapons just got flagged on the radar. Get them to the drop at ~y~" +
                         LocationHelper.AreaName(dropOff) + "~w~ (" + Util.Distance(dropOff.DistanceTo2D(truck.Position)) + ").");
            return true;
        }

        protected override void OnUpdate()
        {
            if (!Alive(truck))
            {
                Fail("The weapons got destroyed...");
                return;
            }

            bool inTruck = Player.IsInVehicle(truck);

            if (!inTruck)
            {
                if (truckBlip == null || !truckBlip.Exists())
                    truckBlip = EntityBlip(truck, "Stolen Truck", BlipColor.Blue, BlipSprite.GunCar, true);
                dropBlip.ShowRoute = false;
                SetObjective("Get in the ~b~truck~w~.");
                return;
            }

            if (truckBlip != null && truckBlip.Exists()) truckBlip.Delete();
            truckBlip = null;
            dropBlip.ShowRoute = true;
            SetObjective("Deliver the truck to the ~y~drop-off~w~.");

            float distance = truck.Position.DistanceTo(dropOff);
            if (distance < 60f) DrawCheckpoint(dropOff, 6f);

            if (distance < 8f)
            {
                Game.Player.WantedLevel = 0;
                truck.Speed = 0f;
                truck.IsDriveable = false;
                Player.Task.LeaveVehicle(truck, LeaveVehicleFlags.None);
                truck.LockStatus = VehicleLockStatus.PlayerCannotEnter;
                Succeed("I've got other shipments for you, stay sharp.");
            }
        }
    }
}
