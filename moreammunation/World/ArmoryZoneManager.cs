using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace moreammunation
{
    /// <summary>
    /// Owns the Ammu-Nation + locations from the ini: map blips, the parked
    /// delivery truck, the guards, the "Press E / Q" prompts and respawns.
    ///
    /// Trucks and guards now only exist while you're nearby (~220 m) and are
    /// removed when you leave (~320 m). The old version spawned everything for
    /// every zone at startup, which cost performance and let entities fall
    /// through the unloaded map far away.
    /// </summary>
    internal sealed class ArmoryZoneManager
    {
        private const float StreamIn = 220f;
        private const float StreamOut = 320f;

        private sealed class ZoneState
        {
            public ArmoryZone Zone;
            public Blip MapBlip;
            public bool Spawned;
            public Vehicle Truck;
            public int TruckRespawnAt;
            public readonly List<Ped> Guards = new List<Ped>();
            public readonly List<int> GuardRespawnTimes = new List<int>();
        }

        private readonly Settings settings;
        private readonly MissionManager missions;
        private readonly MissionData data;
        private readonly ShopMenu shop;
        private readonly List<ZoneState> states = new List<ZoneState>();
        private readonly List<ArmoryZone> zones = new List<ArmoryZone>();
        private int nextSlowUpdate;

        private ZoneState nearZone;
        private bool nearTruck;

        public ArmoryZoneManager(Settings settings, MissionManager missions, MissionData data, ShopMenu shop)
        {
            this.settings = settings;
            this.missions = missions;
            this.data = data;
            this.shop = shop;
            LoadZones();
        }

        public IReadOnlyList<ArmoryZone> Zones { get { return zones; } }

        public ArmoryZone NearestZone(Vector3 position)
        {
            return zones.OrderBy(z => z.Position.DistanceTo2D(position)).FirstOrDefault();
        }

        // ------------------------------------------------------------------ setup

        private void LoadZones()
        {
            ScriptSettings ini = settings.Ini;
            for (int i = 1; ; i++)
            {
                string section = "ArmoryZone" + i;
                float x = ini.GetValue(section, "LocationX", float.NaN);
                float y = ini.GetValue(section, "LocationY", float.NaN);
                float z = ini.GetValue(section, "LocationZ", float.NaN);
                if (float.IsNaN(x) || float.IsNaN(y) || float.IsNaN(z)) break;

                zones.Add(new ArmoryZone
                {
                    Index = i,
                    Position = new Vector3(x, y, z),
                    BlipSprite = ini.GetValue(section, "BlipSprite", "Ammunation"),
                    BlipColor = ini.GetValue(section, "BlipColor", "BlueLight"),
                    BlipName = ini.GetValue(section, "BlipName", "Ammu-Nation + " + i),
                    SpawnVehicle = ini.GetValue(section, "DeliveryVehicle", true),
                    VehicleName = ini.GetValue(section, "VehicleName", "mule"),
                    VehicleHeading = ini.GetValue(section, "VehicleHeading", 0f),
                    SpawnNpc = ini.GetValue(section, "SpawnNpc", true),
                    NpcModel = ini.GetValue(section, "NpcName", "s_m_m_armoured_01"),
                    NpcNumber = Util.Clamp(ini.GetValue(section, "NpcNumber", 1), 0, 12),
                    HeistReward = ini.GetValue(section, "heistReward", 110000)
                });
            }
        }

        /// <summary>Called on the first tick (natives are safe to use there).</summary>
        public void Initialize()
        {
            foreach (ArmoryZone zone in zones)
            {
                Blip b = World.CreateBlip(zone.Position);
                b.Sprite = ParseSprite(zone.BlipSprite);
                b.Color = ParseColor(zone.BlipColor);
                b.Name = zone.BlipName;
                b.IsShortRange = true;
                states.Add(new ZoneState { Zone = zone, MapBlip = b });
            }
        }

        public void Cleanup()
        {
            foreach (ZoneState s in states)
            {
                Despawn(s);
                if (s.MapBlip != null && s.MapBlip.Exists()) s.MapBlip.Delete();
            }
            states.Clear();
        }

        // ------------------------------------------------------------------ per tick

        public void Update()
        {
            Ped player = Game.Player.Character;
            Vector3 pos = player.Position;

            if (Game.GameTime >= nextSlowUpdate)
            {
                nextSlowUpdate = Game.GameTime + 500;
                foreach (ZoneState s in states) SlowUpdate(s, pos);
            }

            // Which zone/truck is the player standing at?
            nearZone = null;
            nearTruck = false;
            foreach (ZoneState s in states)
            {
                if (s.Truck != null && s.Truck.Exists() && !s.Truck.IsDead && pos.DistanceTo(s.Truck.Position) < settings.ZoneRadius)
                {
                    nearZone = s;
                    nearTruck = true;
                    break;
                }
                if (pos.DistanceTo(s.Zone.Position) < settings.ZoneRadius)
                {
                    nearZone = s;
                    break;
                }
            }

            if (nearZone == null || !player.IsOnFoot || shop.IsOpen) return;

            string menuKey = KeyLabel(settings.MenuKey, "~INPUT_CONTEXT~");
            if (missions.IsActive && settings.BlockShopDuringMissions)
            {
                GTA.UI.Screen.ShowHelpTextThisFrame("~w~Ammu-Nation + is closed while you're on a job.");
            }
            else if (nearTruck && !missions.IsActive)
            {
                string robKey = KeyLabel(settings.RobKey, "~INPUT_CONTEXT_SECONDARY~");
                GTA.UI.Screen.ShowHelpTextThisFrame("Press " + menuKey + " to shop, or " + robKey + " to rob the truck (" +
                                             Util.Money(nearZone.Zone.HeistReward) + ").");
            }
            else
            {
                GTA.UI.Screen.ShowHelpTextThisFrame("Press " + menuKey + " to buy or customize weapons.");
            }
        }

        private void SlowUpdate(ZoneState s, Vector3 playerPos)
        {
            float d = playerPos.DistanceTo(s.Zone.Position);

            if (!s.Spawned && d < StreamIn) Spawn(s);
            else if (s.Spawned && d > StreamOut) Despawn(s);
            if (!s.Spawned) return;

            // Guards: remove dead ones, respawn after a delay
            for (int i = s.Guards.Count - 1; i >= 0; i--)
            {
                Ped g = s.Guards[i];
                if (g != null && g.Exists() && !g.IsDead) continue;
                if (g != null && g.Exists())
                {
                    Blip b = g.AttachedBlip;
                    if (b != null && b.Exists()) b.Delete();
                    g.MarkAsNoLongerNeeded();
                }
                s.Guards.RemoveAt(i);
                s.GuardRespawnTimes.Add(Game.GameTime + settings.GuardRespawnSeconds * 1000);
            }
            for (int i = s.GuardRespawnTimes.Count - 1; i >= 0; i--)
            {
                if (Game.GameTime < s.GuardRespawnTimes[i]) continue;
                // don't pop guards in right next to the player mid-fight
                if (playerPos.DistanceTo(s.Zone.Position) < 25f) continue;
                s.GuardRespawnTimes.RemoveAt(i);
                SpawnGuard(s);
            }

            // Truck: respawn after it's been robbed or destroyed
            if (!s.Zone.SpawnVehicle) return;
            bool truckOk = s.Truck != null && s.Truck.Exists() && !s.Truck.IsDead;
            if (truckOk) return;

            if (s.Truck != null && s.Truck.Exists()) s.Truck.MarkAsNoLongerNeeded();
            s.Truck = null;

            var robbery = missions.Active as TruckRobberyMission;
            if (robbery != null && robbery.Zone == s.Zone) return; // still being robbed

            if (s.TruckRespawnAt == 0) s.TruckRespawnAt = Game.GameTime + settings.TruckRespawnSeconds * 1000;
            if (Game.GameTime >= s.TruckRespawnAt)
            {
                s.TruckRespawnAt = 0;
                SpawnTruck(s);
            }
        }

        // ------------------------------------------------------------------ spawning

        private void Spawn(ZoneState s)
        {
            s.Spawned = true;
            s.TruckRespawnAt = 0;
            s.GuardRespawnTimes.Clear();

            if (s.Zone.SpawnVehicle) SpawnTruck(s);
            if (s.Zone.SpawnNpc)
                for (int i = 0; i < s.Zone.NpcNumber; i++) SpawnGuard(s);
        }

        private void Despawn(ZoneState s)
        {
            s.Spawned = false;
            var robbery = missions.Active as TruckRobberyMission;
            bool truckInUse = robbery != null && robbery.Zone == s.Zone;

            if (!truckInUse && s.Truck != null && s.Truck.Exists()) s.Truck.Delete();
            s.Truck = null;

            foreach (Ped g in s.Guards)
            {
                if (g == null || !g.Exists()) continue;
                Blip b = g.AttachedBlip;
                if (b != null && b.Exists()) b.Delete();
                g.Delete();
            }
            s.Guards.Clear();
            s.GuardRespawnTimes.Clear();
        }

        private void SpawnTruck(ZoneState s)
        {
            Vehicle v = Spawner.CreateVehicle(s.Zone.VehicleName, s.Zone.Position, s.Zone.VehicleHeading);
            if (v == null)
            {
                Notify.Shop("~r~Couldn't load vehicle '" + s.Zone.VehicleName + "' for " + s.Zone.BlipName + ".");
                return;
            }

            v.PlaceOnGround();
            v.LockStatus = VehicleLockStatus.PlayerCannotEnter;
            v.IsDriveable = false;
            v.AreLightsOn = true;
            v.AreBrakeLightsOn = true;

            // Open the back so it looks like a weapons stall
            if (Function.Call<bool>(Hash.GET_IS_DOOR_VALID, v.Handle, 5))
                Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, v.Handle, 5, false, true);
            else
            {
                Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, v.Handle, 2, false, true);
                Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, v.Handle, 3, false, true);
            }
            s.Truck = v;
        }

        private void SpawnGuard(ZoneState s)
        {
            Vector3 center = s.Truck != null && s.Truck.Exists() ? s.Truck.Position : s.Zone.Position;
            Ped g = Spawner.CreatePed(s.Zone.NpcModel, Spawner.OnFootPositionNear(center, 2f, 6f));
            if (g == null) return;

            g.RelationshipGroup = Groups.Guards;
            g.Weapons.Give(Spawner.RandomEnemyWeapon(), 250, true, true);
            g.CanSwitchWeapons = true;
            g.Accuracy = 50;
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, g.Handle, 46, true);
            Vector3 p = g.Position;
            Function.Call(Hash.TASK_WANDER_IN_AREA, g.Handle, p.X, p.Y, p.Z, 4f, 3f, 3f);
            s.Guards.Add(g);
        }

        // ------------------------------------------------------------------ player actions

        public void TryOpenShop()
        {
            if (nearZone == null || !Game.Player.Character.IsOnFoot) return;
            if (shop.IsOpen) { shop.Close(); return; }

            if (missions.IsActive && settings.BlockShopDuringMissions)
            {
                Notify.Shop("~w~The shop opens again once you finish your job.");
                return;
            }
            shop.Toggle();
        }

        public void TryRob()
        {
            if (nearZone == null || !nearTruck || shop.IsOpen || !Game.Player.Character.IsOnFoot) return;
            if (missions.IsActive)
            {
                Notify.Agent("~w~One job at a time. Finish what you're doing first.");
                return;
            }

            ZoneState s = nearZone;
            Vehicle truck = s.Truck;
            if (!missions.Start(new TruckRobberyMission(settings, data, s.Zone, truck))) return;

            // The truck belongs to the mission now; the zone will spawn a new one later
            s.Truck = null;
            s.TruckRespawnAt = Game.GameTime + settings.TruckRespawnSeconds * 1000;

            foreach (Ped g in s.Guards)
            {
                if (g == null || !g.Exists() || g.IsDead) continue;
                g.RelationshipGroup = Groups.Hostiles;
                Function.Call(Hash.TASK_COMBAT_PED, g.Handle, Game.Player.Character.Handle, 0, 16);
                if (g.AttachedBlip == null) Spawner.EnemyBlip(g);
            }
        }

        // ------------------------------------------------------------------ helpers

        private static string KeyLabel(Keys key, string icon)
        {
            if (key == Keys.E && icon == "~INPUT_CONTEXT~") return icon;
            if (key == Keys.Q && icon == "~INPUT_CONTEXT_SECONDARY~") return icon;
            return "~b~" + key + "~w~";
        }

        private static BlipSprite ParseSprite(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return BlipSprite.AmmuNation;
            int code;
            if (int.TryParse(text, out code)) return (BlipSprite)code;
            BlipSprite sprite;
            if (Enum.TryParse(text.Replace(" ", "").Replace("-", ""), true, out sprite)) return sprite;
            return BlipSprite.AmmuNation;
        }

        private static BlipColor ParseColor(string text)
        {
            BlipColor color;
            if (!string.IsNullOrWhiteSpace(text) && Enum.TryParse(text.Trim(), true, out color)) return color;
            return BlipColor.BlueLight;
        }
    }
}
