using System;
using System.Collections.Generic;
using System.Text;
using static LangRoids;

namespace Engine.Types
{
    public class Bound2d
    {
        public (float, float) X;
        public (float, float) Y;

        public Bound2d(float X1, float X2, float Y1, float Y2) {
            X = (X1, X2);
            Y = (Y1, Y2);
        }

        public Bound2d((float, float) X, (float, float) Y) 
            : this(X.Item1, X.Item2, Y.Item1, Y.Item2) => DoNothing( );
    }
}
