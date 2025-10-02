using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using static System.Runtime.CompilerServices.RuntimeHelpers;


namespace moreammunation
{
    public class WeaponUI
    {
        public NativeItem BuyItem;
        public NativeItem SellItem;
        public NativeItem EquipItem;
        public NativeItem BuyAmmoItem;
        public NativeItem BuyFullAmmoItem;
        public NativeMenu WeaponMenu;
    }
    public class ArmoryZone
    {
        public Vector3 Position { get; set; }
        public string BlipSprite { get; set; }
        public string BlipColor { get; set; }
        public string BlipName { get; set; }
        public bool SpawnVehicle { get; set; }
        public string VehicleName { get; set; }

        public int HeistReward;
        public bool SpawnNpc { get; set; }
        public string NpcModel { get; set; }

        public int NpcNumber { get; set; }

    }

    public class Main : Script
    {
        //Dictionary For Weapons
        public Dictionary<WeaponHash, int> WeaponValues = new Dictionary<WeaponHash, int>
        {
            // Melee 
            { WeaponHash.Knife, 400 },
            { WeaponHash.Nightstick, 400 },
            { WeaponHash.Hammer, 500 },
            { WeaponHash.Bat, 120 },
            { WeaponHash.GolfClub, 110 },
            { WeaponHash.Crowbar, 130 },
            { WeaponHash.Bottle, 50 },
            { WeaponHash.SwitchBlade, 150 },
            { WeaponHash.BattleAxe, 300 },
            { WeaponHash.PoolCue, 75 },
            { WeaponHash.Wrench, 85 },
            { WeaponHash.StoneHatchet, 250 },
            { WeaponHash.CandyCane, 10 },
            { WeaponHash.KnuckleDuster, 180 },
            { WeaponHash.Machete, 200 },
            { WeaponHash.Dagger, 175 },
            { WeaponHash.Hatchet, 160 },

            // Handguns
            { WeaponHash.Pistol, 500 },
            { WeaponHash.PistolMk2, 1000 },
            { WeaponHash.CombatPistol, 600 },
            { WeaponHash.APPistol, 1500 },
            { WeaponHash.Pistol50, 1500 },
            { WeaponHash.FlareGun, 300 },
            { WeaponHash.MarksmanPistol, 4350 },
            { WeaponHash.Revolver, 1000 },
            { WeaponHash.RevolverMk2, 1200 },
            { WeaponHash.DoubleActionRevolver, 1100 },
            { WeaponHash.UpNAtomizer, 399000 },
            { WeaponHash.CeramicPistol, 700 },
            { WeaponHash.NavyRevolver, 1300 },
            { WeaponHash.PericoPistol, 1500 },
            { WeaponHash.WM29Pistol, 850 },
            { WeaponHash.HeavyPistol, 700 },
            { WeaponHash.SNSPistol, 400 },
            { WeaponHash.SNSPistolMk2, 1000 },
            { WeaponHash.VintagePistol, 450 },
            { WeaponHash.MachinePistol, 850 },

            // SMGs
            { WeaponHash.MicroSMG, 1000 },
            { WeaponHash.SMG, 1200 },
            { WeaponHash.SMGMk2, 1500 },
            { WeaponHash.AssaultSMG, 1800 },
            { WeaponHash.CombatPDW, 1600 },
            { WeaponHash.MiniSMG, 1100 },
            { WeaponHash.TacticalSMG, 1200 },

            // Rifles
            { WeaponHash.AssaultRifle, 8550 },
            { WeaponHash.AssaultrifleMk2, 9875 },
            { WeaponHash.CarbineRifle, 13000 },
            { WeaponHash.CarbineRifleMk2, 14000 },
            { WeaponHash.CompactRifle, 14650 },
            { WeaponHash.MilitaryRifle, 15000 },
            { WeaponHash.ServiceCarbine, 370000 },
            { WeaponHash.BattleRifle, 15000 },
            { WeaponHash.AdvancedRifle, 14250 },
            { WeaponHash.BullpupRifle, 14000 },
            { WeaponHash.BullpupRifleMk2, 14500 },
            { WeaponHash.SpecialCarbine, 14000 },
            { WeaponHash.SpecialCarbineMk2, 14500 },
            { WeaponHash.HeavyRifle, 15000 },
    
            // Machine Guns
            { WeaponHash.MG, 13500 },
            { WeaponHash.CombatMG, 14750 },
            { WeaponHash.CombatMGMk2, 15500 },
            { WeaponHash.Gusenberg, 14000 },
            { WeaponHash.UnholyHellbringer, 449000 },

            // Shotguns
            { WeaponHash.PumpShotgun, 550 },
            { WeaponHash.PumpShotgunMk2, 1000 },
            { WeaponHash.SawnOffShotgun, 300 },
            { WeaponHash.AssaultShotgun, 1500 },
            { WeaponHash.BullpupShotgun, 1250 },
            { WeaponHash.DoubleBarrelShotgun, 1450 },
            { WeaponHash.SweeperShotgun, 1700 },
            { WeaponHash.CombatShotgun, 2950 },
            { WeaponHash.HeavyShotgun, 13550 },

            // Snipers
            { WeaponHash.SniperRifle, 5000 },
            { WeaponHash.HeavySniper, 9500 },
            { WeaponHash.HeavySniperMk2, 9875 },
            { WeaponHash.MarksmanRifle, 15750 },
            { WeaponHash.MarksmanRifleMk2, 16000 },
            { WeaponHash.PrecisionRifle, 10000 },

            // Heavy Weapons
            { WeaponHash.RPG, 26250 },
            { WeaponHash.GrenadeLauncher, 32400 },
            { WeaponHash.CompactGrenadeLauncher, 45000 },
            { WeaponHash.Minigun, 50000 },
            { WeaponHash.Firework, 65000 },
            { WeaponHash.HomingLauncher, 75000 },
            { WeaponHash.Widowmaker, 499000 },
            { WeaponHash.Railgun, 730000 },

            // Throwables
            { WeaponHash.Grenade, 250 },
            { WeaponHash.StickyBomb, 600 },
            { WeaponHash.SmokeGrenade, 200 },
            { WeaponHash.Molotov, 200 },
            { WeaponHash.PipeBomb, 500 },
            { WeaponHash.Snowball, 1 },
            { WeaponHash.Flare, 50 },
            { WeaponHash.FireExtinguisher, 50 },
            { WeaponHash.PetrolCan, 50 },
            { WeaponHash.HazardousJerryCan, 50 },
            { WeaponHash.FertilizerCan, 50 },
            { WeaponHash.AcidPackage, 50 }
        };
        public Dictionary<WeaponHash, int> MeleeValues = new Dictionary<WeaponHash, int>
        {
            // Melee
            { WeaponHash.Knife, 400 },
            { WeaponHash.Nightstick, 400 },
            { WeaponHash.Hammer, 500 },
            { WeaponHash.Bat, 120 },
            { WeaponHash.GolfClub, 110 },
            { WeaponHash.Crowbar, 130 },
            { WeaponHash.Bottle, 50 },
            { WeaponHash.SwitchBlade, 150 },
            { WeaponHash.BattleAxe, 300 },
            { WeaponHash.PoolCue, 75 },
            { WeaponHash.Wrench, 85 },
            { WeaponHash.StoneHatchet, 250 },
            { WeaponHash.CandyCane, 10 },
            { WeaponHash.KnuckleDuster, 180 },
            { WeaponHash.Machete, 200 },
            { WeaponHash.Dagger, 175 },
            { WeaponHash.Hatchet, 160 },


        };
        public Dictionary<WeaponHash, int> HandgunsValues = new Dictionary<WeaponHash, int>
        {
            // Handguns
            { WeaponHash.Pistol, 500 },
            { WeaponHash.PistolMk2, 1000 },
            { WeaponHash.CombatPistol, 600 },
            { WeaponHash.APPistol, 1500 },
            { WeaponHash.Pistol50, 1500 },
            { WeaponHash.FlareGun, 300 },
            { WeaponHash.MarksmanPistol, 4350 },
            { WeaponHash.Revolver, 1000 },
            { WeaponHash.RevolverMk2, 1200 },
            { WeaponHash.DoubleActionRevolver, 1100 },
            { WeaponHash.UpNAtomizer, 399000 },
            { WeaponHash.CeramicPistol, 700 },
            { WeaponHash.NavyRevolver, 1300 },
            { WeaponHash.PericoPistol, 1500 },
            { WeaponHash.WM29Pistol, 850 },
            { WeaponHash.HeavyPistol, 700 },
            { WeaponHash.SNSPistol, 400 },
            { WeaponHash.SNSPistolMk2, 1000 },
            { WeaponHash.VintagePistol, 450 },
            { WeaponHash.MachinePistol, 850 },

        };
        public Dictionary<WeaponHash, int> SMGsValues = new Dictionary<WeaponHash, int>
        {

            // SMGs
            { WeaponHash.MicroSMG, 1000 },
            { WeaponHash.SMG, 1200 },
            { WeaponHash.SMGMk2, 1500 },
            { WeaponHash.AssaultSMG, 1800 },
            { WeaponHash.CombatPDW, 1600 },
            { WeaponHash.MiniSMG, 1100 },
            { WeaponHash.TacticalSMG, 1200 },

        };
        public Dictionary<WeaponHash, int> RiflesValues = new Dictionary<WeaponHash, int>
        {

            // Rifles
            { WeaponHash.AssaultRifle, 8550 },
            { WeaponHash.AssaultrifleMk2, 9875 },
            { WeaponHash.CarbineRifle, 13000 },
            { WeaponHash.CarbineRifleMk2, 14000 },
            { WeaponHash.CompactRifle, 14650 },
            { WeaponHash.MilitaryRifle, 15000 },
            { WeaponHash.ServiceCarbine, 370000 },
            { WeaponHash.BattleRifle, 15000 },
            { WeaponHash.AdvancedRifle, 14250 },
            { WeaponHash.BullpupRifle, 14000 },
            { WeaponHash.BullpupRifleMk2, 14500 },
            { WeaponHash.SpecialCarbine, 14000 },
            { WeaponHash.SpecialCarbineMk2, 14500 },
            { WeaponHash.HeavyRifle, 15000 },
        };
        public Dictionary<WeaponHash, int> MachineGunsValues = new Dictionary<WeaponHash, int>
        {
            // Machine Guns
            { WeaponHash.MG, 13500 },
            { WeaponHash.CombatMG, 14750 },
            { WeaponHash.CombatMGMk2, 15500 },
            { WeaponHash.Gusenberg, 14000 },
            { WeaponHash.UnholyHellbringer, 449000 },

        };
        public Dictionary<WeaponHash, int> ShotgunsValues = new Dictionary<WeaponHash, int>
        {

            // Shotguns
            { WeaponHash.PumpShotgun, 550 },
            { WeaponHash.PumpShotgunMk2, 1000 },
            { WeaponHash.SawnOffShotgun, 300 },
            { WeaponHash.AssaultShotgun, 1500 },
            { WeaponHash.BullpupShotgun, 1250 },
            { WeaponHash.DoubleBarrelShotgun, 1450 },
            { WeaponHash.SweeperShotgun, 1700 },
            { WeaponHash.CombatShotgun, 2950 },
            { WeaponHash.HeavyShotgun, 13550 },
        };
        public Dictionary<WeaponHash, int> SnipersValues = new Dictionary<WeaponHash, int>
        {
            // Snipers
            { WeaponHash.SniperRifle, 5000 },
            { WeaponHash.HeavySniper, 9500 },
            { WeaponHash.HeavySniperMk2, 9875 },
            { WeaponHash.MarksmanRifle, 15750 },
            { WeaponHash.MarksmanRifleMk2, 16000 },
            { WeaponHash.PrecisionRifle, 10000 },
        };
        public Dictionary<WeaponHash, int> HeavyWeaponValues = new Dictionary<WeaponHash, int>
        {
            // Heavy Weapons
            { WeaponHash.RPG, 26250 },
            { WeaponHash.GrenadeLauncher, 32400 },
            { WeaponHash.CompactGrenadeLauncher, 45000 },
            { WeaponHash.Minigun, 50000 },
            { WeaponHash.Firework, 65000 },
            { WeaponHash.HomingLauncher, 75000 },
            { WeaponHash.Widowmaker, 499000 },
            { WeaponHash.Railgun, 730000 },

        };
        public Dictionary<WeaponHash, int> ThrowablesValues = new Dictionary<WeaponHash, int>
        {
            // Throwables
            { WeaponHash.Grenade, 250 },
            { WeaponHash.StickyBomb, 600 },
            { WeaponHash.SmokeGrenade, 200 },
            { WeaponHash.Molotov, 200 },
            { WeaponHash.PipeBomb, 500 },
            { WeaponHash.Snowball, 1 },
            { WeaponHash.Flare, 50 },
            { WeaponHash.FireExtinguisher, 50 },
            { WeaponHash.PetrolCan, 50 },
            { WeaponHash.HazardousJerryCan, 50 },
            { WeaponHash.FertilizerCan, 50 },
            { WeaponHash.AcidPackage, 50 }
        };

        // Dictionary to hold WeaponUI instances for each weapon
        Dictionary<WeaponHash, WeaponUI> weaponUIs = new Dictionary<WeaponHash, WeaponUI>();

        // Struct to manage vehicle respawn timing
        class VehicleRespawn
        {
            public ArmoryZone Zone { get; set; }
            public Vehicle Vehicle { get; set; } // the exact vehicle instance
            public DateTime RespawnTime { get; set; }
        }



        // LemonUI components
        private ObjectPool pool;
        private NativeMenu armoryMenu;
        private NativeMenu cHWeapon;
        private NativeMenu meleeSubMenu;
        private NativeMenu handgunsSubMenu;
        private NativeMenu smgSubMenu;
        private NativeMenu riflesSubMenu;
        private NativeMenu machinegunsSubMenu;
        private NativeMenu shotgunsSubMenu;
        private NativeMenu snipersSubMenu;
        private NativeMenu heavyweaponSubMenu;
        private NativeMenu throwableSubMenu;

        // Zone management
        private bool isNearArmoryZone = false;
        private List<ArmoryZone> armoryZones = new List<ArmoryZone>();
        private Dictionary<Vehicle, Blip> vehicleBlips = new Dictionary<Vehicle, Blip>();

        private List<Blip> staticZoneBlips = new List<Blip>();
        private List<VehicleRespawn> vehiclesToRespawn = new List<VehicleRespawn>();
        private float armoryZoneRadius = 6.0f;
        private bool zonesCreated = false;
        private Vehicle vehicle = null;

        //HEIST
        private Blip heistBlip = null;
        private bool cleanupDone = false;
        private Keys heistKey; // Key to start heist
        private Dictionary<ArmoryZone, bool> heistActive = new Dictionary<ArmoryZone, bool>();
        private int lastWantedLevel = 0;
        private Vector3 heistTarget = new Vector3(1746.0f, 3267.0f, 41.1f);
        private List<(Vehicle vehicle, DateTime deleteAt)> vehiclesToDelete = new List<(Vehicle, DateTime)>();
        private Dictionary<Vehicle, ArmoryZone> vehicleZoneMapping = new Dictionary<Vehicle, ArmoryZone>();
        private Vehicle activeHeistVehicle = null;
        private bool AnyHeistActive()
        {
            bool active = heistActive.Values.Any(h => h);

            // If no heist active but cooldown not finished, treat as active
            if (!active && (DateTime.Now - lastHeistEndTime) < heistCooldown)
                return true;

            return active;
        }
        private DateTime lastHeistEndTime = DateTime.MinValue;
        private readonly TimeSpan heistCooldown = TimeSpan.FromSeconds(11);
        private Random rnd = new Random();
        private List<Ped> spawnedPeds = new List<Ped>();

        //NPC STUFF


        // Config and key settings
        ScriptSettings config;
        Keys enable;
        private void CleanupExistingArmoryPeds()
        {
            foreach (var ped in spawnedPeds)
            {
                if (ped != null && ped.Exists())
                {
                    ped.Delete();
                }
            }
            spawnedPeds.Clear();
        }
        private void OnAborted(object sender, EventArgs e)
        {
            int vehicleCount = vehicleBlips.Count;
            int blipCount = vehicleBlips.Count; // same as vehicles, one blip per vehicle

            // Delete vehicle-linked blips and vehicles
            foreach (var pair in vehicleBlips)
            {
                Vehicle v = pair.Key;
                Blip b = pair.Value;

                if (b != null && b.Exists())
                    b.Delete();

                if (v != null && v.Exists())
                    v.Delete();
            }
            vehicleBlips.Clear();

            // If you have separate static blips (zones without vehicles), clean them too
            int staticBlipCount = staticZoneBlips.Count;
            foreach (var blip in staticZoneBlips)
            {
                if (blip != null && blip.Exists())
                    blip.Delete();
            }
            staticZoneBlips.Clear();

            // Remove the delivery blip
            if (heistBlip != null && heistBlip.Exists())
                heistBlip.Delete(); // Just in case a previous blip wasn't cleaned up

        }
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (!Game.Player.Character.IsOnFoot || !isNearArmoryZone)
                return;

            // Block actions if any heist is active OR cooldown is active
            if (AnyHeistActive())
            {
                if (e.KeyCode == enable)
                {

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~Armory is unavailable right now", true, false);
                }
                else if (e.KeyCode == heistKey)
                {
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~You cannot start another heist yet!", true, false);
                }
                return;
            }

            // Open armory menu
            if (e.KeyCode == enable)
            {
                armoryMenu.Visible = !armoryMenu.Visible;
            }

