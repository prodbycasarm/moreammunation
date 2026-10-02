using GTA;
using GTA.Native;
using LemonUI.Menus;
using System.Collections.Generic;
using System.Linq;

namespace moreammunation
{
    /// <summary>
    /// Builds the "Customize" page for any weapon.
    /// This single class replaces the ~550-line customization block that was
    /// copy-pasted into every weapon category.
    ///
    /// UX change: parts that are mutually exclusive (magazine, optic, muzzle,
    /// camo...) are now left/right lists that apply instantly, instead of a
    /// row of checkboxes where you could tick two scopes at once.
    /// </summary>
    internal static class WeaponCustomizer
    {
        private enum Slot { Magazine, Barrel, Optic, Muzzle, Flashlight, Grip, Camo, SlideCamo, Finish, Variant, Other }
        private static void LoadNewAmmoType(WeaponHash hash, Settings settings)
        {
            Ped player = Game.Player.Character;
            Weapon weapon = player.Weapons[hash];
            if (weapon == null || !weapon.IsPresent) return;

            if (weapon.Ammo <= 0)
            {
                Function.Call(Hash.ADD_AMMO_TO_PED, player.Handle, (uint)hash, settings.AmmoPackAmount);
                Notify.Shop("~w~New rounds loaded: ~b~" + settings.AmmoPackAmount +
                            "~w~ free. Use Buy Ammo or Fill Ammo for more.");
            }

            if (player.Weapons.Current.Hash == hash)
                player.Weapons.Select(hash, true);
        }
        private static readonly Dictionary<uint, string> MeleeVariants = new Dictionary<uint, string>
        {
            { 4007263587u, "Ballas" }, { 4081463091u, "Base" }, { 2539772380u, "Diamond" },
            { 1351683121u, "Dollar" }, { 2112683568u, "Hate" }, { 3800804335u, "King" },
            { 1062111910u, "Love" }, { 3323197061u, "Pimp" }, { 146278587u, "Player" },
            { 2062808965u, "Vagos" }, { 2436343040u, "Base" }, { 1530822070u, "Variant 1" },
            { 3885209186u, "Variant 2" }, { 3372082259u, "Green" }, { 3014965697u, "Orange" }
        };

        private static readonly string[] StandardTints =
        {
            "Black", "Green", "Gold", "Pink", "Army", "LSPD", "Orange", "Platinum"
        };

        private static readonly string[] Mk2Tints =
        {
            "Classic Black", "Classic Gray", "Classic Two-Tone", "Classic White", "Classic Beige", "Classic Green",
            "Classic Blue", "Classic Earth", "Classic Brown & Black", "Red Contrast", "Blue Contrast", "Yellow Contrast",
            "Orange Contrast", "Bold Pink", "Bold Purple & Yellow", "Bold Orange", "Bold Green & Purple",
            "Bold Red Features", "Bold Green Features", "Bold Cyan Features", "Bold Yellow Features",
            "Bold Red & White", "Bold Blue & White", "Metallic Gold", "Metallic Platinum", "Metallic Gray & Lilac",
            "Metallic Purple & Lime", "Metallic Red", "Metallic Green", "Metallic Blue", "Metallic White & Aqua",
            "Metallic Orange & Yellow", "Metallic Red and Yellow"
        };

        private static readonly string[] SpecialAmmoWords = { "armorpiercing", "fmj", "incendiary", "tracer", "hollowpoint", "explosive" };

        // tokens dropped/renamed when turning enum names into labels
        private static readonly HashSet<string> DropTokens = new HashSet<string> { "At", "Ar", "Pi", "Sr", "Sc", "Mg", "Af", "Sb", "Mrfl" };
        private static readonly Dictionary<string, string> RenameTokens = new Dictionary<string, string>
        {
            { "Supp", "Suppressor" }, { "Flsh", "Flashlight" }, { "Comp", "Compensator" }, { "Fmj", "FMJ" },
            { "Luxe", "Luxury Finish" }, { "Varmod", "Finish" }
        };

