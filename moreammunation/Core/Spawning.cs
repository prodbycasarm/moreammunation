using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace moreammunation
{
    /// <summary>Relationship groups shared by the whole mod.</summary>
    internal static class Groups
    {
        public static RelationshipGroup Guards;    // Ammu-Nation guards: leave the player alone unless provoked
        public static RelationshipGroup Hostiles;  // mission enemies: attack the player and the guards

        private static bool created;

        /// <summary>Call before every mission, since the player can switch characters.</summary>
        public static void Refresh()
        {
            if (!created)
            {
                Guards = World.AddRelationshipGroup("AMMU_PLUS_GUARDS");
                Hostiles = World.AddRelationshipGroup("AMMU_PLUS_HOSTILES");
                created = true;
            }

            RelationshipGroup player = Game.Player.Character.RelationshipGroup;

            Set(Relationship.Companion, Hostiles, Hostiles);
            Set(Relationship.Hate, Hostiles, player);
            Set(Relationship.Hate, player, Hostiles);
            Set(Relationship.Hate, Hostiles, Guards);
            Set(Relationship.Hate, Guards, Hostiles);

            Set(Relationship.Companion, Guards, Guards);
            Set(Relationship.Neutral, Guards, player);
            Set(Relationship.Neutral, player, Guards);
        }

        private static void Set(Relationship relationship, RelationshipGroup a, RelationshipGroup b)
        {
            Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)relationship, a, b);
        }
    }

    internal static class Spawner
    {
        private static readonly Random Rng = new Random();

        public static bool LoadModel(Model model, int timeoutMs = 5000)
        {
            if (!model.IsValid || !model.IsInCdImage) return false;
            model.Request();
            int deadline = Game.GameTime + timeoutMs;
            while (!model.IsLoaded)
            {
                if (Game.GameTime > deadline) return false;
                Script.Yield();
            }
            return true;
        }

        public static Vehicle CreateVehicle(string modelName, Vector3 position, float heading)
        {
            if (string.IsNullOrWhiteSpace(modelName)) return null;
            var model = new Model(modelName.Trim());
            if (!LoadModel(model))
            {
                Log.Write("Vehicle model could not be loaded: " + modelName);
                return null;
            }

            Vehicle v = World.CreateVehicle(model, position, heading);
            model.MarkAsNoLongerNeeded();
            if (v == null || !v.Exists()) return null;

            v.IsPersistent = true;
            Function.Call(Hash.SET_ENTITY_LOAD_COLLISION_FLAG, v.Handle, true, 1);
            return v;
        }

        public static Ped CreatePed(string modelName, Vector3 position, float heading = 0f)
        {
            if (string.IsNullOrWhiteSpace(modelName)) return null;
            var model = new Model(modelName.Trim());
            if (!LoadModel(model))
            {
                Log.Write("Ped model could not be loaded: " + modelName);
                return null;
            }

            Ped p = World.CreatePed(model, position, heading);
            model.MarkAsNoLongerNeeded();
            if (p == null || !p.Exists()) return null;

            p.IsPersistent = true;
            Function.Call(Hash.SET_ENTITY_LOAD_COLLISION_FLAG, p.Handle, true, 1);
            return p;
        }

        /// <summary>
        /// Seat index 0 = driver, 1 = front passenger, 2+ = rear/extra seats.
        /// If the vehicle is full, the ped is placed on foot next to it instead
        /// of being silently dropped (the old code lost those peds).
        /// </summary>
        public static Ped CreatePedInVehicle(Vehicle vehicle, int seatIndex, string modelName)
        {
            if (vehicle == null || !vehicle.Exists()) return null;

            VehicleSeat seat = (VehicleSeat)(seatIndex - 1);
            bool hasSeat = seatIndex == 0 || (seatIndex - 1) < vehicle.PassengerCapacity;

            if (hasSeat && vehicle.IsSeatFree(seat))
            {
                var model = new Model(modelName.Trim());
                if (!LoadModel(model)) return null;
                Ped p = vehicle.CreatePedOnSeat(seat, model);
                model.MarkAsNoLongerNeeded();
                if (p != null && p.Exists())
                {
                    p.IsPersistent = true;
                    return p;
                }
                return null;
            }

            return CreatePed(modelName, OnFootPositionNear(vehicle.Position, 3f, 6f));
        }

        /// <summary>A walkable spot near a point. Only reliable when the area is streamed in.</summary>
        public static Vector3 OnFootPositionNear(Vector3 center, float minRadius, float maxRadius)
        {
            for (int i = 0; i < 6; i++)
            {
                double angle = Rng.NextDouble() * Math.PI * 2.0;
                float dist = minRadius + (float)Rng.NextDouble() * (maxRadius - minRadius);
                var probe = new Vector3(center.X + (float)Math.Cos(angle) * dist,
                                        center.Y + (float)Math.Sin(angle) * dist,
                                        center.Z);

                Vector3 safe = World.GetSafeCoordForPed(probe, false, 0);
                if (safe != Vector3.Zero && safe.DistanceTo(center) < maxRadius + 15f)
                    return safe;

                float ground = World.GetGroundHeight(new Vector3(probe.X, probe.Y, center.Z + 5f));
                if (ground > 0f && Math.Abs(ground - center.Z) < 8f)
                    return new Vector3(probe.X, probe.Y, ground);
            }
            return center + new Vector3(0f, 0f, 1f);
        }

        public static void MakeHostile(Ped p, WeaponHash weapon, int accuracy = 40, int armor = 0)
        {
            if (p == null || !p.Exists()) return;

            p.RelationshipGroup = Groups.Hostiles;
            p.BlockPermanentEvents = false;
            p.Weapons.Give(weapon, 9999, true, true);
            p.CanSwitchWeapons = true;
            p.Accuracy = accuracy;
            if (armor > 0) p.Armor = armor;

            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p.Handle, 46, true); // always fight
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, p.Handle, 5, true);  // fight armed peds when unarmed
            Function.Call(Hash.SET_PED_COMBAT_ABILITY, p.Handle, 2);
            Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, p.Handle, 0, false);
        }

        public static Blip EnemyBlip(Ped p)
        {
            if (p == null || !p.Exists()) return null;
            Blip b = p.AddBlip();
            b.Sprite = BlipSprite.Enemy;
            b.Color = BlipColor.Red;
            b.Scale = 0.7f;
            b.Name = "Enemy";
            return b;
        }

        public static T Pick<T>(IList<T> list)
        {
            return list[Rng.Next(list.Count)];
        }

        public static int Range(int minInclusive, int maxInclusive)
        {
            return Rng.Next(minInclusive, maxInclusive + 1);
        }


        private static readonly WeaponHash[] EnemyRifles = { WeaponHash.AssaultRifle, WeaponHash.CarbineRifle, WeaponHash.AdvancedRifle, WeaponHash.SpecialCarbine, WeaponHash.BullpupRifle, WeaponHash.CompactRifle };
        private static readonly WeaponHash[] EnemySMGs = { WeaponHash.MicroSMG, WeaponHash.SMG, WeaponHash.AssaultSMG, WeaponHash.CombatPDW, WeaponHash.MiniSMG };
        private static readonly WeaponHash[] EnemyShotguns = { WeaponHash.PumpShotgun, WeaponHash.SawnOffShotgun, WeaponHash.AssaultShotgun, WeaponHash.BullpupShotgun };
        private static readonly WeaponHash[] EnemyPistols = { WeaponHash.Pistol, WeaponHash.CombatPistol, WeaponHash.Pistol50, WeaponHash.HeavyPistol, WeaponHash.SNSPistol, WeaponHash.APPistol };
        private static readonly WeaponHash[] EnemyMGs = { WeaponHash.MG, WeaponHash.CombatMG, WeaponHash.Gusenberg };

        public static WeaponHash RandomEnemyWeapon()
        {
            int roll = Rng.Next(100);
            if (roll < 40) return Pick(EnemyRifles);
            if (roll < 60) return Pick(EnemySMGs);
            if (roll < 75) return Pick(EnemyShotguns);
            if (roll < 90) return Pick(EnemyPistols);
            return Pick(EnemyMGs);

        }
    }

    /// <summary>
    /// Remembers every entity and blip a mission creates so it can all be
    /// cleaned up in one call (abort, fail, success, or script reload).
    /// </summary>
    internal sealed class EntityTracker
    {
        private readonly List<Entity> entities = new List<Entity>();
        private readonly List<Blip> blips = new List<Blip>();

        public T Add<T>(T entity) where T : Entity
        {
            if (entity != null && entity.Exists() && !entities.Contains(entity))
                entities.Add(entity);
            return entity;
        }

        public Blip Add(Blip blip)
        {
            if (blip != null) blips.Add(blip);
            return blip;
        }

        public void RemoveBlip(Blip blip)
        {
            if (blip == null) return;
            if (blip.Exists()) blip.Delete();
            blips.Remove(blip);
        }

        /// <summary>Stop tracking an entity without touching it (e.g. handed over to someone else).</summary>
        public void Forget(Entity entity)
        {
            entities.Remove(entity);
        }

        /// <summary>Removes the red blip from peds that died this frame.</summary>
        public void PruneDeadPedBlips()
        {
            foreach (Entity e in entities)
            {
                var p = e as Ped;
                if (p == null || !p.Exists() || !p.IsDead) continue;
                Blip b = p.AttachedBlip;
                if (b != null && b.Exists()) b.Delete();
            }
        }

        /// <param name="delete">true = remove instantly (abort). false = let the game clean up naturally.</param>
        public void Release(bool delete)
        {
            foreach (Blip b in blips)
            {
                try { if (b != null && b.Exists()) b.Delete(); } catch { }
            }
            blips.Clear();

            foreach (Entity e in entities)
            {
                try
                {
                    if (e == null || !e.Exists()) continue;
                    Blip attached = e.AttachedBlip;
                    if (attached != null && attached.Exists()) attached.Delete();
                    if (delete) e.Delete();
                    else e.MarkAsNoLongerNeeded();
                }
                catch { }
            }
            entities.Clear();
        }
    }
}
