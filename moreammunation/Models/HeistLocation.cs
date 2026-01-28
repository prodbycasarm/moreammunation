using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;

namespace moreammunation
{
    public class HeistLocation
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; } // central heist location
        public float Radius { get; set; }
        public string Description { get; set; }

        public int Reward;
        //Ground troops
        public string VehicleModel { get; set; } = null;
        // Target Vehicle details
        public List<string> GroundTroops { get; set; } = new List<string>();


        //Vehcle Driver Model
        public List<string> PedModels = new List<string>();
        public string VehicleDriverModel { get; set; } = null;
        public List<string> TargetPedModels = new List<string>();

        public Vector3 VehiclePosition { get; set; } = Vector3.Zero;
        public float TargetRotation { get; set; }

        // Extra vehicles at the location
        public List<AreaVehicle> AreaVehicles { get; set; } = new List<AreaVehicle>();
        public string DriverModel { get; set; } = null;

        public List<Vector3> GroundTroopPositions { get; set; } = new List<Vector3>();


    }

}
