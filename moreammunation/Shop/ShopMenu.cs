using GTA;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace moreammunation
{
    /// <summary>
    /// The Ammu-Nation + menu. Layout:
    ///
    ///   Current Weapon  >>>
    ///   ── Weapons ──
    ///   Melee / Handguns / SMGs / ...  (each shows how many you own)
    ///   ── Services ──
    ///   Body Armor, Refill All Ammo, Buy All, Sell All
    ///   ── Agent 16 ──
    ///   Jobs >>>
    /// </summary>
    internal sealed class ShopMenu
    {
        private readonly ObjectPool pool;
        private readonly Settings settings;
        private readonly ShopEconomy economy;
        private readonly NativeMenu root;

        private readonly WeaponPanel currentPanel;
        private readonly NativeMenu currentMenu;

        private readonly List<Action> categoryRefreshers = new List<Action>();

        private NativeItem armorItem, refillAllItem, buyAllItem, sellAllItem;
        private int confirmBuyAllUntil, confirmSellAllUntil;

        public ShopMenu(ObjectPool pool, Settings settings, MissionManager missions)
        {
            this.pool = pool;
            this.settings = settings;
            economy = new ShopEconomy(settings);

            root = CreateMenu("CATEGORIES", "Buy, sell and customize weapons.");

            // --- Current weapon ---
            currentMenu = CreateMenu("CURRENT WEAPON", "Manage the weapon in your hands.");
            NativeMenu currentCustomize = CreateMenu("CUSTOMIZE", "Attachments, tints and camos.");
            currentPanel = new WeaponPanel(currentMenu, currentCustomize, economy, settings, RefreshAll);
            NativeSubmenuItem currentItem = root.AddSubMenu(currentMenu);
            currentItem.Title = "Current Weapon";
            currentMenu.Shown += (s, e) => ShowCurrentWeapon();

            // --- Categories ---
            root.Add(new NativeSeparatorItem("Weapons"));
            foreach (WeaponCategory category in WeaponCatalog.Categories)
                BuildCategory(category);

            // --- Services ---
            root.Add(new NativeSeparatorItem("Services"));

            armorItem = new NativeItem("Body Armor", "Full body armor.");
            armorItem.Activated += (s, e) => BuyArmor();
            root.Add(armorItem);

            refillAllItem = new NativeItem("Refill All Ammo", "Top up every weapon you own.");
            refillAllItem.Activated += (s, e) => RefillAll();
            root.Add(refillAllItem);

            buyAllItem = new NativeItem("Buy All Weapons", "Buy every weapon you don't own yet. Press twice to confirm.");
            buyAllItem.Activated += (s, e) => BuyAll();
            root.Add(buyAllItem);

            sellAllItem = new NativeItem("Sell All Weapons", "Sell every weapon from this catalog. Press twice to confirm.");
            sellAllItem.Activated += (s, e) => SellAll();
            root.Add(sellAllItem);

            // --- Jobs (same board as the phone contact, but with this menu as its parent) ---
            root.Add(new NativeSeparatorItem("Agent 16"));
            NativeMenu board = missions.CreateBoardMenu(this);
            NativeSubmenuItem jobsItem = root.AddSubMenu(board);
            jobsItem.Title = "Jobs";
            jobsItem.Description = "Contracts from Agent 16. Jobs start at the location nearest to you.";

            root.Shown += (s, e) => RefreshAll();
        }

        public bool IsOpen { get { return pool.AreAnyVisible; } }

        public void Toggle()
        {
            if (pool.AreAnyVisible) pool.HideAll();
            else root.Visible = true;
        }

        public void Close()
        {
            pool.HideAll();
        }

        /// <summary>Every menu in the mod goes through here so the banner and alignment are consistent.</summary>
        public NativeMenu CreateMenu(string subtitle, string description)
        {
            var menu = new NativeMenu("", subtitle, description,
                new ScaledTexture(PointF.Empty, new SizeF(431, 107), "thumbnail_ammunation_net", "ammunation_banner"));
            menu.Alignment = GTA.UI.Alignment.Left;
            pool.Add(menu);
            return menu;
        }

        private void BuildCategory(WeaponCategory category)
        {
            NativeMenu categoryMenu = CreateMenu(category.Name.ToUpperInvariant(), category.Description);
            NativeSubmenuItem categoryItem = root.AddSubMenu(categoryMenu);
            categoryItem.Title = category.Name;
            categoryItem.Description = category.Description;

            var weaponItems = new List<KeyValuePair<WeaponHash, NativeSubmenuItem>>();

            foreach (var entry in category.Weapons)
            {
                WeaponHash hash = entry.Key;
                string name = WeaponCatalog.DisplayName(hash);

                NativeMenu weaponMenu = CreateMenu(name.ToUpperInvariant(), "Manage your " + name + ".");
                NativeMenu customizeMenu = CreateMenu(name.ToUpperInvariant(), "Attachments, tints and camos.");
                var panel = new WeaponPanel(weaponMenu, customizeMenu, economy, settings, RefreshAll);
                panel.SetWeapon(hash);

                NativeSubmenuItem item = categoryMenu.AddSubMenu(weaponMenu);
                item.Title = name;
                weaponItems.Add(new KeyValuePair<WeaponHash, NativeSubmenuItem>(hash, item));
            }

            Action refresh = () =>
            {
                var weapons = Game.Player.Character.Weapons;
                int owned = 0;
                foreach (var pair in weaponItems)
                {
                    int price;
                    WeaponCatalog.TryGetPrice(pair.Key, out price);
                    bool has = weapons.HasWeapon(pair.Key);
                    if (has) owned++;
                    pair.Value.AltTitle = has ? "~g~Owned" : Util.Money(price);
                }
                categoryItem.AltTitle = owned + "/" + weaponItems.Count;
            };

            categoryRefreshers.Add(refresh);
            categoryMenu.Shown += (s, e) => refresh();
        }

        private void ShowCurrentWeapon()
        {
            WeaponHash held = Game.Player.Character.Weapons.Current.Hash;
            int price;
            if (held == WeaponHash.Unarmed || !WeaponCatalog.TryGetPrice(held, out price))
            {
                currentMenu.Clear();
                currentMenu.Add(new NativeItem("~c~Hold a weapon from the catalog",
                    "Switch to a weapon sold here, then open this page again.") { Enabled = false });
                return;
            }

            currentPanel.SetWeapon(held);
            currentMenu.Name = currentPanel.Name.ToUpperInvariant();
        }

        // ---------------------------------------------------------------- services

        private IEnumerable<KeyValuePair<WeaponHash, int>> OwnedCatalogWeapons()
        {
            var weapons = Game.Player.Character.Weapons;
            return WeaponCatalog.Categories.SelectMany(c => c.Weapons).Where(w => weapons.HasWeapon(w.Key));
        }

        private int RefillAllCost()
        {
            var weapons = Game.Player.Character.Weapons;
            int total = 0;
            foreach (var w in OwnedCatalogWeapons())
            {
                WeaponCategory cat = WeaponCatalog.CategoryOf(w.Key);
                if (cat == null || cat.Ammo == AmmoKind.None) continue;
                total += economy.FillPrice(cat.Ammo, weapons[w.Key]);
            }
            return total;
        }

        private void RefreshAll()
        {
            foreach (Action a in categoryRefreshers) a();

            Ped player = Game.Player.Character;
            bool armorFull = player.Armor >= 100;
            armorItem.Enabled = !armorFull;
            armorItem.AltTitle = armorFull ? "Full" : Util.Money(settings.ArmorPrice);

            int refill = RefillAllCost();
            refillAllItem.Enabled = refill > 0;
            refillAllItem.AltTitle = refill > 0 ? Util.Money(refill) : "Full";

            var weapons = player.Weapons;
            var all = WeaponCatalog.Categories.SelectMany(c => c.Weapons).ToList();
            int buyAllCost = all.Where(w => !weapons.HasWeapon(w.Key)).Sum(w => w.Value);
            int sellAllValue = all.Where(w => weapons.HasWeapon(w.Key)).Sum(w => economy.SellPrice(w.Value));

            buyAllItem.Enabled = buyAllCost > 0;
            buyAllItem.AltTitle = buyAllCost > 0 ? Util.Money(buyAllCost) : "Owned";
            sellAllItem.Enabled = sellAllValue > 0;
            sellAllItem.AltTitle = sellAllValue > 0 ? "+" + Util.Money(sellAllValue) : "";

            if (Game.GameTime > confirmBuyAllUntil) buyAllItem.Title = "Buy All Weapons";
            if (Game.GameTime > confirmSellAllUntil) sellAllItem.Title = "Sell All Weapons";
        }

        private void BuyArmor()
        {
            Ped player = Game.Player.Character;
            if (player.Armor >= 100) return;
            if (!economy.TryPay(settings.ArmorPrice)) return;
            player.Armor = 100;
            Notify.Shop("~w~Body armor equipped for ~r~" + Util.Money(settings.ArmorPrice), "Receipt");
            RefreshAll();
        }

        private void RefillAll()
        {
            int cost = RefillAllCost();
            if (cost <= 0) return;
            if (!economy.TryPay(cost)) return;

            var weapons = Game.Player.Character.Weapons;
            foreach (var w in OwnedCatalogWeapons().ToList())
            {
                WeaponCategory cat = WeaponCatalog.CategoryOf(w.Key);
                if (cat == null || cat.Ammo == AmmoKind.None) continue;
                Weapon weapon = weapons[w.Key];
                weapon.Ammo = weapon.MaxAmmo;
            }
            Notify.Shop("~w~All weapons refilled for ~r~" + Util.Money(cost), "Receipt");
            RefreshAll();
        }

        private void BuyAll()
        {
            if (Game.GameTime > confirmBuyAllUntil)
            {
                confirmBuyAllUntil = Game.GameTime + 4000;
                buyAllItem.Title = "~y~Press again to confirm";
                return;
            }
            confirmBuyAllUntil = 0;

            var weapons = Game.Player.Character.Weapons;
            var missing = WeaponCatalog.Categories.SelectMany(c => c.Weapons).Where(w => !weapons.HasWeapon(w.Key)).ToList();
            int total = missing.Sum(w => w.Value);
            if (total <= 0 || !economy.TryPay(total)) { RefreshAll(); return; }

            foreach (var w in missing)
                weapons.Give(w.Key, 999, false, true);

            Notify.Shop("~w~Purchased ~b~" + missing.Count + "~w~ weapons for ~r~" + Util.Money(total), "Receipt");
            RefreshAll();
        }

        private void SellAll()
        {
            if (Game.GameTime > confirmSellAllUntil)
            {
                confirmSellAllUntil = Game.GameTime + 4000;
                sellAllItem.Title = "~y~Press again to confirm";
                return;
            }
            confirmSellAllUntil = 0;

            var weapons = Game.Player.Character.Weapons;
            var owned = OwnedCatalogWeapons().ToList();
            int total = 0;
            foreach (var w in owned)
            {
                weapons.Remove(w.Key);
                total += economy.SellPrice(w.Value);
            }
            Game.Player.Money += total;

            Notify.Shop("~w~Sold ~b~" + owned.Count + "~w~ weapons for ~g~" + Util.Money(total), "Receipt");
            RefreshAll();
        }
    }
}
