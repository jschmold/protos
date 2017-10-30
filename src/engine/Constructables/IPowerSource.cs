using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Constructables
{
    /// <summary>
    /// A source of power for the energy grid
    /// </summary>
    public interface IPowerSource
    {
        /// <summary>
        /// The total amount that can be stored in the PowerSource
        /// </summary>
        uint PowerCapacity {
            get;
        }
        /// <summary>
        /// The total amount that is stored in the PowerSource
        /// </summary>
        uint PowerAvailable {
            get;
        }

        /// <summary>
        /// Whether or not the power source is at its capacity
        /// </summary>
        bool IsFull {
            get;
        }
        /// <summary>
        /// Whether or not the power source has no energy 
        /// </summary>
        bool IsEmpty {
            get;
        }

        /// <summary>
        /// Expend energy from the source
        /// </summary>
        /// <param name="amt">The amount to expend</param>
        void ExpendEnergy(uint amt);

        /// <summary>
        /// Regenerate the source
        /// </summary>
        void Regen();
        /// <summary>
        /// Decay the source
        /// </summary>
        void Decay();
    }
}