        public static void Populate(NativeMenu menu, WeaponHash hash, Settings settings)
        {
            menu.Clear();

            Ped player = Game.Player.Character;
            Weapon weapon = player.Weapons[hash];
            if (weapon == null || !weapon.IsPresent)
            {
                menu.Add(new NativeItem("~c~You don't own this weapon") { Enabled = false });
                return;
            }

            string weaponName = hash.ToString();
            var groups = new Dictionary<Slot, List<KeyValuePair<WeaponComponent, string>>>();

            foreach (WeaponComponent component in weapon.Components)
            {
                if (component == null) continue;

                string label;
                Slot? slot = Classify(component, weaponName, settings, out label);
                if (slot == null) continue;

                List<KeyValuePair<WeaponComponent, string>> list;
                if (!groups.TryGetValue(slot.Value, out list))
                {
                    list = new List<KeyValuePair<WeaponComponent, string>>();
                    groups[slot.Value] = list;
                }
                list.Add(new KeyValuePair<WeaponComponent, string>(component, label));
            }

            //AddChoice(menu, groups, Slot.Magazine, "Magazine", false);
            NativeListItem<string> magazine = AddChoice(menu, groups, Slot.Magazine, "Magazine", false);
            if (magazine != null)
                magazine.ItemChanged += (s, e) => LoadNewAmmoType(hash, settings);
            AddChoice(menu, groups, Slot.Barrel, "Barrel", false);
            AddChoice(menu, groups, Slot.Optic, "Optic", true);
            AddChoice(menu, groups, Slot.Muzzle, "Muzzle", true);
            AddToggles(menu, groups, Slot.Flashlight);
            AddToggles(menu, groups, Slot.Grip);
            AddChoice(menu, groups, Slot.Variant, "Style", true);
            AddChoice(menu, groups, Slot.Finish, "Finish", true);

            AddTint(menu, hash, player);

            NativeListItem<string> camo = AddChoice(menu, groups, Slot.Camo, "Camo", true);
            if (camo != null) AddCamoColor(menu, groups, Slot.Camo, "Camo Color", hash, player);

            NativeListItem<string> slide = AddChoice(menu, groups, Slot.SlideCamo, "Slide Camo", true);
            if (slide != null) AddCamoColor(menu, groups, Slot.SlideCamo, "Slide Color", hash, player);

            AddToggles(menu, groups, Slot.Other);

            if (menu.Items.Count == 0)
                menu.Add(new NativeItem("~c~No customizations available") { Enabled = false });
        }

        private static Slot? Classify(WeaponComponent component, string weaponName, Settings settings, out string label)
        {
            uint raw = (uint)component.ComponentHash;
            string variantName;
            if (MeleeVariants.TryGetValue(raw, out variantName))
            {
                label = variantName;
                return Slot.Variant;
            }

            string name = component.ComponentHash.ToString();
            label = null;

            uint dummy;
            if (uint.TryParse(name, out dummy)) return null;            // unknown to SHVDN, can't name it
            if (name == "Invalid" || name == "GunrunMk2Upgrade") return null;

            // Strip the weapon's own name so "CompactRifleClip01" isn't mistaken for a compensator
            string stripped = name.StartsWith(weaponName, System.StringComparison.OrdinalIgnoreCase)
                ? name.Substring(weaponName.Length)
                : name;
            if (stripped.Length == 0) stripped = name;
            string low = stripped.ToLowerInvariant();

            if (low.Contains("clip") && SpecialAmmoWords.Any(w => low.Contains(w)) && !settings.AllowSpecialAmmo)
                return null;

            label = Pretty(stripped);

            if (low.Contains("camo") && low.EndsWith("slide")) return Slot.SlideCamo;
            if (low.Contains("camo")) return Slot.Camo;
            if (low.Contains("luxe") || low.Contains("varmod")) return Slot.Finish;
            if (low.Contains("clip")) return Slot.Magazine;
            if (low.Contains("barrel")) return Slot.Barrel;
            if (low.Contains("scope") || low.Contains("sight")) return Slot.Optic;
            if (low.Contains("flsh") || low.Contains("flashlight")) return Slot.Flashlight;
            if (low.Contains("grip")) return Slot.Grip;
            if (low.Contains("supp") || low.Contains("muzzle") || low.Contains("comp")) return Slot.Muzzle;
            return Slot.Other;
        }

