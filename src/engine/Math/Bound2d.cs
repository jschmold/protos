using System;
using System.Collections.Generic;
using System.Text;
using static LangRoids;
using Engine;
namespace Engine.Math
{
    /// <summary>
    /// Describing the boundaries of a box (width x height)
    /// </summary>
    public class Bound2d
    {
        /// <summary>
        /// The X range
        /// </summary>
        public (float, float) X;
        /// <summary>
        /// The Y range
        /// </summary>
        public (float, float) Y;

        /// <summary>
        /// Create a new 2d bounding box
        /// </summary>
        /// <param name="X1"><see cref="X"/></param>
        /// <param name="X2"><see cref="X"/></param>
        /// <param name="Y1"><see cref="Y"/></param>
        /// <param name="Y2"><see cref="Y"/></param>
        public Bound2d(float X1, float X2, float Y1, float Y2) {
            X = (X1, X2);
            Y = (Y1, Y2);
        }
        /// <see cref="Bound2d(float, float, float, float)"/>
        public Bound2d((float, float) X, (float, float) Y) 
            : this(X.Item1, X.Item2, Y.Item1, Y.Item2) => DoNothing( );

    }
}
