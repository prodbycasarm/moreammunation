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
        // private List<Vehicle> armoryZoneVehicles = new List<Vehicle>();
        //private List<Blip> armoryZoneBlips = new List<Blip>();
        private Dictionary<Vehicle, Blip> vehicleBlips = new Dictionary<Vehicle, Blip>();
        private List<Blip> staticZoneBlips = new List<Blip>();
        private List<VehicleRespawn> vehiclesToRespawn = new List<VehicleRespawn>();
        private class VehicleRespawn
        {
            public ArmoryZone Zone;
            public DateTime RespawnTime;
        }

        private float armoryZoneRadius = 6.0f;
        private int notificationHandle = -1;
        private bool zonesCreated = false;

        private Vehicle nearestArmoryVehicle = null;

        //HEIST

        private Keys heistKey; // Key to start heist
        private int buyWeaponsNotification = -1;
        private int heistNotification = -1;
        private Dictionary<ArmoryZone, bool> heistActive = new Dictionary<ArmoryZone, bool>();
        private int lastWantedLevel = 0;

        // Config and key settings
        ScriptSettings config;
        Keys enable;

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
                // Buy/modify weapons notification
                if (buyWeaponsNotification == -1)
                {
                    buyWeaponsNotification = GTA.UI.Notification.Show(
                        $"~w~Press ~b~{enable}~w~ to buy/modify weapons"
                    );
                }

                // Heist notification
                if (heistNotification == -1)
                {
                    heistNotification = GTA.UI.Notification.Show(
                        $"~w~Press ~b~{heistKey}~w~ to rob the vehicle"
                    );
                }
            }
            else
            {
                // Hide notifications when leaving zone
                if (buyWeaponsNotification != -1)
                {
                    GTA.UI.Notification.Hide(buyWeaponsNotification);
                    buyWeaponsNotification = -1;
                }
                if (heistNotification != -1)
                {
                    GTA.UI.Notification.Hide(heistNotification);
                    heistNotification = -1;
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
                    
                    ArmoryZone zone = armoryZones.FirstOrDefault(z => v.Model.Hash == new Model(z.VehicleName).Hash);
                    if (zone != null)
                    {
                        
                        vehiclesToRespawn.Add(new VehicleRespawn
                        {
                            Zone = zone,
                            RespawnTime = DateTime.Now.AddSeconds(60)
                        });
                    }
                    toRemove.Add(v);
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
                foreach (var vehicle in World.GetAllVehicles())
                {
                    if (vehicle.Exists() && vehicle.Model.Hash == new Model(respawn.Zone.VehicleName).Hash)
                    {
                        vehicle.Delete();
                    }
                }
                // Spawn vehicle again
                Model vehicleModel = new Model(respawn.Zone.VehicleName);
                if (vehicleModel.IsInCdImage && vehicleModel.IsValid)
                {
                    vehicleModel.Request(500);
                    while (!vehicleModel.IsLoaded) Script.Yield();

                    Vehicle vehicle = World.CreateVehicle(vehicleModel, respawn.Zone.Position + new Vector3(2f, 2f, 0f));
                    vehicle.IsPersistent = true;
                    vehicle.AreLightsOn = true;
                    vehicle.AreBrakeLightsOn = true;

                    // Blip
                    int vehicleBlipHandle = Function.Call<int>(Hash.ADD_BLIP_FOR_ENTITY, vehicle.Handle);
                    Function.Call(Hash.SET_BLIP_SPRITE, vehicleBlipHandle, (int)GetBlipSprite(respawn.Zone.BlipSprite));
                    Function.Call(Hash.SET_BLIP_COLOUR, vehicleBlipHandle, (int)GetBlipColor(respawn.Zone.BlipColor));
                    Function.Call(Hash.BEGIN_TEXT_COMMAND_SET_BLIP_NAME, "STRING");
                    Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, respawn.Zone.BlipName);
                    Function.Call(Hash.END_TEXT_COMMAND_SET_BLIP_NAME, vehicleBlipHandle);

                    Blip vehicleBlipObj = new Blip(vehicleBlipHandle);
                    vehicleBlips.Add(vehicle, vehicleBlipObj);
                }

                vehiclesToRespawn.Remove(respawn); // done respawning
            }


            //Logic For Armory Heist
            if (lastWantedLevel > 0 && Game.Player.WantedLevel == 0)
            {
                // Player just lost all wanted stars
                foreach (var pair in heistActive.ToList())
                {
                    if (pair.Value) // heist was active
                    {
                        // Give the reward
                        int reward = pair.Key.HeistReward;
                        Game.Player.Money += reward;
                        GTA.UI.Notification.Show($"~g~Heist complete! You earned ${reward:N0}.");

                        // Mark heist as done
                        heistActive[pair.Key] = false;
                    }
                }
            }

            lastWantedLevel = Game.Player.WantedLevel;

            // Update the global reference to the nearest vehicle for OnKeyUp
            nearestArmoryVehicle = nearestVehicle;

            // Process LemonUI menu pool
            pool.Process();
        }
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (Game.Player.Character.IsOnFoot && isNearArmoryZone)
            {
                // Open armory menu
                if (e.KeyCode == enable)
                {
                    armoryMenu.Visible = !armoryMenu.Visible;
                }

                // Start heist (wanted level)
                if (e.KeyCode == heistKey && nearestArmoryVehicle != null)
                {
                    Game.Player.WantedLevel = 3; // Safe, instant
                    GTA.UI.Notification.Show("~r~Heist started! Wanted level set to 3 stars.");

                    // Find which armory zone we are near
                    ArmoryZone currentZone = armoryZones.FirstOrDefault(z =>
                        nearestArmoryVehicle.Model.Hash == new Model(z.VehicleName).Hash
                    );

                    if (currentZone != null)
                    {
                        if (!heistActive.ContainsKey(currentZone))
                            heistActive.Add(currentZone, true);
                        else
                            heistActive[currentZone] = true;
                    }
                }
            }
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

                armoryZones.Add(new ArmoryZone()
                {
                    Position = new Vector3(x, y, z),
                    BlipSprite = blipSpriteString,
                    BlipColor = blipColorString,
                    BlipName = blipName,
                    SpawnVehicle = spawnVehicle,
                    VehicleName = vehicleName,
                    HeistReward = config.GetValue<int>($"ArmoryZone{zoneIndex}", "heistReward", 110000),

                });

                zoneIndex++;
            }

            Notification.Show($"~g~Loaded {armoryZones.Count} armory zones.");
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


                
                    }
                    
                    else
                    {
                        Notification.Show($"~r~Failed to load {zone.VehicleName}");
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
                Notification.Show("Failed to parse key, using default 'Enter'");
            }

            string heistKeyString = config.GetValue<string>("Options", "HeistButton", "H"); // default H
            if (!Enum.TryParse(heistKeyString, out heistKey))
            {
                heistKey = Keys.H; // fallback
                GTA.UI.Notification.Show("Failed to parse heist key, using default 'H'");
            }


            LoadarmoryZonePositions(); // Only loads coordinates

            string blipSpriteString = config.GetValue<string>("Blip", "Sprite", "ammunation");
            string blipColorString = config.GetValue<string>("Blip", "Color", "BlueLight");
            string blipName = config.GetValue<string>("Blip", "Name", "Armoury");

            // Vehicle spawn settings
            bool spawnvehicle = config.GetValue<bool>("VehicleOptions", "DeliveryVehicle", true);
            string vehicleName = config.GetValue<string>("VehicleOptions", "VehicleName", "mule");
            

            GTA.UI.Notification.Show("Armory script loaded, waiting to create blips...");





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
            var cHWeaponItem = armoryMenu.AddSubMenu(cHWeapon);
            cHWeapon.Shown += (sender, args) =>
            {
                cHWeapon.Clear();

                WeaponHash heldWeaponHash = Game.Player.Character.Weapons.Current.Hash;

                if (!WeaponValues.ContainsKey(heldWeaponHash))
                {
                    GTA.UI.Notification.Show("~y~You are not holding a supported weapon.");
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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }

                    int ammoPrice = 70;
                    int ammoAmount = 50;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${ammoPrice}");
                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show($"~g~Purchased {ammoAmount} rounds for ${ammoPrice}");
                };
                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var cuweapons = Game.Player.Character.Weapons;
                    var weapon = weapons[heldWeaponHash];

                    if (!weapons.HasWeapon(heldWeaponHash))
                    {
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }

                    int refillPrice = 5000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${refillPrice}");
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;

                    GTA.UI.Notification.Show($"~g~Fully refilled ammo for ${refillPrice}");
                };

                // SELL
                var sellItem = new NativeItem("Sell", $"Resell for ${price}");
                sellItem.Activated += (s1, a1) =>
                {
                    if (!weapons.HasWeapon(heldWeaponHash))
                    {
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Remove(heldWeaponHash);
                    Game.Player.Money += price;

                    GTA.UI.Notification.Show($"~g~Sold {name} for ${price}");
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
                // START HERE

                customizeMenu.Shown += (s, e) =>
                {
                    if (!weapons.HasWeapon(heldWeaponHash))
                    {
                        customizeMenu.Visible = false;
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
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

                GTA.UI.Notification.Show($"~g~Currently holding: {name}");

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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon

                    GTA.UI.Notification.Show($"~g~Equipped {name}");

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
                customizeMenu.Shown += (s, e) =>
                {
                    if (!Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        customizeMenu.Visible = false;
                        weaponMenu.Visible = true;  // 👈 Show the weapon menu again
                        GTA.UI.Notification.Show($"~r~You no longer own the {name}.");
                    }
                };
                customizeMenu.Add(openCompMenuItem);
                openCompMenuItem.Activated += (sender1, args1) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
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
                        GTA.UI.Notification.Show($"~g~You already own the {name}.");
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${price}");
                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show($"~g~You purchased {name}");


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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show($"~r~Sold {name} for ${price}");



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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon

                    GTA.UI.Notification.Show($"~g~Equipped {name}");

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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }
                    int ammoPrice = 70;
                    int ammoAmount = 50;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${ammoPrice}");
                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show($"~g~Purchased {ammoAmount} rounds for ${ammoPrice}");
                };

                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }
                    int refillPrice = 5000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${refillPrice}");
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;

                    GTA.UI.Notification.Show($"~g~Fully refilled ammo for ${refillPrice}");
                };


                // Buy weapon logic
                buyItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show($"~g~You already own the {name}.");
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${price}");
                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show($"~g~You purchased {name}");
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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show($"~r~Sold {name} for ${price}");
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
                customizeMenu.Shown += (s, e) =>
                {
                    if (!Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        customizeMenu.Visible = false;
                        weaponMenu.Visible = true;  // 👈 Show the weapon menu again
                        GTA.UI.Notification.Show($"~r~You no longer own the {name}.");
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
                        GTA.UI.Notification.Show($"~r~You need to own the {name} to customize it.");
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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon

                    GTA.UI.Notification.Show($"~g~Equipped {name}");

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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }

                    int ammoPrice = 70;
                    int ammoAmount = 50;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${ammoPrice}");
                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show($"~g~Purchased {ammoAmount} rounds for ${ammoPrice}");
                };

                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }

                    int refillPrice = 5000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${refillPrice}");
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;

                    GTA.UI.Notification.Show($"~g~Fully refilled ammo for ${refillPrice}");
                };

                // Buy weapon logic
                buyItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show($"~g~You already own the {name}.");
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${price}");
                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show($"~g~You purchased {name}");

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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show($"~r~Sold {name} for ${price}");

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
                customizeMenu.Shown += (s, e) =>
                {
                    if (!Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        customizeMenu.Visible = false;
                        weaponMenu.Visible = true;
                        GTA.UI.Notification.Show($"~r~You no longer own the {name}.");
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
                        GTA.UI.Notification.Show($"~r~You need to own the {name} to customize it.");
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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon

                    GTA.UI.Notification.Show($"~g~Equipped {name}");

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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }

                    int ammoPrice = 70;
                    int ammoAmount = 50;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${ammoPrice}");
                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show($"~g~Purchased {ammoAmount} rounds for ${ammoPrice}");
                };

                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }

                    int refillPrice = 5000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${refillPrice}");
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;

                    GTA.UI.Notification.Show($"~g~Fully refilled ammo for ${refillPrice}");
                };

                // Buy weapon logic
                buyItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show($"~g~You already own the {name}.");
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${price}");
                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show($"~g~You purchased {name}");

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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show($"~r~Sold {name} for ${price}");

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
                customizeMenu.Shown += (s, e) =>
                {
                    if (!Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        customizeMenu.Visible = false;
                        weaponMenu.Visible = true;  // 👈 Show the weapon menu again
                        GTA.UI.Notification.Show($"~r~You no longer own the {name}.");
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
                        GTA.UI.Notification.Show($"~r~You need to own the {name} to customize it.");
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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon

                    GTA.UI.Notification.Show($"~g~Equipped {name}");

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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }

                    int ammoPrice = 70;
                    int ammoAmount = 50;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${ammoPrice}");
                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show($"~g~Purchased {ammoAmount} rounds for ${ammoPrice}");
                };

                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }

                    int refillPrice = 5000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${refillPrice}");
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;

                    GTA.UI.Notification.Show($"~g~Fully refilled ammo for ${refillPrice}");
                };

                // Buy weapon logic
                buyItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show($"~g~You already own the {name}.");
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${price}");
                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show($"~g~You purchased {name}");

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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show($"~r~Sold {name} for ${price}");

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
                customizeMenu.Shown += (s, e) =>
                {
                    if (!Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        customizeMenu.Visible = false;
                        weaponMenu.Visible = true;  // 👈 Show the weapon menu again
                        GTA.UI.Notification.Show($"~r~You no longer own the {name}.");
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
                        GTA.UI.Notification.Show($"~r~You need to own the {name} to customize it.");
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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon

                    GTA.UI.Notification.Show($"~g~Equipped {name}");

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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }

                    int ammoPrice = 70;
                    int ammoAmount = 50;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${ammoPrice}");
                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show($"~g~Purchased {ammoAmount} rounds for ${ammoPrice}");
                };

                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }

                    int refillPrice = 5000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${refillPrice}");
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;

                    GTA.UI.Notification.Show($"~g~Fully refilled ammo for ${refillPrice}");
                };

                // Buy weapon logic
                buyItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show($"~g~You already own the {name}.");
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${price}");
                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show($"~g~You purchased {name}");

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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show($"~r~Sold {name} for ${price}");

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
                customizeMenu.Shown += (s, e) =>
                {
                    if (!Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        customizeMenu.Visible = false;
                        weaponMenu.Visible = true;  // 👈 Show the weapon menu again
                        GTA.UI.Notification.Show($"~r~You no longer own the {name}.");
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
                        GTA.UI.Notification.Show($"~r~You need to own the {name} to customize it.");
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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon

                    GTA.UI.Notification.Show($"~g~Equipped {name}");

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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }

                    int ammoPrice = 70;
                    int ammoAmount = 50;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${ammoPrice}");
                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show($"~g~Purchased {ammoAmount} rounds for ${ammoPrice}");
                };

                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }

                    int refillPrice = 5000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${refillPrice}");
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;

                    GTA.UI.Notification.Show($"~g~Fully refilled ammo for ${refillPrice}");
                };

                // Buy weapon logic
                buyItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show($"~g~You already own the {name}.");
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${price}");
                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show($"~g~You purchased {name}");

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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show($"~r~Sold {name} for ${price}");

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
                customizeMenu.Shown += (s, e) =>
                {
                    if (!Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        customizeMenu.Visible = false;
                        weaponMenu.Visible = true;  // 👈 Show the weapon menu again
                        GTA.UI.Notification.Show($"~r~You no longer own the {name}.");
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
                        GTA.UI.Notification.Show($"~r~You need to own the {name} to customize it.");
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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon

                    GTA.UI.Notification.Show($"~g~Equipped {name}");

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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }

                    int ammoPrice = 70;
                    int ammoAmount = 50;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${ammoPrice}");
                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show($"~g~Purchased {ammoAmount} rounds for ${ammoPrice}");
                };

                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }

                    int refillPrice = 5000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${refillPrice}");
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;

                    GTA.UI.Notification.Show($"~g~Fully refilled ammo for ${refillPrice}");
                };

                // Buy weapon logic
                buyItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show($"~g~You already own the {name}.");
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${price}");
                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show($"~g~You purchased {name}");

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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show($"~r~Sold {name} for ${price}");

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
                customizeMenu.Shown += (s, e) =>
                {
                    if (!Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        customizeMenu.Visible = false;
                        weaponMenu.Visible = true;  // 👈 Show the weapon menu again
                        GTA.UI.Notification.Show($"~r~You no longer own the {name}.");
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
                        GTA.UI.Notification.Show($"~r~You need to own the {name} to customize it.");
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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Select(hash, true); // Equip the weapon

                    GTA.UI.Notification.Show($"~g~Equipped {name}");

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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }

                    int ammoPrice = 50;
                    int ammoAmount = 1;

                    if (Game.Player.Money < ammoPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${ammoPrice}");
                        return;
                    }

                    Game.Player.Money -= ammoPrice;
                    weapon.Ammo += ammoAmount;

                    GTA.UI.Notification.Show($"~g~Purchased {ammoAmount} rounds for ${ammoPrice}");
                };

                // Full Refill Logic
                buyFullAmmoItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;
                    var weapon = weapons[hash];

                    if (!weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show($"~r~You don't own the {name}. Buy the weapon first.");
                        return;
                    }

                    int refillPrice = 3000;

                    if (Game.Player.Money < refillPrice)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${refillPrice}");
                        return;
                    }

                    Game.Player.Money -= refillPrice;
                    weapon.Ammo = weapon.MaxAmmo;

                    GTA.UI.Notification.Show($"~g~Fully refilled ammo for ${refillPrice}");
                };

                // Buy weapon logic
                buyItem.Activated += (s, a) =>
                {
                    var weapons = Game.Player.Character.Weapons;

                    if (weapons.HasWeapon(hash))
                    {
                        GTA.UI.Notification.Show($"~g~You already own the {name}.");
                        return;
                    }

                    if (Game.Player.Money < price)
                    {
                        GTA.UI.Notification.Show($"~r~Not enough money! You need ${price}");
                        return;
                    }

                    Game.Player.Money -= price;
                    weapons.Give(hash, 1, true, true);
                    GTA.UI.Notification.Show($"~g~You purchased {name}");

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
                        GTA.UI.Notification.Show($"~r~You don't own the {name}.");
                        return;
                    }

                    weapons.Remove(hash);
                    Game.Player.Money += price;
                    GTA.UI.Notification.Show($"~r~Sold {name} for ${price}");

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
                    GTA.UI.Notification.Show("~g~You already own all weapons.");
                    return;
                }

                if (Game.Player.Money < totalCost)
                {
                    GTA.UI.Notification.Show($"~r~Not enough money! You need ~y~${totalCost}");
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

                GTA.UI.Notification.Show($"~g~Purchased all weapons for ${totalCost}");
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
                    GTA.UI.Notification.Show("~r~You do not have any weapons!");
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
                GTA.UI.Notification.Show($"~r~Sold all weapons for ${totalValue}");

            }
            catch (Exception ex)
            {
                GTA.UI.Notification.Show("~r~An error occurred: " + ex.Message);
            }
        }

    }
}