using System;
using System.Collections.Generic;
using System.Text;
using static LangRoids;

namespace Engine.Types {
    /// <summary>
    /// The type representing the Region of something. These are declared by Spires, used by discs, and distributed to Bays. A disc will occupy a 3d grid a spire's 4d bounds.
    /// </summary>
    public class Bound3d : Bound2d {
        public (float, float) Z;

        /// <summary>
        /// X1, X2, Y1, Y2, Z1, Z2 bounds in that order where Z is depth, X is left and right, and Y is vertical.
        /// </summary>
        /// <param name="X1"></param>
        /// <param name="X2"></param>
        /// <param name="Y1"></param>
        /// <param name="Y2"></param>
        /// <param name="Z1"></param>
        /// <param name="Z2"></param>
        public Bound3d(float X1, float X2, float Y1, float Y2, float Z1, float Z2) : base(X1, X2, Y1, Y2) {
            X = (X1, X2);
            Y = (Y1, Y2);
            Z = (Z1, Z2);
        }

        public Bound3d((float, float) X, (float, float) Y, (float, float) Z) 
            : this(X.Item1, X.Item2, Y.Item1, Y.Item2, Z.Item1, Z.Item2) => DoNothing( );

        public Bound3d(Bound3d loc) 
            : this(loc.X.Item1, loc.X.Item2, loc.Y.Item1, loc.Y.Item2, loc.Z.Item1, loc.Z.Item2) => DoNothing( );
       
    }
}
