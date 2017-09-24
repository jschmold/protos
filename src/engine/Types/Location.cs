using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Types {
    public class Location {
        public int Spire {
            get; set;
        }
        public int Disc {
            get; set;
        }
        public int RegionX {
            get; set;
        }
        public int RegionY {
            get; set;
        }

        public Location(int sp, int dsc, int x, int y) {
            Spire = sp;
            Disc = dsc;
            RegionX = x;
            RegionY = y;
        }

        /// <summary>
        /// Adds all of the parameters of a Location together
        /// </summary>
        /// <param name="a">The first Location</param>
        /// <param name="b">The second Location</param>
        /// <returns>A sum of the each parameter of the first and second location in a new Location object</returns>
        public static Location operator +(Location a, Location b) => new Location(a.Spire + b.Spire, a.Disc + b.Disc, a.RegionX + b.RegionX, a.RegionY + b.RegionY);
        /// <summary>
        /// Subtracts all of the parameters of a Location from one another.
        /// </summary>
        /// <param name="a">The first Location</param>
        /// <param name="b">the second Location</param>
        /// <returns>A new Location object containing a difference of each of a's, subtracting each of b's</returns>
        public static Location operator -(Location a, Location b) => new Location(a.Spire - b.Spire, a.Disc - b.Disc, a.RegionX - b.RegionX, a.RegionY - b.RegionY);
    }
}
