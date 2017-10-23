using System;
using System.Collections.Generic;
using System.Text;
using Engine.Types;


namespace Engine.Interfaces {
    /// <summary>
    /// Indicating something that can be powered by the energy grid
    /// </summary>
    public interface IPowerable {
        /// <summary>
        /// Whether or not the object is allowed to draw energy
        /// </summary>
        bool EnergySwitch {
            get; set;
        }
        /// <summary>
        /// The max amount of energy that can be drawn from the grid at any time
        /// </summary>
        uint EnergyMaxDraw {
            get; set;
        }
        /// <summary>
        /// The energy grid itself
        /// </summary>
        List<IPowerSource> EnergySources {
            get; set;
        }

        /// <summary>
        /// Draw energy from a specific source
        /// </summary>
        /// <param name="amt">The amount to draw</param>
        /// <param name="energySource">The source to draw from</param>
        /// <param name="onNotEnoughEnergy">What to do if there's not enough energy</param>
        /// <returns>The amount drawn</returns>
        uint DrawEnergy(uint amt, IPowerSource energySource, Action onNotEnoughEnergy = null);

        /// <summary>
        /// Draw energy from a bay programmatically indicated
        /// </summary>
        /// <param name="amt">The amount to draw</param>
        /// <param name="onNotEnoughEnergy">What to do if there's not enough energy</param>
        /// <returns>The amount drawn</returns>
        uint DrawEnergy(uint amt, Action onNotEnoughEnergy = null);
    }
}
