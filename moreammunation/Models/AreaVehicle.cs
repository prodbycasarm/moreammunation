using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;

namespace moreammunation
{
    public class VehiclePedInfo
    {
        public string Models;
        public VehicleSeat Seats;
    }

    public class AreaVehicle
    {
        public string ModelName;
        public Vector3 Position;
        public float Rotation;

        public List<string> PedModels = new List<string>();
    }




}
