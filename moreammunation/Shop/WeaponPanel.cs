using GTA;
using LemonUI.Menus;
using System;

namespace moreammunation
{
    /// <summary>Money and ammo math shared by every page of the shop.</summary>
    internal sealed class ShopEconomy
    {
        private readonly Settings s;
        public ShopEconomy(Settings settings) { s = settings; }

        public int SellPrice(int price) { return (int)Math.Round(price * (s.SellPercent / 100.0)); }

        public int PackPrice(AmmoKind kind) { return kind == AmmoKind.Throwable ? s.ThrowablePackPrice : s.AmmoPackPrice; }
        public int PackAmount(AmmoKind kind) { return kind == AmmoKind.Throwable ? s.ThrowablePackAmount : s.AmmoPackAmount; }

        /// <summary>
        /// Refill now costs what you're actually missing (in packs), capped at the old flat price.
        /// Topping up 10 bullets no longer costs $5,000.
        /// </summary>
        public int FillPrice(AmmoKind kind, Weapon weapon)
        {
            if (kind == AmmoKind.None || weapon == null || !weapon.IsPresent) return 0;
            int missing = weapon.MaxAmmo - weapon.Ammo;
            if (missing <= 0) return 0;
            int packs = (int)Math.Ceiling(missing / (double)PackAmount(kind));
            int cap = kind == AmmoKind.Throwable ? s.ThrowableRefillPriceCap : s.FullRefillPriceCap;
            return Math.Min(cap, packs * PackPrice(kind));
        }

        public bool TryPay(int amount)
        {
            if (Game.Player.Money < amount)
            {
                Notify.Shop("~r~Not enough money!~w~ You need ~b~" + Util.Money(amount), "Insufficient Funds");
                return false;
            }
            Game.Player.Money -= amount;
            return true;
        }
    }

    /// <summary>
    /// One weapon page. Items are created once and simply enabled/disabled,
    /// instead of being inserted/removed (which the old code did, and which
    /// shifted the cursor around). The same class serves both the catalog
    /// pages and the "Current Weapon" page, which just re-targets it.
    /// </summary>
    internal sealed class WeaponPanel
    {
        private readonly NativeMenu menu;
        private readonly NativeMenu customizeMenu;
        private readonly NativeSubmenuItem customizeItem;
        private readonly ShopEconomy economy;
        private readonly Settings settings;
        private readonly Action onChanged;

        private readonly NativeItem buyItem = new NativeItem("Buy");
        private readonly NativeItem equipItem = new NativeItem("Equip");
        private readonly NativeItem ammoItem = new NativeItem("Buy Ammo");
        private readonly NativeItem fillItem = new NativeItem("Fill Ammo");
        private readonly NativeItem sellItem = new NativeItem("Sell");

        public WeaponHash Hash { get; private set; }
        public int Price { get; private set; }
        public WeaponCategory Category { get; private set; }
        public string Name { get; private set; }

        public WeaponPanel(NativeMenu menu, NativeMenu customizeMenu, ShopEconomy economy, Settings settings, Action onChanged)
        {
            this.menu = menu;
            this.customizeMenu = customizeMenu;
            this.economy = economy;
            this.settings = settings;
            this.onChanged = onChanged;

            customizeItem = menu.AddSubMenu(customizeMenu);
            customizeItem.Title = "Customize";
            customizeItem.Description = "Attachments, tints and camos.";

            buyItem.Activated += (s, e) => Buy();
            equipItem.Activated += (s, e) => Equip();
            ammoItem.Activated += (s, e) => BuyAmmo();
            fillItem.Activated += (s, e) => FillAmmo();
            sellItem.Activated += (s, e) => Sell();

            menu.Shown += (s, e) => Refresh();
            customizeMenu.Shown += (s, e) => WeaponCustomizer.Populate(customizeMenu, Hash, settings);
        }

        /// <summary>Point this page at a weapon and rebuild its item list.</summary>
        public void SetWeapon(WeaponHash hash)
        {
            int price;
            WeaponCatalog.TryGetPrice(hash, out price);
            Hash = hash;
            Price = price;
            Category = WeaponCatalog.CategoryOf(hash);
            Name = WeaponCatalog.DisplayName(hash);

            menu.Clear();
            menu.Add(buyItem);
            menu.Add(equipItem);
            if (Category != null && Category.Ammo != AmmoKind.None)
            {
                menu.Add(ammoItem);
                menu.Add(fillItem);
            }
            if (Category != null && Category.Customizable)
                menu.Add(customizeItem);
            menu.Add(sellItem);

            Refresh();
        }

