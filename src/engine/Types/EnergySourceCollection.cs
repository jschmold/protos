using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Types
{
    public class EnergySourceCollection : List<RegeneratingBank>
    {
        public RegeneratingBank HighestBank() {
            if (Count == 1) {
                return this[0];
            }
            RegeneratingBank bank = this[0];
            for (int i = 1 ; i < Count ; i++) {
                bank = bank.Quantity < this[i].Quantity ? this[i] : bank;
                if (bank.IsFull) {
                    return bank;
                }
            }
            return bank;
        }
        public RegeneratingBank LowestBank() {
            if (Count == 1) {
                return this[0];
            }
            RegeneratingBank bank = this[0];
            for (int i = 1 ; i < Count ; i++) {
                bank = bank.Quantity > this[i].Quantity ? this[i] : bank;
                if (bank.Quantity == 0) {
                    return bank;
                }
            }
            return bank;
        }
    }
}
