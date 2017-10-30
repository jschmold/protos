using System;
using System.Collections.Generic;
using System.Text;
using static LangRoids;

namespace Engine.Math
{
    public static class Collisions
    {
        /// <summary>
        /// Order a (float, float) tuple to be (smaller, larger)
        /// </summary>
        /// <param name="tup"></param>
        /// <returns></returns>
        public static (float, float) Order((float a, float b) tup) => tup.a < tup.b ? tup : (tup.b, tup.a);

        /// <summary>
        /// Order a bound's values so Item1 is the smaller, and Item2 is the larger for both X and Y
        /// </summary>
        /// <param name="bound"></param>
        /// <returns></returns>
        public static Bound2d Order(Bound2d bound) => new Bound2d(Order(bound.X), Order(bound.Y));

        /// <summary>
        /// Order a bound's values so Item1 is the smaller, and Item2 is the larger for X, Y, and Z
        /// </summary>
        /// <param name="bound"></param>
        /// <returns></returns>
        public static Bound3d Order(Bound3d bound) => new Bound3d(Order(bound.X), Order(bound.Y), Order(bound.Z));

        /// <summary>
        /// Order a bound's values so Item1 is the smaller, and Item2 is the larger for X, Y, Z, and Q
        /// </summary>
        /// <param name="bound"></param>
        /// <returns></returns>
        public static Bound4d Order(Bound4d bound) => new Bound4d(Order(bound.X), Order(bound.Y), Order(bound.Z), Order(bound.Q));

        /// <summary>
        /// Whether or not two (float, float) tuples are overlapping.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsOverlapping((float, float) a, (float, float) b) {
            var orderedA = Order(a);
            var orderedB = Order(b);
            return orderedA.Item1 < orderedB.Item2 && orderedA.Item2 < orderedB.Item1;
        }

        /// <summary>
        /// Whether or not two Bound2d objects are overlapping
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsOverlapping(Bound2d a, Bound2d b) => 
            IsOverlapping(a.X, b.X) || IsOverlapping(a.Y, b.Y);
        /// <summary>
        /// Whether or not two Bound3d objects are overlapping
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsOverlapping(Bound3d a, Bound3d b) => 
            IsOverlapping(a.X, b.X) || IsOverlapping(a.Y, b.Y) || IsOverlapping(a.Z, b.Z);
        /// <summary>
        /// Whether or not 2 Bound4d objects are colliding
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsOverlapping(Bound4d a, Bound4d b) => 
            IsOverlapping(a.X, b.X) || IsOverlapping(a.Y, b.Y) || IsOverlapping(a.Z, b.Z) || IsOverlapping(a.Q, b.Q);

        /// <summary>
        /// Whether or not one bound is overlapping any of an array of other bounds.
        /// </summary>
        /// <param name="bound"></param>
        /// <param name="others"></param>
        /// <returns></returns>
        public static bool IsOverlapping(Bound2d bound, params Bound2d[] others) {
            foreach (Bound2d other in others) {
                if (IsOverlapping(bound, other)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Whether or not one bound is overlapping any of an array of other bounds
        /// </summary>
        /// <param name="bound"></param>
        /// <param name="others"></param>
        /// <returns></returns>
        public static bool IsOverlapping(Bound3d bound, params Bound3d[] others) {
            foreach (Bound3d other in others) {
                if (IsOverlapping(bound, other)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Whether or not one bound is overlapping any of an array of other bounds
        /// </summary>
        /// <param name="bound"></param>
        /// <param name="others"></param>
        /// <returns></returns>
        public static bool IsOverlapping(Bound4d bound, params Bound4d[] others) {
            foreach (Bound4d other in others) {
                if (IsOverlapping(bound, other)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Whether or not one bound is within another, including on the borders.
        /// </summary>
        /// <param name="bound"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public static bool IsContained((float, float) bound, (float, float) container) {
            bound = Order(bound);
            container = Order(container);
            return bound.Item1 >= container.Item1 && bound.Item1 <= container.Item2;
        }

        /// <summary>
        /// Whether or not one bound is within another, including on the borders.
        /// </summary>
        /// <returns></returns>
        public static bool IsContained(Bound2d bound, Bound2d container) 
            => IsContained(bound.X, container.X) && IsContained(bound.Y, container.Y);
        /// <summary>
        /// Whether or not one bound is within another, including on the borders.
        /// </summary>
        /// <param name="bound"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public static bool IsContained(Bound3d bound, Bound3d container)
            => IsContained(bound.X, container.X) && IsContained(bound.Y, container.Y) && IsContained(bound.Z, container.Z);
        /// <summary>
        /// Whether or not one bound is within another, including on the borders.
        /// </summary>
        /// <param name="bound"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public static bool IsContained(Bound4d bound, Bound4d container)
            => IsContained(bound.X, container.X) && IsContained(bound.Y, container.Y) && IsContained(bound.Z, container.Z) && IsContained(bound.Z, container.Z);
    }
}
