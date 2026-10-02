# More Ammu-Nation – rework

## Code structure
`Class1.cs` (9,400 lines) is split into small files:

| Folder | What's in it |
|---|---|
| `Main.cs` | Entry point: wiring, ticks, keys, phone contact |
| `Core/` | Settings, spawning, relationship groups, entity cleanup, nearest-location logic, logging |
| `Data/WeaponCatalog.cs` | The single price list (was duplicated 3 times) |
| `Shop/` | Menu, one reusable weapon page, weapon customizer (replaces 9 copy-pasted blocks) |
| `World/ArmoryZoneManager.cs` | Store locations, trucks, guards, prompts, respawns |
| `Missions/` | Mission base class, manager/job board, one file per mission |

Errors now go to `scripts\MoreAmmunationsMod\log.txt` instead of disappearing.

## Missions
- All jobs start **near the player**. Contact missions use the nearest XML location (`[Missions] Selection` in the ini).
- The truck robbery drops off at the **nearest** delivery point that's at least `MinDeliveryDistance` away.
- Enemies spawn when you get close instead of instantly across the map.
- New: **Hijack the Convoy**, **Raid a Gun Stash**, **Eliminate the Dealer**, **Defend the Store** (at your nearest Ammu-Nation).
- Agent 16's board opens from the phone, from the shop ("Jobs"), or an optional hotkey. It shows the nearest location, distance, reward, and your record (saved in `stats.txt`). During a job it shows the objective and lets you abort.

## Shop
- Prices / "Owned" shown on the right of every item, owned count per category.
- Fill Ammo charges for what you're missing (capped at the old price).
- New: Body Armor, Refill All Ammo. Buy All / Sell All ask for a second press.
- Customize: exclusive parts (magazine, optic, muzzle, camo...) are left/right lists that apply instantly; tints use the game's real tint count.
- Weapon names use the game's own names ("Pistol .50").

## Bugs fixed
- `GetRandomWeapon` could only return rifles or machine guns (branch order).
- Ground troops ignored unless the XML also had `<AreaVehicles>`.
- `float.Parse` broke XML loading on French/German Windows.
- Holding E toggled the menu every frame; Q started the robbery twice.
- "Current Weapon" leaked a new menu into the pool every time it opened.
- Contact mission positions were snapped to roads at load time, before road data was loaded.
- Barrel checkboxes were stored in the clip list; selling a weapon overwrote the wrong description.
- Keys in the ini were documented but never read.
