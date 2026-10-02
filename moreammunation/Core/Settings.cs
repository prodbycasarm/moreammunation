using GTA;
using System;
using System.Windows.Forms;

namespace moreammunation
{
    /// <summary>How a mission location is picked from the list of candidates.</summary>
    internal enum MissionSelectionMode
    {
        Nearest,     // always the closest valid location
        NearestFew,  // random pick among the 3 closest (a bit of variety)
        Random       // old behaviour: anywhere on the map
    }

    /// <summary>
    /// Every tunable value in one place. Everything has a default, so an old
    /// moreammunation.ini keeps working without any changes.
    /// </summary>
    internal sealed class Settings
    {
        public const string IniPath = "scripts\\moreammunation.ini";
        public const string DataFolder = "scripts\\MoreAmmunationsMod";

        public ScriptSettings Ini { get; private set; }

        // [Options]
        public Keys MenuKey = Keys.E;
        public Keys RobKey = Keys.Q;
        public Keys JobBoardKey = Keys.None;
        public float ZoneRadius = 6f;
        public bool DisableWantedLevel = true;
        public bool ShowStartupNotifications = true;
        public bool BlockShopDuringMissions = true;

        // [Shop]
        public int AmmoPackPrice = 70;
        public int AmmoPackAmount = 50;
        public int FullRefillPriceCap = 5000;
        public int ThrowablePackPrice = 50;
        public int ThrowablePackAmount = 1;
        public int ThrowableRefillPriceCap = 3000;
        public int StarterAmmo = 1;
        public int SellPercent = 100;
        public int ArmorPrice = 2500;
        public bool AllowSpecialAmmo = false;

        // [Missions]
        public MissionSelectionMode Selection = MissionSelectionMode.Nearest;
        public float MinMissionDistance = 150f;
        public float MinDeliveryDistance = 800f;
        public int CooldownSeconds = 30;
        public float RewardMultiplier = 1f;
        public int RobberyWantedLevel = 3;
        public int ConvoyWantedLevel = 2;
        public int DefenseWaves = 3;
        public int GuardRespawnSeconds = 10;
        public int TruckRespawnSeconds = 20;

        public static Settings Load()
        {
            var s = new Settings();
            s.Ini = ScriptSettings.Load(IniPath);
            var ini = s.Ini;

            s.MenuKey = ReadKey(ini, "Options", "MenuKey", s.MenuKey);
            s.RobKey = ReadKey(ini, "Options", "RobKey", s.RobKey);
            s.JobBoardKey = ReadKey(ini, "Options", "JobBoardKey", s.JobBoardKey);
            s.ZoneRadius = ini.GetValue("Options", "ZoneRadius", s.ZoneRadius);
            s.ShowStartupNotifications = ini.GetValue("Options", "StartupNotifications", s.ShowStartupNotifications);
            s.BlockShopDuringMissions = ini.GetValue("Options", "BlockShopDuringMissions", s.BlockShopDuringMissions);
            s.DisableWantedLevel = ini.GetValue("Missions", "DisableWantedLevel", s.DisableWantedLevel);
            s.AmmoPackPrice = ini.GetValue("Shop", "AmmoPackPrice", s.AmmoPackPrice);
            s.AmmoPackAmount = Math.Max(1, ini.GetValue("Shop", "AmmoPackAmount", s.AmmoPackAmount));
            s.FullRefillPriceCap = ini.GetValue("Shop", "FullRefillPriceCap", s.FullRefillPriceCap);
            s.ThrowablePackPrice = ini.GetValue("Shop", "ThrowablePackPrice", s.ThrowablePackPrice);
            s.ThrowablePackAmount = Math.Max(1, ini.GetValue("Shop", "ThrowablePackAmount", s.ThrowablePackAmount));
            s.ThrowableRefillPriceCap = ini.GetValue("Shop", "ThrowableRefillPriceCap", s.ThrowableRefillPriceCap);
            s.StarterAmmo = Math.Max(1, ini.GetValue("Shop", "StarterAmmo", s.StarterAmmo));
            s.SellPercent = Util.Clamp(ini.GetValue("Shop", "SellPercent", s.SellPercent), 0, 100);
            s.ArmorPrice = ini.GetValue("Shop", "ArmorPrice", s.ArmorPrice);
            s.AllowSpecialAmmo = ini.GetValue("Shop", "AllowSpecialAmmo", s.AllowSpecialAmmo);

            string mode = ini.GetValue("Missions", "Selection", "Nearest");
            MissionSelectionMode parsed;
            if (Enum.TryParse(mode, true, out parsed)) s.Selection = parsed;

            s.MinMissionDistance = ini.GetValue("Missions", "MinMissionDistance", s.MinMissionDistance);
            s.MinDeliveryDistance = ini.GetValue("Missions", "MinDeliveryDistance", s.MinDeliveryDistance);
            s.CooldownSeconds = Math.Max(0, ini.GetValue("Missions", "CooldownSeconds", s.CooldownSeconds));
            s.RewardMultiplier = Math.Max(0f, ini.GetValue("Missions", "RewardMultiplier", s.RewardMultiplier));
            s.RobberyWantedLevel = Util.Clamp(ini.GetValue("Missions", "RobberyWantedLevel", s.RobberyWantedLevel), 0, 5);
            s.ConvoyWantedLevel = Util.Clamp(ini.GetValue("Missions", "ConvoyWantedLevel", s.ConvoyWantedLevel), 0, 5);
            s.DefenseWaves = Util.Clamp(ini.GetValue("Missions", "DefenseWaves", s.DefenseWaves), 1, 10);
            s.GuardRespawnSeconds = Math.Max(1, ini.GetValue("Missions", "GuardRespawnSeconds", s.GuardRespawnSeconds));
            s.TruckRespawnSeconds = Math.Max(1, ini.GetValue("Missions", "TruckRespawnSeconds", s.TruckRespawnSeconds));

            return s;
        }

        private static Keys ReadKey(ScriptSettings ini, string section, string key, Keys fallback)
        {
            string raw = ini.GetValue(section, key, fallback.ToString());
            Keys k;
            return Enum.TryParse(raw, true, out k) ? k : fallback;
        }
    }
}
