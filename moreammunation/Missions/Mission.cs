using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;

namespace moreammunation
{
    internal enum MissionStatus { Running, Succeeded, Failed }

    /// <summary>
    /// Base class for every job. A mission:
    ///  - picks its location in Start() (near the player),
    ///  - usually waits until the player is close before spawning enemies
    ///    (so nothing falls through an unloaded map),
    ///  - reports Succeed()/Fail() from Update(),
    ///  - never cleans up by hand: everything goes through Tracked.
    /// </summary>
    internal abstract class Mission
    {
        protected readonly EntityTracker Tracked = new EntityTracker();
        protected readonly Settings Settings;
        protected readonly MissionData Data;

        private string objective;

        protected Mission(Settings settings, MissionData data)
        {
            Settings = settings;
            Data = data;
        }

        private void UpdateGps()
        {
            Blip target = null;
            for (int i = routeBlips.Count - 1; i >= 0; i--)
            {
                Blip b = routeBlips[i];
                if (b == null || !b.Exists()) { routeBlips.RemoveAt(i); continue; }
                if (b.ShowRoute) { target = b; break; }
            }

            if (target == null) { ClearGps(); return; }

            Vector3 pos = target.Position;
            int color = HudColorFor(target.Color);

            // Only re-plot when the target moved (e.g. a fleeing car) or the color changed
            if (gpsActive && pos.DistanceTo(gpsTarget) < 15f && color == gpsColor) return;

            gpsTarget = pos;
            gpsColor = color;
            gpsActive = true;

            Function.Call(Hash.CLEAR_GPS_MULTI_ROUTE);
            Function.Call(Hash.START_GPS_MULTI_ROUTE, color, true, true); // color, start from player, show on foot
            Function.Call(Hash.ADD_POINT_TO_GPS_MULTI_ROUTE, pos.X, pos.Y, pos.Z);
            Function.Call(Hash.SET_GPS_MULTI_ROUTE_RENDER, true);
        }

        private void ClearGps()
        {
            if (!gpsActive) return;
            gpsActive = false;
            Function.Call(Hash.SET_GPS_MULTI_ROUTE_RENDER, false);
            Function.Call(Hash.CLEAR_GPS_MULTI_ROUTE);
        }

        private static int HudColorFor(BlipColor color)
        {
            switch (color)
            {
                case BlipColor.Red: return 6;     // HUD_COLOUR_RED
                case BlipColor.Blue: return 9;    // HUD_COLOUR_BLUE
                case BlipColor.Green: return 18;  // HUD_COLOUR_GREEN
                default: return 12;               // HUD_COLOUR_YELLOW
            }
        }

        public abstract string Title { get; }
        public int Reward { get; protected set; }
        public MissionStatus Status { get; private set; }
        public string ResultMessage { get; private set; }
        public string Objective { get { return objective; } }

        public readonly List<Blip> routeBlips = new List<Blip>();
        public Vector3 gpsTarget;
        public int gpsColor = -1;
        public bool gpsActive;

        protected static Ped Player { get { return Game.Player.Character; } }

        /// <summary>Set up the mission. Return false (and set ResultMessage) if it can't start here.</summary>
        public abstract bool Start(Vector3 origin);

        protected abstract void OnUpdate();

        public void Update()
        {
            Tracked.PruneDeadPedBlips();
            OnUpdate();
            UpdateGps();
        }

        public virtual void Cleanup(bool abort)
        {
            ClearGps();
            Tracked.Release(abort);
        }

        protected void Succeed(string message = null)
        {
            if (Status != MissionStatus.Running) return;
            Status = MissionStatus.Succeeded;
            ResultMessage = message;
        }

        public void Fail(string reason)
        {
            if (Status != MissionStatus.Running) return;
            Status = MissionStatus.Failed;
            ResultMessage = reason;
        }

        protected void CannotStart(string reason)
        {
            ResultMessage = reason;
        }

        protected void SetObjective(string text)
        {
            if (text == objective) return;
            objective = text;
            Screen.ShowSubtitle(text, 8000);
        }

        protected Blip LocationBlip(Vector3 position, string name, BlipColor color, BlipSprite sprite = BlipSprite.Standard)
        {
            Blip b = World.CreateBlip(position);
            b.Sprite = sprite;
            b.Color = color;
            b.Name = name;
            b.ShowRoute = true;
            routeBlips.Add(b);
            return Tracked.Add(b);
        }

