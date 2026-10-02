using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;

namespace moreammunation
{
    /// <summary>
    /// Picks where missions happen. The core idea for this version:
    /// missions start close to the player instead of on the other side of the map.
    /// </summary>
    internal static class LocationHelper
    {
        private static readonly Random Rng = new Random();

        /// <summary>
        /// Picks a candidate according to the selection mode.
        /// Candidates closer than minDistance are skipped (so enemies don't spawn in your face),
        /// unless every candidate is that close.
        /// </summary>
        public static T Pick<T>(IList<T> candidates, Func<T, Vector3> position, Vector3 from,
                                float minDistance, MissionSelectionMode mode) where T : class
        {
            if (candidates == null || candidates.Count == 0) return null;

            List<T> sorted = candidates
                .OrderBy(c => position(c).DistanceTo2D(from))
                .ToList();

            List<T> farEnough = sorted.Where(c => position(c).DistanceTo2D(from) >= minDistance).ToList();
            List<T> pool = farEnough.Count > 0 ? farEnough : sorted;

            switch (mode)
            {
                case MissionSelectionMode.Random:
                    return pool[Rng.Next(pool.Count)];
                case MissionSelectionMode.NearestFew:
                    return pool[Rng.Next(Math.Min(3, pool.Count))];
                default:
                    return pool[0];
            }
        }

        public static Vector3? Nearest(IList<Vector3> points, Vector3 from, float minDistance)
        {
            if (points == null || points.Count == 0) return null;
            var valid = points.Where(p => p.DistanceTo2D(from) >= minDistance)
                              .OrderBy(p => p.DistanceTo2D(from))
                              .ToList();
            if (valid.Count == 0) return null;
            return valid[0];
        }

        /// <summary>
        /// Finds a road position between minDistance and maxDistance from the origin.
        /// Road nodes are only loaded around the player, so keep maxDistance under ~1500 m.
        /// </summary>
        public static Vector3 StreetPointNear(Vector3 origin, float minDistance, float maxDistance)
        {
            Vector3 best = Vector3.Zero;
            for (int attempt = 0; attempt < 20; attempt++)
            {
                double angle = Rng.NextDouble() * Math.PI * 2.0;
                float dist = minDistance + (float)Rng.NextDouble() * (maxDistance - minDistance);
                var probe = new Vector3(origin.X + (float)Math.Cos(angle) * dist,
                                        origin.Y + (float)Math.Sin(angle) * dist,
                                        origin.Z);

                Vector3 street = World.GetNextPositionOnStreet(probe, true);
                if (street == Vector3.Zero) continue;

                float d = street.DistanceTo2D(origin);
                if (d >= minDistance * 0.8f && d <= maxDistance * 1.3f)
                    return street;

                if (best == Vector3.Zero) best = street;
            }

            return best != Vector3.Zero ? best : World.GetNextPositionOnStreet(origin.Around(minDistance), true);
        }

        /// <summary>Heading pointing from one position to another.</summary>
        public static float HeadingTo(Vector3 from, Vector3 to)
        {
            Vector3 dir = to - from;
            double deg = Math.Atan2(dir.Y, dir.X) * 180.0 / Math.PI - 90.0;
            if (deg < 0) deg += 360.0;
            return (float)deg;
        }

        public static string AreaName(Vector3 position)
        {
            try
            {
                string zone = World.GetZoneLocalizedName(position);
                string street = World.GetStreetName(position);
                if (!string.IsNullOrEmpty(street) && !string.IsNullOrEmpty(zone)) return street + ", " + zone;
                return !string.IsNullOrEmpty(zone) ? zone : street;
            }
            catch
            {
                return "the marked location";
            }
        }
    }
}
