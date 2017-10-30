using System;
using System.Collections.Generic;
using System.Text;
using Engine.Entities;
using Engine.Constructables;
using static LangRoids;
using Engine.Exceptions;
using static System.Math;

namespace Engine.Constructables
{
    /// <summary>
    /// Utils for anything that is able to receive power
    /// </summary>
    public static class IPowerableUtils
    {
        /// <summary>
        /// Get the first source in a list of sources that has enough power for amt
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="sources"></param>
        /// <returns></returns>
        public static IPowerSource FirstWithEnough(uint amt, List<IPowerSource> sources) => sources.Find(src => src.PowerAvailable >= amt);
        /// <summary>
        /// Draw an amount of energy from many sources
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="sources"></param>
        /// <param name="energySwitch"></param>
        /// <param name="onNotEnoughEnergy"></param>
        /// <returns></returns>
        public static uint DrawFromManySources(uint amt, List<IPowerSource> sources, bool energySwitch = true, Action onNotEnoughEnergy = null) {
            uint amtDrawn = 0;
            int iter = 0;
            while (amtDrawn < amt && iter < sources.Count) {
                uint draw = Min(amt, sources[iter].PowerAvailable);
                sources[iter].ExpendEnergy(draw);
                amtDrawn += draw;
                iter += 1;
            }
            DoOrThrow(amtDrawn < amt, onNotEnoughEnergy, new NotEnoughEnergyException( ));
            return amtDrawn;
        }
        /// <summary>
        /// Draw energy from a source
        /// </summary>
        /// <param name="amt">Amount to draw</param>
        /// <param name="src">The source to draw from</param>
        /// <param name="energySwitch">The energy switch</param>
        /// <param name="onNotEnoughEnergy">What to do when there's not enough energy</param>
        /// <returns>The amount drawn</returns>
        public static uint Draw(uint amt, IPowerSource src, bool energySwitch = true, Action onNotEnoughEnergy = null) {
            try {
                src.ExpendEnergy(amt);
            } catch(NotEnoughEnergyException) {
                DoOrThrow(onNotEnoughEnergy, new NotEnoughEnergyException( ));
            }
            return amt;
        }
        /// <summary>
        /// The total power of all power sources in a collection of sources
        /// </summary>
        /// <param name="sources"></param>
        /// <returns></returns>
        public static uint PowerAvailable(IEnumerable<IPowerSource> sources) {
            uint amt = 0;
            ForEach(sources, src => amt += src.PowerAvailable);
            return amt;
        }
        /// <summary>
        /// The total capacity of all power sources in a collection of sources
        /// </summary>
        /// <param name="sources"></param>
        /// <returns></returns>
        public static uint PowerCapacity(IEnumerable<IPowerSource> sources) {
            uint amt = 0;
            ForEach(sources, src => amt += src.PowerCapacity);
            return amt;
        }
    }
}
