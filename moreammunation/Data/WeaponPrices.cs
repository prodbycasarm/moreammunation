using GTA;
using System.Collections.Generic;

namespace moreammunation
{
    public static class WeaponPrices
    {
        //Dictionary For Weapons
        public static Dictionary<WeaponHash, int> WeaponValues = new Dictionary<WeaponHash, int>
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
        public static Dictionary<WeaponHash, int> MeleeValues = new Dictionary<WeaponHash, int>
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
        public static Dictionary<WeaponHash, int> HandgunsValues = new Dictionary<WeaponHash, int>
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
        public static Dictionary<WeaponHash, int> SMGsValues = new Dictionary<WeaponHash, int>
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
        public static Dictionary<WeaponHash, int> RiflesValues = new Dictionary<WeaponHash, int>
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
        public static Dictionary<WeaponHash, int> MachineGunsValues = new Dictionary<WeaponHash, int>
        {
            // Machine Guns
            { WeaponHash.MG, 13500 },
            { WeaponHash.CombatMG, 14750 },
            { WeaponHash.CombatMGMk2, 15500 },
            { WeaponHash.Gusenberg, 14000 },
            { WeaponHash.UnholyHellbringer, 449000 },

        };
        public static Dictionary<WeaponHash, int> ShotgunsValues = new Dictionary<WeaponHash, int>
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
        public static Dictionary<WeaponHash, int> SnipersValues = new Dictionary<WeaponHash, int>
        {
            // Snipers
            { WeaponHash.SniperRifle, 5000 },
            { WeaponHash.HeavySniper, 9500 },
            { WeaponHash.HeavySniperMk2, 9875 },
            { WeaponHash.MarksmanRifle, 15750 },
            { WeaponHash.MarksmanRifleMk2, 16000 },
            { WeaponHash.PrecisionRifle, 10000 },
        };
        public static Dictionary<WeaponHash, int> HeavyWeaponValues = new Dictionary<WeaponHash, int>
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
        public static Dictionary<WeaponHash, int> ThrowablesValues = new Dictionary<WeaponHash, int>
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
        static Dictionary<WeaponHash, WeaponUI> weaponUIs = new Dictionary<WeaponHash, WeaponUI>();

    }
}