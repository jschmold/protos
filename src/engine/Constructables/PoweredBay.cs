using System;
using System.Collections.Generic;
using System.Text;

using static Engine.Helpers.IPowerableUtils;

namespace Engine.Constructables
{
    /// <summary>
    /// A bay that draws power from the grid
    /// </summary>
    public abstract class PoweredBay : Bay, IPowerable {
        /// <summary>
        /// Create a new bay that is capable of drawing from the energy grid
        /// </summary>
        /// <param name="energyGrid"><see cref="EnergySources"/></param>
        /// <param name="maxEnergyDraw"><see cref="EnergyMaxDraw"/></param>
        public PoweredBay(List<IPowerSource> energyGrid,
                uint maxEnergyDraw) : base() {
            EnergySwitch = true;
            EnergyMaxDraw = EnergyMaxDraw;
            EnergySources = energyGrid;
        }

        /// <summary>
        /// Whether or not the bay is allowed to draw energy this frame
        /// </summary>
        public bool EnergySwitch {
            get;
            set;
        }
        /// <summary>
        /// The maximum amount of energy the bay is permitted to draw
        /// </summary>
        public uint EnergyMaxDraw {
            get;
            set;
        }
        /// <summary>
        /// The grid of energy sources provided
        /// </summary>
        public List<IPowerSource> EnergySources {
            get;
            set;
        }

        /// <summary>
        /// Draw energy from a specific source
        /// </summary>
        /// <param name="amt">The amount to draw</param>
        /// <param name="energySource">The source to draw from</param>
        /// <param name="onNotEnoughEnergy">What to do if there's not enough energy to draw</param>
        /// <returns>How much energy has been drawn</returns>
        public virtual uint DrawEnergy(uint amt, IPowerSource energySource, Action onNotEnoughEnergy = null) => Draw(amt, energySource, EnergySwitch, onNotEnoughEnergy);

        /// <summary>
        /// Draw energy from many sources in the grid (recommended)
        /// </summary>
        /// <param name="amt">The amount of energy to draw from</param>
        /// <param name="onNotEnoughEnergy">What to do if there's not enough energy in the grid</param>
        /// <returns>How much energy has been drawn</returns>
        public virtual uint DrawEnergy(uint amt, Action onNotEnoughEnergy = null) => DrawFromManySources(amt, EnergySources);
    }
}