        private Weapon Current { get { return Game.Player.Character.Weapons[Hash]; } }
        private bool Owned { get { return Game.Player.Character.Weapons.HasWeapon(Hash); } }

        public void Refresh()
        {
            if (Category == null) return;

            bool owned = Owned;
            bool equipped = owned && Game.Player.Character.Weapons.Current.Hash == Hash;
            Weapon w = owned ? Current : null;

            buyItem.Enabled = !owned;
            buyItem.AltTitle = owned ? "Owned" : Util.Money(Price);
            buyItem.Description = owned ? "You already own the " + Name + "." : "Purchase the " + Name + ".";

            equipItem.Enabled = owned && !equipped;
            equipItem.AltTitle = equipped ? "Equipped" : "";
            equipItem.Description = owned ? "Put the " + Name + " in your hands." : "~c~Buy the weapon first.";

            if (Category.Ammo != AmmoKind.None)
            {
                int pack = economy.PackAmount(Category.Ammo);
                bool full = w != null && w.Ammo >= w.MaxAmmo;
                string counter = w != null ? " (" + w.Ammo + "/" + w.MaxAmmo + ")" : "";

                ammoItem.Enabled = owned && !full;
                ammoItem.AltTitle = Util.Money(economy.PackPrice(Category.Ammo));
                ammoItem.Description = owned ? "+" + pack + " rounds" + counter : "~c~Buy the weapon first.";

                int fill = economy.FillPrice(Category.Ammo, w);
                fillItem.Enabled = owned && !full;
                fillItem.AltTitle = full ? "Full" : Util.Money(fill);
                fillItem.Description = owned ? "Top up to max ammo" + counter + "." : "~c~Buy the weapon first.";
            }

            customizeItem.Enabled = owned;
            customizeItem.AltTitle = owned ? ">>>" : "";

            int sell = economy.SellPrice(Price);
            sellItem.Enabled = owned;
            sellItem.AltTitle = owned ? "+" + Util.Money(sell) : "";
            sellItem.Description = owned ? "Sell the " + Name + " back." : "~c~You don't own this weapon.";
        }

        private void Changed()
        {
            Refresh();
            if (onChanged != null) onChanged();
        }

        private void Buy()
        {
            var weapons = Game.Player.Character.Weapons;
            if (weapons.HasWeapon(Hash)) return;
            if (!economy.TryPay(Price)) return;

            weapons.Give(Hash, settings.StarterAmmo, true, true);
            Notify.Shop("~w~You purchased the ~g~" + Name + "~w~ for ~r~" + Util.Money(Price), "Receipt");
            Changed();
        }

        private void Equip()
        {
            if (!Owned) return;
            Game.Player.Character.Weapons.Select(Hash, true);
            Changed();
        }

        private void BuyAmmo()
        {
            Weapon w = Current;
            if (w == null || !w.IsPresent) return;
            if (w.Ammo >= w.MaxAmmo)
            {
                Notify.Shop("~w~Your ~b~" + Name + "~w~ is already full.");
                return;
            }

            int price = economy.PackPrice(Category.Ammo);
            if (!economy.TryPay(price)) return;

            int amount = economy.PackAmount(Category.Ammo);
            w.Ammo = Math.Min(w.MaxAmmo, w.Ammo + amount);
            Notify.Shop("~w~Purchased ~b~" + amount + "~w~ rounds for ~r~" + Util.Money(price), "Receipt");
            Changed();
        }

        private void FillAmmo()
        {
            Weapon w = Current;
            if (w == null || !w.IsPresent) return;
            int price = economy.FillPrice(Category.Ammo, w);
            if (price <= 0) return;
            if (!economy.TryPay(price)) return;

            w.Ammo = w.MaxAmmo;
            Notify.Shop("~w~Ammo filled for ~r~" + Util.Money(price), "Receipt");
            Changed();
        }

        private void Sell()
        {
            var weapons = Game.Player.Character.Weapons;
            if (!weapons.HasWeapon(Hash)) return;

            int value = economy.SellPrice(Price);
            weapons.Remove(Hash);
            Game.Player.Money += value;
            Notify.Shop("~w~Sold the ~b~" + Name + "~w~ for ~g~" + Util.Money(value), "Receipt");
            Changed();
        }
    }
}