        protected Blip EntityBlip(Entity entity, string name, BlipColor color, BlipSprite sprite, bool route)
        {
            Blip b = entity.AddBlip();
            b.Sprite = sprite;
            b.Color = color;
            b.Name = name;
            b.ShowRoute = route;
            routeBlips.Add(b);
            return b;
        }

        protected Ped SpawnEnemy(string model, Vector3 position, int accuracy = 40, int armor = 0, bool blip = true)
        {
            Ped p = Tracked.Add(Spawner.CreatePed(model, position, (float)(Spawner.Range(0, 359))));
            if (p == null) return null;
            Spawner.MakeHostile(p, Spawner.RandomEnemyWeapon(), accuracy, armor);
            if (blip) Spawner.EnemyBlip(p);
            return p;
        }

        protected Ped SpawnEnemyInVehicle(Vehicle vehicle, int seatIndex, string model, int accuracy = 40, int armor = 0, bool blip = true)
        {
            Ped p = Tracked.Add(Spawner.CreatePedInVehicle(vehicle, seatIndex, model));
            if (p == null) return null;
            Spawner.MakeHostile(p, Spawner.RandomEnemyWeapon(), accuracy, armor);
            if (blip) Spawner.EnemyBlip(p);
            return p;
        }

        protected Vehicle SpawnVehicle(string model, Vector3 position, float heading)
        {
            return Tracked.Add(Spawner.CreateVehicle(model, position, heading));
        }

        protected static bool Alive(Entity e)
        {
            return e != null && e.Exists() && !e.IsDead;
        }

        protected static int AliveCount(IEnumerable<Ped> peds)
        {
            return peds.Count(p => Alive(p));
        }

        protected static void DrawCheckpoint(Vector3 position, float size)
        {
            World.DrawMarker(MarkerType.VerticalCylinder, position - new Vector3(0f, 0f, 1f), Vector3.Zero, Vector3.Zero,
                             new Vector3(size, size, 1.5f), Color.FromArgb(140, 240, 200, 80));
        }

        /// <summary>Makes a driver go somewhere and keep going, even when shot at.</summary>
        protected static void DriveTo(Ped driver, Vehicle vehicle, Vector3 destination, float speed, bool aggressive)
        {
            if (!Alive(driver) || !Alive(vehicle)) return;
            int style = aggressive ? 786468 : 786603; // "avoid traffic" vs "normal"
            driver.BlockPermanentEvents = true;
            Function.Call(Hash.SET_DRIVER_ABILITY, driver.Handle, 1.0f);
            Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, driver.Handle, vehicle.Handle,
                          destination.X, destination.Y, destination.Z, speed, style, 10f);
            Function.Call(Hash.SET_PED_KEEP_TASK, driver.Handle, true);
        }

        /// <summary>Makes a vehicle follow another one (convoy escort).</summary>
        protected static void Escort(Ped driver, Vehicle vehicle, Vehicle target, int mode)
        {
            if (!Alive(driver) || !Alive(vehicle) || !Alive(target)) return;
            Function.Call(Hash.TASK_VEHICLE_ESCORT, driver.Handle, vehicle.Handle, target.Handle, mode, 40f, 786603, 8f, 0, 20f);
            Function.Call(Hash.SET_PED_KEEP_TASK, driver.Handle, true);
        }

