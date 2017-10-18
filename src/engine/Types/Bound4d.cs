using System;
using System.Collections.Generic;
using System.Text;
using static Engine.Helpers.Lang;

namespace Engine.Types {
    public class Bound4d : Bound3d {
        public (float, float) Q {
            get; set;
        }
        public Bound4d(float x1, float x2, float y1, float y2, float z1, float z2, float q1, float q2) 
            : base(x1, x2, y1, y2, z1, z2) 
            => Q = (q1, q2);

        public Bound4d(Bound4d bound) 
            : this(bound.X.Item1, bound.X.Item2, bound.Y.Item1, bound.Y.Item2, bound.Z.Item1, bound.Z.Item2, bound.Q.Item1, bound.Q.Item2) 
            => DoNothing( );
    }
}
