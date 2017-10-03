using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Types {
    /// <summary>
    /// The type representing the Location of something.
    /// </summary>
    public class Location {
        public int Spire {
            get; set;
        }
        public int Disc {
            get; set;
        }
        public (int X, int Y) Region {
            get;set;
        }

        public Location(int sp, int dsc, int x, int y) {
            Spire = sp;
            Disc = dsc;
            Region = (x, y);
        }

        public Location((int spire, int disc, int x, int y) loc) {
            Spire = loc.spire;
            Disc = loc.disc;
            Region = (loc.x, loc.y);
        }

        /// <summary>
        /// Adds all of the parameters of a Location together
        /// </summary>
        /// <param name="a">The first Location</param>
        /// <param name="b">The second Location</param>
        /// <returns>A sum of the each parameter of the first and second location in a new Location object</returns>
        public static Location operator +(Location a, Location b) => new Location(a.Spire + b.Spire, a.Disc + b.Disc, a.Region.X + b.Region.X, a.Region.Y + b.Region.Y);

        /// <summary>
        /// Subtracts all of the parameters of a Location from one another.
        /// </summary>
        /// <param name="a">The first Location</param>
        /// <param name="b">the second Location</param>
        /// <returns>A new Location object containing a difference of each of a's, subtracting each of b's</returns>
        public static Location operator -(Location a, Location b) => new Location(a.Spire - b.Spire, a.Disc - b.Disc, a.Region.X - b.Region.X, a.Region.Y - b.Region.Y);
    }
}
