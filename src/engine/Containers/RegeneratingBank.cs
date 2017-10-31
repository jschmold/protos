using System;
using System.Collections.Generic;
using System.Text;
using Engine.Exceptions;
using static System.Math;
using static LangRoids;

namespace Engine.Containers {
    /// <summary>
    /// A bank with regeneration and decay added on.
    /// </summary>
    public class RegeneratingBank : Bank {
        /// <summary>
        /// The default decay rate of the bank
        /// </summary>
        public uint DecayRate { get; set; } = 0;
        /// <summary>
        /// The default regen rate of the bank
        /// </summary>
        public uint RegenRate { get; set; } = 0;

        /// <summary>
        /// Decay the energy bank by the set DecayRate
        /// Calls onFailure, or throws NotEnoughEnergyException if there is not enough energy in the bank.
        /// </summary>
        /// <param name="onFailure">The function to call if there is not enough energy in the bank.</param>
        public void Decay(Action onFailure = null) => Decay(DecayRate, onFailure);

        /// <summary>
        /// Decay the energy bank by an amount. 
        /// Calls onFailure, or throws NotEnoughEnergyException if there is not enough energy in the bank.
        /// </summary>
        /// <param name="amt">The amount to remove from the bank.</param>
        /// <param name="onFailure">The function to call if there is not enough energy in the bank.</param>
        public void Decay(uint amt, Action onFailure = null) => Perform((int)Quantity - (int)amt >= 0, 
            (onFailure, new NotEnoughEnergyException( )), () => Quantity -= amt);

        /// <summary>
        /// Regenerate the energy bank by the set RegenRate. Will cap out at MaxEnergy.
        /// </summary>
        public void Regen() => Regen(RegenRate);

        /// <summary>
        /// Regenerate the energy bank by a an amount. Will cap out at MaxEnergy.
        /// </summary>
        /// <param name="amt">The amount to add to the bank. </param>
        public void Regen(uint amt) => Quantity = Min(this + amt, Maximum);
    }
}