            // Start heist (wanted level)
            if (e.KeyCode == heistKey && vehicle != null)
            {
                Game.Player.WantedLevel = 3; // Safe, instant
                
                // Unlock the vehicle so the player can enter
                vehicle.LockStatus = VehicleLockStatus.Unlocked;
                vehicle.IsDriveable = true;
                Function.Call(Hash.TASK_ENTER_VEHICLE,
                    Game.Player.Character.Handle,
                    vehicle.Handle,
                    -1,   // Timeout (-1 = infinite)
                    -1,   // Seat index (-1 = driver)
                    2.0f, // Speed
                    1,    // Flag (1 = normal entry)
                    0     // p6 (unused)
                );
                Function.Call(Hash.SET_VEHICLE_DOOR_SHUT, vehicle.Handle, 5, false);

                // Determine the current armory zone
                if (!vehicleZoneMapping.TryGetValue(vehicle, out ArmoryZone currentZone))
                {
                    GTA.UI.Notification.Show("~r~Error: Could not determine vehicle zone!");
                    return;
                }

                activeHeistVehicle = vehicle;

                if (!heistActive.ContainsKey(currentZone) || !heistActive[currentZone])
                {
                    heistActive[currentZone] = true;

                    // Delete old blip if it exists
                    if (heistBlip != null && heistBlip.Exists())
                        heistBlip.Delete();

                    // Create new delivery blip
                    heistBlip = World.CreateBlip(heistTarget);
                    heistBlip.Sprite = BlipSprite.Standard;
                    heistBlip.Color = BlipColor.Yellow;
                    heistBlip.Name = "Delivery Point";
                    heistBlip.ShowRoute = true;

                    GTA.UI.Screen.ShowSubtitle("~g~ Heist started!~g~ ~w~ Deliver the weapons to the destination.", 5000);


                }
            }
        }
        private void OnTick(object sender, EventArgs e)
        {
            // Request texture (optional, keep as is)
            Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "moreammunation", true);
            Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "moreammunation");

            // Reset flag and nearest vehicle
            isNearArmoryZone = false;
            Vehicle nearestVehicle = null;

            // Create blips and vehicles if not done yet
            if (!zonesCreated)
            {
                CleanupExistingArmoryVehicles();
                CreatearmoryZoneBlips();
                zonesCreated = true;
            }


            // Check if the player is near any spawned armory vehicle (using dictionary keys)
            foreach (var vehicle in vehicleBlips.Keys)
            {
                if (vehicle != null && vehicle.Exists())
                {
                    float distance = Game.Player.Character.Position.DistanceTo(vehicle.Position);
                    if (distance < armoryZoneRadius)
                    {
                        isNearArmoryZone = true;
                        nearestVehicle = vehicle;
                        break; // stop at the first nearby vehicle
                    }
                }
            }


            // Show notification if near a vehicle
            if (isNearArmoryZone && nearestVehicle != null && Game.Player.Character.IsOnFoot)
            {
                // 🔒 Prevent help text if ANY heist is active
                if (!AnyHeistActive())
                {
                    // Buy/modify weapons help text
                    GTA.UI.Screen.ShowHelpText($"~w~Press ~b~{enable}~w~ to buy or modify weapons, or press ~b~{heistKey}~w~ to rob the vehicle.", 3000);
                }
            }


            // Logic for destroyed vehicles
            var toRemove = new List<Vehicle>();

            foreach (var pair in vehicleBlips)
            {
                Vehicle v = pair.Key;
                Blip b = pair.Value;

                if (!v.Exists() || v.IsDead)
                {
                    if (b.Exists()) b.Delete();

                    // Get the zone associated with this vehicle
                    if (vehicleZoneMapping.TryGetValue(v, out ArmoryZone zone))
                    {
                        vehiclesToRespawn.Add(new VehicleRespawn
                        {
                            Zone = zone,      // use the zone from the dictionary
                            Vehicle = v,      // use the vehicle being iterated
                            RespawnTime = DateTime.Now.AddSeconds(10)
                        });
                    }

                    toRemove.Add(v);
                }
            }

            for (int i = vehiclesToDelete.Count - 1; i >= 0; i--)
            {
                var (veh, deleteAt) = vehiclesToDelete[i];
                if (DateTime.Now >= deleteAt)
                {
                    if (veh != null && veh.Exists())
                        veh.Delete();

                    vehiclesToDelete.RemoveAt(i);
                }
            }

            // Remove destroyed vehicles from dictionary
            foreach (var v in toRemove)
            {
                vehicleBlips.Remove(v);
            }




            // Check for vehicles that need to respawn
            var respawnNow = vehiclesToRespawn.Where(v => DateTime.Now >= v.RespawnTime).ToList();

            foreach (var respawn in respawnNow)
            {
                // Delete the old vehicle safely
                if (respawn.Vehicle != null && respawn.Vehicle.Exists())
                    respawn.Vehicle.Delete();

                // Spawn a new vehicle for that zone
                Model vehicleModel = new Model(respawn.Zone.VehicleName);
                vehicleModel.Request(500);
                while (!vehicleModel.IsLoaded) Script.Yield();

                Vehicle newVehicle = World.CreateVehicle(vehicleModel, respawn.Zone.Position + new Vector3(2f, 2f, 0f));
                newVehicle.IsPersistent = true;
                newVehicle.AreLightsOn = true;
                newVehicle.AreBrakeLightsOn = true;
                newVehicle.LockStatus = VehicleLockStatus.PlayerCannotEnter;
                newVehicle.IsDriveable = false;
                // Decide which door to open
                int doorIndex = 5; // try trunk first

                // Check if the trunk exists
                if (!Function.Call<bool>(Hash.SET_VEHICLE_DOOR_OPEN, newVehicle.Handle, doorIndex))
                {
                    // If no trunk, use rear right door (index 2)
                    doorIndex = 2;

                    // If even door 2 doesn't exist, just use door 0 (front left) as a fallback
                    if (!Function.Call<bool>(Hash.SET_VEHICLE_DOOR_OPEN, newVehicle.Handle, doorIndex))
                    {
                        doorIndex = 0;
                    }
                }
                // Create blip
                int vehicleBlipHandle = Function.Call<int>(Hash.ADD_BLIP_FOR_ENTITY, newVehicle.Handle);
                Function.Call(Hash.SET_BLIP_SPRITE, vehicleBlipHandle, (int)GetBlipSprite(respawn.Zone.BlipSprite));
                Function.Call(Hash.SET_BLIP_COLOUR, vehicleBlipHandle, (int)GetBlipColor(respawn.Zone.BlipColor));
                Function.Call(Hash.BEGIN_TEXT_COMMAND_SET_BLIP_NAME, "STRING");
                Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, respawn.Zone.BlipName);
                Function.Call(Hash.END_TEXT_COMMAND_SET_BLIP_NAME, vehicleBlipHandle);

                Blip vehicleBlipObj = new Blip(vehicleBlipHandle);
                vehicleBlips.Add(newVehicle, vehicleBlipObj);
                vehicleZoneMapping[newVehicle] = respawn.Zone; // link the new vehicle to the zone

                vehiclesToRespawn.Remove(respawn);
            }



            //Logic For Armory Heist


            lastWantedLevel = Game.Player.WantedLevel;

            if (!cleanupDone)
            {
                // remove any leftover delivery blips
                foreach (Blip b in World.GetAllBlips())
                {
                    if (b.Exists() && b.Name == "Delivery Point")
                    {
                        b.Delete();
                    }
                }
                cleanupDone = true;
            }
            foreach (var pair in heistActive.ToList())
            {
                ArmoryZone zone = pair.Key;
                bool isActive = pair.Value;
                
                
                if (!isActive) continue; // skip inactive heists
                
                    
                Vehicle playerVehicle = activeHeistVehicle;

                // Check if vehicle exists and is not destroyed
                if (playerVehicle != null && playerVehicle.Exists() && !playerVehicle.IsDead)
                {
                    float distanceToTarget = playerVehicle.Position.DistanceTo(heistTarget);

                    if (distanceToTarget < 8.0f) // Heist success radius
                    {
                        activeHeistVehicle = null;
                        // Reward player
                        Game.Player.Money += zone.HeistReward;
                        GTA.UI.Screen.ShowSubtitle($"~g~Heist Completed! ~w~You earned ~w~~b~${zone.HeistReward:N0}~b~.");
                        Game.Player.WantedLevel = 0;

                        heistActive[zone] = false;
                        

                        // Make player leave vehicle
                        if (Game.Player.Character.IsInVehicle())
                            Game.Player.Character.Task.LeaveVehicle(playerVehicle, LeaveVehicleFlags.None);

                        // Stop and lock vehicle
                        playerVehicle.Speed = 0f;
                        playerVehicle.Velocity = Vector3.Zero;
                        playerVehicle.IsDriveable = false;
                        playerVehicle.LockStatus = VehicleLockStatus.PlayerCannotEnter;
                        playerVehicle.AreBrakeLightsOn = true;

                        // Mark heist as complete
                        heistActive[zone] = false;
                        lastHeistEndTime = DateTime.Now;
                        // Queue respawn
                        vehiclesToRespawn.Add(new VehicleRespawn
                        {
                            Zone = zone,
                            Vehicle = playerVehicle,
                            RespawnTime = DateTime.Now.AddSeconds(10)
                        });

                        // Queue deletion
                        vehiclesToDelete.Add((playerVehicle, DateTime.Now.AddSeconds(5)));

                        // Remove delivery blip
                        if (heistBlip != null && heistBlip.Exists())
                        {
                            heistBlip.Delete();
                            heistBlip = null;
                        }
                    }
                }
                else // Vehicle destroyed or null
                {
                    GTA.UI.Notification.Show("~r~Heist Failed! The Weapons Got Damaged.");
                    heistActive[zone] = false;
                    activeHeistVehicle = null;
                    if (heistBlip != null && heistBlip.Exists())
                    {
                        heistBlip.Delete();
                        heistBlip = null;
                    }
                }

                // Player dead failure
                if (Game.Player.IsDead)
                {
                    GTA.UI.Notification.Show("~r~Heist Failed! You Got Wasted.");
                    heistActive[zone] = false;
                    activeHeistVehicle = null;
                    if (heistBlip != null && heistBlip.Exists())
                    {
                        heistBlip.Delete();
                        heistBlip = null;
                    }
                }
            }

            // Process delayed vehicle deletions
            for (int i = vehiclesToDelete.Count - 1; i >= 0; i--)
            {
                var (veh, deleteAt) = vehiclesToDelete[i];
                if (DateTime.Now >= deleteAt)
                {
                    if (veh != null && veh.Exists())
                        veh.Delete();
                    vehiclesToDelete.RemoveAt(i);
                }
            }


            // Update the global reference to the nearest vehicle for OnKeyUp
            vehicle = nearestVehicle;

            // Process LemonUI menu pool
            pool.Process();
        }

        
        private void LoadarmoryZonePositions()
        {
            int zoneIndex = 1;
            while (true)
            {
                float x = config.GetValue<float>($"ArmoryZone{zoneIndex}", "LocationX", float.NaN);
                float y = config.GetValue<float>($"ArmoryZone{zoneIndex}", "LocationY", float.NaN);
                float z = config.GetValue<float>($"ArmoryZone{zoneIndex}", "LocationZ", float.NaN);
                if (float.IsNaN(x) || float.IsNaN(y) || float.IsNaN(z))
                    break;

                string blipSpriteString = config.GetValue<string>($"ArmoryZone{zoneIndex}", "BlipSprite", "ammunation");
                string blipColorString = config.GetValue<string>($"ArmoryZone{zoneIndex}", "BlipColor", "BlueLight");
                string blipName = config.GetValue<string>($"ArmoryZone{zoneIndex}", "BlipName", $"Armory {zoneIndex}");
                bool spawnVehicle = config.GetValue<bool>($"ArmoryZone{zoneIndex}", "DeliveryVehicle", true);
                string vehicleName = config.GetValue<string>($"ArmoryZone{zoneIndex}", "VehicleName", "mule");
                bool spawnNpc = config.GetValue<bool>($"ArmoryZone{zoneIndex}", "SpawnNpc", true);
                string npcName = config.GetValue<string>($"ArmoryZone{zoneIndex}", "NpcName", "s_m_m_armoured_01");
                int npcNumber = config.GetValue<int>($"ArmoryZone{zoneIndex}", "NpcNumber", 1);


                armoryZones.Add(new ArmoryZone()
                {
                    Position = new Vector3(x, y, z),
                    BlipSprite = blipSpriteString,
                    BlipColor = blipColorString,
                    BlipName = blipName,
                    SpawnVehicle = spawnVehicle,
                    VehicleName = vehicleName,
                    SpawnNpc = spawnNpc,
                    NpcModel = npcName,
                    NpcNumber = npcNumber,
                    HeistReward = config.GetValue<int>($"ArmoryZone{zoneIndex}", "heistReward", 110000),

                });

                zoneIndex++;
            }

            GTA.UI.Notification.Show(
                GTA.UI.NotificationIcon.Ammunation,         // portrait (Hao’s face)
                "Ammunation",                               // sender name (shows above the subject)
                "Ammunation +",                 // subject line
                $"~w~Loaded ~b~{armoryZones.Count} ~w~new Ammunations on the map.", // message body
                true,                                // fade in
                false                                // blinking
            );

        }
        private void CleanupExistingArmoryVehicles()
        {
            foreach (var vehicle in World.GetAllVehicles())
            {
                foreach (var zone in armoryZones)
                {
                    if (vehicle.Model.Hash == new Model(zone.VehicleName).Hash)
                    {
                        if (vehicle.Exists())
                            vehicle.Delete();
                    }
                }
            }

            vehicleBlips.Clear();
            vehiclesToRespawn.Clear();

            
        }

        private void CreatearmoryZoneBlips()
        {
            
            foreach (var zone in armoryZones)
            {

 
                if (zone.SpawnVehicle)
                {
                    // --- Spawn vehicle ---
                    Model vehicleModel = new Model(zone.VehicleName);
                    if (vehicleModel.IsInCdImage && vehicleModel.IsValid)
                    {
                        vehicleModel.Request(500);
                        while (!vehicleModel.IsLoaded) Script.Yield();

                        Vehicle vehicle = World.CreateVehicle(vehicleModel, zone.Position + new Vector3(2f, 2f, 0f));
                        vehicle.IsPersistent = true;
                        vehicle.AreLightsOn = true;
                        vehicle.AreBrakeLightsOn = true;
                        vehicle.LockStatus = VehicleLockStatus.PlayerCannotEnter;
                        vehicle.IsDriveable = false;

                        // Decide which door to open
                        int doorIndex = 5; // try trunk first

                        // Check if the trunk exists
                        if (!Function.Call<bool>(Hash.SET_VEHICLE_DOOR_OPEN, vehicle.Handle, doorIndex))
                        {
                            // If no trunk, use rear right door (index 2)
                            doorIndex = 2;

                            // If even door 2 doesn't exist, just use door 0 (front left) as a fallback
                            if (!Function.Call<bool>(Hash.SET_VEHICLE_DOOR_OPEN, vehicle.Handle, doorIndex))
                            {
                                doorIndex = 0;
                            }
                        }

                        // --- Create blip attached to vehicle ---
                        int vehicleBlipHandle = Function.Call<int>(Hash.ADD_BLIP_FOR_ENTITY, vehicle.Handle);
                        Function.Call(Hash.SET_BLIP_SPRITE, vehicleBlipHandle, (int)GetBlipSprite(zone.BlipSprite));
                        Function.Call(Hash.SET_BLIP_COLOUR, vehicleBlipHandle, (int)GetBlipColor(zone.BlipColor));
                        Function.Call(Hash.BEGIN_TEXT_COMMAND_SET_BLIP_NAME, "STRING");
                        Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, zone.BlipName);
                        Function.Call(Hash.END_TEXT_COMMAND_SET_BLIP_NAME, vehicleBlipHandle);

                        Blip vehicleBlipObj = new Blip(vehicleBlipHandle);
                        vehicleBlips.Add(vehicle, vehicleBlipObj);
                        vehicleZoneMapping[vehicle] = zone;


                    }

                    else
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~Failed to load {zone.VehicleName}", true, false);

                    }

                }
                
                if (zone.SpawnNpc)
                {
                    Model pedModel = new Model(zone.NpcModel);
                    pedModel.Request(500);
                    while (!pedModel.IsLoaded) Script.Yield();
                    Random rnd = new Random();

                     
                    for (int i = 0; i < zone.NpcNumber; i++)
                    {
                        float offsetX = (float)(rnd.NextDouble() * 10.0 - 5.0);
                        float offsetY = (float)(rnd.NextDouble() * 10.0 - 5.0);

                        Vector3 basePosition = vehicle != null ? vehicle.Position : zone.Position;

                        // Get actual ground height at this X/Y
                        float groundZ = World.GetGroundHeight(basePosition);

                        Vector3 npcPosition = new Vector3(
                            basePosition.X + offsetX,
                            basePosition.Y + offsetY,
                            groundZ
                        );

                        Ped npc = World.CreatePed(pedModel, npcPosition);
                        npc.IsPersistent = true;
                        spawnedPeds.Add(npc);


                        var allWeapons = new List<WeaponHash>();
                        allWeapons.AddRange(HandgunsValues.Keys);
                        allWeapons.AddRange(SMGsValues.Keys);
                        allWeapons.AddRange(RiflesValues.Keys);
                        allWeapons.AddRange(MachineGunsValues.Keys);
                        allWeapons.AddRange(ShotgunsValues.Keys);

                        // Pick a random weapon from the combined list

                        WeaponHash randomWeapon = allWeapons[rnd.Next(allWeapons.Count)];

                        int ammo = 100; // default ammo
                        if (HandgunsValues.ContainsKey(randomWeapon))
                            ammo = HandgunsValues[randomWeapon] / 10;
                        else if (SMGsValues.ContainsKey(randomWeapon))
                            ammo = SMGsValues[randomWeapon] / 10;
                        else if (RiflesValues.ContainsKey(randomWeapon))
                            ammo = RiflesValues[randomWeapon] / 10;
                        else if (MachineGunsValues.ContainsKey(randomWeapon))
                            ammo = MachineGunsValues[randomWeapon] / 10;
                        else if (ShotgunsValues.ContainsKey(randomWeapon))
                            ammo = ShotgunsValues[randomWeapon] / 10;

                        // Weapons
                        npc.Weapons.Give(randomWeapon, ammo, true, true);
                        npc.CanSwitchWeapons = true;

                        // Relationship & AI setup
                        npc.RelationshipGroup = RelationshipGroupHash.Army;
                        Function.Call(Hash.SET_PED_AS_ENEMY, npc.Handle, true);
                        Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, npc.Handle, 46, true); // Use vehicles
                        Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, npc.Handle, 5, true);  // Aggressive
                        Function.Call(Hash.SET_PED_ACCURACY, npc.Handle, 50);

                        // Guard or wander near the vehicle

                        Function.Call(Hash.TASK_WANDER_IN_AREA, npc.Handle, npcPosition.X, npcPosition.Y, npcPosition.Z, 2.0f, 1.0f, 1.0f);
                    }

                    
                }



                else
                {
                    // --- Create static zone blip ---
                    Blip zoneBlip = World.CreateBlip(zone.Position);
                    zoneBlip.Sprite = GetBlipSprite(zone.BlipSprite);
                    zoneBlip.Color = GetBlipColor(zone.BlipColor);
                    zoneBlip.Name = zone.BlipName;
                    staticZoneBlips.Add(zoneBlip);
                }
            }
        }

        public Main()
        {

            Tick += OnTick;
            KeyUp += OnKeyUp;
            Aborted += OnAborted;

            // Load settings
            config = ScriptSettings.Load("scripts\\moreammunation.ini");
            string enableKeyString = config.GetValue<string>("Options", "Button", "Enter");
            if (!Enum.TryParse(enableKeyString, out enable))
            {
                enable = Keys.Enter;
                GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"Failed to parse key, using default ~b~'Enter'", true, false);
            }

            string heistKeyString = config.GetValue<string>("Options", "HeistButton", "H"); // default H
            if (!Enum.TryParse(heistKeyString, out heistKey))
            {
                heistKey = Keys.H; // fallback
                GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"Failed to parse heist key, ~b~using default 'H'", true, false);
            }


            LoadarmoryZonePositions(); // Only loads coordinates

            



            // Initialize LemonUI components

            pool = new ObjectPool();
            armoryMenu = new NativeMenu(
                "",
                "CATEGORIES",
                "Categories",
                new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                )
            );

            pool.Add(armoryMenu);
            Dictionary<WeaponComponentHash, string> chcustomComponentNames = new Dictionary<WeaponComponentHash, string>
                {
                    {(WeaponComponentHash)4007263587u, "Knuckle Varmod Ballas"},
                    {(WeaponComponentHash)4081463091u, "Knuckle Varmod Base"},
                    {(WeaponComponentHash)2539772380u, "Knuckle Varmod Diamond"},
                    {(WeaponComponentHash)1351683121u, "Knuckle Varmod Dollar"},
                    {(WeaponComponentHash)2112683568u, "Knuckle Varmod Hate"},
                    {(WeaponComponentHash)3800804335u, "Knuckle Varmod King"},
                    {(WeaponComponentHash)1062111910u, "Knuckle Varmod Love"},
                    {(WeaponComponentHash)3323197061u, "Knuckle Varmod Pimp"},
                    {(WeaponComponentHash)146278587u,  "Knuckle Varmod Player"},
                    {(WeaponComponentHash)2062808965u, "Knuckle Varmod Vagos"},
                    {(WeaponComponentHash)2436343040u, "Switchblade Varmod Base"},
                    {(WeaponComponentHash)1530822070u, "Switchblade Varmod Var1"},
                    {(WeaponComponentHash)3885209186u, "Switchblade Varmod Var2"},
                    {(WeaponComponentHash)3372082259u, "Green Version"},
                    {(WeaponComponentHash)3014965697u, "Orange Version"}
                };
            List<WeaponComponentHash> chcustomComponentHashes = new List<WeaponComponentHash>
                {
                    (WeaponComponentHash)4007263587u, // KnuckleVarmodBallas
                    (WeaponComponentHash)4081463091u, // KnuckleVarmodBase
                    (WeaponComponentHash)2539772380u, // KnuckleVarmodDiamond
                    (WeaponComponentHash)1351683121u, // KnuckleVarmodDollar
                    (WeaponComponentHash)2112683568u, // KnuckleVarmodHate
                    (WeaponComponentHash)3800804335u, // KnuckleVarmodKing
                    (WeaponComponentHash)1062111910u, // KnuckleVarmodLove
                    (WeaponComponentHash)3323197061u, // KnuckleVarmodPimp
                    (WeaponComponentHash)146278587u,  // KnuckleVarmodPlayer
                    (WeaponComponentHash)2062808965u, // KnuckleVarmodVagos
                    (WeaponComponentHash)2436343040u, // SwitchbladeVarmodBase
                    (WeaponComponentHash)1530822070u, // SwitchbladeVarmodVar1
                    (WeaponComponentHash)3885209186u, // SwitchbladeVarmodVar2
                    (WeaponComponentHash)3372082259u, // Example switchblade ID you gave
                    (WeaponComponentHash)3014965697u  // Another ID you gave


                };
            armoryMenu.Alignment = Alignment.Right;
            // Currently Held Weapon logic
            cHWeapon = new NativeMenu(
                "",
                "Currently Held Weapon",
                "Customize or sell the weapon you’re currently holding.",
                new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                )
            );
            pool.Add(cHWeapon);
            cHWeapon.Alignment = Alignment.Right;
            var cHWeaponItem = armoryMenu.AddSubMenu(cHWeapon);
            cHWeapon.Shown += (sender, args) =>
            {
                cHWeapon.Clear();

                WeaponHash heldWeaponHash = Game.Player.Character.Weapons.Current.Hash;

                if (!WeaponValues.ContainsKey(heldWeaponHash))
                {


                    GTA.UI.Notification.Show(
                        GTA.UI.NotificationIcon.Ammunation,         // portrait 
                        "Ammunation",                               // sender name (shows above the subject)
                        "Important",                 // subject line
                        $"~r~You are not holding a supported weapon.", // message body
                        true,                                // fade in
                        false                                // blinking
                    );
                    return;
                }

                int price = WeaponValues[heldWeaponHash];
                string name = heldWeaponHash.ToString(); // Optional: use friendly name
                var weapons = Game.Player.Character.Weapons;

                // Buy Ammo

                var buyFullAmmoItem = new NativeItem("Buy All Ammo", "Buy ammunition for this weapon.");
                var buyAmmoItem = new NativeItem("Buy Ammo", "Buy ammunition for this weapon.");

                // Buy 50 Rounds Logic
                buyAmmoItem.Activated += (s, a) =>
                {
                    var cuweapons = Game.Player.Character.Weapons;
                    var weapon = weapons[heldWeaponHash];

                    if (!weapons.HasWeapon(heldWeaponHash))
                    {
                        GTA.UI.Notification.Show(
                            GTA.UI.NotificationIcon.Ammunation,         // portrait 
                            "Ammunation",                               // sender name (shows above the subject)
                            "Important",                 // subject line
                            $"~w~You don't own the ~b~{name}. ~w~Buy the weapon first.", // message body
                            true,                                // fade in
                            false                                // blinking
                        );
                        return;

                    }

                    int ammoPrice = 70;
                    int ammoAmount = 50;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show(
                            GTA.UI.NotificationIcon.Ammunation,         // portrait 
                            "Ammunation",                               // sender name (shows above the subject)
                            "Important",                 // subject line
                            $"~r~Not enough money! You need ~b~${ammoPrice}", // message body
                            true,                                // fade in
                            false                                // blinking
                        );

                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show(
                            GTA.UI.NotificationIcon.Ammunation,         // portrait 
                            "Ammunation",                               // sender name (shows above the subject)
                            "Important",                 // subject line
                            $"~w~You purchased ~b~{ammoAmount}~w~ rounds for ~r~-${ammoPrice}", // message body
                            true,                                // fade in
                            false                                // blinking
                        );
                   
                };
                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var cuweapons = Game.Player.Character.Weapons;
                    var weapon = weapons[heldWeaponHash];

                    if (!weapons.HasWeapon(heldWeaponHash))
                    {
                        GTA.UI.Notification.Show(
                            GTA.UI.NotificationIcon.Ammunation,         // portrait 
                            "Ammunation",                               // sender name (shows above the subject)
                            "Important",                 // subject line
                            $"~r~You don't own the ~b~{name}. ~w~Buy the weapon first.", // message body
                            true,                                // fade in
                            false                                // blinking
                        );
                        return;
                    }

                    int refillPrice = 5000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show(
                            GTA.UI.NotificationIcon.Ammunation,         // portrait 
                            "Ammunation",                               // sender name (shows above the subject)
                            "Important",                 // subject line
                            $"~r~Not enough money!~w~ You need ~b~${refillPrice}", // message body
                            true,                                // fade in
                            false                                // blinking
                        );
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;
                    GTA.UI.Notification.Show(
                            GTA.UI.NotificationIcon.Ammunation,         // portrait 
                            "Ammunation",                               // sender name (shows above the subject)
                            "Important",                 // subject line
                            $"~w~Fully refilled ammo for ~r~${refillPrice}", // message body
                            true,                                // fade in
                            false                                // blinking
                        );
                };

                // SELL
                var sellItem = new NativeItem("Sell", $"Resell for ${price}");
                sellItem.Activated += (s1, a1) =>
                {
                    if (!weapons.HasWeapon(heldWeaponHash))
                    {
                        GTA.UI.Notification.Show(
                            GTA.UI.NotificationIcon.Ammunation,         // portrait 
                            "Ammunation",                               // sender name (shows above the subject)
                            "Important",                 // subject line
                            $"~w~You don't own the ~b~{name}.", // message body
                            true,                                // fade in
                            false                                // blinking
                        );
                        return;
                    }

                    weapons.Remove(heldWeaponHash);
                    Game.Player.Money += price;

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~Sold~b~ {name} for ~g~${price}", true, false);   
                    sellItem.Enabled = false;
                    sellItem.Description = "~c~You no longer own this weapon";

                };

                // CUSTOMIZE
                var customizeMenu = new NativeMenu(
                    "",
                    $"{name} Customization",
                    $"Customize your {name}",
                    new ScaledTexture(
                        PointF.Empty,
                        new SizeF(431, 107),
                        "thumbnail_ammunation_net",
                        "ammunation_banner"
                    )
                );
                pool.Add(customizeMenu);
                customizeMenu.Alignment = Alignment.Right;
                // START HERE

                customizeMenu.Shown += (s, e) =>
                {
                    if (!weapons.HasWeapon(heldWeaponHash))
                    {
                        customizeMenu.Visible = false;
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                };
                var excludedComponents = new HashSet<WeaponComponentHash>
                    {
                        WeaponComponentHash.GunrunMk2Upgrade,
                        
                        
                        
                        
                        // Pistol Mk2
                        WeaponComponentHash.PistolMk2ClipHollowPoint,
                        WeaponComponentHash.PistolMk2ClipIncendiary,
                        WeaponComponentHash.PistolMk2ClipTracer,
                        WeaponComponentHash.PistolMk2ClipFMJ,
                        WeaponComponentHash.GunrunMk2Upgrade,
                        
                        // Pump Shotgun Mk2
                        WeaponComponentHash.PumpShotgunMk2ClipExplosive,
                        WeaponComponentHash.PumpShotgunMk2ClipHollowPoint,
                        WeaponComponentHash.PumpShotgunMk2ClipIncendiary,
                        WeaponComponentHash.PumpShotgunMk2ClipArmorPiercing,

                        // Revolver Mk2
                        WeaponComponentHash.RevolverMk2ClipFMJ,
                        WeaponComponentHash.RevolverMk2ClipHollowPoint,
                        WeaponComponentHash.RevolverMk2ClipIncendiary,
                        WeaponComponentHash.RevolverMk2ClipTracer,

                        // SMG Mk2
                        WeaponComponentHash.SMGMk2ClipFMJ,
                        WeaponComponentHash.SMGMk2ClipHollowPoint,
                        WeaponComponentHash.SMGMk2ClipIncendiary,
                        WeaponComponentHash.SMGMk2ClipTracer,

                        // SNS Pistol Mk2
                        WeaponComponentHash.SNSPistolMk2ClipFMJ,
                        WeaponComponentHash.SNSPistolMk2ClipHollowPoint,
                        WeaponComponentHash.SNSPistolMk2ClipIncendiary,
                        WeaponComponentHash.SNSPistolMk2ClipTracer,

                        // Assault Rifle Mk2
                        WeaponComponentHash.AssaultRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.AssaultRifleMk2ClipFMJ,
                        WeaponComponentHash.AssaultRifleMk2ClipIncendiary,
                        WeaponComponentHash.AssaultRifleMk2ClipTracer,

                        // Bullpup Rifle Mk2
                        WeaponComponentHash.BullpupRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.BullpupRifleMk2ClipFMJ,
                        WeaponComponentHash.BullpupRifleMk2ClipIncendiary,
                        WeaponComponentHash.BullpupRifleMk2ClipTracer,

                        // Carbine Rifle Mk2
                        WeaponComponentHash.CarbineRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.CarbineRifleMk2ClipFMJ,
                        WeaponComponentHash.CarbineRifleMk2ClipIncendiary,
                        WeaponComponentHash.CarbineRifleMk2ClipTracer,

                        // Combat MG Mk2
                        WeaponComponentHash.CombatMGMk2ClipArmorPiercing,
                        WeaponComponentHash.CombatMGMk2ClipFMJ,
                        WeaponComponentHash.CombatMGMk2ClipIncendiary,
                        WeaponComponentHash.CombatMGMk2ClipTracer,

                        // Marksman Rifle Mk2
                        WeaponComponentHash.MarksmanRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.MarksmanRifleMk2ClipFMJ,
                        WeaponComponentHash.MarksmanRifleMk2ClipIncendiary,
                        WeaponComponentHash.MarksmanRifleMk2ClipTracer,

                        // HeavySniper Rifle Mk2
                        WeaponComponentHash.HeavySniperMk2ClipArmorPiercing,
                        WeaponComponentHash.HeavySniperMk2ClipFMJ,
                        WeaponComponentHash.HeavySniperMk2ClipIncendiary,
                        WeaponComponentHash.HeavySniperMk2ClipExplosive
                    };

                // Check if it's a Mk2 weapon (simple detection based on name)
                bool isMk2 = name.Contains("Mk2");

                // Mk2 Tint Names
                string[] mk2Tints =
                {
                    "Classic Black", "Classic Gray", "Classic Two-Tone", "Classic White", "Classic Beige", "Classic Green", "Classic Blue", "Classic Earth",
                    "Classic Brown & Black", "Red Contrast", "Blue Contrast", "Yellow Contrast", "Orange Contrast", "Bold Pink", "Bold Purple & Yellow",
                    "Bold Orange", "Bold Green & Purple", "Bold Red Features", "Bold Green Features", "Bold Cyan Features", "Bold Yellow Features",
                    "Bold Red & White", "Bold Blue & White", "Metallic Gold", "Metallic Platinum", "Metallic Gray & Lilac", "Metallic Purple & Lime",
                    "Metallic Red", "Metallic Green", "Metallic Blue", "Metallic White & Aqua", "Metallic Orange & Yellow", "Metallic Red and Yellow"
                };

                // Regular tint names for non-Mk2 weapons
                string[] standardTints =
                {
                    "Default / Black", "Green", "Gold", "Pink", "Army", "LSPD", "Orange", "Platinum"
                };

                // Choose the right list
                string[] tintOptions = isMk2 ? mk2Tints : standardTints;


                //FINISH HERE
                var openCompMenuItem = new NativeItem("Manage Attachments");

                openCompMenuItem.Activated += (sender1, args1) =>
                {
                    var ped = Game.Player.Character;
                    var weapon = ped.Weapons.Current;
                    var wHash = weapon.Hash;

                    customizeMenu.Clear();
                    bool hasAny = false;

                    var clipCheckboxes = new List<NativeCheckboxItem>();
                    var otherComponents = new List<NativeCheckboxItem>();

                    List<WeaponComponent> allComponents = new List<WeaponComponent>();
                    for (int i = 0; i < weapon.Components.Count; i++)
                    {
                        var comp = weapon.Components[i];
                        allComponents.Add(comp);
                    }

                    List<string> camos = new List<string>();
                    List<string> camoSlides = new List<string>();

                    for (int i = 0; i < allComponents.Count; i++)
                    {
                        var component = allComponents[i];

                        string compName = component.ComponentHash.ToString()
                            .Replace("WeaponComponentHash.", "")
                            .Replace('_', ' ')
                            .ToLower();

                        if (excludedComponents.Contains(component.ComponentHash))
                            continue;

                        if (compName.Contains("camo") && compName.EndsWith("slide"))
                        {
                            camoSlides.Add(compName);
                        }
                        else if (compName.Contains("camo"))
                        {
                            camos.Add(compName);
                        }
                        else if (compName.Contains("clip"))
                        {
                            hasAny = true;
                            var clipItem = new NativeCheckboxItem(compName, component.Active);

                            clipItem.Activated += (s, e) =>
                            {
                                for (int j = 0; j < allComponents.Count; j++)
                                {
                                    var otherComp = allComponents[j];
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ')
                                        .ToLower();

                                    if (otherName.Contains("clip"))
                                        otherComp.Active = false;
                                }

                                component.Active = true;

                                foreach (var checkbox in clipCheckboxes)
                                    checkbox.Checked = (checkbox == clipItem);
                            };

                            clipCheckboxes.Add(clipItem);
                        }
                        else if (compName.Contains("muzzle") || compName.Contains("barrel") || compName.Contains("supp") || compName.Contains("comp") || compName.Contains("scope") || compName.Contains("sight"))
                        {
                            hasAny = true;
                            var item = new NativeCheckboxItem(compName, component.Active);

                            item.Activated += (s, e) =>
                            {
                                for (int j = 0; j < allComponents.Count; j++)
                                {
                                    var otherComp = allComponents[j];
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ')
                                        .ToLower();

                                    if ((compName.Contains("muzzle") && otherName.Contains("muzzle")) ||
                                        (compName.Contains("barrel") && otherName.Contains("barrel")) ||
                                        ((compName.Contains("supp") || compName.Contains("comp")) &&
                                         (otherName.Contains("supp") || otherName.Contains("comp"))) ||
                                        ((compName.Contains("scope") || compName.Contains("sight")) &&
                                         (otherName.Contains("scope") || otherName.Contains("sight"))))
                                    {
                                        otherComp.Active = false;
                                    }
                                }

                                component.Active = true;

                                foreach (var checkbox in otherComponents)
                                {
                                    if (checkbox.Title.ToLower().Contains("muzzle") == compName.Contains("muzzle") ||
                                        checkbox.Title.ToLower().Contains("barrel") == compName.Contains("barrel") ||
                                        (checkbox.Title.ToLower().Contains("supp") || checkbox.Title.ToLower().Contains("comp")) ==
                                        (compName.Contains("supp") || compName.Contains("comp")) ||
                                        (checkbox.Title.ToLower().Contains("scope") || checkbox.Title.ToLower().Contains("sight")) ==
                                        (compName.Contains("scope") || compName.Contains("sight")))
                                    {
                                        checkbox.Checked = (checkbox == item);
                                    }
                                }
                            };

                            otherComponents.Add(item);
                        }
                        else
                        {
                            hasAny = true;
                            var item = new NativeCheckboxItem(compName, component.Active);
                            item.Activated += (s, e) => component.Active = !component.Active;
                            otherComponents.Add(item);
                        }
                    }

                    // Add clips
                    for (int i = 0; i < clipCheckboxes.Count; i++)
                        customizeMenu.Add(clipCheckboxes[i]);

                    // Add other parts
                    for (int i = 0; i < otherComponents.Count; i++)
                        customizeMenu.Add(otherComponents[i]);

                    // No customizations
                    if (!hasAny && clipCheckboxes.Count == 0)
                    {
                        var noneItem = new NativeCheckboxItem("~c~No available customizations");
                        noneItem.Enabled = false;
                        customizeMenu.Add(noneItem);
                    }

                    // Weapon tint
                    int currentTintIndex = Function.Call<int>(GTA.Native.Hash.GET_PED_WEAPON_TINT_INDEX, ped, (uint)wHash);

                    if (currentTintIndex >= 0 && tintOptions.Length > 1)
                    {
                        var tintListItem = new NativeListItem<string>("Tint", tintOptions)
                        {
                            SelectedIndex = currentTintIndex
                        };

                        tintListItem.Activated += (senderTint, argsTint) =>
                        {
                            Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, ped, (uint)wHash, tintListItem.SelectedIndex);
                        };

                        customizeMenu.Add(tintListItem);
                    }

                    // You can re-add camo/camoSlide logic here with simple loops too if needed
                };


                customizeMenu.Add(openCompMenuItem);


                cHWeapon.Add(buyFullAmmoItem);
                cHWeapon.Add(buyAmmoItem);
                cHWeapon.Add(sellItem);
                cHWeapon.AddSubMenu(customizeMenu);


            };



            var removeAllItem = new NativeItem("Sell All Weapons");
            var giveAllItem = new NativeItem("Buy All Weapons");
            // Badge creator
            ScaledTexture CreateBadge() => new ScaledTexture(
                PointF.Empty,
                new SizeF(45, 45),
                "commonmenu",
                "shop_gunclub_icon_a"
            );
            // Function to update which item has the badge
            void UpdateBadges(NativeItem selected, NativeItem other)
            {
                selected.RightBadge = CreateBadge();  // Show on selected
                other.RightBadge = null;             // Hide on other
            }


            // Logic to check if player owns all weapons 
            bool HasAllWeapons()
            {
                foreach (var kv in WeaponValues)
                {
                    if (!Game.Player.Character.Weapons.HasWeapon(kv.Key))
                        return false;
                }
                return true;
            }
            bool HasNoWeapons()
            {
                foreach (var kv in WeaponValues)
                {
                    if (Game.Player.Character.Weapons.HasWeapon(kv.Key))
                        return false;
                }
                return true;
            }
            // SELL ALL logic
            removeAllItem.Activated += (sender, args) =>
            {
                if (HasNoWeapons())
                {

                    removeAllItem.Enabled = false;
                    return;
                }
                UpdateBadges(removeAllItem, giveAllItem);
                RemoveWeapons();
                if (HasNoWeapons())
                {
                    removeAllItem.Enabled = false;
                }
                giveAllItem.Enabled = true;
            };
            giveAllItem.Activated += (sender, args) =>
            {
                if (HasAllWeapons())
                {

                    giveAllItem.Enabled = false;

                    return;
                }
                UpdateBadges(giveAllItem, removeAllItem);

                BuyAllWeapons();

                // Disable Buy All if everything is now owned
                if (HasAllWeapons())
                {
                    giveAllItem.Enabled = false;

                }
                // Re-enable Sell All
                removeAllItem.Enabled = true;
            };
            void RefreshBuySellItems()
            {
                bool hasAll = HasAllWeapons();
                bool hasNone = HasNoWeapons();
                giveAllItem.RightBadge = null;
                removeAllItem.RightBadge = null;
                // Update Buy All
                giveAllItem.Enabled = !hasAll;
                // Update Sell All
                removeAllItem.Enabled = !hasNone;
            }
            // Update badges based on current state
            armoryMenu.Shown += (sender, args) =>
            {
                RefreshBuySellItems();
            };
            armoryMenu.Add(removeAllItem);
            armoryMenu.Add(giveAllItem);
            RefreshBuySellItems();


            //MELEE SECTION
            // Dictionary for melees
            Dictionary<WeaponHash, NativeItem> meleeItems = new Dictionary<WeaponHash, NativeItem>();
            Dictionary<WeaponHash, NativeItem> meleeSellItems = new Dictionary<WeaponHash, NativeItem>();
            Dictionary<WeaponHash, NativeItem> meleeEquipItems = new Dictionary<WeaponHash, NativeItem>();
            Dictionary<WeaponHash, NativeItem> meleeAttachments = new Dictionary<WeaponHash, NativeItem>();
            Dictionary<WeaponComponentHash, string> customComponentNames = new Dictionary<WeaponComponentHash, string>
                {
                    {(WeaponComponentHash)4007263587u, "Knuckle Varmod Ballas"},
                    {(WeaponComponentHash)4081463091u, "Knuckle Varmod Base"},
                    {(WeaponComponentHash)2539772380u, "Knuckle Varmod Diamond"},
                    {(WeaponComponentHash)1351683121u, "Knuckle Varmod Dollar"},
                    {(WeaponComponentHash)2112683568u, "Knuckle Varmod Hate"},
                    {(WeaponComponentHash)3800804335u, "Knuckle Varmod King"},
                    {(WeaponComponentHash)1062111910u, "Knuckle Varmod Love"},
                    {(WeaponComponentHash)3323197061u, "Knuckle Varmod Pimp"},
                    {(WeaponComponentHash)146278587u,  "Knuckle Varmod Player"},
                    {(WeaponComponentHash)2062808965u, "Knuckle Varmod Vagos"},
                    {(WeaponComponentHash)2436343040u, "Switchblade Varmod Base"},
                    {(WeaponComponentHash)1530822070u, "Switchblade Varmod Var1"},
                    {(WeaponComponentHash)3885209186u, "Switchblade Varmod Var2"},
                    {(WeaponComponentHash)3372082259u, "Green Version"},
                    {(WeaponComponentHash)3014965697u, "Orange Version"}
                };
            List<WeaponComponentHash> customComponentHashes = new List<WeaponComponentHash>
                {
                    (WeaponComponentHash)4007263587u, // KnuckleVarmodBallas
                    (WeaponComponentHash)4081463091u, // KnuckleVarmodBase
                    (WeaponComponentHash)2539772380u, // KnuckleVarmodDiamond
                    (WeaponComponentHash)1351683121u, // KnuckleVarmodDollar
                    (WeaponComponentHash)2112683568u, // KnuckleVarmodHate
                    (WeaponComponentHash)3800804335u, // KnuckleVarmodKing
                    (WeaponComponentHash)1062111910u, // KnuckleVarmodLove
                    (WeaponComponentHash)3323197061u, // KnuckleVarmodPimp
                    (WeaponComponentHash)146278587u,  // KnuckleVarmodPlayer
                    (WeaponComponentHash)2062808965u, // KnuckleVarmodVagos
                    (WeaponComponentHash)2436343040u, // SwitchbladeVarmodBase
                    (WeaponComponentHash)1530822070u, // SwitchbladeVarmodVar1
                    (WeaponComponentHash)3885209186u, // SwitchbladeVarmodVar2
                    (WeaponComponentHash)3372082259u, // Example switchblade ID you gave
                    (WeaponComponentHash)3014965697u  // Another ID you gave


                };
            // Create Melee submenu
            meleeSubMenu = new NativeMenu(
                "",
                "Melee",
                "Select a melee weapon to purchase.",
                new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                )
            );
            pool.Add(meleeSubMenu);
            meleeSubMenu.Alignment = Alignment.Right;
            var meleeSubMenuItem = armoryMenu.AddSubMenu(meleeSubMenu);
            foreach (var melee in MeleeValues)
            {
                WeaponHash hash = melee.Key;
                int price = melee.Value;
                string name = hash.ToString().Replace("WeaponHash.", "").Replace('_', ' ');
                var weaponMenu = new NativeMenu("", name, $"Manage your {name}",
                    new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                )
                );
                pool.Add(weaponMenu);
                weaponMenu.Alignment = Alignment.Right;
                var equipItem = new NativeItem("Equip", $"Equiped ${price}");
                var buyItem = new NativeItem("Buy", $"Price: ${price}");
                var sellItem = new NativeItem("Sell", $"Resell for ${price}");
                var openCompMenuItem = new NativeItem("Manage Attachments");
                // Store references
                meleeEquipItems[hash] = equipItem;
                meleeItems[hash] = buyItem;
                meleeSellItems[hash] = sellItem;
                meleeAttachments[hash] = openCompMenuItem;
                equipItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon
                    
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~Equipped~b~ {name}", true, false);

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You already have this weapon equipped";
                };
                var customizeMenu = new NativeMenu(
                    "",
                    $"{name} Customization",
                    $"Customize your {name}",
                    new ScaledTexture(
                        PointF.Empty,
                        new SizeF(431, 107),
                        "thumbnail_ammunation_net",
                        "ammunation_banner"
                    )
                );
                pool.Add(customizeMenu);
                customizeMenu.Alignment = Alignment.Right;
                customizeMenu.Shown += (s, e) =>
                {
                    if (!Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        customizeMenu.Visible = false;
                        weaponMenu.Visible = true;  // 👈 Show the weapon menu again
                        GTA.UI.Notification.Show(
                            GTA.UI.NotificationIcon.Ammunation,         // portrait 
                            "Ammunation",                               // sender name (shows above the subject)
                            "Important",                 // subject line
                            $"~r~You no longer own the ~b~{name}.", // message body
                            true,                                // fade in
                            false                                // blinking
                        );
                        
                    }
                };
                customizeMenu.Add(openCompMenuItem);
                openCompMenuItem.Activated += (sender1, args1) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(
                            GTA.UI.NotificationIcon.Ammunation,         // portrait 
                            "Ammunation",                               // sender name (shows above the subject)
                            "Important",                 // subject line
                            $"~r~You don't own the ~b~{name}.", // message body
                            true,                                // fade in
                            false                                // blinking
                        );

                        return;
                    }

                    var weapon = weapons[hash];

                    // ✅ Clear and rebuild the menu only when weapon is owned
                    customizeMenu.Clear();
                    customizeMenu.Items.Clear(); // Safe cleanup

                    var otherComponents = new List<NativeItem>();

                    foreach (var component in weapon.Components)
                    {
                        if (!customComponentHashes.Contains(component.ComponentHash))
                            continue;

                        string displayName = customComponentNames.TryGetValue(component.ComponentHash, out var nameo)
                            ? nameo
                            : component.ComponentHash.ToString();

                        var compItem = new NativeItem(displayName);

                        compItem.Activated += (sender2, args2) =>
                        {
                            component.Active = !component.Active;

                            foreach (var item in otherComponents)
                            {
                                WeaponComponent matchingComponent = null;

                                foreach (var comp in weapon.Components)
                                {
                                    if (customComponentNames.TryGetValue(comp.ComponentHash, out var compDisplayName))
                                    {
                                        if (compDisplayName == item.Title)
                                        {
                                            matchingComponent = comp;
                                            break;
                                        }
                                    }
                                }

                                item.RightBadge = (matchingComponent != null && matchingComponent.Active)
                                    ? CreateBadge()
                                    : null;
                            }
                        };

                        otherComponents.Add(compItem);
                    }


                    // Set initial badges
                    foreach (var item in otherComponents)
                    {
                        WeaponComponent matchingComponent = null;

                        foreach (var comp in weapon.Components)
                        {
                            if (customComponentNames.TryGetValue(comp.ComponentHash, out var compDisplayName))
                            {
                                if (compDisplayName == item.Title)
                                {
                                    matchingComponent = comp;
                                    break;
                                }
                            }
                        }

                        item.RightBadge = (matchingComponent != null && matchingComponent.Active)
                            ? CreateBadge()
                            : null;
                    }

                    foreach (var item in otherComponents)
                        customizeMenu.Add(item);

                    customizeMenu.Visible = true;
                };
                buyItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You already own the ~b~{name}.", true, false);   
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money!~w~ You need ~b~${price}", true, false);
                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~You purchased ~g~{name}", true, false);


                    equipItem.Enabled = false;

                    buyItem.Enabled = false;
                    buyItem.Description = "~c~You already own this weapon";

                    sellItem.Enabled = true;
                    sellItem.Description = $"Resell for ${price}";

                    openCompMenuItem.Enabled = true;
                    openCompMenuItem.Description = "Customize this weapon";
                };
                sellItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Sold {name} for ~g~${price}", true, false);



                    buyItem.Enabled = true;
                    buyItem.Description = $"Price: ${price}";

                    sellItem.Enabled = false;
                    sellItem.Description = "~c~You do not own this weapon";

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You do not own this weapon";

                    openCompMenuItem.Enabled = false;
                    openCompMenuItem.Description = "~c~You must own this weapon to customize it";
                };
                weaponMenu.Add(equipItem);
                weaponMenu.Add(buyItem);
                weaponMenu.Add(sellItem);
                weaponMenu.AddSubMenu(customizeMenu);
                meleeSubMenu.AddSubMenu(weaponMenu);
            }
            // Refresh buy/sell button states each time the menu is shown
            meleeSubMenu.Shown += (sender, args) =>
            {
                var weapons = Game.Player.Character.Weapons;
                foreach (var pair in meleeItems)
                {
                    WeaponHash hash = pair.Key;
                    var buyItem = pair.Value;
                    var sellItem = meleeSellItems[hash];
                    int price = MeleeValues[hash];
                    var openCompMenuItem = meleeAttachments[hash];
                    var equipItem = meleeEquipItems[hash];

                    if (weapons.HasWeapon(hash))
                    {
                        buyItem.Enabled = false;
                        buyItem.Description = "~c~You already own this weapon";

                        sellItem.Enabled = true;
                        sellItem.Description = $"Resell for ${price}";

                        equipItem.Enabled = true;
                        equipItem.Description = "Equip this weapon";

                        openCompMenuItem.Enabled = true;
                        openCompMenuItem.Description = "Customize this weapon";
                    }
                    else
                    {
                        buyItem.Enabled = true;
                        buyItem.Description = $"Price: ${price}";

                        sellItem.Enabled = false;
                        sellItem.Description = "~c~You do not own this weapon";

                        equipItem.Enabled = false;
                        equipItem.Description = "~c~You do not own this weapon";

                        openCompMenuItem.Enabled = false;
                        openCompMenuItem.Description = "~c~You must own this weapon to customize it";
                    }
                }
            };

            Dictionary<WeaponHash, NativeItem> handgunsItems = new Dictionary<WeaponHash, NativeItem>();
            // Dictionary to track sell buttons
            Dictionary<WeaponHash, NativeItem> handgunsSellItems = new Dictionary<WeaponHash, NativeItem>();

            Dictionary<WeaponHash, NativeItem> handgunsEquipItems = new Dictionary<WeaponHash, NativeItem>();





            handgunsSubMenu = new NativeMenu(
                "",
                "Handguns",
                "",
                new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                )
            );

            pool.Add(handgunsSubMenu);
            handgunsSubMenu.Alignment = Alignment.Right;
            var handgunsSubMenuItem = armoryMenu.AddSubMenu(handgunsSubMenu);

            foreach (var handgun in HandgunsValues)
            {
                WeaponHash hash = handgun.Key;
                int price = handgun.Value;
                string name = hash.ToString().Replace("WeaponHash.", "").Replace('_', ' ');

                var weaponMenu = new NativeMenu("", name, $"Manage your {name}",
                    new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                ));

                pool.Add(weaponMenu);
                weaponMenu.Alignment = Alignment.Right;
                var buyFullAmmoItem = new NativeItem("Buy All Ammo", "Buy ammunition for this weapon.");
                var buyAmmoItem = new NativeItem("Buy Ammo", "Buy ammunition for this weapon.");
                var equipItem = new NativeItem("Equip", $"Equiped ${price}");
                var buyItem = new NativeItem("Buy", $"Price: ${price}");
                var sellItem = new NativeItem("Sell", $"Resell for ${price}");

                // Store references
                handgunsEquipItems[hash] = equipItem;
                handgunsItems[hash] = buyItem;
                handgunsSellItems[hash] = sellItem;

                weaponUIs[hash] = new WeaponUI
                {
                    BuyItem = buyItem,
                    SellItem = sellItem,
                    EquipItem = equipItem,
                    BuyAmmoItem = buyAmmoItem,
                    BuyFullAmmoItem = buyFullAmmoItem,
                    WeaponMenu = weaponMenu
                };

                equipItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~Equipped~b~ {name}", true, false);

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You already have this weapon equiped";
                };

                // Buy Ammo logic
                // Buy 50 Rounds Logic
                buyAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~You don't own the {name}. ~w~Buy the weapon first.", true, false);   
                        return;
                    }
                    int ammoPrice = 70;
                    int ammoAmount = 50;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money!~w~ You need ~b~${ammoPrice}", true, false);
                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Purchased~b~ {ammoAmount} ~w~rounds for ~r~${ammoPrice}", true, false);
                };

                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~You don't own the {name}. ~w~Buy the weapon first.", true, false);   
                        return;
                    }
                    int refillPrice = 5000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money! You need ${refillPrice}", true, false);
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Fully refilled ammo for ~r~${refillPrice}", true, false);
                };


                // Buy weapon logic
                buyItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You already own the ~b~{name}.", true, false);   
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money!~w~ You need ~b~${price}", true, false);
                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~You purchased ~g~{name}", true, false);
                    buyItem.Enabled = false;
                    buyItem.Description = "~c~You already own this weapon";

                    sellItem.Enabled = true;
                    sellItem.Description = $"Resell for ${price}";

                    if (!weaponMenu.Items.Contains(buyAmmoItem))
                        weaponMenu.Items.Insert(0, buyAmmoItem);
                    if (!weaponMenu.Items.Contains(buyFullAmmoItem))
                        weaponMenu.Items.Insert(1, buyFullAmmoItem);

                };

                // Sell weapon logic
                sellItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Sold {name} for ~g~${price}", true, false);
                    buyItem.Enabled = true;
                    buyItem.Description = $"Price: ${price}";

                    sellItem.Enabled = false;
                    sellItem.Description = "~c~You do not own this weapon";

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You do not own this weapon";

                    // Remove Buy Ammo dynamically
                    if (weaponMenu.Items.Contains(buyAmmoItem))
                        weaponMenu.Items.Remove(buyAmmoItem);
                    if (weaponMenu.Items.Contains(buyFullAmmoItem))
                        weaponMenu.Items.Remove(buyFullAmmoItem);
                };





                var customizeMenu = new NativeMenu(
                    "",
                    $"{name} Customization",
                    $"Customize your {name}",
                    new ScaledTexture(
                        PointF.Empty,
                        new SizeF(431, 107),
                        "thumbnail_ammunation_net",
                        "ammunation_banner"
                    )
                );
                pool.Add(customizeMenu);
                customizeMenu.Alignment = Alignment.Right;
                customizeMenu.Shown += (s, e) =>
                {
                    if (!Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        customizeMenu.Visible = false;
                        weaponMenu.Visible = true;  // 👈 Show the weapon menu again
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Important", $"~r~You no longer own the {name}.", true, false);
                    }
                };


                var excludedComponents = new HashSet<WeaponComponentHash>
                    {
                        // Pistol Mk2
                        WeaponComponentHash.PistolMk2ClipHollowPoint,
                        WeaponComponentHash.PistolMk2ClipIncendiary,
                        WeaponComponentHash.PistolMk2ClipTracer,
                        WeaponComponentHash.PistolMk2ClipFMJ,
                        WeaponComponentHash.GunrunMk2Upgrade,

                        // Pump Shotgun Mk2
                        WeaponComponentHash.PumpShotgunMk2ClipExplosive,
                        WeaponComponentHash.PumpShotgunMk2ClipHollowPoint,
                        WeaponComponentHash.PumpShotgunMk2ClipIncendiary,
                        WeaponComponentHash.PumpShotgunMk2ClipArmorPiercing,

                        // Revolver Mk2
                        WeaponComponentHash.RevolverMk2ClipFMJ,
                        WeaponComponentHash.RevolverMk2ClipHollowPoint,
                        WeaponComponentHash.RevolverMk2ClipIncendiary,
                        WeaponComponentHash.RevolverMk2ClipTracer,

                        // SMG Mk2
                        WeaponComponentHash.SMGMk2ClipFMJ,
                        WeaponComponentHash.SMGMk2ClipHollowPoint,
                        WeaponComponentHash.SMGMk2ClipIncendiary,
                        WeaponComponentHash.SMGMk2ClipTracer,

                        // SNS Pistol Mk2
                        WeaponComponentHash.SNSPistolMk2ClipFMJ,
                        WeaponComponentHash.SNSPistolMk2ClipHollowPoint,
                        WeaponComponentHash.SNSPistolMk2ClipIncendiary,
                        WeaponComponentHash.SNSPistolMk2ClipTracer,

                        // Assault Rifle Mk2
                        WeaponComponentHash.AssaultRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.AssaultRifleMk2ClipFMJ,
                        WeaponComponentHash.AssaultRifleMk2ClipIncendiary,
                        WeaponComponentHash.AssaultRifleMk2ClipTracer,

                        // Bullpup Rifle Mk2
                        WeaponComponentHash.BullpupRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.BullpupRifleMk2ClipFMJ,
                        WeaponComponentHash.BullpupRifleMk2ClipIncendiary,
                        WeaponComponentHash.BullpupRifleMk2ClipTracer,

                        // Carbine Rifle Mk2
                        WeaponComponentHash.CarbineRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.CarbineRifleMk2ClipFMJ,
                        WeaponComponentHash.CarbineRifleMk2ClipIncendiary,
                        WeaponComponentHash.CarbineRifleMk2ClipTracer,

                        // Combat MG Mk2
                        WeaponComponentHash.CombatMGMk2ClipArmorPiercing,
                        WeaponComponentHash.CombatMGMk2ClipFMJ,
                        WeaponComponentHash.CombatMGMk2ClipIncendiary,
                        WeaponComponentHash.CombatMGMk2ClipTracer,

                        // Marksman Rifle Mk2
                        WeaponComponentHash.MarksmanRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.MarksmanRifleMk2ClipFMJ,
                        WeaponComponentHash.MarksmanRifleMk2ClipIncendiary,
                        WeaponComponentHash.MarksmanRifleMk2ClipTracer
                    };


                // Check if it's a Mk2 weapon (simple detection based on name)
                bool isMk2 = name.Contains("Mk2");

                // Mk2 Tint Names
                string[] mk2Tints =
                {
                    "Classic Black", "Classic Gray", "Classic Two-Tone", "Classic White", "Classic Beige", "Classic Green", "Classic Blue", "Classic Earth",
                    "Classic Brown & Black", "Red Contrast", "Blue Contrast", "Yellow Contrast", "Orange Contrast", "Bold Pink", "Bold Purple & Yellow",
                    "Bold Orange", "Bold Green & Purple", "Bold Red Features", "Bold Green Features", "Bold Cyan Features", "Bold Yellow Features",
                    "Bold Red & White", "Bold Blue & White", "Metallic Gold", "Metallic Platinum", "Metallic Gray & Lilac", "Metallic Purple & Lime",
                    "Metallic Red", "Metallic Green", "Metallic Blue", "Metallic White & Aqua", "Metallic Orange & Yellow", "Metallic Red and Yellow"
                };

                // Regular tint names for non-Mk2 weapons
                string[] standardTints =
                {
                    "Default / Black", "Green", "Gold", "Pink", "Army", "LSPD", "Orange", "Platinum"
                };

                // Choose the right list
                string[] tintOptions = isMk2 ? mk2Tints : standardTints;



                var checkTexture = new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "commonmenu",
                    "shop_gunclub_icon_a"
                );

                var openCompMenuItem = new NativeItem("Manage Attachments");

                openCompMenuItem.Activated += (sender1, args1) =>
                {
                    var weapon = Game.Player.Character.Weapons[hash];

                    if (!weapon.IsPresent)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You need to own the ~b~{name} to customize it.", true, false);   
                        return;
                    }

                    customizeMenu.Clear(); // Reset the customize menu

                    bool hasAny = false;



                    var camos = new List<string>();
                    var camoSlides = new List<string>();







                    var clipCheckboxes = new List<NativeCheckboxItem>();
                    var suppressorCheckboxes = new List<NativeCheckboxItem>();
                    var clipComponents = new List<WeaponComponent>();
                    var otherComponents = new List<NativeCheckboxItem>();

                    foreach (var component in weapon.Components)
                    {
                        if (excludedComponents.Contains(component.ComponentHash))
                            continue;

                        string compName = component.ComponentHash.ToString()
                            .Replace("WeaponComponentHash.", "")
                            .Replace('_', ' ');

                        if (compName.ToLower().Contains("camo") && compName.ToLower().EndsWith("slide"))
                        {
                            camoSlides.Add(compName);
                        }
                        else if (compName.ToLower().Contains("camo"))
                        {
                            camos.Add(compName);
                        }
                        else if (compName.ToLower().Contains("clip"))
                        {
                            hasAny = true;

                            var clipItem = new NativeCheckboxItem(compName, component.Active);

                            clipItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (otherName.ToLower().Contains("clip"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Update UI: ensure only this clip is checked
                                foreach (var clipCheckbox in clipCheckboxes)
                                {
                                    clipCheckbox.Checked = (clipCheckbox == clipItem && newState);
                                }
                            };

                            clipCheckboxes.Add(clipItem);
                        }



                        else if (compName.ToLower().Contains("supp") || compName.ToLower().Contains("comp"))
                        {
                            hasAny = true;

                            var suppressorItem = new NativeCheckboxItem(compName, component.Active);

                            suppressorItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ')
                                        .ToLower();

                                    if (otherName.Contains("supp") || otherName.Contains("comp"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Optional: Update UI to only check this one
                                foreach (var item in otherComponents)
                                {
                                    if (item.Title.ToLower().Contains("supp") || item.Title.ToLower().Contains("comp"))
                                        item.Checked = (item == suppressorItem && newState);
                                }
                            };

                            otherComponents.Add(suppressorItem);
                        }
                        else if (compName.ToLower().Contains("scope") || compName.ToLower().Contains("sight"))
                        {
                            hasAny = true;

                            var scopeItem = new NativeCheckboxItem(compName, component.Active);

                            scopeItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ')
                                        .ToLower();

                                    if (otherName.Contains("scope") || otherName.Contains("sight"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Optional: Update UI to only check this one
                                foreach (var item in otherComponents)
                                {
                                    if (item.Title.ToLower().Contains("scope") || item.Title.ToLower().Contains("sight"))
                                        item.Checked = (item == scopeItem && newState);
                                }
                            };

                            otherComponents.Add(scopeItem);
                        }
                        else
                        {
                            hasAny = true;

                            var compItem = new NativeCheckboxItem(compName, component.Active);
                            compItem.Activated += (sender2, args2) =>
                            {
                                component.Active = !component.Active;
                            };

                            otherComponents.Add(compItem);
                        }
                    }

                    // Add all clips
                    foreach (var clip in clipCheckboxes)
                        customizeMenu.Add(clip);

                    // Add other components
                    foreach (var item in otherComponents)
                        customizeMenu.Add(item);

                    if (!hasAny && clipCheckboxes.Count == 0)
                    {
                        var noneItem = new NativeCheckboxItem("~c~No available customizations");
                        noneItem.Enabled = false;
                        customizeMenu.Add(noneItem);
                    }


                    int currentTintIndex = Function.Call<int>(Hash.GET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (uint)hash);

                    // Only add the tint selector if the weapon supports tints
                    if (currentTintIndex >= 0 && tintOptions.Length > 1)
                    {
                        var tintListItem = new NativeListItem<string>("Tint", tintOptions)
                        {
                            SelectedIndex = currentTintIndex
                        };

                        tintListItem.Activated += (senderTint, argsTint) =>
                        {
                            int selectedIndex = tintListItem.SelectedIndex;
                            Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (uint)hash, selectedIndex);
                        };

                        customizeMenu.Add(tintListItem);
                    }
                    // Add Camos List
                    if (camos.Count > 0)
                    {
                        string[] liveryColors =
                        {
                            "Default", "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7"
                        };
                        hasAny = true;
                        // Create a new array with "None" as first element + existing camos
                        var camoOptions = new string[camos.Count + 1];
                        camoOptions[0] = "None";
                        camos.CopyTo(camoOptions, 1);
                        var camoListItem = new NativeListItem<string>("Camos 01", camoOptions);
                        var liveryColorItem = new NativeListItem<string>("Camo Color", liveryColors);
                        liveryColorItem.SelectedIndex = 0;
                        liveryColorItem.Activated += (sender, args) =>
                        {
                            var ped = Game.Player.Character;
                            var weapons = Game.Player.Character.Weapons[hash];

                            if (!weapon.IsPresent) return;

                            WeaponComponent activeCamo = null;

                            foreach (var comp in weapon.Components)
                            {
                                string compName = comp.ComponentHash.ToString().Replace("WeaponComponentHash.", "").Replace('_', ' ').ToLower();

                                // Only apply to active main camos (not slide)
                                if (compName.Contains("camo") && !compName.EndsWith("slide") && comp.Active)
                                {
                                    activeCamo = comp;
                                    break;
                                }
                            }

                            if (activeCamo != null)
                            {
                                int selectedColor = liveryColorItem.SelectedIndex;
                                Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, ped, (uint)hash, (uint)activeCamo.ComponentHash, selectedColor);
                            }
                        };

                        // Find which camo is active, else select "None"
                        camoListItem.SelectedIndex = 0; // Default to None
                        for (int i = 0; i < camos.Count; i++)
                        {
                            string camo = camos[i];
                            foreach (var component in weapon.Components)
                            {
                                string compName = component.ComponentHash.ToString()
                                    .Replace("WeaponComponentHash.", "")
                                    .Replace('_', ' ');

                                if (compName == camo && component.Active)
                                {
                                    camoListItem.SelectedIndex = i + 1; // +1 because of "None" at 0
                                    break;
                                }
                            }
                        }
                        camoListItem.Activated += (sender3, args3) =>
                        {
                            string selectedCamo = camoListItem.SelectedItem;

                            if (selectedCamo == "None")
                            {
                                // Deactivate all camos
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName.ToLower().Contains("camo") && !compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                            else
                            {
                                // Activate selected camo, deactivate others
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName == selectedCamo)
                                        component.Active = true;
                                    else if (compName.ToLower().Contains("camo") && !compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                        };
                        customizeMenu.Add(camoListItem);
                        customizeMenu.Add(liveryColorItem);
                    }
                    if (camoSlides.Count > 0)
                    {
                        hasAny = true;

                        // Insert "None" option at the start
                        var camoSlideOptions = new string[camoSlides.Count + 1];
                        camoSlideOptions[0] = "None";
                        camoSlides.CopyTo(camoSlideOptions, 1);
                        string[] liveryColors =
                        {
                            "Default", "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7"
                         };
                        var camoSlideListItem = new NativeListItem<string>("Camo 02", camoSlideOptions);



                        var liveryColorItem2 = new NativeListItem<string>("Camo Color 2", liveryColors);
                        liveryColorItem2.SelectedIndex = 0;
                        liveryColorItem2.Activated += (sender2, args2) =>
                        {
                            var ped = Game.Player.Character;
                            var weapons = Game.Player.Character.Weapons[hash];

                            if (!weapon.IsPresent) return;

                            WeaponComponent activeSlideCamo = null;

                            foreach (var comp in weapon.Components)
                            {
                                string compName = comp.ComponentHash.ToString().Replace("WeaponComponentHash.", "").Replace('_', ' ').ToLower();

                                // Only apply to active slide camos
                                if (compName.EndsWith("slide") && comp.Active)
                                {
                                    activeSlideCamo = comp;
                                    break;
                                }
                            }

                            if (activeSlideCamo != null)
                            {
                                int selectedColor = liveryColorItem2.SelectedIndex;
                                Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, ped, (uint)hash, (uint)activeSlideCamo.ComponentHash, selectedColor);
                            }
                        };



                        // Default select "None"
                        camoSlideListItem.SelectedIndex = 0;

                        for (int i = 0; i < camoSlides.Count; i++)
                        {
                            string camoSlide = camoSlides[i];
                            foreach (var component in weapon.Components)
                            {
                                string compName = component.ComponentHash.ToString()
                                    .Replace("WeaponComponentHash.", "")
                                    .Replace('_', ' ');

                                if (compName == camoSlide && component.Active)
                                {
                                    camoSlideListItem.SelectedIndex = i + 1; // +1 for None at 0
                                    break;
                                }
                            }
                        }

                        camoSlideListItem.Activated += (sender4, args4) =>
                        {
                            string selectedCamoSlide = camoSlideListItem.SelectedItem;

                            if (selectedCamoSlide == "None")
                            {
                                // Deactivate all camo slides
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                            else
                            {
                                // Activate selected slide, deactivate others
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName == selectedCamoSlide)
                                        component.Active = true;
                                    else if (compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                        };

                        customizeMenu.Add(camoSlideListItem);
                        customizeMenu.Add(liveryColorItem2);
                    }
                };

                customizeMenu.Add(openCompMenuItem);

                // Only add Buy and Sell now; BuyAmmo added dynamically
                weaponMenu.Add(equipItem);
                weaponMenu.Add(buyItem);
                weaponMenu.Add(sellItem);
                weaponMenu.AddSubMenu(customizeMenu);

                handgunsSubMenu.AddSubMenu(weaponMenu);
            }

            // Optional: remove the `Shown` refresh if handling it dynamically

            // Refresh buy/sell button states each time the menu is shown
            handgunsSubMenu.Shown += (sender, args) =>
            {
                var weapons = Game.Player.Character.Weapons;

                foreach (var pair in handgunsItems)
                {
                    WeaponHash hash = pair.Key;
                    var buyItem = pair.Value;
                    var sellItem = handgunsSellItems[hash];
                    int price = HandgunsValues[hash];

                    // Assuming you have a dictionary of equipItems for handguns:
                    var equipItem = handgunsEquipItems[hash]; // You need to have this collection set up.

                    if (weapons.HasWeapon(hash))
                    {
                        buyItem.Enabled = false;
                        buyItem.Description = "~c~You already own this weapon";

                        sellItem.Enabled = true;
                        sellItem.Description = $"Resell for ${price}";

                        equipItem.Enabled = true;       // Enable equip option

                        equipItem.Description = "Equip this weapon";
                    }
                    else
                    {
                        buyItem.Enabled = true;
                        buyItem.Description = $"Price: ${price}";

                        sellItem.Enabled = false;
                        sellItem.Description = "~c~You do not own this weapon";

                        equipItem.Enabled = false;     // Disable equip option
                        equipItem.Description = "~c~You do not own this weapon";
                    }
                }
            };








            //SMG





            Dictionary<WeaponHash, NativeItem> smgItems = new Dictionary<WeaponHash, NativeItem>();
            // Dictionary to track sell buttons
            Dictionary<WeaponHash, NativeItem> smgSellItems = new Dictionary<WeaponHash, NativeItem>();

            Dictionary<WeaponHash, NativeItem> smgEquipItems = new Dictionary<WeaponHash, NativeItem>();



            smgSubMenu = new NativeMenu(
            "",
            "SMG's",
            "",
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "thumbnail_ammunation_net",
                "ammunation_banner"
            )
            );
            pool.Add(smgSubMenu);
            smgSubMenu.Alignment = Alignment.Right;
            var smgSubMenuItem = armoryMenu.AddSubMenu(smgSubMenu);


            foreach (var handgun in SMGsValues)
            {
                WeaponHash hash = handgun.Key;
                int price = handgun.Value;
                string name = hash.ToString().Replace("WeaponHash.", "").Replace('_', ' ');

                var weaponMenu = new NativeMenu("", name, $"Manage your {name}",
                    new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                ));
                pool.Add(weaponMenu);
                weaponMenu.Alignment = Alignment.Right;
                var buyFullAmmoItem = new NativeItem("Buy All Ammo", "Buy ammunition for this weapon.");
                var buyAmmoItem = new NativeItem("Buy Ammo", "Buy ammunition for this weapon.");
                var equipItem = new NativeItem("Equip", $"Equiped ${price}");
                var buyItem = new NativeItem("Buy", $"Price: ${price}");
                var sellItem = new NativeItem("Sell", $"Resell for ${price}");

                // Store references
                smgEquipItems[hash] = equipItem;
                smgItems[hash] = buyItem;
                smgSellItems[hash] = sellItem;


                equipItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~Equipped~b~ {name}", true, false);

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You already have this weapon equiped";
                };

                // Buy Ammo logic
                // Buy 50 Rounds Logic
                buyAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~You don't own the {name}. ~w~Buy the weapon first.", true, false);   
                        return;
                    }

                    int ammoPrice = 70;
                    int ammoAmount = 50;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money!~w~ You need ~b~${ammoPrice}", true, false);
                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Purchased~b~ {ammoAmount} ~w~rounds for ~r~${ammoPrice}", true, false);
                };

                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~You don't own the {name}. ~w~Buy the weapon first.", true, false);   
                        return;
                    }

                    int refillPrice = 5000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money! You need ${refillPrice}", true, false);
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Fully refilled ammo for ~r~${refillPrice}", true, false);
                };

                // Buy weapon logic
                buyItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You already own the ~b~{name}.", true, false);   
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money!~w~ You need ~b~${price}", true, false);
                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~You purchased ~g~{name}", true, false);

                    buyItem.Enabled = false;
                    buyItem.Description = "~c~You already own this weapon";

                    sellItem.Enabled = true;
                    sellItem.Description = $"Resell for ${price}";

                    if (!weaponMenu.Items.Contains(buyAmmoItem))
                        weaponMenu.Items.Insert(0, buyAmmoItem);
                    if (!weaponMenu.Items.Contains(buyFullAmmoItem))
                        weaponMenu.Items.Insert(1, buyFullAmmoItem);

                };

                // Sell weapon logic
                sellItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Sold {name} for ~g~${price}", true, false);

                    buyItem.Enabled = true;
                    buyItem.Description = $"Price: ${price}";

                    sellItem.Enabled = false;
                    sellItem.Description = "~c~You do not own this weapon";

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You do not own this weapon";


                    if (weaponMenu.Items.Contains(buyAmmoItem))
                        weaponMenu.Items.Remove(buyAmmoItem);
                    if (weaponMenu.Items.Contains(buyFullAmmoItem))
                        weaponMenu.Items.Remove(buyFullAmmoItem);
                };

                var customizeMenu = new NativeMenu(
                    "",
                    $"{name} Customization",
                    $"Customize your {name}",
                    new ScaledTexture(
                        PointF.Empty,
                        new SizeF(431, 107),
                        "thumbnail_ammunation_net",
                        "ammunation_banner"
                    )
                );
                pool.Add(customizeMenu);
                customizeMenu.Alignment = Alignment.Right;
                customizeMenu.Shown += (s, e) =>
                {
                    if (!Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        customizeMenu.Visible = false;
                        weaponMenu.Visible = true;
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Important", $"~r~You no longer own the {name}.", true, false);
                    }
                };
                var excludedComponents = new HashSet<WeaponComponentHash>
                    {
                        WeaponComponentHash.GunrunMk2Upgrade,
                        // Pistol Mk2
                        WeaponComponentHash.PistolMk2ClipHollowPoint,
                        WeaponComponentHash.PistolMk2ClipIncendiary,
                        WeaponComponentHash.PistolMk2ClipTracer,
                        WeaponComponentHash.PistolMk2ClipFMJ,
                        WeaponComponentHash.GunrunMk2Upgrade,

                        
                        // Pump Shotgun Mk2
                        WeaponComponentHash.PumpShotgunMk2ClipExplosive,
                        WeaponComponentHash.PumpShotgunMk2ClipHollowPoint,
                        WeaponComponentHash.PumpShotgunMk2ClipIncendiary,
                        WeaponComponentHash.PumpShotgunMk2ClipArmorPiercing,

                        // Revolver Mk2
                        WeaponComponentHash.RevolverMk2ClipFMJ,
                        WeaponComponentHash.RevolverMk2ClipHollowPoint,
                        WeaponComponentHash.RevolverMk2ClipIncendiary,
                        WeaponComponentHash.RevolverMk2ClipTracer,

                        // SMG Mk2
                        WeaponComponentHash.SMGMk2ClipFMJ,
                        WeaponComponentHash.SMGMk2ClipHollowPoint,
                        WeaponComponentHash.SMGMk2ClipIncendiary,
                        WeaponComponentHash.SMGMk2ClipTracer,

                        // SNS Pistol Mk2
                        WeaponComponentHash.SNSPistolMk2ClipFMJ,
                        WeaponComponentHash.SNSPistolMk2ClipHollowPoint,
                        WeaponComponentHash.SNSPistolMk2ClipIncendiary,
                        WeaponComponentHash.SNSPistolMk2ClipTracer,

                        // Assault Rifle Mk2
                        WeaponComponentHash.AssaultRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.AssaultRifleMk2ClipFMJ,
                        WeaponComponentHash.AssaultRifleMk2ClipIncendiary,
                        WeaponComponentHash.AssaultRifleMk2ClipTracer,

                        // Bullpup Rifle Mk2
                        WeaponComponentHash.BullpupRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.BullpupRifleMk2ClipFMJ,
                        WeaponComponentHash.BullpupRifleMk2ClipIncendiary,
                        WeaponComponentHash.BullpupRifleMk2ClipTracer,

                        // Carbine Rifle Mk2
                        WeaponComponentHash.CarbineRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.CarbineRifleMk2ClipFMJ,
                        WeaponComponentHash.CarbineRifleMk2ClipIncendiary,
                        WeaponComponentHash.CarbineRifleMk2ClipTracer,

                        // Combat MG Mk2
                        WeaponComponentHash.CombatMGMk2ClipArmorPiercing,
                        WeaponComponentHash.CombatMGMk2ClipFMJ,
                        WeaponComponentHash.CombatMGMk2ClipIncendiary,
                        WeaponComponentHash.CombatMGMk2ClipTracer,

                        // Marksman Rifle Mk2
                        WeaponComponentHash.MarksmanRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.MarksmanRifleMk2ClipFMJ,
                        WeaponComponentHash.MarksmanRifleMk2ClipIncendiary,
                        WeaponComponentHash.MarksmanRifleMk2ClipTracer,

                        // HeavySniper Rifle Mk2
                        WeaponComponentHash.HeavySniperMk2ClipArmorPiercing,
                        WeaponComponentHash.HeavySniperMk2ClipFMJ,
                        WeaponComponentHash.HeavySniperMk2ClipIncendiary,
                        WeaponComponentHash.HeavySniperMk2ClipExplosive
                    };

                // Check if it's a Mk2 weapon (simple detection based on name)
                bool isMk2 = name.Contains("Mk2");

                // Mk2 Tint Names
                string[] mk2Tints =
                {
    "Classic Black", "Classic Gray", "Classic Two-Tone", "Classic White", "Classic Beige", "Classic Green", "Classic Blue", "Classic Earth",
    "Classic Brown & Black", "Red Contrast", "Blue Contrast", "Yellow Contrast", "Orange Contrast", "Bold Pink", "Bold Purple & Yellow",
    "Bold Orange", "Bold Green & Purple", "Bold Red Features", "Bold Green Features", "Bold Cyan Features", "Bold Yellow Features",
    "Bold Red & White", "Bold Blue & White", "Metallic Gold", "Metallic Platinum", "Metallic Gray & Lilac", "Metallic Purple & Lime",
    "Metallic Red", "Metallic Green", "Metallic Blue", "Metallic White & Aqua", "Metallic Orange & Yellow", "Metallic Red and Yellow"
};

                // Regular tint names for non-Mk2 weapons
                string[] standardTints =
                {
    "Default / Black", "Green", "Gold", "Pink", "Army", "LSPD", "Orange", "Platinum"
};

                // Choose the right list
                string[] tintOptions = isMk2 ? mk2Tints : standardTints;

                var openCompMenuItem = new NativeItem("Manage Attachments");

                openCompMenuItem.Activated += (sender1, args1) =>
                {
                    var weapon = Game.Player.Character.Weapons[hash];

                    if (!weapon.IsPresent)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You need to own the ~b~{name} to customize it.", true, false);   
                        return;
                    }

                    customizeMenu.Clear(); // Reset the customize menu

                    bool hasAny = false;



                    var camos = new List<string>();
                    var camoSlides = new List<string>();
                    var clipCheckboxes = new List<NativeCheckboxItem>();
                    var suppressorCheckboxes = new List<NativeCheckboxItem>();
                    var clipComponents = new List<WeaponComponent>();
                    var otherComponents = new List<NativeCheckboxItem>();

                    foreach (var component in weapon.Components)
                    {
                        if (excludedComponents.Contains(component.ComponentHash))
                            continue;

                        string compName = component.ComponentHash.ToString()
                            .Replace("WeaponComponentHash.", "")
                            .Replace('_', ' ');

                        if (compName.ToLower().Contains("camo") && compName.ToLower().EndsWith("slide"))
                        {
                            camoSlides.Add(compName);
                        }
                        else if (compName.ToLower().Contains("camo"))
                        {
                            camos.Add(compName);
                        }
                        else if (compName.ToLower().Contains("clip"))
                        {
                            hasAny = true;

                            var clipItem = new NativeCheckboxItem(compName, component.Active);

                            clipItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (otherName.ToLower().Contains("clip"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Update UI: ensure only this clip is checked
                                foreach (var clipCheckbox in clipCheckboxes)
                                {
                                    clipCheckbox.Checked = (clipCheckbox == clipItem && newState);
                                }
                            };

                            clipCheckboxes.Add(clipItem);
                        }
                        else if (compName.ToLower().Contains("muzzle"))
                        {
                            hasAny = true;

                            var muzzleItem = new NativeCheckboxItem(compName, component.Active);

                            muzzleItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (otherName.ToLower().Contains("muzzle"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Update UI: ensure only this muzzle is checked
                                foreach (var muzzleCheckbox in clipCheckboxes)
                                {
                                    muzzleCheckbox.Checked = (muzzleCheckbox == muzzleItem && newState);
                                }
                            };

                            clipCheckboxes.Add(muzzleItem);
                        }
                        else if (compName.ToLower().Contains("barrel"))
                        {
                            hasAny = true;

                            var barrelItem = new NativeCheckboxItem(compName, component.Active);

                            barrelItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (otherName.ToLower().Contains("barrel"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Update UI: ensure only this clip is checked
                                foreach (var barrelCheckbox in clipCheckboxes)
                                {
                                    barrelCheckbox.Checked = (barrelCheckbox == barrelItem && newState);
                                }
                            };

                            clipCheckboxes.Add(barrelItem);
                        }
                        else if (compName.ToLower().Contains("supp") || compName.ToLower().Contains("comp"))
                        {
                            hasAny = true;

                            var suppressorItem = new NativeCheckboxItem(compName, component.Active);

                            suppressorItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ')
                                        .ToLower();

                                    if (otherName.Contains("supp") || otherName.Contains("comp"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Optional: Update UI to only check this one
                                foreach (var item in otherComponents)
                                {
                                    if (item.Title.ToLower().Contains("supp") || item.Title.ToLower().Contains("comp"))
                                        item.Checked = (item == suppressorItem && newState);
                                }
                            };

                            otherComponents.Add(suppressorItem);
                        }
                        else if (compName.ToLower().Contains("scope") || compName.ToLower().Contains("sight"))
                        {
                            hasAny = true;

                            var suppressorItem = new NativeCheckboxItem(compName, component.Active);

                            suppressorItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ')
                                        .ToLower();

                                    if (otherName.Contains("scope") || otherName.Contains("sight"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Optional: Update UI to only check this one
                                foreach (var item in otherComponents)
                                {
                                    if (item.Title.ToLower().Contains("scope") || item.Title.ToLower().Contains("sight"))
                                        item.Checked = (item == suppressorItem && newState);
                                }
                            };

                            otherComponents.Add(suppressorItem);
                        }

                        else
                        {
                            hasAny = true;

                            var compItem = new NativeCheckboxItem(compName, component.Active);
                            compItem.Activated += (sender2, args2) =>
                            {
                                component.Active = !component.Active;
                            };

                            otherComponents.Add(compItem);
                        }
                    }

                    // Add all clips
                    foreach (var clip in clipCheckboxes)
                        customizeMenu.Add(clip);

                    // Add other components
                    foreach (var item in otherComponents)
                        customizeMenu.Add(item);

                    if (!hasAny && clipCheckboxes.Count == 0)
                    {
                        var noneItem = new NativeCheckboxItem("~c~No available customizations");
                        noneItem.Enabled = false;
                        customizeMenu.Add(noneItem);
                    }


                    int currentTintIndex = Function.Call<int>(Hash.GET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (uint)hash);

                    // Only add the tint selector if the weapon supports tints
                    if (currentTintIndex >= 0 && tintOptions.Length > 1)
                    {
                        var tintListItem = new NativeListItem<string>("Tint", tintOptions)
                        {
                            SelectedIndex = currentTintIndex
                        };

                        tintListItem.Activated += (senderTint, argsTint) =>
                        {
                            int selectedIndex = tintListItem.SelectedIndex;
                            Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (uint)hash, selectedIndex);
                        };

                        customizeMenu.Add(tintListItem);
                    }
                    // Add Camos List
                    if (camos.Count > 0)
                    {
                        string[] liveryColors =
                        {
        "Default", "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7"
    };
                        hasAny = true;
                        // Create a new array with "None" as first element + existing camos
                        var camoOptions = new string[camos.Count + 1];
                        camoOptions[0] = "None";
                        camos.CopyTo(camoOptions, 1);
                        var camoListItem = new NativeListItem<string>("Camos 01", camoOptions);
                        var liveryColorItem = new NativeListItem<string>("Camo Color", liveryColors);
                        liveryColorItem.SelectedIndex = 0;
                        liveryColorItem.Activated += (sender, args) =>
                        {
                            var ped = Game.Player.Character;
                            var weapons = Game.Player.Character.Weapons[hash];

                            if (!weapon.IsPresent) return;

                            WeaponComponent activeCamo = null;

                            foreach (var comp in weapon.Components)
                            {
                                string compName = comp.ComponentHash.ToString().Replace("WeaponComponentHash.", "").Replace('_', ' ').ToLower();

                                // Only apply to active main camos (not slide)
                                if (compName.Contains("camo") && !compName.EndsWith("slide") && comp.Active)
                                {
                                    activeCamo = comp;
                                    break;
                                }
                            }

                            if (activeCamo != null)
                            {
                                int selectedColor = liveryColorItem.SelectedIndex;
                                Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, ped, (uint)hash, (uint)activeCamo.ComponentHash, selectedColor);
                            }
                        };

                        // Find which camo is active, else select "None"
                        camoListItem.SelectedIndex = 0; // Default to None
                        for (int i = 0; i < camos.Count; i++)
                        {
                            string camo = camos[i];
                            foreach (var component in weapon.Components)
                            {
                                string compName = component.ComponentHash.ToString()
                                    .Replace("WeaponComponentHash.", "")
                                    .Replace('_', ' ');

                                if (compName == camo && component.Active)
                                {
                                    camoListItem.SelectedIndex = i + 1; // +1 because of "None" at 0
                                    break;
                                }
                            }
                        }
                        camoListItem.Activated += (sender3, args3) =>
                        {
                            string selectedCamo = camoListItem.SelectedItem;

                            if (selectedCamo == "None")
                            {
                                // Deactivate all camos
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName.ToLower().Contains("camo") && !compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                            else
                            {
                                // Activate selected camo, deactivate others
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName == selectedCamo)
                                        component.Active = true;
                                    else if (compName.ToLower().Contains("camo") && !compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                        };
                        customizeMenu.Add(camoListItem);
                        customizeMenu.Add(liveryColorItem);
                    }
                    if (camoSlides.Count > 0)
                    {
                        hasAny = true;

                        // Insert "None" option at the start
                        var camoSlideOptions = new string[camoSlides.Count + 1];
                        camoSlideOptions[0] = "None";
                        camoSlides.CopyTo(camoSlideOptions, 1);
                        string[] liveryColors =
                        {
                            "Default", "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7"
                         };
                        var camoSlideListItem = new NativeListItem<string>("Camo 02", camoSlideOptions);



                        var liveryColorItem2 = new NativeListItem<string>("Camo Color 2", liveryColors);
                        liveryColorItem2.SelectedIndex = 0;
                        liveryColorItem2.Activated += (sender2, args2) =>
                        {
                            var ped = Game.Player.Character;
                            var weapons = Game.Player.Character.Weapons[hash];

                            if (!weapon.IsPresent) return;

                            WeaponComponent activeSlideCamo = null;

                            foreach (var comp in weapon.Components)
                            {
                                string compName = comp.ComponentHash.ToString().Replace("WeaponComponentHash.", "").Replace('_', ' ').ToLower();

                                // Only apply to active slide camos
                                if (compName.EndsWith("slide") && comp.Active)
                                {
                                    activeSlideCamo = comp;
                                    break;
                                }
                            }

                            if (activeSlideCamo != null)
                            {
                                int selectedColor = liveryColorItem2.SelectedIndex;
                                Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, ped, (uint)hash, (uint)activeSlideCamo.ComponentHash, selectedColor);
                            }
                        };



                        // Default select "None"
                        camoSlideListItem.SelectedIndex = 0;

                        for (int i = 0; i < camoSlides.Count; i++)
                        {
                            string camoSlide = camoSlides[i];
                            foreach (var component in weapon.Components)
                            {
                                string compName = component.ComponentHash.ToString()
                                    .Replace("WeaponComponentHash.", "")
                                    .Replace('_', ' ');

                                if (compName == camoSlide && component.Active)
                                {
                                    camoSlideListItem.SelectedIndex = i + 1; // +1 for None at 0
                                    break;
                                }
                            }
                        }

                        camoSlideListItem.Activated += (sender4, args4) =>
                        {
                            string selectedCamoSlide = camoSlideListItem.SelectedItem;

                            if (selectedCamoSlide == "None")
                            {
                                // Deactivate all camo slides
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                            else
                            {
                                // Activate selected slide, deactivate others
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName == selectedCamoSlide)
                                        component.Active = true;
                                    else if (compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                        };

                        customizeMenu.Add(camoSlideListItem);
                        customizeMenu.Add(liveryColorItem2);
                    }

                };

                customizeMenu.Add(openCompMenuItem);

                // Only add Buy and Sell now; BuyAmmo added dynamically
                weaponMenu.Add(equipItem);
                weaponMenu.Add(buyItem);
                weaponMenu.Add(sellItem);
                weaponMenu.AddSubMenu(customizeMenu);

                smgSubMenu.AddSubMenu(weaponMenu);
            }

            // Optional: remove the `Shown` refresh if handling it dynamically


            // Refresh buy/sell button states each time the menu is shown
            smgSubMenu.Shown += (sender, args) =>
            {
                var weapons = Game.Player.Character.Weapons;

                foreach (var pair in smgItems)
                {
                    WeaponHash hash = pair.Key;
                    var buyItem = pair.Value;
                    var sellItem = smgSellItems[hash];
                    int price = SMGsValues[hash];

                    // Assuming you have a dictionary of equipItems for handguns:
                    var equipItem = smgEquipItems[hash]; // You need to have this collection set up.

                    if (weapons.HasWeapon(hash))
                    {
                        buyItem.Enabled = false;
                        buyItem.Description = "~c~You already own this weapon";

                        sellItem.Enabled = true;
                        sellItem.Description = $"Resell for ${price}";

                        equipItem.Enabled = true;       // Enable equip option

                        equipItem.Description = "Equip this weapon";
                    }
                    else
                    {
                        buyItem.Enabled = true;
                        buyItem.Description = $"Price: ${price}";

                        sellItem.Enabled = false;
                        sellItem.Description = "~c~You do not own this weapon";

                        equipItem.Enabled = false;     // Disable equip option
                        equipItem.Description = "~c~You do not own this weapon";
                    }
                }
            };




            Dictionary<WeaponHash, NativeItem> riflesItems = new Dictionary<WeaponHash, NativeItem>();
            // Dictionary to track sell buttons
            Dictionary<WeaponHash, NativeItem> riflesSellItems = new Dictionary<WeaponHash, NativeItem>();

            Dictionary<WeaponHash, NativeItem> riflesEquipItems = new Dictionary<WeaponHash, NativeItem>();


            riflesSubMenu = new NativeMenu(
                "",
                "Riffle's",
                "",
                new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                )
                );
            pool.Add(riflesSubMenu);
            riflesSubMenu.Alignment = Alignment.Right;
            var riflesSubMenuItem = armoryMenu.AddSubMenu(riflesSubMenu);

            foreach (var handgun in RiflesValues)
            {
                WeaponHash hash = handgun.Key;
                int price = handgun.Value;
                string name = hash.ToString().Replace("WeaponHash.", "").Replace('_', ' ');

                var weaponMenu = new NativeMenu("", name, $"Manage your {name}",
                    new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                ));
                pool.Add(weaponMenu);
                weaponMenu.Alignment = Alignment.Right;
                var buyFullAmmoItem = new NativeItem("Buy All Ammo", "Buy ammunition for this weapon.");
                var buyAmmoItem = new NativeItem("Buy Ammo", "Buy ammunition for this weapon.");
                var equipItem = new NativeItem("Equip", $"Equiped ${price}");
                var buyItem = new NativeItem("Buy", $"Price: ${price}");
                var sellItem = new NativeItem("Sell", $"Resell for ${price}");

                // Store references
                riflesEquipItems[hash] = equipItem;
                riflesItems[hash] = buyItem;
                riflesSellItems[hash] = sellItem;


                equipItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~Equipped~b~ {name}", true, false);

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You already have this weapon equiped";
                };

                // Buy Ammo logic
                // Buy 50 Rounds Logic
                buyAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~You don't own the {name}. ~w~Buy the weapon first.", true, false);   
                        return;
                    }

                    int ammoPrice = 70;
                    int ammoAmount = 50;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money!~w~ You need ~b~${ammoPrice}", true, false);
                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Purchased~b~ {ammoAmount} ~w~rounds for ~r~${ammoPrice}", true, false);
                };

                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~You don't own the {name}. ~w~Buy the weapon first.", true, false);   
                        return;
                    }

                    int refillPrice = 5000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money! You need ${refillPrice}", true, false);
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Fully refilled ammo for ~r~${refillPrice}", true, false);
                };

                // Buy weapon logic
                buyItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You already own the ~b~{name}.", true, false);   
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money!~w~ You need ~b~${price}", true, false);
                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~You purchased ~g~{name}", true, false);

                    buyItem.Enabled = false;
                    buyItem.Description = "~c~You already own this weapon";

                    sellItem.Enabled = true;
                    sellItem.Description = $"Resell for ${price}";

                    if (!weaponMenu.Items.Contains(buyAmmoItem))
                        weaponMenu.Items.Insert(0, buyAmmoItem);
                    if (!weaponMenu.Items.Contains(buyFullAmmoItem))
                        weaponMenu.Items.Insert(1, buyFullAmmoItem);

                };

                // Sell weapon logic
                sellItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Sold {name} for ~g~${price}", true, false);

                    buyItem.Enabled = true;
                    buyItem.Description = $"Price: ${price}";

                    sellItem.Enabled = false;
                    sellItem.Description = "~c~You do not own this weapon";

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You do not own this weapon";

                    // Remove Buy Ammo dynamically
                    if (weaponMenu.Items.Contains(buyAmmoItem))
                        weaponMenu.Items.Remove(buyAmmoItem);
                    if (weaponMenu.Items.Contains(buyFullAmmoItem))
                        weaponMenu.Items.Remove(buyFullAmmoItem);
                };

                var customizeMenu = new NativeMenu(
                    "",
                    $"{name} Customization",
                    $"Customize your {name}",
                    new ScaledTexture(
                        PointF.Empty,
                        new SizeF(431, 107),
                        "thumbnail_ammunation_net",
                        "ammunation_banner"
                    )
                );
                pool.Add(customizeMenu);
                customizeMenu.Alignment = Alignment.Right;
                customizeMenu.Shown += (s, e) =>
                {
                    if (!Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        customizeMenu.Visible = false;
                        weaponMenu.Visible = true;  // 👈 Show the weapon menu again
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Important", $"~r~You no longer own the {name}.", true, false);
                    }
                };
                var excludedComponents = new HashSet<WeaponComponentHash>
                    {
                        WeaponComponentHash.GunrunMk2Upgrade,
                        // Pump Shotgun Mk2
                        WeaponComponentHash.PumpShotgunMk2ClipExplosive,
                        WeaponComponentHash.PumpShotgunMk2ClipHollowPoint,
                        WeaponComponentHash.PumpShotgunMk2ClipIncendiary,
                        WeaponComponentHash.PumpShotgunMk2ClipArmorPiercing,

                        // Revolver Mk2
                        WeaponComponentHash.RevolverMk2ClipFMJ,
                        WeaponComponentHash.RevolverMk2ClipHollowPoint,
                        WeaponComponentHash.RevolverMk2ClipIncendiary,
                        WeaponComponentHash.RevolverMk2ClipTracer,

                        //Specail Carbine Mk2
                        WeaponComponentHash.SpecialCarbineMk2ClipArmorPiercing,
                        WeaponComponentHash.SpecialCarbineMk2ClipFMJ,
                        WeaponComponentHash.SpecialCarbineMk2ClipIncendiary,
                        WeaponComponentHash.SpecialCarbineMk2ClipTracer,


                        // SMG Mk2
                        WeaponComponentHash.SMGMk2ClipFMJ,
                        WeaponComponentHash.SMGMk2ClipHollowPoint,
                        WeaponComponentHash.SMGMk2ClipIncendiary,
                        WeaponComponentHash.SMGMk2ClipTracer,

                        // SNS Pistol Mk2
                        WeaponComponentHash.SNSPistolMk2ClipFMJ,
                        WeaponComponentHash.SNSPistolMk2ClipHollowPoint,
                        WeaponComponentHash.SNSPistolMk2ClipIncendiary,
                        WeaponComponentHash.SNSPistolMk2ClipTracer,

                        // Assault Rifle Mk2
                        WeaponComponentHash.AssaultRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.AssaultRifleMk2ClipFMJ,
                        WeaponComponentHash.AssaultRifleMk2ClipIncendiary,
                        WeaponComponentHash.AssaultRifleMk2ClipTracer,

                        // Bullpup Rifle Mk2
                        WeaponComponentHash.BullpupRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.BullpupRifleMk2ClipFMJ,
                        WeaponComponentHash.BullpupRifleMk2ClipIncendiary,
                        WeaponComponentHash.BullpupRifleMk2ClipTracer,

                        // Carbine Rifle Mk2
                        WeaponComponentHash.CarbineRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.CarbineRifleMk2ClipFMJ,
                        WeaponComponentHash.CarbineRifleMk2ClipIncendiary,
                        WeaponComponentHash.CarbineRifleMk2ClipTracer,

                        // Combat MG Mk2
                        WeaponComponentHash.CombatMGMk2ClipArmorPiercing,
                        WeaponComponentHash.CombatMGMk2ClipFMJ,
                        WeaponComponentHash.CombatMGMk2ClipIncendiary,
                        WeaponComponentHash.CombatMGMk2ClipTracer,

                        // Marksman Rifle Mk2
                        WeaponComponentHash.MarksmanRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.MarksmanRifleMk2ClipFMJ,
                        WeaponComponentHash.MarksmanRifleMk2ClipIncendiary,
                        WeaponComponentHash.MarksmanRifleMk2ClipTracer
                    };

                // Check if it's a Mk2 weapon (simple detection based on name)
                bool isMk2 = name.Contains("Mk2");

                // Mk2 Tint Names
                string[] mk2Tints =
                {
                    "Classic Black", "Classic Gray", "Classic Two-Tone", "Classic White", "Classic Beige", "Classic Green", "Classic Blue", "Classic Earth",
                    "Classic Brown & Black", "Red Contrast", "Blue Contrast", "Yellow Contrast", "Orange Contrast", "Bold Pink", "Bold Purple & Yellow",
                    "Bold Orange", "Bold Green & Purple", "Bold Red Features", "Bold Green Features", "Bold Cyan Features", "Bold Yellow Features",
                    "Bold Red & White", "Bold Blue & White", "Metallic Gold", "Metallic Platinum", "Metallic Gray & Lilac", "Metallic Purple & Lime",
                    "Metallic Red", "Metallic Green", "Metallic Blue", "Metallic White & Aqua", "Metallic Orange & Yellow", "Metallic Red and Yellow"
                };

                // Regular tint names for non-Mk2 weapons
                string[] standardTints =
                {
                    "Default / Black", "Green", "Gold", "Pink", "Army", "LSPD", "Orange", "Platinum"
                };

                // Choose the right list
                string[] tintOptions = isMk2 ? mk2Tints : standardTints;

                var openCompMenuItem = new NativeItem("Manage Attachments");

                openCompMenuItem.Activated += (sender1, args1) =>
                {
                    var weapon = Game.Player.Character.Weapons[hash];

                    if (!weapon.IsPresent)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You need to own the ~b~{name} to customize it.", true, false);   
                        return;
                    }

                    customizeMenu.Clear(); // Reset the customize menu

                    bool hasAny = false;



                    var camos = new List<string>();
                    var camoSlides = new List<string>();
                    var clipCheckboxes = new List<NativeCheckboxItem>();
                    var suppressorCheckboxes = new List<NativeCheckboxItem>();
                    var clipComponents = new List<WeaponComponent>();
                    var otherComponents = new List<NativeCheckboxItem>();

                    foreach (var component in weapon.Components)
                    {
                        if (excludedComponents.Contains(component.ComponentHash))
                            continue;

                        string compName = component.ComponentHash.ToString()
                            .Replace("WeaponComponentHash.", "")
                            .Replace('_', ' ');

                        if (compName.ToLower().Contains("camo") && compName.ToLower().EndsWith("slide"))
                        {
                            camoSlides.Add(compName);
                        }
                        else if (compName.ToLower().Contains("camo"))
                        {
                            camos.Add(compName);
                        }
                        else if (compName.ToLower().Contains("clip"))
                        {
                            hasAny = true;

                            var clipItem = new NativeCheckboxItem(compName, component.Active);

                            clipItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (otherName.ToLower().Contains("clip"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Update UI: ensure only this clip is checked
                                foreach (var clipCheckbox in clipCheckboxes)
                                {
                                    clipCheckbox.Checked = (clipCheckbox == clipItem && newState);
                                }
                            };

                            clipCheckboxes.Add(clipItem);
                        }
                        else if (compName.ToLower().Contains("barrel"))
                        {
                            hasAny = true;

                            var barrelItem = new NativeCheckboxItem(compName, component.Active);

                            barrelItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (otherName.ToLower().Contains("barrel"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Update UI: ensure only this clip is checked
                                foreach (var barrelCheckbox in clipCheckboxes)
                                {
                                    barrelCheckbox.Checked = (barrelCheckbox == barrelItem && newState);
                                }
                            };

                            clipCheckboxes.Add(barrelItem);
                        }
                        else if (compName.ToLower().Contains("supp") || compName.ToLower().Contains("comp") || compName.ToLower().Contains("muzzle"))
                        {
                            hasAny = true;

                            var suppressorItem = new NativeCheckboxItem(compName, component.Active);

                            suppressorItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ')
                                        .ToLower();

                                    if (otherName.Contains("supp") || otherName.Contains("comp") || otherName.Contains("muzzle"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Optional: Update UI to only check this one
                                foreach (var item in otherComponents)
                                {
                                    if (item.Title.ToLower().Contains("supp") || item.Title.ToLower().Contains("comp") || item.Title.ToLower().Contains("muzzle"))
                                        item.Checked = (item == suppressorItem && newState);
                                }
                            };

                            otherComponents.Add(suppressorItem);
                        }
                        else if (compName.ToLower().Contains("scope") || compName.ToLower().Contains("sight"))
                        {
                            hasAny = true;

                            var scopeItem = new NativeCheckboxItem(compName, component.Active);

                            scopeItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ')
                                        .ToLower();

                                    if (otherName.Contains("scope") || otherName.Contains("sight"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Optional: Update UI to only check this one
                                foreach (var item in otherComponents)
                                {
                                    if (item.Title.ToLower().Contains("scope") || item.Title.ToLower().Contains("sight"))
                                        item.Checked = (item == scopeItem && newState);
                                }
                            };

                            otherComponents.Add(scopeItem);
                        }

                        else
                        {
                            hasAny = true;

                            var compItem = new NativeCheckboxItem(compName, component.Active);
                            compItem.Activated += (sender2, args2) =>
                            {
                                component.Active = !component.Active;
                            };

                            otherComponents.Add(compItem);
                        }
                    }

                    // Add all clips
                    foreach (var clip in clipCheckboxes)
                        customizeMenu.Add(clip);

                    // Add other components
                    foreach (var item in otherComponents)
                        customizeMenu.Add(item);

                    if (!hasAny && clipCheckboxes.Count == 0)
                    {
                        var noneItem = new NativeCheckboxItem("~c~No available customizations");
                        noneItem.Enabled = false;
                        customizeMenu.Add(noneItem);
                    }


                    int currentTintIndex = Function.Call<int>(Hash.GET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (uint)hash);

                    // Only add the tint selector if the weapon supports tints
                    if (currentTintIndex >= 0 && tintOptions.Length > 1)
                    {
                        var tintListItem = new NativeListItem<string>("Tint", tintOptions)
                        {
                            SelectedIndex = currentTintIndex
                        };

                        tintListItem.Activated += (senderTint, argsTint) =>
                        {
                            int selectedIndex = tintListItem.SelectedIndex;
                            Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (uint)hash, selectedIndex);
                        };

                        customizeMenu.Add(tintListItem);
                    }
                    // Add Camos List
                    if (camos.Count > 0)
                    {
                        string[] liveryColors =
                        {
        "Default", "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7"
    };
                        hasAny = true;
                        // Create a new array with "None" as first element + existing camos
                        var camoOptions = new string[camos.Count + 1];
                        camoOptions[0] = "None";
                        camos.CopyTo(camoOptions, 1);
                        var camoListItem = new NativeListItem<string>("Camos 01", camoOptions);
                        var liveryColorItem = new NativeListItem<string>("Camo Color", liveryColors);
                        liveryColorItem.SelectedIndex = 0;
                        liveryColorItem.Activated += (sender, args) =>
                        {
                            var ped = Game.Player.Character;
                            var weapons = Game.Player.Character.Weapons[hash];

                            if (!weapon.IsPresent) return;

                            WeaponComponent activeCamo = null;

                            foreach (var comp in weapon.Components)
                            {
                                string compName = comp.ComponentHash.ToString().Replace("WeaponComponentHash.", "").Replace('_', ' ').ToLower();

                                // Only apply to active main camos (not slide)
                                if (compName.Contains("camo") && !compName.EndsWith("slide") && comp.Active)
                                {
                                    activeCamo = comp;
                                    break;
                                }
                            }

                            if (activeCamo != null)
                            {
                                int selectedColor = liveryColorItem.SelectedIndex;
                                Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, ped, (uint)hash, (uint)activeCamo.ComponentHash, selectedColor);
                            }
                        };

                        // Find which camo is active, else select "None"
                        camoListItem.SelectedIndex = 0; // Default to None
                        for (int i = 0; i < camos.Count; i++)
                        {
                            string camo = camos[i];
                            foreach (var component in weapon.Components)
                            {
                                string compName = component.ComponentHash.ToString()
                                    .Replace("WeaponComponentHash.", "")
                                    .Replace('_', ' ');

                                if (compName == camo && component.Active)
                                {
                                    camoListItem.SelectedIndex = i + 1; // +1 because of "None" at 0
                                    break;
                                }
                            }
                        }
                        camoListItem.Activated += (sender3, args3) =>
                        {
                            string selectedCamo = camoListItem.SelectedItem;

                            if (selectedCamo == "None")
                            {
                                // Deactivate all camos
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName.ToLower().Contains("camo") && !compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                            else
                            {
                                // Activate selected camo, deactivate others
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName == selectedCamo)
                                        component.Active = true;
                                    else if (compName.ToLower().Contains("camo") && !compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                        };
                        customizeMenu.Add(camoListItem);
                        customizeMenu.Add(liveryColorItem);
                    }
                    if (camoSlides.Count > 0)
                    {
                        hasAny = true;

                        // Insert "None" option at the start
                        var camoSlideOptions = new string[camoSlides.Count + 1];
                        camoSlideOptions[0] = "None";
                        camoSlides.CopyTo(camoSlideOptions, 1);
                        string[] liveryColors =
                        {
                            "Default", "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7"
                         };
                        var camoSlideListItem = new NativeListItem<string>("Camo 02", camoSlideOptions);



                        var liveryColorItem2 = new NativeListItem<string>("Camo Color 2", liveryColors);
                        liveryColorItem2.SelectedIndex = 0;
                        liveryColorItem2.Activated += (sender2, args2) =>
                        {
                            var ped = Game.Player.Character;
                            var weapons = Game.Player.Character.Weapons[hash];

                            if (!weapon.IsPresent) return;

                            WeaponComponent activeSlideCamo = null;

                            foreach (var comp in weapon.Components)
                            {
                                string compName = comp.ComponentHash.ToString().Replace("WeaponComponentHash.", "").Replace('_', ' ').ToLower();

                                // Only apply to active slide camos
                                if (compName.EndsWith("slide") && comp.Active)
                                {
                                    activeSlideCamo = comp;
                                    break;
                                }
                            }

                            if (activeSlideCamo != null)
                            {
                                int selectedColor = liveryColorItem2.SelectedIndex;
                                Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, ped, (uint)hash, (uint)activeSlideCamo.ComponentHash, selectedColor);
                            }
                        };



                        // Default select "None"
                        camoSlideListItem.SelectedIndex = 0;

                        for (int i = 0; i < camoSlides.Count; i++)
                        {
                            string camoSlide = camoSlides[i];
                            foreach (var component in weapon.Components)
                            {
                                string compName = component.ComponentHash.ToString()
                                    .Replace("WeaponComponentHash.", "")
                                    .Replace('_', ' ');

                                if (compName == camoSlide && component.Active)
                                {
                                    camoSlideListItem.SelectedIndex = i + 1; // +1 for None at 0
                                    break;
                                }
                            }
                        }

                        camoSlideListItem.Activated += (sender4, args4) =>
                        {
                            string selectedCamoSlide = camoSlideListItem.SelectedItem;

                            if (selectedCamoSlide == "None")
                            {
                                // Deactivate all camo slides
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                            else
                            {
                                // Activate selected slide, deactivate others
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName == selectedCamoSlide)
                                        component.Active = true;
                                    else if (compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                        };

                        customizeMenu.Add(camoSlideListItem);
                        customizeMenu.Add(liveryColorItem2);
                    }

                };

                customizeMenu.Add(openCompMenuItem);

                // Only add Buy and Sell now; BuyAmmo added dynamically
                weaponMenu.Add(equipItem);
                weaponMenu.Add(buyItem);
                weaponMenu.Add(sellItem);
                weaponMenu.AddSubMenu(customizeMenu);

                riflesSubMenu.AddSubMenu(weaponMenu);
            }

            // Optional: remove the `Shown` refresh if handling it dynamically

            // Refresh buy/sell button states each time the menu is shown
            riflesSubMenu.Shown += (sender, args) =>
            {
                var weapons = Game.Player.Character.Weapons;

                foreach (var pair in riflesItems)
                {
                    WeaponHash hash = pair.Key;
                    var buyItem = pair.Value;
                    var sellItem = riflesSellItems[hash];
                    int price = RiflesValues[hash];

                    // Assuming you have a dictionary of equipItems for handguns:
                    var equipItem = riflesEquipItems[hash]; // You need to have this collection set up.

                    if (weapons.HasWeapon(hash))
                    {
                        buyItem.Enabled = false;
                        buyItem.Description = "~c~You already own this weapon";

                        sellItem.Enabled = true;
                        sellItem.Description = $"Resell for ${price}";

                        equipItem.Enabled = true;       // Enable equip option

                        equipItem.Description = "Equip this weapon";
                    }
                    else
                    {
                        buyItem.Enabled = true;
                        buyItem.Description = $"Price: ${price}";

                        sellItem.Enabled = false;
                        sellItem.Description = "~c~You do not own this weapon";

                        equipItem.Enabled = false;     // Disable equip option
                        equipItem.Description = "~c~You do not own this weapon";
                    }
                }
            };



            Dictionary<WeaponHash, NativeItem> machineGunsItems = new Dictionary<WeaponHash, NativeItem>();
            // Dictionary to track sell buttons
            Dictionary<WeaponHash, NativeItem> machineGunsSellItems = new Dictionary<WeaponHash, NativeItem>();

            Dictionary<WeaponHash, NativeItem> machineGunsEquipItems = new Dictionary<WeaponHash, NativeItem>();




            machinegunsSubMenu = new NativeMenu(
                "",
                "Machine Gun's",
                "",
                new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                )
                );
            pool.Add(machinegunsSubMenu);
            machinegunsSubMenu.Alignment = Alignment.Right;
            var machinegunsSubMenuItem = armoryMenu.AddSubMenu(machinegunsSubMenu);



            foreach (var handgun in MachineGunsValues)
            {
                WeaponHash hash = handgun.Key;
                int price = handgun.Value;
                string name = hash.ToString().Replace("WeaponHash.", "").Replace('_', ' ');

                var weaponMenu = new NativeMenu("", name, $"Manage your {name}",
                    new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                ));
                pool.Add(weaponMenu);
                weaponMenu.Alignment = Alignment.Right;
                var buyFullAmmoItem = new NativeItem("Buy All Ammo", "Buy ammunition for this weapon.");
                var buyAmmoItem = new NativeItem("Buy Ammo", "Buy ammunition for this weapon.");
                var equipItem = new NativeItem("Equip", $"Equiped ${price}");
                var buyItem = new NativeItem("Buy", $"Price: ${price}");
                var sellItem = new NativeItem("Sell", $"Resell for ${price}");

                // Store references
                machineGunsEquipItems[hash] = equipItem;
                machineGunsItems[hash] = buyItem;
                machineGunsSellItems[hash] = sellItem;


                equipItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~Equipped~b~ {name}", true, false);

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You already have this weapon equiped";
                };

                // Buy Ammo logic
                // Buy 50 Rounds Logic
                buyAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~You don't own the {name}. ~w~Buy the weapon first.", true, false);   
                        return;
                    }

                    int ammoPrice = 70;
                    int ammoAmount = 50;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money!~w~ You need ~b~${ammoPrice}", true, false);
                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Purchased~b~ {ammoAmount} ~w~rounds for ~r~${ammoPrice}", true, false);
                };

                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~You don't own the {name}. ~w~Buy the weapon first.", true, false);   
                        return;
                    }

                    int refillPrice = 5000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money! You need ${refillPrice}", true, false);
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Fully refilled ammo for ~r~${refillPrice}", true, false);
                };

                // Buy weapon logic
                buyItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You already own the ~b~{name}.", true, false);   
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money!~w~ You need ~b~${price}", true, false);
                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~You purchased ~g~{name}", true, false);

                    buyItem.Enabled = false;
                    buyItem.Description = "~c~You already own this weapon";

                    sellItem.Enabled = true;
                    sellItem.Description = $"Resell for ${price}";

                    if (!weaponMenu.Items.Contains(buyAmmoItem))
                        weaponMenu.Items.Insert(0, buyAmmoItem);
                    if (!weaponMenu.Items.Contains(buyFullAmmoItem))
                        weaponMenu.Items.Insert(1, buyFullAmmoItem);

                };

                // Sell weapon logic
                sellItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Sold {name} for ~g~${price}", true, false);

                    buyItem.Enabled = true;
                    buyItem.Description = $"Price: ${price}";

                    sellItem.Enabled = false;
                    sellItem.Description = "~c~You do not own this weapon";

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You do not own this weapon";

                    // Remove Buy Ammo dynamically
                    if (weaponMenu.Items.Contains(buyAmmoItem))
                        weaponMenu.Items.Remove(buyAmmoItem);
                    if (weaponMenu.Items.Contains(buyFullAmmoItem))
                        weaponMenu.Items.Remove(buyFullAmmoItem);
                };

                var customizeMenu = new NativeMenu(
                    "",
                    $"{name} Customization",
                    $"Customize your {name}",
                    new ScaledTexture(
                        PointF.Empty,
                        new SizeF(431, 107),
                        "thumbnail_ammunation_net",
                        "ammunation_banner"
                    )
                );
                pool.Add(customizeMenu);
                customizeMenu.Alignment = Alignment.Right;
                customizeMenu.Shown += (s, e) =>
                {
                    if (!Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        customizeMenu.Visible = false;
                        weaponMenu.Visible = true;  // 👈 Show the weapon menu again
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Important", $"~r~You no longer own the {name}.", true, false);
                    }
                };
                var excludedComponents = new HashSet<WeaponComponentHash>
                    {
                        WeaponComponentHash.GunrunMk2Upgrade,
                        // Pump Shotgun Mk2
                        WeaponComponentHash.PumpShotgunMk2ClipExplosive,
                        WeaponComponentHash.PumpShotgunMk2ClipHollowPoint,
                        WeaponComponentHash.PumpShotgunMk2ClipIncendiary,
                        WeaponComponentHash.PumpShotgunMk2ClipArmorPiercing,

                        // Revolver Mk2
                        WeaponComponentHash.RevolverMk2ClipFMJ,
                        WeaponComponentHash.RevolverMk2ClipHollowPoint,
                        WeaponComponentHash.RevolverMk2ClipIncendiary,
                        WeaponComponentHash.RevolverMk2ClipTracer,

                        // SMG Mk2
                        WeaponComponentHash.SMGMk2ClipFMJ,
                        WeaponComponentHash.SMGMk2ClipHollowPoint,
                        WeaponComponentHash.SMGMk2ClipIncendiary,
                        WeaponComponentHash.SMGMk2ClipTracer,

                        // SNS Pistol Mk2
                        WeaponComponentHash.SNSPistolMk2ClipFMJ,
                        WeaponComponentHash.SNSPistolMk2ClipHollowPoint,
                        WeaponComponentHash.SNSPistolMk2ClipIncendiary,
                        WeaponComponentHash.SNSPistolMk2ClipTracer,

                        // Assault Rifle Mk2
                        WeaponComponentHash.AssaultRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.AssaultRifleMk2ClipFMJ,
                        WeaponComponentHash.AssaultRifleMk2ClipIncendiary,
                        WeaponComponentHash.AssaultRifleMk2ClipTracer,

                        // Bullpup Rifle Mk2
                        WeaponComponentHash.BullpupRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.BullpupRifleMk2ClipFMJ,
                        WeaponComponentHash.BullpupRifleMk2ClipIncendiary,
                        WeaponComponentHash.BullpupRifleMk2ClipTracer,

                        // Carbine Rifle Mk2
                        WeaponComponentHash.CarbineRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.CarbineRifleMk2ClipFMJ,
                        WeaponComponentHash.CarbineRifleMk2ClipIncendiary,
                        WeaponComponentHash.CarbineRifleMk2ClipTracer,

                        // Combat MG Mk2
                        WeaponComponentHash.CombatMGMk2ClipArmorPiercing,
                        WeaponComponentHash.CombatMGMk2ClipFMJ,
                        WeaponComponentHash.CombatMGMk2ClipIncendiary,
                        WeaponComponentHash.CombatMGMk2ClipTracer,

                        // Marksman Rifle Mk2
                        WeaponComponentHash.MarksmanRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.MarksmanRifleMk2ClipFMJ,
                        WeaponComponentHash.MarksmanRifleMk2ClipIncendiary,
                        WeaponComponentHash.MarksmanRifleMk2ClipTracer
                    };

                // Check if it's a Mk2 weapon (simple detection based on name)
                bool isMk2 = name.Contains("Mk2");

                // Mk2 Tint Names
                string[] mk2Tints =
                {
                    "Classic Black", "Classic Gray", "Classic Two-Tone", "Classic White", "Classic Beige", "Classic Green", "Classic Blue", "Classic Earth",
                    "Classic Brown & Black", "Red Contrast", "Blue Contrast", "Yellow Contrast", "Orange Contrast", "Bold Pink", "Bold Purple & Yellow",
                    "Bold Orange", "Bold Green & Purple", "Bold Red Features", "Bold Green Features", "Bold Cyan Features", "Bold Yellow Features",
                    "Bold Red & White", "Bold Blue & White", "Metallic Gold", "Metallic Platinum", "Metallic Gray & Lilac", "Metallic Purple & Lime",
                    "Metallic Red", "Metallic Green", "Metallic Blue", "Metallic White & Aqua", "Metallic Orange & Yellow", "Metallic Red and Yellow"
                };

                // Regular tint names for non-Mk2 weapons
                string[] standardTints =
                {
                    "Default / Black", "Green", "Gold", "Pink", "Army", "LSPD", "Orange", "Platinum"
                };

                // Choose the right list
                string[] tintOptions = isMk2 ? mk2Tints : standardTints;

                var openCompMenuItem = new NativeItem("Manage Attachments");

                openCompMenuItem.Activated += (sender1, args1) =>
                {
                    var weapon = Game.Player.Character.Weapons[hash];

                    if (!weapon.IsPresent)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You need to own the ~b~{name} to customize it.", true, false);   
                        return;
                    }

                    customizeMenu.Clear(); // Reset the customize menu

                    bool hasAny = false;



                    var camos = new List<string>();
                    var camoSlides = new List<string>();
                    var clipCheckboxes = new List<NativeCheckboxItem>();
                    var suppressorCheckboxes = new List<NativeCheckboxItem>();
                    var clipComponents = new List<WeaponComponent>();
                    var otherComponents = new List<NativeCheckboxItem>();

                    foreach (var component in weapon.Components)
                    {
                        if (excludedComponents.Contains(component.ComponentHash))
                            continue;

                        string compName = component.ComponentHash.ToString()
                            .Replace("WeaponComponentHash.", "")
                            .Replace('_', ' ');

                        if (compName.ToLower().Contains("camo") && compName.ToLower().EndsWith("slide"))
                        {
                            camoSlides.Add(compName);
                        }
                        else if (compName.ToLower().Contains("camo"))
                        {
                            camos.Add(compName);
                        }
                        else if (compName.ToLower().Contains("clip"))
                        {
                            hasAny = true;

                            var clipItem = new NativeCheckboxItem(compName, component.Active);

                            clipItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (otherName.ToLower().Contains("clip"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Update UI: ensure only this clip is checked
                                foreach (var clipCheckbox in clipCheckboxes)
                                {
                                    clipCheckbox.Checked = (clipCheckbox == clipItem && newState);
                                }
                            };

                            clipCheckboxes.Add(clipItem);
                        }
                        else if (compName.ToLower().Contains("barrel"))
                        {
                            hasAny = true;

                            var barrelItem = new NativeCheckboxItem(compName, component.Active);

                            barrelItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (otherName.ToLower().Contains("barrel"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Update UI: ensure only this clip is checked
                                foreach (var barrelCheckbox in clipCheckboxes)
                                {
                                    barrelCheckbox.Checked = (barrelCheckbox == barrelItem && newState);
                                }
                            };

                            clipCheckboxes.Add(barrelItem);
                        }
                        else if (compName.ToLower().Contains("supp") || compName.ToLower().Contains("comp") || compName.ToLower().Contains("muzzle"))
                        {
                            hasAny = true;

                            var suppressorItem = new NativeCheckboxItem(compName, component.Active);

                            suppressorItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ')
                                        .ToLower();

                                    if (otherName.Contains("supp") || otherName.Contains("comp") || otherName.Contains("muzzle"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Optional: Update UI to only check this one
                                foreach (var item in otherComponents)
                                {
                                    if (item.Title.ToLower().Contains("supp") || item.Title.ToLower().Contains("comp") || item.Title.ToLower().Contains("muzzle"))
                                        item.Checked = (item == suppressorItem && newState);
                                }
                            };

                            otherComponents.Add(suppressorItem);
                        }
                        else if (compName.ToLower().Contains("scope") || compName.ToLower().Contains("sight"))
                        {
                            hasAny = true;

                            var scopeItem = new NativeCheckboxItem(compName, component.Active);

                            scopeItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ')
                                        .ToLower();

                                    if (otherName.Contains("scope") || otherName.Contains("sight"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Optional: Update UI to only check this one
                                foreach (var item in otherComponents)
                                {
                                    if (item.Title.ToLower().Contains("scope") || item.Title.ToLower().Contains("sight"))
                                        item.Checked = (item == scopeItem && newState);
                                }
                            };

                            otherComponents.Add(scopeItem);
                        }

                        else
                        {
                            hasAny = true;

                            var compItem = new NativeCheckboxItem(compName, component.Active);
                            compItem.Activated += (sender2, args2) =>
                            {
                                component.Active = !component.Active;
                            };

                            otherComponents.Add(compItem);
                        }
                    }

                    // Add all clips
                    foreach (var clip in clipCheckboxes)
                        customizeMenu.Add(clip);

                    // Add other components
                    foreach (var item in otherComponents)
                        customizeMenu.Add(item);

                    if (!hasAny && clipCheckboxes.Count == 0)
                    {
                        var noneItem = new NativeCheckboxItem("~c~No available customizations");
                        noneItem.Enabled = false;
                        customizeMenu.Add(noneItem);
                    }


                    int currentTintIndex = Function.Call<int>(Hash.GET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (uint)hash);

                    // Only add the tint selector if the weapon supports tints
                    if (currentTintIndex >= 0 && tintOptions.Length > 1)
                    {
                        var tintListItem = new NativeListItem<string>("Tint", tintOptions)
                        {
                            SelectedIndex = currentTintIndex
                        };

                        tintListItem.Activated += (senderTint, argsTint) =>
                        {
                            int selectedIndex = tintListItem.SelectedIndex;
                            Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (uint)hash, selectedIndex);
                        };

                        customizeMenu.Add(tintListItem);
                    }
                    // Add Camos List
                    if (camos.Count > 0)
                    {
                        string[] liveryColors =
                        {
                            "Default", "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7"
                        };
                        hasAny = true;
                        // Create a new array with "None" as first element + existing camos
                        var camoOptions = new string[camos.Count + 1];
                        camoOptions[0] = "None";
                        camos.CopyTo(camoOptions, 1);
                        var camoListItem = new NativeListItem<string>("Camos 01", camoOptions);
                        var liveryColorItem = new NativeListItem<string>("Camo Color", liveryColors);
                        liveryColorItem.SelectedIndex = 0;
                        liveryColorItem.Activated += (sender, args) =>
                        {
                            var ped = Game.Player.Character;
                            var weapons = Game.Player.Character.Weapons[hash];

                            if (!weapon.IsPresent) return;

                            WeaponComponent activeCamo = null;

                            foreach (var comp in weapon.Components)
                            {
                                string compName = comp.ComponentHash.ToString().Replace("WeaponComponentHash.", "").Replace('_', ' ').ToLower();

                                // Only apply to active main camos (not slide)
                                if (compName.Contains("camo") && !compName.EndsWith("slide") && comp.Active)
                                {
                                    activeCamo = comp;
                                    break;
                                }
                            }

                            if (activeCamo != null)
                            {
                                int selectedColor = liveryColorItem.SelectedIndex;
                                Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, ped, (uint)hash, (uint)activeCamo.ComponentHash, selectedColor);
                            }
                        };

                        // Find which camo is active, else select "None"
                        camoListItem.SelectedIndex = 0; // Default to None
                        for (int i = 0; i < camos.Count; i++)
                        {
                            string camo = camos[i];
                            foreach (var component in weapon.Components)
                            {
                                string compName = component.ComponentHash.ToString()
                                    .Replace("WeaponComponentHash.", "")
                                    .Replace('_', ' ');

                                if (compName == camo && component.Active)
                                {
                                    camoListItem.SelectedIndex = i + 1; // +1 because of "None" at 0
                                    break;
                                }
                            }
                        }
                        camoListItem.Activated += (sender3, args3) =>
                        {
                            string selectedCamo = camoListItem.SelectedItem;

                            if (selectedCamo == "None")
                            {
                                // Deactivate all camos
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName.ToLower().Contains("camo") && !compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                            else
                            {
                                // Activate selected camo, deactivate others
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName == selectedCamo)
                                        component.Active = true;
                                    else if (compName.ToLower().Contains("camo") && !compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                        };
                        customizeMenu.Add(camoListItem);
                        customizeMenu.Add(liveryColorItem);
                    }
                    if (camoSlides.Count > 0)
                    {
                        hasAny = true;

                        // Insert "None" option at the start
                        var camoSlideOptions = new string[camoSlides.Count + 1];
                        camoSlideOptions[0] = "None";
                        camoSlides.CopyTo(camoSlideOptions, 1);
                        string[] liveryColors =
                        {
                            "Default", "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7"
                         };
                        var camoSlideListItem = new NativeListItem<string>("Camo 02", camoSlideOptions);



                        var liveryColorItem2 = new NativeListItem<string>("Camo Color 2", liveryColors);
                        liveryColorItem2.SelectedIndex = 0;
                        liveryColorItem2.Activated += (sender2, args2) =>
                        {
                            var ped = Game.Player.Character;
                            var weapons = Game.Player.Character.Weapons[hash];

                            if (!weapon.IsPresent) return;

                            WeaponComponent activeSlideCamo = null;

                            foreach (var comp in weapon.Components)
                            {
                                string compName = comp.ComponentHash.ToString().Replace("WeaponComponentHash.", "").Replace('_', ' ').ToLower();

                                // Only apply to active slide camos
                                if (compName.EndsWith("slide") && comp.Active)
                                {
                                    activeSlideCamo = comp;
                                    break;
                                }
                            }

                            if (activeSlideCamo != null)
                            {
                                int selectedColor = liveryColorItem2.SelectedIndex;
                                Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, ped, (uint)hash, (uint)activeSlideCamo.ComponentHash, selectedColor);
                            }
                        };



                        // Default select "None"
                        camoSlideListItem.SelectedIndex = 0;

                        for (int i = 0; i < camoSlides.Count; i++)
                        {
                            string camoSlide = camoSlides[i];
                            foreach (var component in weapon.Components)
                            {
                                string compName = component.ComponentHash.ToString()
                                    .Replace("WeaponComponentHash.", "")
                                    .Replace('_', ' ');

                                if (compName == camoSlide && component.Active)
                                {
                                    camoSlideListItem.SelectedIndex = i + 1; // +1 for None at 0
                                    break;
                                }
                            }
                        }

                        camoSlideListItem.Activated += (sender4, args4) =>
                        {
                            string selectedCamoSlide = camoSlideListItem.SelectedItem;

                            if (selectedCamoSlide == "None")
                            {
                                // Deactivate all camo slides
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                            else
                            {
                                // Activate selected slide, deactivate others
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName == selectedCamoSlide)
                                        component.Active = true;
                                    else if (compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                        };

                        customizeMenu.Add(camoSlideListItem);
                        customizeMenu.Add(liveryColorItem2);
                    }

                };

                customizeMenu.Add(openCompMenuItem);

                // Only add Buy and Sell now; BuyAmmo added dynamically
                weaponMenu.Add(equipItem);
                weaponMenu.Add(buyItem);
                weaponMenu.Add(sellItem);
                weaponMenu.AddSubMenu(customizeMenu);

                machinegunsSubMenu.AddSubMenu(weaponMenu);
            }

            // Optional: remove the `Shown` refresh if handling it dynamically

            // Refresh buy/sell button states each time the menu is shown
            machinegunsSubMenu.Shown += (sender, args) =>
            {
                var weapons = Game.Player.Character.Weapons;

                foreach (var pair in machineGunsItems)
                {
                    WeaponHash hash = pair.Key;
                    var buyItem = pair.Value;
                    var sellItem = machineGunsSellItems[hash];
                    int price = MachineGunsValues[hash];

                    // Assuming you have a dictionary of equipItems for handguns:
                    var equipItem = machineGunsEquipItems[hash]; // You need to have this collection set up.

                    if (weapons.HasWeapon(hash))
                    {
                        buyItem.Enabled = false;
                        buyItem.Description = "~c~You already own this weapon";

                        sellItem.Enabled = true;
                        sellItem.Description = $"Resell for ${price}";

                        equipItem.Enabled = true;       // Enable equip option

                        equipItem.Description = "Equip this weapon";
                    }
                    else
                    {
                        buyItem.Enabled = true;
                        buyItem.Description = $"Price: ${price}";

                        sellItem.Enabled = false;
                        sellItem.Description = "~c~You do not own this weapon";

                        equipItem.Enabled = false;     // Disable equip option
                        equipItem.Description = "~c~You do not own this weapon";
                    }
                }
            };







            Dictionary<WeaponHash, NativeItem> shotgunsItems = new Dictionary<WeaponHash, NativeItem>();
            // Dictionary to track sell buttons
            Dictionary<WeaponHash, NativeItem> shotgunsSellItems = new Dictionary<WeaponHash, NativeItem>();

            Dictionary<WeaponHash, NativeItem> shotgunsEquipItems = new Dictionary<WeaponHash, NativeItem>();



            shotgunsSubMenu = new NativeMenu(
                "",
                "Shotgun's",
                "",
                new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                )
                );
            pool.Add(shotgunsSubMenu);
            shotgunsSubMenu.Alignment = Alignment.Right;
            var shotgunsSubMenuItem = armoryMenu.AddSubMenu(shotgunsSubMenu);


            foreach (var handgun in ShotgunsValues)
            {
                WeaponHash hash = handgun.Key;
                int price = handgun.Value;
                string name = hash.ToString().Replace("WeaponHash.", "").Replace('_', ' ');

                var weaponMenu = new NativeMenu("", name, $"Manage your {name}",
                    new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                ));
                pool.Add(weaponMenu);
                weaponMenu.Alignment = Alignment.Right;
                var buyFullAmmoItem = new NativeItem("Buy All Ammo", "Buy ammunition for this weapon.");
                var buyAmmoItem = new NativeItem("Buy Ammo", "Buy ammunition for this weapon.");
                var equipItem = new NativeItem("Equip", $"Equiped ${price}");
                var buyItem = new NativeItem("Buy", $"Price: ${price}");
                var sellItem = new NativeItem("Sell", $"Resell for ${price}");

                // Store references
                shotgunsEquipItems[hash] = equipItem;
                shotgunsItems[hash] = buyItem;
                shotgunsSellItems[hash] = sellItem;


                equipItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~Equipped~b~ {name}", true, false);

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You already have this weapon equiped";
                };

                // Buy Ammo logic
                // Buy 50 Rounds Logic
                buyAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~You don't own the {name}. ~w~Buy the weapon first.", true, false);   
                        return;
                    }

                    int ammoPrice = 70;
                    int ammoAmount = 50;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money!~w~ You need ~b~${ammoPrice}", true, false);
                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Purchased~b~ {ammoAmount} ~w~rounds for ~r~${ammoPrice}", true, false);
                };

                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~You don't own the {name}. ~w~Buy the weapon first.", true, false);   
                        return;
                    }

                    int refillPrice = 5000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money! You need ${refillPrice}", true, false);
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Fully refilled ammo for ~r~${refillPrice}", true, false);
                };

                // Buy weapon logic
                buyItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You already own the ~b~{name}.", true, false);   
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money!~w~ You need ~b~${price}", true, false);
                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~You purchased ~g~{name}", true, false);

                    buyItem.Enabled = false;
                    buyItem.Description = "~c~You already own this weapon";

                    sellItem.Enabled = true;
                    sellItem.Description = $"Resell for ${price}";

                    if (!weaponMenu.Items.Contains(buyAmmoItem))
                        weaponMenu.Items.Insert(0, buyAmmoItem);
                    if (!weaponMenu.Items.Contains(buyFullAmmoItem))
                        weaponMenu.Items.Insert(1, buyFullAmmoItem);

                };

                // Sell weapon logic
                sellItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Sold {name} for ~g~${price}", true, false);

                    buyItem.Enabled = true;
                    buyItem.Description = $"Price: ${price}";

                    sellItem.Enabled = false;
                    sellItem.Description = "~c~You do not own this weapon";

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You do not own this weapon";

                    // Remove Buy Ammo dynamically
                    if (weaponMenu.Items.Contains(buyAmmoItem))
                        weaponMenu.Items.Remove(buyAmmoItem);
                    if (weaponMenu.Items.Contains(buyFullAmmoItem))
                        weaponMenu.Items.Remove(buyFullAmmoItem);
                };

                var customizeMenu = new NativeMenu(
                    "",
                    $"{name} Customization",
                    $"Customize your {name}",
                    new ScaledTexture(
                        PointF.Empty,
                        new SizeF(431, 107),
                        "thumbnail_ammunation_net",
                        "ammunation_banner"
                    )
                );
                pool.Add(customizeMenu);
                customizeMenu.Alignment = Alignment.Right;
                customizeMenu.Shown += (s, e) =>
                {
                    if (!Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        customizeMenu.Visible = false;
                        weaponMenu.Visible = true;  // 👈 Show the weapon menu again
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Important", $"~r~You no longer own the {name}.", true, false);
                    }
                };
                var excludedComponents = new HashSet<WeaponComponentHash>
                    {
                        WeaponComponentHash.GunrunMk2Upgrade,
                        // Pump Shotgun Mk2
                        WeaponComponentHash.PumpShotgunMk2ClipExplosive,
                        WeaponComponentHash.PumpShotgunMk2ClipHollowPoint,
                        WeaponComponentHash.PumpShotgunMk2ClipIncendiary,
                        WeaponComponentHash.PumpShotgunMk2ClipArmorPiercing,

                        // Revolver Mk2
                        WeaponComponentHash.RevolverMk2ClipFMJ,
                        WeaponComponentHash.RevolverMk2ClipHollowPoint,
                        WeaponComponentHash.RevolverMk2ClipIncendiary,
                        WeaponComponentHash.RevolverMk2ClipTracer,

                        // SMG Mk2
                        WeaponComponentHash.SMGMk2ClipFMJ,
                        WeaponComponentHash.SMGMk2ClipHollowPoint,
                        WeaponComponentHash.SMGMk2ClipIncendiary,
                        WeaponComponentHash.SMGMk2ClipTracer,

                        // SNS Pistol Mk2
                        WeaponComponentHash.SNSPistolMk2ClipFMJ,
                        WeaponComponentHash.SNSPistolMk2ClipHollowPoint,
                        WeaponComponentHash.SNSPistolMk2ClipIncendiary,
                        WeaponComponentHash.SNSPistolMk2ClipTracer,

                        // Assault Rifle Mk2
                        WeaponComponentHash.AssaultRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.AssaultRifleMk2ClipFMJ,
                        WeaponComponentHash.AssaultRifleMk2ClipIncendiary,
                        WeaponComponentHash.AssaultRifleMk2ClipTracer,

                        // Bullpup Rifle Mk2
                        WeaponComponentHash.BullpupRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.BullpupRifleMk2ClipFMJ,
                        WeaponComponentHash.BullpupRifleMk2ClipIncendiary,
                        WeaponComponentHash.BullpupRifleMk2ClipTracer,

                        // Carbine Rifle Mk2
                        WeaponComponentHash.CarbineRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.CarbineRifleMk2ClipFMJ,
                        WeaponComponentHash.CarbineRifleMk2ClipIncendiary,
                        WeaponComponentHash.CarbineRifleMk2ClipTracer,

                        // Combat MG Mk2
                        WeaponComponentHash.CombatMGMk2ClipArmorPiercing,
                        WeaponComponentHash.CombatMGMk2ClipFMJ,
                        WeaponComponentHash.CombatMGMk2ClipIncendiary,
                        WeaponComponentHash.CombatMGMk2ClipTracer,

                        // Marksman Rifle Mk2
                        WeaponComponentHash.MarksmanRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.MarksmanRifleMk2ClipFMJ,
                        WeaponComponentHash.MarksmanRifleMk2ClipIncendiary,
                        WeaponComponentHash.MarksmanRifleMk2ClipTracer
                    };

                // Check if it's a Mk2 weapon (simple detection based on name)
                bool isMk2 = name.Contains("Mk2");

                // Mk2 Tint Names
                string[] mk2Tints =
                {
                    "Classic Black", "Classic Gray", "Classic Two-Tone", "Classic White", "Classic Beige", "Classic Green", "Classic Blue", "Classic Earth",
                    "Classic Brown & Black", "Red Contrast", "Blue Contrast", "Yellow Contrast", "Orange Contrast", "Bold Pink", "Bold Purple & Yellow",
                    "Bold Orange", "Bold Green & Purple", "Bold Red Features", "Bold Green Features", "Bold Cyan Features", "Bold Yellow Features",
                    "Bold Red & White", "Bold Blue & White", "Metallic Gold", "Metallic Platinum", "Metallic Gray & Lilac", "Metallic Purple & Lime",
                    "Metallic Red", "Metallic Green", "Metallic Blue", "Metallic White & Aqua", "Metallic Orange & Yellow", "Metallic Red and Yellow"
                };

                // Regular tint names for non-Mk2 weapons
                string[] standardTints =
                {
                    "Default / Black", "Green", "Gold", "Pink", "Army", "LSPD", "Orange", "Platinum"
                };

                // Choose the right list
                string[] tintOptions = isMk2 ? mk2Tints : standardTints;

                var openCompMenuItem = new NativeItem("Manage Attachments");

                openCompMenuItem.Activated += (sender1, args1) =>
                {
                    var weapon = Game.Player.Character.Weapons[hash];

                    if (!weapon.IsPresent)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You need to own the ~b~{name} to customize it.", true, false);   
                        return;
                    }

                    customizeMenu.Clear(); // Reset the customize menu

                    bool hasAny = false;



                    var camos = new List<string>();
                    var camoSlides = new List<string>();
                    var clipCheckboxes = new List<NativeCheckboxItem>();
                    var suppressorCheckboxes = new List<NativeCheckboxItem>();
                    var clipComponents = new List<WeaponComponent>();
                    var otherComponents = new List<NativeCheckboxItem>();

                    foreach (var component in weapon.Components)
                    {
                        if (excludedComponents.Contains(component.ComponentHash))
                            continue;

                        string compName = component.ComponentHash.ToString()
                            .Replace("WeaponComponentHash.", "")
                            .Replace('_', ' ');

                        if (compName.ToLower().Contains("camo") && compName.ToLower().EndsWith("slide"))
                        {
                            camoSlides.Add(compName);
                        }
                        else if (compName.ToLower().Contains("camo"))
                        {
                            camos.Add(compName);
                        }
                        else if (compName.ToLower().Contains("clip"))
                        {
                            hasAny = true;

                            var clipItem = new NativeCheckboxItem(compName, component.Active);

                            clipItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (otherName.ToLower().Contains("clip"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Update UI: ensure only this clip is checked
                                foreach (var clipCheckbox in clipCheckboxes)
                                {
                                    clipCheckbox.Checked = (clipCheckbox == clipItem && newState);
                                }
                            };

                            clipCheckboxes.Add(clipItem);
                        }
                        else if (compName.ToLower().Contains("barrel"))
                        {
                            hasAny = true;

                            var barrelItem = new NativeCheckboxItem(compName, component.Active);

                            barrelItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (otherName.ToLower().Contains("barrel"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Update UI: ensure only this clip is checked
                                foreach (var barrelCheckbox in clipCheckboxes)
                                {
                                    barrelCheckbox.Checked = (barrelCheckbox == barrelItem && newState);
                                }
                            };

                            clipCheckboxes.Add(barrelItem);
                        }
                        else if (compName.ToLower().Contains("supp") || compName.ToLower().Contains("comp") || compName.ToLower().Contains("muzzle"))
                        {
                            hasAny = true;

                            var suppressorItem = new NativeCheckboxItem(compName, component.Active);

                            suppressorItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ')
                                        .ToLower();

                                    if (otherName.Contains("supp") || otherName.Contains("comp") || otherName.Contains("muzzle"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Optional: Update UI to only check this one
                                foreach (var item in otherComponents)
                                {
                                    if (item.Title.ToLower().Contains("supp") || item.Title.ToLower().Contains("comp") || item.Title.ToLower().Contains("muzzle"))
                                        item.Checked = (item == suppressorItem && newState);
                                }
                            };

                            otherComponents.Add(suppressorItem);
                        }
                        else if (compName.ToLower().Contains("scope") || compName.ToLower().Contains("sight"))
                        {
                            hasAny = true;

                            var scopeItem = new NativeCheckboxItem(compName, component.Active);

                            scopeItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ')
                                        .ToLower();

                                    if (otherName.Contains("scope") || otherName.Contains("sight"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Optional: Update UI to only check this one
                                foreach (var item in otherComponents)
                                {
                                    if (item.Title.ToLower().Contains("scope") || item.Title.ToLower().Contains("sight"))
                                        item.Checked = (item == scopeItem && newState);
                                }
                            };

                            otherComponents.Add(scopeItem);
                        }

                        else
                        {
                            hasAny = true;

                            var compItem = new NativeCheckboxItem(compName, component.Active);
                            compItem.Activated += (sender2, args2) =>
                            {
                                component.Active = !component.Active;
                            };

                            otherComponents.Add(compItem);
                        }
                    }

                    // Add all clips
                    foreach (var clip in clipCheckboxes)
                        customizeMenu.Add(clip);

                    // Add other components
                    foreach (var item in otherComponents)
                        customizeMenu.Add(item);

                    if (!hasAny && clipCheckboxes.Count == 0)
                    {
                        var noneItem = new NativeCheckboxItem("~c~No available customizations");
                        noneItem.Enabled = false;
                        customizeMenu.Add(noneItem);
                    }


                    int currentTintIndex = Function.Call<int>(Hash.GET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (uint)hash);

                    // Only add the tint selector if the weapon supports tints
                    if (currentTintIndex >= 0 && tintOptions.Length > 1)
                    {
                        var tintListItem = new NativeListItem<string>("Tint", tintOptions)
                        {
                            SelectedIndex = currentTintIndex
                        };

                        tintListItem.Activated += (senderTint, argsTint) =>
                        {
                            int selectedIndex = tintListItem.SelectedIndex;
                            Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (uint)hash, selectedIndex);
                        };

                        customizeMenu.Add(tintListItem);
                    }
                    // Add Camos List
                    if (camos.Count > 0)
                    {
                        string[] liveryColors =
                        {
                            "Default", "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7"
                        };
                        hasAny = true;
                        // Create a new array with "None" as first element + existing camos
                        var camoOptions = new string[camos.Count + 1];
                        camoOptions[0] = "None";
                        camos.CopyTo(camoOptions, 1);
                        var camoListItem = new NativeListItem<string>("Camos 01", camoOptions);
                        var liveryColorItem = new NativeListItem<string>("Camo Color", liveryColors);
                        liveryColorItem.SelectedIndex = 0;
                        liveryColorItem.Activated += (sender, args) =>
                        {
                            var ped = Game.Player.Character;
                            var weapons = Game.Player.Character.Weapons[hash];

                            if (!weapon.IsPresent) return;

                            WeaponComponent activeCamo = null;

                            foreach (var comp in weapon.Components)
                            {
                                string compName = comp.ComponentHash.ToString().Replace("WeaponComponentHash.", "").Replace('_', ' ').ToLower();

                                // Only apply to active main camos (not slide)
                                if (compName.Contains("camo") && !compName.EndsWith("slide") && comp.Active)
                                {
                                    activeCamo = comp;
                                    break;
                                }
                            }

                            if (activeCamo != null)
                            {
                                int selectedColor = liveryColorItem.SelectedIndex;
                                Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, ped, (uint)hash, (uint)activeCamo.ComponentHash, selectedColor);
                            }
                        };

                        // Find which camo is active, else select "None"
                        camoListItem.SelectedIndex = 0; // Default to None
                        for (int i = 0; i < camos.Count; i++)
                        {
                            string camo = camos[i];
                            foreach (var component in weapon.Components)
                            {
                                string compName = component.ComponentHash.ToString()
                                    .Replace("WeaponComponentHash.", "")
                                    .Replace('_', ' ');

                                if (compName == camo && component.Active)
                                {
                                    camoListItem.SelectedIndex = i + 1; // +1 because of "None" at 0
                                    break;
                                }
                            }
                        }
                        camoListItem.Activated += (sender3, args3) =>
                        {
                            string selectedCamo = camoListItem.SelectedItem;

                            if (selectedCamo == "None")
                            {
                                // Deactivate all camos
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName.ToLower().Contains("camo") && !compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                            else
                            {
                                // Activate selected camo, deactivate others
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName == selectedCamo)
                                        component.Active = true;
                                    else if (compName.ToLower().Contains("camo") && !compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                        };
                        customizeMenu.Add(camoListItem);
                        customizeMenu.Add(liveryColorItem);
                    }
                    if (camoSlides.Count > 0)
                    {
                        hasAny = true;

                        // Insert "None" option at the start
                        var camoSlideOptions = new string[camoSlides.Count + 1];
                        camoSlideOptions[0] = "None";
                        camoSlides.CopyTo(camoSlideOptions, 1);
                        string[] liveryColors =
                        {
                            "Default", "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7"
                         };
                        var camoSlideListItem = new NativeListItem<string>("Camo 02", camoSlideOptions);



                        var liveryColorItem2 = new NativeListItem<string>("Camo Color 2", liveryColors);
                        liveryColorItem2.SelectedIndex = 0;
                        liveryColorItem2.Activated += (sender2, args2) =>
                        {
                            var ped = Game.Player.Character;
                            var weapons = Game.Player.Character.Weapons[hash];

                            if (!weapon.IsPresent) return;

                            WeaponComponent activeSlideCamo = null;

                            foreach (var comp in weapon.Components)
                            {
                                string compName = comp.ComponentHash.ToString().Replace("WeaponComponentHash.", "").Replace('_', ' ').ToLower();

                                // Only apply to active slide camos
                                if (compName.EndsWith("slide") && comp.Active)
                                {
                                    activeSlideCamo = comp;
                                    break;
                                }
                            }

                            if (activeSlideCamo != null)
                            {
                                int selectedColor = liveryColorItem2.SelectedIndex;
                                Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, ped, (uint)hash, (uint)activeSlideCamo.ComponentHash, selectedColor);
                            }
                        };



                        // Default select "None"
                        camoSlideListItem.SelectedIndex = 0;

                        for (int i = 0; i < camoSlides.Count; i++)
                        {
                            string camoSlide = camoSlides[i];
                            foreach (var component in weapon.Components)
                            {
                                string compName = component.ComponentHash.ToString()
                                    .Replace("WeaponComponentHash.", "")
                                    .Replace('_', ' ');

                                if (compName == camoSlide && component.Active)
                                {
                                    camoSlideListItem.SelectedIndex = i + 1; // +1 for None at 0
                                    break;
                                }
                            }
                        }

                        camoSlideListItem.Activated += (sender4, args4) =>
                        {
                            string selectedCamoSlide = camoSlideListItem.SelectedItem;

                            if (selectedCamoSlide == "None")
                            {
                                // Deactivate all camo slides
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                            else
                            {
                                // Activate selected slide, deactivate others
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName == selectedCamoSlide)
                                        component.Active = true;
                                    else if (compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                        };

                        customizeMenu.Add(camoSlideListItem);
                        customizeMenu.Add(liveryColorItem2);
                    }

                };

                customizeMenu.Add(openCompMenuItem);

                // Only add Buy and Sell now; BuyAmmo added dynamically
                weaponMenu.Add(equipItem);
                weaponMenu.Add(buyItem);
                weaponMenu.Add(sellItem);
                weaponMenu.AddSubMenu(customizeMenu);

                shotgunsSubMenu.AddSubMenu(weaponMenu);
            }

            // Optional: remove the `Shown` refresh if handling it dynamically

            // Refresh buy/sell button states each time the menu is shown
            shotgunsSubMenu.Shown += (sender, args) =>
            {
                var weapons = Game.Player.Character.Weapons;

                foreach (var pair in shotgunsItems)
                {
                    WeaponHash hash = pair.Key;
                    var buyItem = pair.Value;
                    var sellItem = shotgunsSellItems[hash];
                    int price = ShotgunsValues[hash];

                    // Assuming you have a dictionary of equipItems for handguns:
                    var equipItem = shotgunsEquipItems[hash]; // You need to have this collection set up.

                    if (weapons.HasWeapon(hash))
                    {
                        buyItem.Enabled = false;
                        buyItem.Description = "~c~You already own this weapon";

                        sellItem.Enabled = true;
                        sellItem.Description = $"Resell for ${price}";

                        equipItem.Enabled = true;       // Enable equip option

                        equipItem.Description = "Equip this weapon";
                    }
                    else
                    {
                        buyItem.Enabled = true;
                        buyItem.Description = $"Price: ${price}";

                        sellItem.Enabled = false;
                        sellItem.Description = "~c~You do not own this weapon";

                        equipItem.Enabled = false;     // Disable equip option
                        equipItem.Description = "~c~You do not own this weapon";
                    }
                }
            };





            Dictionary<WeaponHash, NativeItem> snipersItems = new Dictionary<WeaponHash, NativeItem>();
            // Dictionary to track sell buttons
            Dictionary<WeaponHash, NativeItem> snipersSellItems = new Dictionary<WeaponHash, NativeItem>();

            Dictionary<WeaponHash, NativeItem> snipersEquipItems = new Dictionary<WeaponHash, NativeItem>();


            snipersSubMenu = new NativeMenu(
                "",
                "Sniper's",
                "",
                new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                )
                );
            pool.Add(snipersSubMenu);
            snipersSubMenu.Alignment = Alignment.Right;
            var snipersSubMenuItem = armoryMenu.AddSubMenu(snipersSubMenu);


            foreach (var handgun in SnipersValues)
            {
                WeaponHash hash = handgun.Key;
                int price = handgun.Value;
                string name = hash.ToString().Replace("WeaponHash.", "").Replace('_', ' ');

                var weaponMenu = new NativeMenu("", name, $"Manage your {name}",
                    new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                ));
                pool.Add(weaponMenu);
                weaponMenu.Alignment = Alignment.Right;
                var buyFullAmmoItem = new NativeItem("Buy All Ammo", "Buy ammunition for this weapon.");
                var buyAmmoItem = new NativeItem("Buy Ammo", "Buy ammunition for this weapon.");
                var equipItem = new NativeItem("Equip", $"Equiped ${price}");
                var buyItem = new NativeItem("Buy", $"Price: ${price}");
                var sellItem = new NativeItem("Sell", $"Resell for ${price}");

                // Store references
                snipersEquipItems[hash] = equipItem;
                snipersItems[hash] = buyItem;
                snipersSellItems[hash] = sellItem;


                equipItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~Equipped~b~ {name}", true, false);

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You already have this weapon equiped";
                };

                // Buy Ammo logic
                // Buy 50 Rounds Logic
                buyAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~You don't own the {name}. ~w~Buy the weapon first.", true, false);   
                        return;
                    }

                    int ammoPrice = 70;
                    int ammoAmount = 50;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money!~w~ You need ~b~${ammoPrice}", true, false);
                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Purchased~b~ {ammoAmount} ~w~rounds for ~r~${ammoPrice}", true, false);
                };

                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~You don't own the {name}. ~w~Buy the weapon first.", true, false);   
                        return;
                    }

                    int refillPrice = 5000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money! You need ${refillPrice}", true, false);
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Fully refilled ammo for ~r~${refillPrice}", true, false);
                };

                // Buy weapon logic
                buyItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You already own the ~b~{name}.", true, false);   
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Important", $"~r~Not enough money! ~w~You need ~b~${price}", true, false);

                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~You purchased ~g~{name}", true, false);

                    buyItem.Enabled = false;
                    buyItem.Description = "~c~You already own this weapon";

                    sellItem.Enabled = true;
                    sellItem.Description = $"Resell for ${price}";

                    if (!weaponMenu.Items.Contains(buyAmmoItem))
                        weaponMenu.Items.Insert(0, buyAmmoItem);
                    if (!weaponMenu.Items.Contains(buyFullAmmoItem))
                        weaponMenu.Items.Insert(1, buyFullAmmoItem);

                };

                // Sell weapon logic
                sellItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Sold {name} for ~g~${price}", true, false);

                    buyItem.Enabled = true;
                    buyItem.Description = $"Price: ${price}";

                    sellItem.Enabled = false;
                    sellItem.Description = "~c~You do not own this weapon";

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You do not own this weapon";

                    // Remove Buy Ammo dynamically s
                    if (weaponMenu.Items.Contains(buyAmmoItem))
                        weaponMenu.Items.Remove(buyAmmoItem);
                    if (weaponMenu.Items.Contains(buyFullAmmoItem))
                        weaponMenu.Items.Remove(buyFullAmmoItem);
                };

                var customizeMenu = new NativeMenu(
                    "",
                    $"{name} Customization",
                    $"Customize your {name}",
                    new ScaledTexture(
                        PointF.Empty,
                        new SizeF(431, 107),
                        "thumbnail_ammunation_net",
                        "ammunation_banner"
                    )
                );
                pool.Add(customizeMenu);
                customizeMenu.Alignment = Alignment.Right;
                customizeMenu.Shown += (s, e) =>
                {
                    if (!Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        customizeMenu.Visible = false;
                        weaponMenu.Visible = true;  // 👈 Show the weapon menu again
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Important", $"~r~You no longer own the {name}.", true, false);
                    }
                };
                var excludedComponents = new HashSet<WeaponComponentHash>
                    {
                        WeaponComponentHash.GunrunMk2Upgrade,
                        // Pump Shotgun Mk2
                        WeaponComponentHash.PumpShotgunMk2ClipExplosive,
                        WeaponComponentHash.PumpShotgunMk2ClipHollowPoint,
                        WeaponComponentHash.PumpShotgunMk2ClipIncendiary,
                        WeaponComponentHash.PumpShotgunMk2ClipArmorPiercing,

                        // Revolver Mk2
                        WeaponComponentHash.RevolverMk2ClipFMJ,
                        WeaponComponentHash.RevolverMk2ClipHollowPoint,
                        WeaponComponentHash.RevolverMk2ClipIncendiary,
                        WeaponComponentHash.RevolverMk2ClipTracer,

                        // SMG Mk2
                        WeaponComponentHash.SMGMk2ClipFMJ,
                        WeaponComponentHash.SMGMk2ClipHollowPoint,
                        WeaponComponentHash.SMGMk2ClipIncendiary,
                        WeaponComponentHash.SMGMk2ClipTracer,

                        // SNS Pistol Mk2
                        WeaponComponentHash.SNSPistolMk2ClipFMJ,
                        WeaponComponentHash.SNSPistolMk2ClipHollowPoint,
                        WeaponComponentHash.SNSPistolMk2ClipIncendiary,
                        WeaponComponentHash.SNSPistolMk2ClipTracer,

                        // Assault Rifle Mk2
                        WeaponComponentHash.AssaultRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.AssaultRifleMk2ClipFMJ,
                        WeaponComponentHash.AssaultRifleMk2ClipIncendiary,
                        WeaponComponentHash.AssaultRifleMk2ClipTracer,

                        // Bullpup Rifle Mk2
                        WeaponComponentHash.BullpupRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.BullpupRifleMk2ClipFMJ,
                        WeaponComponentHash.BullpupRifleMk2ClipIncendiary,
                        WeaponComponentHash.BullpupRifleMk2ClipTracer,

                        // Carbine Rifle Mk2
                        WeaponComponentHash.CarbineRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.CarbineRifleMk2ClipFMJ,
                        WeaponComponentHash.CarbineRifleMk2ClipIncendiary,
                        WeaponComponentHash.CarbineRifleMk2ClipTracer,

                        // Combat MG Mk2
                        WeaponComponentHash.CombatMGMk2ClipArmorPiercing,
                        WeaponComponentHash.CombatMGMk2ClipFMJ,
                        WeaponComponentHash.CombatMGMk2ClipIncendiary,
                        WeaponComponentHash.CombatMGMk2ClipTracer,

                        // Marksman Rifle Mk2
                        WeaponComponentHash.MarksmanRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.MarksmanRifleMk2ClipFMJ,
                        WeaponComponentHash.MarksmanRifleMk2ClipIncendiary,
                        WeaponComponentHash.MarksmanRifleMk2ClipTracer,

                        // HeavySniper Rifle Mk2
                        WeaponComponentHash.HeavySniperMk2ClipArmorPiercing,
                        WeaponComponentHash.HeavySniperMk2ClipFMJ,
                        WeaponComponentHash.HeavySniperMk2ClipIncendiary,
                        WeaponComponentHash.HeavySniperMk2ClipExplosive
                    };

                // Check if it's a Mk2 weapon (simple detection based on name)
                bool isMk2 = name.Contains("Mk2");

                // Mk2 Tint Names
                string[] mk2Tints =
                {
                    "Classic Black", "Classic Gray", "Classic Two-Tone", "Classic White", "Classic Beige", "Classic Green", "Classic Blue", "Classic Earth",
                    "Classic Brown & Black", "Red Contrast", "Blue Contrast", "Yellow Contrast", "Orange Contrast", "Bold Pink", "Bold Purple & Yellow",
                    "Bold Orange", "Bold Green & Purple", "Bold Red Features", "Bold Green Features", "Bold Cyan Features", "Bold Yellow Features",
                    "Bold Red & White", "Bold Blue & White", "Metallic Gold", "Metallic Platinum", "Metallic Gray & Lilac", "Metallic Purple & Lime",
                    "Metallic Red", "Metallic Green", "Metallic Blue", "Metallic White & Aqua", "Metallic Orange & Yellow", "Metallic Red and Yellow"
                };

                // Regular tint names for non-Mk2 weapons
                string[] standardTints =
                {
                    "Default / Black", "Green", "Gold", "Pink", "Army", "LSPD", "Orange", "Platinum"
                };

                // Choose the right list
                string[] tintOptions = isMk2 ? mk2Tints : standardTints;

                var openCompMenuItem = new NativeItem("Manage Attachments");

                openCompMenuItem.Activated += (sender1, args1) =>
                {
                    var weapon = Game.Player.Character.Weapons[hash];

                    if (!weapon.IsPresent)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You need to own the ~b~{name} to customize it.", true, false);   
                        return;
                    }

                    customizeMenu.Clear(); // Reset the customize menu

                    bool hasAny = false;



                    var camos = new List<string>();
                    var camoSlides = new List<string>();
                    var clipCheckboxes = new List<NativeCheckboxItem>();
                    var suppressorCheckboxes = new List<NativeCheckboxItem>();
                    var clipComponents = new List<WeaponComponent>();
                    var otherComponents = new List<NativeCheckboxItem>();

                    foreach (var component in weapon.Components)
                    {
                        if (excludedComponents.Contains(component.ComponentHash))
                            continue;

                        string compName = component.ComponentHash.ToString()
                            .Replace("WeaponComponentHash.", "")
                            .Replace('_', ' ');

                        if (compName.ToLower().Contains("camo") && compName.ToLower().EndsWith("slide"))
                        {
                            camoSlides.Add(compName);
                        }
                        else if (compName.ToLower().Contains("camo"))
                        {
                            camos.Add(compName);
                        }
                        else if (compName.ToLower().Contains("clip"))
                        {
                            hasAny = true;

                            var clipItem = new NativeCheckboxItem(compName, component.Active);

                            clipItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (otherName.ToLower().Contains("clip"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Update UI: ensure only this clip is checked
                                foreach (var clipCheckbox in clipCheckboxes)
                                {
                                    clipCheckbox.Checked = (clipCheckbox == clipItem && newState);
                                }
                            };

                            clipCheckboxes.Add(clipItem);
                        }
                        else if (compName.ToLower().Contains("barrel"))
                        {
                            hasAny = true;

                            var barrelItem = new NativeCheckboxItem(compName, component.Active);

                            barrelItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (otherName.ToLower().Contains("barrel"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Update UI: ensure only this clip is checked
                                foreach (var barrelCheckbox in clipCheckboxes)
                                {
                                    barrelCheckbox.Checked = (barrelCheckbox == barrelItem && newState);
                                }
                            };

                            clipCheckboxes.Add(barrelItem);
                        }
                        else if (compName.ToLower().Contains("supp") || compName.ToLower().Contains("comp") || compName.ToLower().Contains("muzzle"))
                        {
                            hasAny = true;

                            var suppressorItem = new NativeCheckboxItem(compName, component.Active);

                            suppressorItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ')
                                        .ToLower();

                                    if (otherName.Contains("supp") || otherName.Contains("comp") || otherName.Contains("muzzle"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Optional: Update UI to only check this one
                                foreach (var item in otherComponents)
                                {
                                    if (item.Title.ToLower().Contains("supp") || item.Title.ToLower().Contains("comp") || item.Title.ToLower().Contains("muzzle"))
                                        item.Checked = (item == suppressorItem && newState);
                                }
                            };

                            otherComponents.Add(suppressorItem);
                        }
                        else if (compName.ToLower().Contains("scope") || compName.ToLower().Contains("sight"))
                        {
                            hasAny = true;

                            var scopeItem = new NativeCheckboxItem(compName, component.Active);

                            scopeItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ')
                                        .ToLower();

                                    if (otherName.Contains("scope") || otherName.Contains("sight"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Optional: Update UI to only check this one
                                foreach (var item in otherComponents)
                                {
                                    if (item.Title.ToLower().Contains("scope") || item.Title.ToLower().Contains("sight"))
                                        item.Checked = (item == scopeItem && newState);
                                }
                            };

                            otherComponents.Add(scopeItem);
                        }

                        else
                        {
                            hasAny = true;

                            var compItem = new NativeCheckboxItem(compName, component.Active);
                            compItem.Activated += (sender2, args2) =>
                            {
                                component.Active = !component.Active;
                            };

                            otherComponents.Add(compItem);
                        }
                    }

                    // Add all clips
                    foreach (var clip in clipCheckboxes)
                        customizeMenu.Add(clip);

                    // Add other components
                    foreach (var item in otherComponents)
                        customizeMenu.Add(item);

                    if (!hasAny && clipCheckboxes.Count == 0)
                    {
                        var noneItem = new NativeCheckboxItem("~c~No available customizations");
                        noneItem.Enabled = false;
                        customizeMenu.Add(noneItem);
                    }


                    int currentTintIndex = Function.Call<int>(Hash.GET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (uint)hash);

                    // Only add the tint selector if the weapon supports tints
                    if (currentTintIndex >= 0 && tintOptions.Length > 1)
                    {
                        var tintListItem = new NativeListItem<string>("Tint", tintOptions)
                        {
                            SelectedIndex = currentTintIndex
                        };

                        tintListItem.Activated += (senderTint, argsTint) =>
                        {
                            int selectedIndex = tintListItem.SelectedIndex;
                            Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (uint)hash, selectedIndex);
                        };

                        customizeMenu.Add(tintListItem);
                    }
                    // Add Camos List
                    if (camos.Count > 0)
                    {
                        string[] liveryColors =
                        {
                            "Default", "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7"
                        };
                        hasAny = true;
                        // Create a new array with "None" as first element + existing camos
                        var camoOptions = new string[camos.Count + 1];
                        camoOptions[0] = "None";
                        camos.CopyTo(camoOptions, 1);
                        var camoListItem = new NativeListItem<string>("Camos 01", camoOptions);
                        var liveryColorItem = new NativeListItem<string>("Camo Color", liveryColors);
                        liveryColorItem.SelectedIndex = 0;
                        liveryColorItem.Activated += (sender, args) =>
                        {
                            var ped = Game.Player.Character;
                            var weapons = Game.Player.Character.Weapons[hash];

                            if (!weapon.IsPresent) return;

                            WeaponComponent activeCamo = null;

                            foreach (var comp in weapon.Components)
                            {
                                string compName = comp.ComponentHash.ToString().Replace("WeaponComponentHash.", "").Replace('_', ' ').ToLower();

                                // Only apply to active main camos (not slide)
                                if (compName.Contains("camo") && !compName.EndsWith("slide") && comp.Active)
                                {
                                    activeCamo = comp;
                                    break;
                                }
                            }

                            if (activeCamo != null)
                            {
                                int selectedColor = liveryColorItem.SelectedIndex;
                                Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, ped, (uint)hash, (uint)activeCamo.ComponentHash, selectedColor);
                            }
                        };

                        // Find which camo is active, else select "None"
                        camoListItem.SelectedIndex = 0; // Default to None
                        for (int i = 0; i < camos.Count; i++)
                        {
                            string camo = camos[i];
                            foreach (var component in weapon.Components)
                            {
                                string compName = component.ComponentHash.ToString()
                                    .Replace("WeaponComponentHash.", "")
                                    .Replace('_', ' ');

                                if (compName == camo && component.Active)
                                {
                                    camoListItem.SelectedIndex = i + 1; // +1 because of "None" at 0
                                    break;
                                }
                            }
                        }
                        camoListItem.Activated += (sender3, args3) =>
                        {
                            string selectedCamo = camoListItem.SelectedItem;

                            if (selectedCamo == "None")
                            {
                                // Deactivate all camos
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName.ToLower().Contains("camo") && !compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                            else
                            {
                                // Activate selected camo, deactivate others
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName == selectedCamo)
                                        component.Active = true;
                                    else if (compName.ToLower().Contains("camo") && !compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                        };
                        customizeMenu.Add(camoListItem);
                        customizeMenu.Add(liveryColorItem);
                    }
                    if (camoSlides.Count > 0)
                    {
                        hasAny = true;

                        // Insert "None" option at the start
                        var camoSlideOptions = new string[camoSlides.Count + 1];
                        camoSlideOptions[0] = "None";
                        camoSlides.CopyTo(camoSlideOptions, 1);
                        string[] liveryColors =
                        {
                            "Default", "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7"
                         };
                        var camoSlideListItem = new NativeListItem<string>("Camo 02", camoSlideOptions);



                        var liveryColorItem2 = new NativeListItem<string>("Camo Color 2", liveryColors);
                        liveryColorItem2.SelectedIndex = 0;
                        liveryColorItem2.Activated += (sender2, args2) =>
                        {
                            var ped = Game.Player.Character;
                            var weapons = Game.Player.Character.Weapons[hash];

                            if (!weapon.IsPresent) return;

                            WeaponComponent activeSlideCamo = null;

                            foreach (var comp in weapon.Components)
                            {
                                string compName = comp.ComponentHash.ToString().Replace("WeaponComponentHash.", "").Replace('_', ' ').ToLower();

                                // Only apply to active slide camos
                                if (compName.EndsWith("slide") && comp.Active)
                                {
                                    activeSlideCamo = comp;
                                    break;
                                }
                            }

                            if (activeSlideCamo != null)
                            {
                                int selectedColor = liveryColorItem2.SelectedIndex;
                                Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, ped, (uint)hash, (uint)activeSlideCamo.ComponentHash, selectedColor);
                            }
                        };



                        // Default select "None"
                        camoSlideListItem.SelectedIndex = 0;

                        for (int i = 0; i < camoSlides.Count; i++)
                        {
                            string camoSlide = camoSlides[i];
                            foreach (var component in weapon.Components)
                            {
                                string compName = component.ComponentHash.ToString()
                                    .Replace("WeaponComponentHash.", "")
                                    .Replace('_', ' ');

                                if (compName == camoSlide && component.Active)
                                {
                                    camoSlideListItem.SelectedIndex = i + 1; // +1 for None at 0
                                    break;
                                }
                            }
                        }

                        camoSlideListItem.Activated += (sender4, args4) =>
                        {
                            string selectedCamoSlide = camoSlideListItem.SelectedItem;

                            if (selectedCamoSlide == "None")
                            {
                                // Deactivate all camo slides
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                            else
                            {
                                // Activate selected slide, deactivate others
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName == selectedCamoSlide)
                                        component.Active = true;
                                    else if (compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                        };

                        customizeMenu.Add(camoSlideListItem);
                        customizeMenu.Add(liveryColorItem2);
                    }

                };

                customizeMenu.Add(openCompMenuItem);

                // Only add Buy and Sell now; BuyAmmo added dynamically
                weaponMenu.Add(equipItem);
                weaponMenu.Add(buyItem);
                weaponMenu.Add(sellItem);
                weaponMenu.AddSubMenu(customizeMenu);

                snipersSubMenu.AddSubMenu(weaponMenu);
            }

            // Optional: remove the `Shown` refresh if handling it dynamically

            // Refresh buy/sell button states each time the menu is shown
            snipersSubMenu.Shown += (sender, args) =>
            {
                var weapons = Game.Player.Character.Weapons;

                foreach (var pair in snipersItems)
                {
                    WeaponHash hash = pair.Key;
                    var buyItem = pair.Value;
                    var sellItem = snipersSellItems[hash];
                    int price = SnipersValues[hash];

                    // Assuming you have a dictionary of equipItems for handguns:
                    var equipItem = snipersEquipItems[hash]; // You need to have this collection set up.

                    if (weapons.HasWeapon(hash))
                    {
                        buyItem.Enabled = false;
                        buyItem.Description = "~c~You already own this weapon";

                        sellItem.Enabled = true;
                        sellItem.Description = $"Resell for ${price}";

                        equipItem.Enabled = true;       // Enable equip option

                        equipItem.Description = "Equip this weapon";
                    }
                    else
                    {
                        buyItem.Enabled = true;
                        buyItem.Description = $"Price: ${price}";

                        sellItem.Enabled = false;
                        sellItem.Description = "~c~You do not own this weapon";

                        equipItem.Enabled = false;     // Disable equip option
                        equipItem.Description = "~c~You do not own this weapon";
                    }
                }
            };




            Dictionary<WeaponHash, NativeItem> heavyweaponItems = new Dictionary<WeaponHash, NativeItem>();
            // Dictionary to track sell buttons
            Dictionary<WeaponHash, NativeItem> heavyweaponSellItems = new Dictionary<WeaponHash, NativeItem>();

            Dictionary<WeaponHash, NativeItem> heavyweaponEquipItems = new Dictionary<WeaponHash, NativeItem>();




            heavyweaponSubMenu = new NativeMenu(
                "",
                "Heavy Weapon's",
                "",
                new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                )
                );
            pool.Add(heavyweaponSubMenu);
            heavyweaponSubMenu.Alignment = Alignment.Right;
            var heavyweaponSubMenuItem = armoryMenu.AddSubMenu(heavyweaponSubMenu);


            foreach (var handgun in HeavyWeaponValues)
            {
                WeaponHash hash = handgun.Key;
                int price = handgun.Value;
                string name = hash.ToString().Replace("WeaponHash.", "").Replace('_', ' ');

                var weaponMenu = new NativeMenu("", name, $"Manage your {name}",
                    new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                ));
                pool.Add(weaponMenu);
                weaponMenu.Alignment = Alignment.Right;
                var buyFullAmmoItem = new NativeItem("Buy All Ammo", "Buy ammunition for this weapon.");
                var buyAmmoItem = new NativeItem("Buy Ammo", "Buy ammunition for this weapon.");
                var equipItem = new NativeItem("Equip", $"Equiped ${price}");
                var buyItem = new NativeItem("Buy", $"Price: ${price}");
                var sellItem = new NativeItem("Sell", $"Resell for ${price}");

                // Store references
                heavyweaponEquipItems[hash] = equipItem;
                heavyweaponItems[hash] = buyItem;
                heavyweaponSellItems[hash] = sellItem;


                equipItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~Equipped~b~ {name}", true, false);

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You already have this weapon equiped";
                };

                // Buy Ammo logic
                // Buy 50 Rounds Logic
                buyAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~You don't own the {name}. ~w~Buy the weapon first.", true, false);   
                        return;
                    }

                    int ammoPrice = 70;
                    int ammoAmount = 50;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money!~w~ You need ~b~${ammoPrice}", true, false);
                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Purchased~b~ {ammoAmount} ~w~rounds for ~r~${ammoPrice}", true, false);
                };

                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~You don't own the {name}. ~w~Buy the weapon first.", true, false);   
                        return;
                    }

                    int refillPrice = 5000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money! You need ${refillPrice}", true, false);
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Fully refilled ammo for ~r~${refillPrice}", true, false);
                };

                // Buy weapon logic
                buyItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You already own the ~b~{name}.", true, false);   
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money!~w~ You need ~b~${price}", true, false);
                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~You purchased ~g~{name}", true, false);

                    buyItem.Enabled = false;
                    buyItem.Description = "~c~You already own this weapon";

                    sellItem.Enabled = true;
                    sellItem.Description = $"Resell for ${price}";

                    if (!weaponMenu.Items.Contains(buyAmmoItem))
                        weaponMenu.Items.Insert(0, buyAmmoItem);
                    if (!weaponMenu.Items.Contains(buyFullAmmoItem))
                        weaponMenu.Items.Insert(1, buyFullAmmoItem);

                };

                // Sell weapon logic
                sellItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Sold {name} for ~g~${price}", true, false);

                    buyItem.Enabled = true;
                    buyItem.Description = $"Price: ${price}";

                    sellItem.Enabled = false;
                    sellItem.Description = "~c~You do not own this weapon";

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You do not own this weapon";

                    // Remove Buy Ammo dynamically
                    if (weaponMenu.Items.Contains(buyAmmoItem))
                        weaponMenu.Items.Remove(buyAmmoItem);
                    if (weaponMenu.Items.Contains(buyFullAmmoItem))
                        weaponMenu.Items.Remove(buyFullAmmoItem);
                };

                var customizeMenu = new NativeMenu(
                    "",
                    $"{name} Customization",
                    $"Customize your {name}",
                    new ScaledTexture(
                        PointF.Empty,
                        new SizeF(431, 107),
                        "thumbnail_ammunation_net",
                        "ammunation_banner"
                    )
                );
                pool.Add(customizeMenu);
                customizeMenu.Alignment = Alignment.Right;
                customizeMenu.Shown += (s, e) =>
                {
                    if (!Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        customizeMenu.Visible = false;
                        weaponMenu.Visible = true;  // 👈 Show the weapon menu again
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Important", $"~r~You no longer own the {name}.", true, false);
                    }
                };
                var excludedComponents = new HashSet<WeaponComponentHash>
                    {
                        WeaponComponentHash.GunrunMk2Upgrade,
                        // Pump Shotgun Mk2
                        WeaponComponentHash.PumpShotgunMk2ClipExplosive,
                        WeaponComponentHash.PumpShotgunMk2ClipHollowPoint,
                        WeaponComponentHash.PumpShotgunMk2ClipIncendiary,
                        WeaponComponentHash.PumpShotgunMk2ClipArmorPiercing,

                        // Revolver Mk2
                        WeaponComponentHash.RevolverMk2ClipFMJ,
                        WeaponComponentHash.RevolverMk2ClipHollowPoint,
                        WeaponComponentHash.RevolverMk2ClipIncendiary,
                        WeaponComponentHash.RevolverMk2ClipTracer,

                        // SMG Mk2
                        WeaponComponentHash.SMGMk2ClipFMJ,
                        WeaponComponentHash.SMGMk2ClipHollowPoint,
                        WeaponComponentHash.SMGMk2ClipIncendiary,
                        WeaponComponentHash.SMGMk2ClipTracer,

                        // SNS Pistol Mk2
                        WeaponComponentHash.SNSPistolMk2ClipFMJ,
                        WeaponComponentHash.SNSPistolMk2ClipHollowPoint,
                        WeaponComponentHash.SNSPistolMk2ClipIncendiary,
                        WeaponComponentHash.SNSPistolMk2ClipTracer,

                        // Assault Rifle Mk2
                        WeaponComponentHash.AssaultRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.AssaultRifleMk2ClipFMJ,
                        WeaponComponentHash.AssaultRifleMk2ClipIncendiary,
                        WeaponComponentHash.AssaultRifleMk2ClipTracer,

                        // Bullpup Rifle Mk2
                        WeaponComponentHash.BullpupRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.BullpupRifleMk2ClipFMJ,
                        WeaponComponentHash.BullpupRifleMk2ClipIncendiary,
                        WeaponComponentHash.BullpupRifleMk2ClipTracer,

                        // Carbine Rifle Mk2
                        WeaponComponentHash.CarbineRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.CarbineRifleMk2ClipFMJ,
                        WeaponComponentHash.CarbineRifleMk2ClipIncendiary,
                        WeaponComponentHash.CarbineRifleMk2ClipTracer,

                        // Combat MG Mk2
                        WeaponComponentHash.CombatMGMk2ClipArmorPiercing,
                        WeaponComponentHash.CombatMGMk2ClipFMJ,
                        WeaponComponentHash.CombatMGMk2ClipIncendiary,
                        WeaponComponentHash.CombatMGMk2ClipTracer,

                        // Marksman Rifle Mk2
                        WeaponComponentHash.MarksmanRifleMk2ClipArmorPiercing,
                        WeaponComponentHash.MarksmanRifleMk2ClipFMJ,
                        WeaponComponentHash.MarksmanRifleMk2ClipIncendiary,
                        WeaponComponentHash.MarksmanRifleMk2ClipTracer,

                        // HeavySniper Rifle Mk2
                        WeaponComponentHash.HeavySniperMk2ClipArmorPiercing,
                        WeaponComponentHash.HeavySniperMk2ClipFMJ,
                        WeaponComponentHash.HeavySniperMk2ClipIncendiary,
                        WeaponComponentHash.HeavySniperMk2ClipExplosive
                    };

                // Check if it's a Mk2 weapon (simple detection based on name)
                bool isMk2 = name.Contains("Mk2");

                // Mk2 Tint Names
                string[] mk2Tints =
                {
                    "Classic Black", "Classic Gray", "Classic Two-Tone", "Classic White", "Classic Beige", "Classic Green", "Classic Blue", "Classic Earth",
                    "Classic Brown & Black", "Red Contrast", "Blue Contrast", "Yellow Contrast", "Orange Contrast", "Bold Pink", "Bold Purple & Yellow",
                    "Bold Orange", "Bold Green & Purple", "Bold Red Features", "Bold Green Features", "Bold Cyan Features", "Bold Yellow Features",
                    "Bold Red & White", "Bold Blue & White", "Metallic Gold", "Metallic Platinum", "Metallic Gray & Lilac", "Metallic Purple & Lime",
                    "Metallic Red", "Metallic Green", "Metallic Blue", "Metallic White & Aqua", "Metallic Orange & Yellow", "Metallic Red and Yellow"
                };

                // Regular tint names for non-Mk2 weapons
                string[] standardTints =
                {
                    "Default / Black", "Green", "Gold", "Pink", "Army", "LSPD", "Orange", "Platinum"
                };

                // Choose the right list
                string[] tintOptions = isMk2 ? mk2Tints : standardTints;

                var openCompMenuItem = new NativeItem("Manage Attachments");

                openCompMenuItem.Activated += (sender1, args1) =>
                {
                    var weapon = Game.Player.Character.Weapons[hash];

                    if (!weapon.IsPresent)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You need to own the ~b~{name} to customize it.", true, false);   
                        return;
                    }

                    customizeMenu.Clear(); // Reset the customize menu

                    bool hasAny = false;



                    var camos = new List<string>();
                    var camoSlides = new List<string>();
                    var clipCheckboxes = new List<NativeCheckboxItem>();
                    var suppressorCheckboxes = new List<NativeCheckboxItem>();
                    var clipComponents = new List<WeaponComponent>();
                    var otherComponents = new List<NativeCheckboxItem>();

                    foreach (var component in weapon.Components)
                    {
                        if (excludedComponents.Contains(component.ComponentHash))
                            continue;

                        string compName = component.ComponentHash.ToString()
                            .Replace("WeaponComponentHash.", "")
                            .Replace('_', ' ');

                        if (compName.ToLower().Contains("camo") && compName.ToLower().EndsWith("slide"))
                        {
                            camoSlides.Add(compName);
                        }
                        else if (compName.ToLower().Contains("camo"))
                        {
                            camos.Add(compName);
                        }
                        else if (compName.ToLower().Contains("clip"))
                        {
                            hasAny = true;

                            var clipItem = new NativeCheckboxItem(compName, component.Active);

                            clipItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (otherName.ToLower().Contains("clip"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Update UI: ensure only this clip is checked
                                foreach (var clipCheckbox in clipCheckboxes)
                                {
                                    clipCheckbox.Checked = (clipCheckbox == clipItem && newState);
                                }
                            };

                            clipCheckboxes.Add(clipItem);
                        }
                        else if (compName.ToLower().Contains("barrel"))
                        {
                            hasAny = true;

                            var barrelItem = new NativeCheckboxItem(compName, component.Active);

                            barrelItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (otherName.ToLower().Contains("barrel"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Update UI: ensure only this clip is checked
                                foreach (var barrelCheckbox in clipCheckboxes)
                                {
                                    barrelCheckbox.Checked = (barrelCheckbox == barrelItem && newState);
                                }
                            };

                            clipCheckboxes.Add(barrelItem);
                        }
                        else if (compName.ToLower().Contains("supp") || compName.ToLower().Contains("comp") || compName.ToLower().Contains("muzzle"))
                        {
                            hasAny = true;

                            var suppressorItem = new NativeCheckboxItem(compName, component.Active);

                            suppressorItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ')
                                        .ToLower();

                                    if (otherName.Contains("supp") || otherName.Contains("comp") || otherName.Contains("muzzle"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Optional: Update UI to only check this one
                                foreach (var item in otherComponents)
                                {
                                    if (item.Title.ToLower().Contains("supp") || item.Title.ToLower().Contains("comp") || item.Title.ToLower().Contains("muzzle"))
                                        item.Checked = (item == suppressorItem && newState);
                                }
                            };

                            otherComponents.Add(suppressorItem);
                        }
                        else if (compName.ToLower().Contains("scope") || compName.ToLower().Contains("sight"))
                        {
                            hasAny = true;

                            var scopeItem = new NativeCheckboxItem(compName, component.Active);

                            scopeItem.Activated += (sender2, args2) =>
                            {
                                bool newState = !component.Active;

                                foreach (var otherComp in weapon.Components)
                                {
                                    string otherName = otherComp.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ')
                                        .ToLower();

                                    if (otherName.Contains("scope") || otherName.Contains("sight"))
                                        otherComp.Active = false;
                                }

                                component.Active = newState;

                                // Optional: Update UI to only check this one
                                foreach (var item in otherComponents)
                                {
                                    if (item.Title.ToLower().Contains("scope") || item.Title.ToLower().Contains("sight"))
                                        item.Checked = (item == scopeItem && newState);
                                }
                            };

                            otherComponents.Add(scopeItem);
                        }

                        else
                        {
                            hasAny = true;

                            var compItem = new NativeCheckboxItem(compName, component.Active);
                            compItem.Activated += (sender2, args2) =>
                            {
                                component.Active = !component.Active;
                            };

                            otherComponents.Add(compItem);
                        }
                    }

                    // Add all clips
                    foreach (var clip in clipCheckboxes)
                        customizeMenu.Add(clip);

                    // Add other components
                    foreach (var item in otherComponents)
                        customizeMenu.Add(item);

                    if (!hasAny && clipCheckboxes.Count == 0)
                    {
                        var noneItem = new NativeCheckboxItem("~c~No available customizations");
                        noneItem.Enabled = false;
                        customizeMenu.Add(noneItem);
                    }


                    int currentTintIndex = Function.Call<int>(Hash.GET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (uint)hash);

                    // Only add the tint selector if the weapon supports tints
                    if (currentTintIndex >= 0 && tintOptions.Length > 1)
                    {
                        var tintListItem = new NativeListItem<string>("Tint", tintOptions)
                        {
                            SelectedIndex = currentTintIndex
                        };

                        tintListItem.Activated += (senderTint, argsTint) =>
                        {
                            int selectedIndex = tintListItem.SelectedIndex;
                            Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (uint)hash, selectedIndex);
                        };

                        customizeMenu.Add(tintListItem);
                    }
                    // Add Camos List
                    if (camos.Count > 0)
                    {
                        string[] liveryColors =
                        {
                            "Default", "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7"
                        };
                        hasAny = true;
                        // Create a new array with "None" as first element + existing camos
                        var camoOptions = new string[camos.Count + 1];
                        camoOptions[0] = "None";
                        camos.CopyTo(camoOptions, 1);
                        var camoListItem = new NativeListItem<string>("Camos 01", camoOptions);
                        var liveryColorItem = new NativeListItem<string>("Camo Color", liveryColors);
                        liveryColorItem.SelectedIndex = 0;
                        liveryColorItem.Activated += (sender, args) =>
                        {
                            var ped = Game.Player.Character;
                            var weapons = Game.Player.Character.Weapons[hash];

                            if (!weapon.IsPresent) return;

                            WeaponComponent activeCamo = null;

                            foreach (var comp in weapon.Components)
                            {
                                string compName = comp.ComponentHash.ToString().Replace("WeaponComponentHash.", "").Replace('_', ' ').ToLower();

                                // Only apply to active main camos (not slide)
                                if (compName.Contains("camo") && !compName.EndsWith("slide") && comp.Active)
                                {
                                    activeCamo = comp;
                                    break;
                                }
                            }

                            if (activeCamo != null)
                            {
                                int selectedColor = liveryColorItem.SelectedIndex;
                                Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, ped, (uint)hash, (uint)activeCamo.ComponentHash, selectedColor);
                            }
                        };

                        // Find which camo is active, else select "None"
                        camoListItem.SelectedIndex = 0; // Default to None
                        for (int i = 0; i < camos.Count; i++)
                        {
                            string camo = camos[i];
                            foreach (var component in weapon.Components)
                            {
                                string compName = component.ComponentHash.ToString()
                                    .Replace("WeaponComponentHash.", "")
                                    .Replace('_', ' ');

                                if (compName == camo && component.Active)
                                {
                                    camoListItem.SelectedIndex = i + 1; // +1 because of "None" at 0
                                    break;
                                }
                            }
                        }
                        camoListItem.Activated += (sender3, args3) =>
                        {
                            string selectedCamo = camoListItem.SelectedItem;

                            if (selectedCamo == "None")
                            {
                                // Deactivate all camos
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName.ToLower().Contains("camo") && !compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                            else
                            {
                                // Activate selected camo, deactivate others
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName == selectedCamo)
                                        component.Active = true;
                                    else if (compName.ToLower().Contains("camo") && !compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                        };
                        customizeMenu.Add(camoListItem);
                        customizeMenu.Add(liveryColorItem);
                    }
                    if (camoSlides.Count > 0)
                    {
                        hasAny = true;

                        // Insert "None" option at the start
                        var camoSlideOptions = new string[camoSlides.Count + 1];
                        camoSlideOptions[0] = "None";
                        camoSlides.CopyTo(camoSlideOptions, 1);
                        string[] liveryColors =
                        {
                            "Default", "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7"
                         };
                        var camoSlideListItem = new NativeListItem<string>("Camo 02", camoSlideOptions);



                        var liveryColorItem2 = new NativeListItem<string>("Camo Color 2", liveryColors);
                        liveryColorItem2.SelectedIndex = 0;
                        liveryColorItem2.Activated += (sender2, args2) =>
                        {
                            var ped = Game.Player.Character;
                            var weapons = Game.Player.Character.Weapons[hash];

                            if (!weapon.IsPresent) return;

                            WeaponComponent activeSlideCamo = null;

                            foreach (var comp in weapon.Components)
                            {
                                string compName = comp.ComponentHash.ToString().Replace("WeaponComponentHash.", "").Replace('_', ' ').ToLower();

                                // Only apply to active slide camos
                                if (compName.EndsWith("slide") && comp.Active)
                                {
                                    activeSlideCamo = comp;
                                    break;
                                }
                            }

                            if (activeSlideCamo != null)
                            {
                                int selectedColor = liveryColorItem2.SelectedIndex;
                                Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, ped, (uint)hash, (uint)activeSlideCamo.ComponentHash, selectedColor);
                            }
                        };



                        // Default select "None"
                        camoSlideListItem.SelectedIndex = 0;

                        for (int i = 0; i < camoSlides.Count; i++)
                        {
                            string camoSlide = camoSlides[i];
                            foreach (var component in weapon.Components)
                            {
                                string compName = component.ComponentHash.ToString()
                                    .Replace("WeaponComponentHash.", "")
                                    .Replace('_', ' ');

                                if (compName == camoSlide && component.Active)
                                {
                                    camoSlideListItem.SelectedIndex = i + 1; // +1 for None at 0
                                    break;
                                }
                            }
                        }

                        camoSlideListItem.Activated += (sender4, args4) =>
                        {
                            string selectedCamoSlide = camoSlideListItem.SelectedItem;

                            if (selectedCamoSlide == "None")
                            {
                                // Deactivate all camo slides
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                            else
                            {
                                // Activate selected slide, deactivate others
                                foreach (var component in weapon.Components)
                                {
                                    string compName = component.ComponentHash.ToString()
                                        .Replace("WeaponComponentHash.", "")
                                        .Replace('_', ' ');

                                    if (compName == selectedCamoSlide)
                                        component.Active = true;
                                    else if (compName.ToLower().EndsWith("slide"))
                                        component.Active = false;
                                }
                            }
                        };

                        customizeMenu.Add(camoSlideListItem);
                        customizeMenu.Add(liveryColorItem2);
                    }

                };

                customizeMenu.Add(openCompMenuItem);

                // Only add Buy and Sell now; BuyAmmo added dynamically
                weaponMenu.Add(equipItem);
                weaponMenu.Add(buyItem);
                weaponMenu.Add(sellItem);
                weaponMenu.AddSubMenu(customizeMenu);

                heavyweaponSubMenu.AddSubMenu(weaponMenu);
            }

            // Optional: remove the `Shown` refresh if handling it dynamically

            // Refresh buy/sell button states each time the menu is shown
            heavyweaponSubMenu.Shown += (sender, args) =>
            {
                var weapons = Game.Player.Character.Weapons;

                foreach (var pair in heavyweaponItems)
                {
                    WeaponHash hash = pair.Key;
                    var buyItem = pair.Value;
                    var sellItem = heavyweaponSellItems[hash];
                    int price = HeavyWeaponValues[hash];

                    // Assuming you have a dictionary of equipItems for handguns:
                    var equipItem = heavyweaponEquipItems[hash]; // You need to have this collection set up.

                    if (weapons.HasWeapon(hash))
                    {
                        buyItem.Enabled = false;
                        buyItem.Description = "~c~You already own this weapon";

                        sellItem.Enabled = true;
                        sellItem.Description = $"Resell for ${price}";

                        equipItem.Enabled = true;       // Enable equip option

                        equipItem.Description = "Equip this weapon";
                    }
                    else
                    {
                        buyItem.Enabled = true;
                        buyItem.Description = $"Price: ${price}";

                        sellItem.Enabled = false;
                        sellItem.Description = "~c~You do not own this weapon";

                        equipItem.Enabled = false;     // Disable equip option
                        equipItem.Description = "~c~You do not own this weapon";
                    }
                }
            };






            Dictionary<WeaponHash, NativeItem> throwableItems = new Dictionary<WeaponHash, NativeItem>();
            // Dictionary to track sell buttons
            Dictionary<WeaponHash, NativeItem> throwableSellItems = new Dictionary<WeaponHash, NativeItem>();

            Dictionary<WeaponHash, NativeItem> throwableEquipItems = new Dictionary<WeaponHash, NativeItem>();



            throwableSubMenu = new NativeMenu(
                "",
                "Throwable's",
                "",
                new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                )
                );
            pool.Add(throwableSubMenu);
            throwableSubMenu.Alignment = Alignment.Right;
            var throwableSubMenuItem = armoryMenu.AddSubMenu(throwableSubMenu);


            foreach (var handgun in ThrowablesValues)
            {
                WeaponHash hash = handgun.Key;
                int price = handgun.Value;
                string name = hash.ToString().Replace("WeaponHash.", "").Replace('_', ' ');

                var weaponMenu = new NativeMenu("", name, $"Manage your {name}",
                    new ScaledTexture(
                    PointF.Empty,
                    new SizeF(431, 107),
                    "thumbnail_ammunation_net",
                    "ammunation_banner"
                ));
                pool.Add(weaponMenu);
                weaponMenu.Alignment = Alignment.Right;
                var buyFullAmmoItem = new NativeItem("Buy All Ammo", "Buy ammunition for this weapon.");
                var buyAmmoItem = new NativeItem("Buy Ammo", "Buy ammunition for this weapon.");
                var equipItem = new NativeItem("Equip", $"Equiped ${price}");
                var buyItem = new NativeItem("Buy", $"Price: ${price}");
                var sellItem = new NativeItem("Sell", $"Resell for ${price}");

                // Store references
                throwableEquipItems[hash] = equipItem;
                throwableItems[hash] = buyItem;
                throwableSellItems[hash] = sellItem;


                equipItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~Equipped~b~ {name}", true, false);

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You already have this weapon equiped";
                };

                // Buy Ammo logic
                // Buy 50 Rounds Logic
                buyAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~You don't own the {name}. ~w~Buy the weapon first.", true, false);   
                        return;
                    }

                    int ammoPrice = 50;
                    int ammoAmount = 1;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money!~w~ You need ~b~${ammoPrice}", true, false);
                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Purchased~b~ {ammoAmount} ~w~rounds for ~r~${ammoPrice}", true, false);
                };

                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~r~You don't own the {name}. ~w~Buy the weapon first.", true, false);   
                        return;
                    }

                    int refillPrice = 3000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money! You need ${refillPrice}", true, false);
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;

                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Fully refilled ammo for ~r~${refillPrice}", true, false);
                };

                // Buy weapon logic
                buyItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You already own the ~b~{name}.", true, false);   
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Insufficient Funds", $"~r~Not enough money!~w~ You need ~b~${price}", true, false);
                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~You purchased ~g~{name}", true, false);

                    buyItem.Enabled = false;
                    buyItem.Description = "~c~You already own this weapon";

                    sellItem.Enabled = true;
                    sellItem.Description = $"Resell for ${price}";

                    if (!weaponMenu.Items.Contains(buyAmmoItem))
                        weaponMenu.Items.Insert(0, buyAmmoItem);
                    if (!weaponMenu.Items.Contains(buyFullAmmoItem))
                        weaponMenu.Items.Insert(1, buyFullAmmoItem);

                };

                // Sell weapon logic
                sellItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Alert", $"~w~You don't own the ~b~{name}.", true, false);    
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show(GTA.UI.NotificationIcon.Ammunation, "Ammunation", "Receipt", $"~w~Sold {name} for ~g~${price}", true, false);

                    buyItem.Enabled = true;
                    buyItem.Description = $"Price: ${price}";

                    sellItem.Enabled = false;
                    sellItem.Description = "~c~You do not own this weapon";

                    equipItem.Enabled = false;
                    buyItem.Description = "~c~You do not own this weapon";

                    // Remove Buy Ammo dynamically
                    if (weaponMenu.Items.Contains(buyAmmoItem))
                        weaponMenu.Items.Remove(buyAmmoItem);
                    if (weaponMenu.Items.Contains(buyFullAmmoItem))
                        weaponMenu.Items.Remove(buyFullAmmoItem);
                };

                // Only add Buy and Sell now; BuyAmmo added dynamically
                weaponMenu.Add(equipItem);
                weaponMenu.Add(buyItem);
                weaponMenu.Add(sellItem);
                throwableSubMenu.AddSubMenu(weaponMenu);
            }

            // Optional: remove the `Shown` refresh if handling it dynamically

            // Refresh buy/sell button states each time the menu is shown
            throwableSubMenu.Shown += (sender, args) =>
            {
                var weapons = Game.Player.Character.Weapons;

                foreach (var pair in throwableItems)
                {
                    WeaponHash hash = pair.Key;
                    var buyItem = pair.Value;
                    var sellItem = throwableSellItems[hash];
                    int price = ThrowablesValues[hash];

                    // Assuming you have a dictionary of equipItems for handguns:
                    var equipItem = throwableEquipItems[hash]; // You need to have this collection set up.

                    if (weapons.HasWeapon(hash))
                    {
                        buyItem.Enabled = false;
                        buyItem.Description = "~c~You already own this weapon";

                        sellItem.Enabled = true;
                        sellItem.Description = $"Resell for ${price}";

                        equipItem.Enabled = true;       // Enable equip option

                        equipItem.Description = "Equip this weapon";
                    }
                    else
                    {
                        buyItem.Enabled = true;
                        buyItem.Description = $"Price: ${price}";

                        sellItem.Enabled = false;
                        sellItem.Description = "~c~You do not own this weapon";

                        equipItem.Enabled = false;     // Disable equip option
                        equipItem.Description = "~c~You do not own this weapon";
                    }
                }
            };







        }
        private BlipSprite GetBlipSprite(string spriteString)
        {
            switch (spriteString.ToLower())
            {
                case "ammunation":
                    return BlipSprite.AmmuNation;
                case "ammunationshootingrange":
                    return BlipSprite.AmmuNationShootingRange;
                default:
                    return BlipSprite.Standard; // This is the standard dot
            }
        }
        private BlipColor GetBlipColor(string colorString)
        {
            switch (colorString.ToLower())
            {
                case "white":
                    return BlipColor.White;
                case "red":
                    return BlipColor.Red;
                case "green":
                    return BlipColor.Green;
                case "blue":
                    return BlipColor.Blue;
                case "yellow":
                    return BlipColor.Yellow;
                case "whitenotpure":
                    return BlipColor.WhiteNotPure;
                case "yellow2":
                    return BlipColor.Yellow2;
                case "greydark":
                    return BlipColor.GreyDark;
                case "redlight":
                    return BlipColor.RedLight;
                case "purple":
                    return BlipColor.Purple;
                case "orange":
                    return BlipColor.Orange;
                case "greendark":
                    return BlipColor.GreenDark;
                case "bluelight":
                    return BlipColor.BlueLight;
                case "bluedark":
                    return BlipColor.BlueDark;
                case "grey":
                    return BlipColor.Grey;
                case "yellowdark":
                    return BlipColor.YellowDark;
                case "pink":
                    return BlipColor.Pink;
                case "greylight":
                    return BlipColor.GreyLight;
                case "blue3":
                    return BlipColor.Blue3;
                case "blue4":
                    return BlipColor.Blue4;
                case "green2":
                    return BlipColor.Green2;
                case "yellow4":
                    return BlipColor.Yellow4;
                case "yellow5":
                    return BlipColor.Yellow5;
                case "white2":
                    return BlipColor.White2;
                case "yellow6":
                    return BlipColor.Yellow6;
                case "blue5":
                    return BlipColor.Blue5;
                case "red4":
                    return BlipColor.Red4;
                case "reddark":
                    return BlipColor.RedDark;
                case "blue6":
                    return BlipColor.Blue6;
                case "bluedark2":
                    return BlipColor.BlueDark2;
                case "reddark2":
                    return BlipColor.RedDark2;
                case "menuyellow":
                    return BlipColor.MenuYellow;
                case "blue7":
                    return BlipColor.Blue7;
                default:
                    return BlipColor.BlueLight; // Default color if not matched
            }
        }
        private void BuyAllWeapons()
        {
            Ped player = Game.Player.Character;
            var weapons = player.Weapons;

            try
            {
                int totalCost = 0;
                List<WeaponHash> weaponsToBuy = new List<WeaponHash>();

                // Loop through all available weapons in the price list 
                foreach (var entry in WeaponValues)
                {
                    WeaponHash hash = entry.Key;
                    int price = entry.Value;

                    // Check if player already has the weapon
                    if (!weapons.HasWeapon(hash))
                    {
                        weaponsToBuy.Add(hash);
                        totalCost += price;
                    }
                }

                if (weaponsToBuy.Count == 0)
                {
                    GTA.UI.Notification.Show(
                        GTA.UI.NotificationIcon.Ammunation,         // portrait (Hao’s face)
                        "Ammunation",                               // sender name (shows above the subject)
                        "Ammunation +",                 // subject line
                        $"~r~You already own all weapons.", // message body
                        true,                                // fade in
                        false                                // blinking
                    );
                    return;
                }

                if (Game.Player.Money < totalCost)
                {
                    GTA.UI.Notification.Show(
                        GTA.UI.NotificationIcon.Ammunation,         // portrait (Hao’s face)
                        "Ammunation",                               // sender name (shows above the subject)
                        "Ammunation +",                 // subject line
                        $"~r~Not enough money! ~w~You need ~y~${totalCost}", // message body
                        true,                                // fade in
                        false                                // blinking
                    );
                    return;
                }

                // Deduct money
                Game.Player.Money -= totalCost;

                // Give weapons and update UI
                foreach (WeaponHash hash in weaponsToBuy)
                {
                    weapons.Give(hash, 999, true, false); // Give weapon with ammo

                    if (weaponUIs.TryGetValue(hash, out var ui))
                    {
                        // Update Buy
                        ui.BuyItem.Enabled = false;
                        ui.BuyItem.Description = "~c~You already own this weapon";

                        // Enable Sell
                        ui.SellItem.Enabled = true;
                        ui.SellItem.Description = $"Resell for ${WeaponValues[hash]}";

                        // Enable Equip
                        ui.EquipItem.Enabled = true;

                        // Add ammo options if not already present
                        if (!ui.WeaponMenu.Items.Contains(ui.BuyAmmoItem))
                            ui.WeaponMenu.Items.Insert(0, ui.BuyAmmoItem);
                        if (!ui.WeaponMenu.Items.Contains(ui.BuyFullAmmoItem))
                            ui.WeaponMenu.Items.Insert(1, ui.BuyFullAmmoItem);
                    }
                }

                    GTA.UI.Notification.Show(
                        GTA.UI.NotificationIcon.Ammunation,         // portrait (Hao’s face)
                        "Ammunation",                               // sender name (shows above the subject)
                        "Ammunation +",                 // subject line
                        $"Purchased all weapons for ~r~ -${totalCost}", // message body
                        true,                                // fade in
                        false                                // blinking
                    );
                    return;

            }
            catch (Exception ex)
            {
                GTA.UI.Notification.Show("~r~An error occurred: " + ex.Message);
            }
        }
        private void RemoveWeapons()
        {
            Ped player = Game.Player.Character;
            var weapons = player.Weapons;

            try
            {
                bool hasWeapons = false;

                foreach (Weapon weapon in weapons)
                {
                    if (weapon.Hash != WeaponHash.Unarmed && weapon.IsPresent)
                    {
                        hasWeapons = true;
                        break;
                    }
                }

                if (!hasWeapons)
                {
                    GTA.UI.Notification.Show(
                        GTA.UI.NotificationIcon.Ammunation,         // portrait (Hao’s face)
                        "Ammunation",                               // sender name (shows above the subject)
                        "Ammunation +",                 // subject line
                        $"~r~You do not have any weapons!", // message body
                        true,                                // fade in
                        false                                // blinking
                    );
                    return;
                }

                int totalValue = 0;

                foreach (Weapon weapon in weapons.ToList())
                {
                    WeaponHash hash = weapon.Hash;

                    if (hash != WeaponHash.Unarmed && weapon.IsPresent)
                    {
                        int value = WeaponValues.TryGetValue(hash, out int v) ? v : 100;
                        totalValue += value;

                        weapons.Remove(hash);

                        // ✅ UI Cleanup
                        if (weaponUIs.TryGetValue(hash, out var ui))
                        {
                            ui.SellItem.Enabled = false;
                            ui.SellItem.Description = "~c~You do not own this weapon";

                            ui.BuyItem.Enabled = true;
                            ui.BuyItem.Description = $"Price: ${value}";

                            ui.EquipItem.Enabled = false;

                            if (ui.WeaponMenu.Items.Contains(ui.BuyAmmoItem))
                                ui.WeaponMenu.Items.Remove(ui.BuyAmmoItem);
                            if (ui.WeaponMenu.Items.Contains(ui.BuyFullAmmoItem))
                                ui.WeaponMenu.Items.Remove(ui.BuyFullAmmoItem);
                        }
                    }
                }

                Game.Player.Money += totalValue;
                
                GTA.UI.Notification.Show(
                    GTA.UI.NotificationIcon.Ammunation,         // portrait (Hao’s face)
                    "Ammunation",                               // sender name (shows above the subject)
                    "Ammunation +",                 // subject line
                    $"~w~Sold all weapons for $~g~{totalValue}", // message body
                    true,                                // fade in
                    false                                // blinking
                );

            }
            catch (Exception ex)
            {
                GTA.UI.Notification.Show("~r~An error occurred: " + ex.Message);
            }
        }

    }
}