using GTA.Math;

namespace moreammunation
{
    public class ArmoryZone
    {
        public Vector3 Position { get; set; }
        public string BlipSprite { get; set; }
        public string BlipColor { get; set; }
        public string BlipName { get; set; }

        public bool SpawnVehicle { get; set; }
        public string VehicleName { get; set; }

        public int HeistReward { get; set; }

        public bool SpawnNpc { get; set; }
        public string NpcModel { get; set; }
        public int NpcNumber { get; set; }
    }
}
