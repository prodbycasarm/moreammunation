using GTA.UI;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace moreammunation
{
    internal static class Util
    {
        public static int Clamp(int value, int min, int max)
        {
            return value < min ? min : (value > max ? max : value);
        }

        public static string Money(int amount)
        {
            return "$" + amount.ToString("N0", CultureInfo.InvariantCulture);
        }

        public static string Distance(float meters)
        {
            return meters >= 1000f
                ? (meters / 1000f).ToString("0.0", CultureInfo.InvariantCulture) + " km"
                : ((int)meters).ToString(CultureInfo.InvariantCulture) + " m";
        }

        /// <summary>
        /// Always parses with a "." decimal separator. The old code used float.Parse,
        /// which throws or misreads "13.41886" on French/German Windows.
        /// </summary>
        public static float ParseFloat(string text, float fallback = 0f)
        {
            float value;
            if (!string.IsNullOrWhiteSpace(text) &&
                float.TryParse(text.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                return value;
            return fallback;
        }

        public static float XmlFloat(XmlNode parent, string child, float fallback = 0f)
        {
            if (parent == null) return fallback;
            XmlNode node = parent[child];
            return node == null ? fallback : ParseFloat(node.InnerText, fallback);
        }

        public static string XmlText(XmlNode parent, string child, string fallback = "")
        {
            if (parent == null) return fallback;
            XmlNode node = parent[child];
            return node == null ? fallback : node.InnerText.Trim();
        }

        /// <summary>"CarbineRifleMk2" -> "Carbine Rifle Mk2"</summary>
        public static string SplitCamelCase(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var sb = new StringBuilder(text.Length + 8);
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (i > 0)
                {
                    char p = text[i - 1];
                    bool upperAfterLower = char.IsUpper(c) && char.IsLower(p);
                    bool digitAfterLetter = char.IsDigit(c) && char.IsLetter(p);
                    bool upperBeforeLower = char.IsUpper(c) && char.IsUpper(p) && i + 1 < text.Length && char.IsLower(text[i + 1]);
                    if (upperAfterLower || digitAfterLetter || upperBeforeLower) sb.Append(' ');
                }
                sb.Append(c == '_' ? ' ' : c);
            }
            return sb.ToString();
        }
    }

    /// <summary>Writes errors to scripts\MoreAmmunationsMod\log.txt instead of failing silently.</summary>
    internal static class Log
    {
        private static readonly string LogPath = Settings.DataFolder + "\\log.txt";

        public static void Write(string message)
        {
            try
            {
                Directory.CreateDirectory(Settings.DataFolder);
                File.AppendAllText(LogPath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "  " + message + Environment.NewLine);
            }
            catch
            {
                // logging must never crash the script
            }
        }

        public static void Error(string context, Exception ex)
        {
            Write("ERROR in " + context + ": " + ex);
        }
    }

    internal static class Notify
    {
        public static void Shop(string message, string subject = "Alert")
        {
            Notification.Show(NotificationIcon.Ammunation, "Ammu-Nation +", subject, message, true, false);
        }

        public static void Agent(string message, string subject = "Ammu-Nation +")
        {
            Notification.Show(NotificationIcon.MpFIBContact, "Agent 16", subject, message, true, false);
        }
    }
}
