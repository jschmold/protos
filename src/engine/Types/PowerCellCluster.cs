using System;
using System.Collections.Generic;
using System.Text;
using static LangRoids;
using static System.Math;
using Engine.Exceptions;
using Engine.Interfaces;

namespace Engine.Types {
    /// <summary>
    /// A cluster of cells that are used for PowerProducingBays, or anything else that may need them. Regenerates and decays automatically on Think.
    /// </summary>
    public class PowerCellCluster : IPowerSource {
        private CappedList<RegeneratingBank> Cells;

        /// <summary>
        /// Create a new power cell cluster with no cells
        /// </summary>
        /// <param name="maxCells"></param>
        public PowerCellCluster(uint maxCells) => Cells = new CappedList<RegeneratingBank>(maxCells);

        /// <summary>
        /// Create a new power cell cluster with cells
        /// </summary>
        /// <param name="maxCells"></param>
        /// <param name="cells"></param>
        public PowerCellCluster(uint maxCells, List<(uint capacity, uint start, uint decay, uint regen)> cells) : this(maxCells, cells.ToArray( )) => DoNothing( );
        /// <summary>
        /// Create a new power cell cluster with cells
        /// </summary>
        /// <param name="maxCells"></param>
        /// <param name="cells"></param>
        public PowerCellCluster(uint maxCells, params (uint capacity, uint start, uint decay, uint regen)[] cells)
            : this(maxCells) => Repeat(cells.Length, i => Add(cells[i]));

        public int Count => Cells.Count;

        /// <summary>
        /// Add a new power cell to the cluster
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="start"></param>
        /// <param name="decay">The rate of decay</param>
        /// <param name="regen">The rate of regenerations</param>
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
        /// <summary>
        /// Add a new power cell to the cluster. PowerCellCluster.Add for more details on cell.
        /// </summary>
        /// <param name="cell"></param>
        public void Add((uint capacity, uint start, uint decay, uint regen) cell) => Add(cell.capacity, cell.start, cell.decay, cell.regen);

        public void AddRange(List<(uint capacity, uint start, uint decay, uint regen)> cells) => Repeat(cells.Count, i => Add(cells[i]));
        
        /// <summary>
        /// The Capacity indicator for how much the entire cluster can hold for power
        /// </summary>
        private uint Capacity {
            get; set;
        }

        /// <summary>
        /// How much power is available in the whole cluster
        /// </summary>
        private uint Available {
            get; set;
        }

        public uint PowerCapacity => Capacity;
        public uint PowerAvailable => Available;
        public bool IsFull => Capacity == Available;
        public bool IsEmpty => Available == 0;

        /// <summary>
        /// Does the cluster cumulatively have enough power for amt?
        /// </summary>
        /// <param name="amt"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Expend amt of energy from the cluster.
        /// </summary>
        /// <param name="amt"></param>
        public void ExpendEnergy(uint amt) => ExpendEnergy(amt, null);

        /// <summary>
        /// Expend amt of energy from cluster.
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="onNotEnoughEnergy"></param>
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
        /// <summary>
        /// Retrieve a capacity, available tuple using the array indexer on a single cell
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public (uint capacity, uint available) this[int key] => (Cells[key].Maximum, Cells[key].Quantity);   
    }
}
