using GTA;
using System.Collections.Generic;
using System.Linq;

namespace moreammunation
{
    internal enum AmmoKind
    {
        None,       // melee
        Bullets,
        Throwable
    }

    internal sealed class WeaponCategory
    {
        public string Name;
        public string Description;
        public AmmoKind Ammo;
        public bool Customizable;
        public List<KeyValuePair<WeaponHash, int>> Weapons = new List<KeyValuePair<WeaponHash, int>>();

        public WeaponCategory Add(WeaponHash hash, int price)
        {
            Weapons.Add(new KeyValuePair<WeaponHash, int>(hash, price));
            return this;
        }
    }

    /// <summary>
    /// The one and only price list. Before, the same prices were written out
    /// three separate times (WeaponValues, the per-category dictionaries and
    /// WeaponPrices.cs), which made them easy to get out of sync.
    /// </summary>
    internal static class WeaponCatalog
    {
        public static readonly List<WeaponCategory> Categories = Build();

        private static readonly Dictionary<WeaponHash, int> prices =
            Categories.SelectMany(c => c.Weapons).GroupBy(w => w.Key).ToDictionary(g => g.Key, g => g.First().Value);

        private static readonly Dictionary<WeaponHash, WeaponCategory> categoryOf =
            Categories.SelectMany(c => c.Weapons.Select(w => new KeyValuePair<WeaponHash, WeaponCategory>(w.Key, c)))
                      .GroupBy(x => x.Key).ToDictionary(g => g.Key, g => g.First().Value);

        public static IEnumerable<WeaponHash> All { get { return prices.Keys; } }

        public static bool TryGetPrice(WeaponHash hash, out int price)
        {
            return prices.TryGetValue(hash, out price);
        }

        public static WeaponCategory CategoryOf(WeaponHash hash)
        {
            WeaponCategory c;
            return categoryOf.TryGetValue(hash, out c) ? c : null;
        }

        /// <summary>Game-localized name ("Pistol .50"), falling back to a readable enum name.</summary>
        public static string DisplayName(WeaponHash hash)
        {
            string name = null;
            try { name = Weapon.GetHumanNameFromHash(hash); } catch { }
            if (string.IsNullOrWhiteSpace(name) || name == "NULL" || name == "Invalid")
                name = Util.SplitCamelCase(hash.ToString());
            return name;
        }

