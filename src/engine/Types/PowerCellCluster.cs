using System;
using System.Collections.Generic;
using System.Text;
using static Engine.LangHelpers;
using static System.Math;
using Engine.Exceptions;
using Engine.Interfaces;

namespace Engine.Types {
    /// <summary>
    /// A capped list of regenerating banks
    /// </summary>
    // Todo: Add tighter control over this in the future to enable caching Capacity and Available
    public class PowerCellCluster : IPowerSource {
        private CappedList<RegeneratingBank> Cells;

        public PowerCellCluster(uint maxCells) => Cells = new CappedList<RegeneratingBank>(maxCells);
        public PowerCellCluster(uint maxCells, params (uint capacity, uint start, uint decay, uint regen)[] cells)
            : this(maxCells) => Repeat(cells.Length - 1, i => Add(cells[i]));

        public int Count => Cells.Count;

        public void Add(uint capacity, uint start, uint decay, uint regen) {
            Cells.Add(new RegeneratingBank {
                Quantity = start,
                Maximum = capacity,
                DecayRate = decay,
                RegenRate = regen
            });
            Capacity += capacity;
            Available += start;
        }
        public void Add((uint capacity, uint start, uint decay, uint regen) cell) => Add(cell.capacity, cell.start, cell.decay, cell.regen);

        private uint Capacity {
            get; set;
        }

        private uint Available {
            get; set;
        }

        public uint PowerCapacity => Capacity;
        public uint PowerAvailable => Available;

        public bool IsFull => Capacity == Available;
        public bool IsEmpty => Available == 0;

        public bool HasEnoughFor(uint amt) => Available >= amt;

        /// <summary>
        /// Calls the Regen function on all cells
        /// </summary>
        public void Regen() => Repeat(Count, i => {
            var cell = Cells[i];
            cell.Regen( );
            Available += cell.RegenRate;
        });
        /// <summary>
        /// Calls the Decay function on all cells
        /// </summary>
        public void Decay() => Repeat(Count, i => {
            var cell = Cells[i];
            cell.Decay( );
            Available -= cell.DecayRate;
        });

        public void ExpendEnergy(uint amt) => ExpendEnergy(amt, null);

        public void ExpendEnergy(uint amt, Action onNotEnoughEnergy = null) => Perform(HasEnoughFor(amt),
            (onNotEnoughEnergy, new NotEnoughEnergyException( )), () => {
                uint amountDrawn = 0;
                int iter = 0;
                while (amountDrawn < amt && iter < Cells.Count) {
                    uint draw = Min(amt - amountDrawn, Cells[iter].Quantity);
                    Cells[iter].Decay(draw);
                    amountDrawn += draw;
                    iter += 1;
                }
                Available -= amountDrawn;
            });
        public (uint capacity, uint available) this[int key] {
            get {
                var cell = Cells[key];
                return (cell.Maximum, cell.Quantity);
            }
        }   
    }
}
