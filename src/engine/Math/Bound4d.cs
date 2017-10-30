using System;
using System.Collections.Generic;
using System.Text;
using static LangRoids;

namespace Engine.Math {
    /// <summary>
    /// A 4d set of bounds
    /// </summary>
    public class Bound4d : Bound3d {
        /// <summary>
        /// The Q bound in X, Y, Z, Q
        /// </summary>
        public (float, float) Q {
            get; set;
        }
        /// <summary>
        /// A 4 dimensional boundary
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        /// <param name="z1"></param>
        /// <param name="z2"></param>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        public Bound4d(float x1, float x2, float y1, float y2, float z1, float z2, float q1, float q2) 
            : base(x1, x2, y1, y2, z1, z2) 
            => Q = (q1, q2);

        /// <summary>
        /// Create a new Bound4d using (float, float) tuples
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="q"></param>
        public Bound4d((float, float) x, (float, float) y, (float, float) z, (float, float) q) : base(x, y, z) => Q = q;

        /// <summary>
        /// Duplicate a Bound4d
        /// </summary>
        /// <param name="bound"></param>
        public Bound4d(Bound4d bound) 
            : this(bound.X.Item1, bound.X.Item2, bound.Y.Item1, bound.Y.Item2, bound.Z.Item1, bound.Z.Item2, bound.Q.Item1, bound.Q.Item2) 
            => DoNothing( );
    }
}