        private static string Pretty(string raw)
        {
            string[] tokens = Util.SplitCamelCase(raw).Split(' ');
            var kept = new List<string>();
            foreach (string t in tokens)
            {
                if (t.Length == 0 || DropTokens.Contains(t)) continue;
                string renamed;
                kept.Add(RenameTokens.TryGetValue(t, out renamed) ? renamed : t);
            }
            string result = string.Join(" ", kept);
            return result.Length > 0 ? result : raw;
        }

        /// <summary>A left/right list where only one part of the group can be active.</summary>
        private static NativeListItem<string> AddChoice(NativeMenu menu, Dictionary<Slot, List<KeyValuePair<WeaponComponent, string>>> groups,
                                                         Slot slot, string title, bool allowNone)
        {
            List<KeyValuePair<WeaponComponent, string>> parts;
            if (!groups.TryGetValue(slot, out parts) || parts.Count == 0) return null;
            if (!allowNone && parts.Count < 2) return null; // only the default part -> nothing to choose

            var labels = new List<string>();
            if (allowNone) labels.Add("None");
            for (int i = 0; i < parts.Count; i++)
            {
                string l = parts[i].Value;
                if (labels.Contains(l)) l = l + " (" + (i + 1) + ")";
                labels.Add(l);
            }

            int offset = allowNone ? 1 : 0;
            int selected = 0;
            for (int i = 0; i < parts.Count; i++)
                if (parts[i].Key.Active) selected = i + offset;

            var item = new NativeListItem<string>(title, "Scroll left/right to preview. Changes apply instantly.", labels.ToArray())
            {
                SelectedIndex = selected
            };

            item.ItemChanged += (s, e) =>
            {
                int index = item.SelectedIndex - offset;
                foreach (var p in parts) p.Key.Active = false;
                if (index >= 0 && index < parts.Count) parts[index].Key.Active = true;
            };

            menu.Add(item);
            return item;
        }

        private static void AddToggles(NativeMenu menu, Dictionary<Slot, List<KeyValuePair<WeaponComponent, string>>> groups, Slot slot)
        {
            List<KeyValuePair<WeaponComponent, string>> parts;
            if (!groups.TryGetValue(slot, out parts)) return;

            foreach (var part in parts)
            {
                WeaponComponent component = part.Key;
                var box = new NativeCheckboxItem(part.Value, component.Active);
                box.CheckboxChanged += (s, e) => component.Active = box.Checked;
                menu.Add(box);
            }
        }

        private static void AddTint(NativeMenu menu, WeaponHash hash, Ped player)
        {
            int count = Function.Call<int>(Hash.GET_WEAPON_TINT_COUNT, (uint)hash);
            if (count <= 1) return;

            string[] source = count > StandardTints.Length ? Mk2Tints : StandardTints;
            var names = new string[count];
            for (int i = 0; i < count; i++)
                names[i] = i < source.Length ? source[i] : "Tint " + (i + 1);

            int current = Function.Call<int>(Hash.GET_PED_WEAPON_TINT_INDEX, player.Handle, (uint)hash);
            var item = new NativeListItem<string>("Tint", "Weapon paint. Applies instantly.", names)
            {
                SelectedIndex = Util.Clamp(current, 0, count - 1)
            };
            item.ItemChanged += (s, e) =>
                Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character.Handle, (uint)hash, item.SelectedIndex);

            menu.Add(item);
        }

        private static void AddCamoColor(NativeMenu menu, Dictionary<Slot, List<KeyValuePair<WeaponComponent, string>>> groups,
                                         Slot slot, string title, WeaponHash hash, Ped player)
        {
            var colors = new string[32];
            colors[0] = "Default";
            for (int i = 1; i < colors.Length; i++) colors[i] = "Color " + i;

            var item = new NativeListItem<string>(title, "Color of the selected camo. Pick a camo first.", colors);
            item.ItemChanged += (s, e) =>
            {
                KeyValuePair<WeaponComponent, string> active = groups[slot].FirstOrDefault(p => p.Key.Active);
                if (active.Key == null) return;
                Function.Call(Hash.SET_PED_WEAPON_COMPONENT_TINT_INDEX, Game.Player.Character.Handle,
                              (uint)hash, (uint)active.Key.ComponentHash, item.SelectedIndex);
            };
            menu.Add(item);
        }
    }
}