        private static List<WeaponCategory> Build()
        {
            var list = new List<WeaponCategory>();

            list.Add(new WeaponCategory { Name = "Melee", Description = "Knives, bats and everything in between.", Ammo = AmmoKind.None, Customizable = true }
                .Add(WeaponHash.Knife, 400).Add(WeaponHash.Nightstick, 400).Add(WeaponHash.Hammer, 500)
                .Add(WeaponHash.Bat, 120).Add(WeaponHash.GolfClub, 110).Add(WeaponHash.Crowbar, 130)
                .Add(WeaponHash.Bottle, 50).Add(WeaponHash.SwitchBlade, 150).Add(WeaponHash.BattleAxe, 300)
                .Add(WeaponHash.PoolCue, 75).Add(WeaponHash.Wrench, 85).Add(WeaponHash.StoneHatchet, 250)
                .Add(WeaponHash.CandyCane, 10).Add(WeaponHash.KnuckleDuster, 180).Add(WeaponHash.Machete, 200)
                .Add(WeaponHash.Dagger, 175).Add(WeaponHash.Hatchet, 160));

            list.Add(new WeaponCategory { Name = "Handguns", Description = "Pistols and revolvers.", Ammo = AmmoKind.Bullets, Customizable = true }
                .Add(WeaponHash.Pistol, 500).Add(WeaponHash.PistolMk2, 1000).Add(WeaponHash.CombatPistol, 600)
                .Add(WeaponHash.APPistol, 1500).Add(WeaponHash.Pistol50, 1500).Add(WeaponHash.FlareGun, 300)
                .Add(WeaponHash.MarksmanPistol, 4350).Add(WeaponHash.Revolver, 1000).Add(WeaponHash.RevolverMk2, 1200)
                .Add(WeaponHash.DoubleActionRevolver, 1100).Add(WeaponHash.UpNAtomizer, 399000).Add(WeaponHash.CeramicPistol, 700)
                .Add(WeaponHash.NavyRevolver, 1300).Add(WeaponHash.PericoPistol, 1500).Add(WeaponHash.WM29Pistol, 850)
                .Add(WeaponHash.HeavyPistol, 700).Add(WeaponHash.SNSPistol, 400).Add(WeaponHash.SNSPistolMk2, 1000)
                .Add(WeaponHash.VintagePistol, 450).Add(WeaponHash.MachinePistol, 850));

            list.Add(new WeaponCategory { Name = "SMGs", Description = "Submachine guns.", Ammo = AmmoKind.Bullets, Customizable = true }
                .Add(WeaponHash.MicroSMG, 1000).Add(WeaponHash.SMG, 1200).Add(WeaponHash.SMGMk2, 1500)
                .Add(WeaponHash.AssaultSMG, 1800).Add(WeaponHash.CombatPDW, 1600).Add(WeaponHash.MiniSMG, 1100)
                .Add(WeaponHash.TacticalSMG, 1200));

            list.Add(new WeaponCategory { Name = "Rifles", Description = "Assault rifles and carbines.", Ammo = AmmoKind.Bullets, Customizable = true }
                .Add(WeaponHash.AssaultRifle, 8550).Add(WeaponHash.AssaultrifleMk2, 9875).Add(WeaponHash.CarbineRifle, 13000)
                .Add(WeaponHash.CarbineRifleMk2, 14000).Add(WeaponHash.CompactRifle, 14650).Add(WeaponHash.MilitaryRifle, 15000)
                .Add(WeaponHash.ServiceCarbine, 370000).Add(WeaponHash.BattleRifle, 15000).Add(WeaponHash.AdvancedRifle, 14250)
                .Add(WeaponHash.BullpupRifle, 14000).Add(WeaponHash.BullpupRifleMk2, 14500).Add(WeaponHash.SpecialCarbine, 14000)
                .Add(WeaponHash.SpecialCarbineMk2, 14500).Add(WeaponHash.HeavyRifle, 15000));

            list.Add(new WeaponCategory { Name = "Machine Guns", Description = "Light machine guns.", Ammo = AmmoKind.Bullets, Customizable = true }
                .Add(WeaponHash.MG, 13500).Add(WeaponHash.CombatMG, 14750).Add(WeaponHash.CombatMGMk2, 15500)
                .Add(WeaponHash.Gusenberg, 14000).Add(WeaponHash.UnholyHellbringer, 449000));

            list.Add(new WeaponCategory { Name = "Shotguns", Description = "Close-range stoppers.", Ammo = AmmoKind.Bullets, Customizable = true }
                .Add(WeaponHash.PumpShotgun, 550).Add(WeaponHash.PumpShotgunMk2, 1000).Add(WeaponHash.SawnOffShotgun, 300)
                .Add(WeaponHash.AssaultShotgun, 1500).Add(WeaponHash.BullpupShotgun, 1250).Add(WeaponHash.DoubleBarrelShotgun, 1450)
                .Add(WeaponHash.SweeperShotgun, 1700).Add(WeaponHash.CombatShotgun, 2950).Add(WeaponHash.HeavyShotgun, 13550));

            list.Add(new WeaponCategory { Name = "Snipers", Description = "Long-range rifles.", Ammo = AmmoKind.Bullets, Customizable = true }
                .Add(WeaponHash.SniperRifle, 5000).Add(WeaponHash.HeavySniper, 9500).Add(WeaponHash.HeavySniperMk2, 9875)
                .Add(WeaponHash.MarksmanRifle, 15750).Add(WeaponHash.MarksmanRifleMk2, 16000).Add(WeaponHash.PrecisionRifle, 10000));

            list.Add(new WeaponCategory { Name = "Heavy Weapons", Description = "Launchers, miniguns and experimental hardware.", Ammo = AmmoKind.Bullets, Customizable = true }
                .Add(WeaponHash.RPG, 26250).Add(WeaponHash.GrenadeLauncher, 32400).Add(WeaponHash.CompactGrenadeLauncher, 45000)
                .Add(WeaponHash.Minigun, 50000).Add(WeaponHash.Firework, 65000).Add(WeaponHash.HomingLauncher, 75000)
                .Add(WeaponHash.Widowmaker, 499000).Add(WeaponHash.Railgun, 730000));

            list.Add(new WeaponCategory { Name = "Throwables", Description = "Grenades, explosives and utility.", Ammo = AmmoKind.Throwable, Customizable = false }
                .Add(WeaponHash.Grenade, 250).Add(WeaponHash.StickyBomb, 600).Add(WeaponHash.SmokeGrenade, 200)
                .Add(WeaponHash.Molotov, 200).Add(WeaponHash.PipeBomb, 500).Add(WeaponHash.Snowball, 1)
                .Add(WeaponHash.Flare, 50).Add(WeaponHash.FireExtinguisher, 50).Add(WeaponHash.PetrolCan, 50)
                .Add(WeaponHash.HazardousJerryCan, 50).Add(WeaponHash.FertilizerCan, 50).Add(WeaponHash.AcidPackage, 50));

            return list;
        }
    }
}