        protected static void AttackPlayer(Ped p)
        {
            if (!Alive(p)) return;
            p.BlockPermanentEvents = false;
            Function.Call(Hash.TASK_COMBAT_PED, p.Handle, Game.Player.Character.Handle, 0, 16);
        }
    }

    /// <summary>Loads ContactMissions\*.xml and DeliveryPoints\*.xml (culture-safe).</summary>
    internal sealed class MissionData
    {
        public readonly List<HeistLocation> ContactLocations = new List<HeistLocation>();
        public readonly List<Vector3> DeliveryPoints = new List<Vector3>();
        public readonly List<string> LoadErrors = new List<string>();

        public void Load()
        {
            ContactLocations.Clear();
            DeliveryPoints.Clear();
            LoadErrors.Clear();

            string contactFolder = Path.Combine(Settings.DataFolder, "ContactMissions");
            string deliveryFolder = Path.Combine(Settings.DataFolder, "DeliveryPoints");
            Directory.CreateDirectory(contactFolder);
            Directory.CreateDirectory(deliveryFolder);

            foreach (string file in Directory.GetFiles(contactFolder, "*.xml", SearchOption.AllDirectories))
            {
                try
                {
                    HeistLocation loc = ParseContactMission(file);
                    if (loc != null) ContactLocations.Add(loc);
                }
                catch (Exception ex)
                {
                    LoadErrors.Add(Path.GetFileName(file));
                    Log.Error("loading " + file, ex);
                }
            }

            foreach (string file in Directory.GetFiles(deliveryFolder, "*.xml", SearchOption.AllDirectories))
            {
                try
                {
                    var doc = new XmlDocument();
                    doc.Load(file);
                    XmlNode root = doc.SelectSingleNode("DeliveryCoordinates");
                    if (root == null) continue;
                    foreach (XmlNode area in root.ChildNodes)
                    {
                        if (area.NodeType != XmlNodeType.Element) continue;
                        DeliveryPoints.Add(new Vector3(Util.XmlFloat(area, "PositionX"), Util.XmlFloat(area, "PositionY"), Util.XmlFloat(area, "PositionZ")));
                    }
                }
                catch (Exception ex)
                {
                    LoadErrors.Add(Path.GetFileName(file));
                    Log.Error("loading " + file, ex);
                }
            }
        }

        private static HeistLocation ParseContactMission(string file)
        {
            var doc = new XmlDocument();
            doc.Load(file);
            XmlNode node = doc.SelectSingleNode("HeistLocation");
            if (node == null) return null;

            var loc = new HeistLocation
            {
                FileName = Path.GetFileName(file),
                Name = Util.XmlText(node, "Name", Path.GetFileNameWithoutExtension(file)),
                // Note: no GetNextPositionOnStreet here anymore. At load time the road
                // data around far-away locations isn't loaded, so it snapped to wrong spots.
                Position = new Vector3(Util.XmlFloat(node, "PositionX"), Util.XmlFloat(node, "PositionY"), Util.XmlFloat(node, "PositionZ")),
                Radius = Util.XmlFloat(node, "Radius", 25f),
                Description = Util.XmlText(node, "Description"),
                Reward = (int)Util.XmlFloat(node, "Reward", 50000f)
            };

            XmlNode target = node.SelectSingleNode("TargetVehicle");
            if (target != null)
            {
                loc.VehicleModel = Util.XmlText(target, "TargetModelName", null);
                loc.VehiclePosition = new Vector3(Util.XmlFloat(target, "PositionX"), Util.XmlFloat(target, "PositionY"), Util.XmlFloat(target, "PositionZ"));
                loc.TargetRotation = Util.XmlFloat(target, "Rotation");

                XmlNode peds = target.SelectSingleNode("Peds");
                if (peds != null)
                    foreach (XmlNode p in peds.SelectNodes("Ped"))
                        if (!string.IsNullOrWhiteSpace(p.InnerText)) loc.TargetPedModels.Add(p.InnerText.Trim());
            }

            XmlNode area = node.SelectSingleNode("AreaVehicles");
            if (area != null)
            {
                foreach (XmlNode v in area.SelectNodes("Vehicle"))
                {
                    var av = new AreaVehicle
                    {
                        ModelName = Util.XmlText(v, "ModelName", null),
                        Position = new Vector3(Util.XmlFloat(v, "PositionX"), Util.XmlFloat(v, "PositionY"), Util.XmlFloat(v, "PositionZ")),
                        Rotation = Util.XmlFloat(v, "Rotation")
                    };
                    XmlNode drivers = v.SelectSingleNode("DriverModels");
                    if (drivers != null)
                        foreach (XmlNode p in drivers.SelectNodes("Ped"))
                            if (!string.IsNullOrWhiteSpace(p.InnerText)) av.PedModels.Add(p.InnerText.Trim());
                    if (!string.IsNullOrEmpty(av.ModelName)) loc.AreaVehicles.Add(av);
                }
            }

            // Bug fix: this used to be nested inside the AreaVehicles block,
            // so missions without area vehicles never got their ground troops.
            XmlNode troops = node.SelectSingleNode("GroundTroops");
            if (troops != null)
                foreach (XmlNode p in troops.SelectNodes("Ped"))
                    if (!string.IsNullOrWhiteSpace(p.InnerText)) loc.GroundTroops.Add(p.InnerText.Trim());

            if (string.IsNullOrEmpty(loc.VehicleModel)) return null;
            return loc;
        }
    }
}
