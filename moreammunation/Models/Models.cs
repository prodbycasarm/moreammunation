using GTA.Math;
using System.Collections.Generic;

namespace moreammunation
{
    /// <summary>An Ammu-Nation + location from moreammunation.ini ([ArmoryZone1], [ArmoryZone2], ...).</summary>
    public class ArmoryZone
    {
        public int Index { get; set; }
        public Vector3 Position { get; set; }
        public string BlipSprite { get; set; }
        public string BlipColor { get; set; }
        public string BlipName { get; set; }

        public bool SpawnVehicle { get; set; }
        public string VehicleName { get; set; }
        public float VehicleHeading { get; set; }

        public int HeistReward { get; set; }

        public bool SpawnNpc { get; set; }
        public string NpcModel { get; set; }
        public int NpcNumber { get; set; }
    }

    /// <summary>A vehicle parked at a contact mission location, with its crew.</summary>
    public class AreaVehicle
    {
        public string ModelName;
        public Vector3 Position;
        public float Rotation;
        public List<string> PedModels = new List<string>();
    }

    /// <summary>One ContactMissions\locationXXX.xml file ("destroy the target" missions).</summary>
    public class HeistLocation
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public float Radius { get; set; }
        public string Description { get; set; }
        public int Reward { get; set; }

        // Target vehicle: first ped is the driver, the rest are passengers
        public string VehicleModel { get; set; }
        public Vector3 VehiclePosition { get; set; }
        public float TargetRotation { get; set; }
        public List<string> TargetPedModels { get; set; } = new List<string>();

        public List<AreaVehicle> AreaVehicles { get; set; } = new List<AreaVehicle>();

        // Ground troops are placed around the target when the area loads
        public List<string> GroundTroops { get; set; } = new List<string>();

        /// <summary>Where the mission actually happens (the target), used for "nearest" sorting.</summary>
        public Vector3 Anchor
        {
            get { return VehiclePosition != Vector3.Zero ? VehiclePosition : Position; }
        }
    }
}
