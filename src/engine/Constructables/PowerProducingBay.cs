using System;
using System.Linq;
using System.Collections.Generic;
using Engine.Types;
using static LangRoids;
using static System.Math;
using Engine.Interfaces;

namespace Engine.Constructables {
    /// <summary>
    /// A PowerProducingBay. Use the Cells property for modifying capability.
    /// </summary>
    public class PowerProducingBay : Bay, IPowerSource {
        /// <summary>
        /// The cluster of cells this power bay has
        /// </summary>
        public PowerCellCluster Cells {
            get; set; 
        }

        /// <summary>
        /// The total amount of power that is contained in the cells
        /// </summary>
        public uint PowerCapacity => ((IPowerSource)Cells).PowerCapacity;
        /// <summary>
        /// The total amount of power available from the cells
        /// </summary>
        public uint PowerAvailable => ((IPowerSource)Cells).PowerAvailable;
        /// <summary>
        /// Whether or not all of the cells are at full charge
        /// </summary>
        public bool IsFull => ((IPowerSource)Cells).IsFull;
        /// <summary>
        /// Whether or not all of the cells are empty.
        /// </summary>
        public bool IsEmpty => ((IPowerSource)Cells).IsEmpty;
        /// <summary>
        /// Regenerate the cells
        /// </summary>
        public void DoRegen() => Cells.Regen( );
        /// <summary>
        /// Decay the cells
        /// </summary>
        public void DoDecay() => Cells.Decay( );

        /// <summary>
        /// Create a new power bay
        /// </summary>
        /// <param name="cellLimit">The maximum quantity of cells that the cell cluster can contain</param>
        public PowerProducingBay(uint cellLimit) 
            : base() => Cells = new PowerCellCluster(cellLimit);

        /// <summary>
        /// On every cycle, the energy draw and gain are calculated
        /// </summary>
        public override void Think() => Compose(DoRegen, DoDecay);

        /// <summary>
        /// Expend energy from the cells.
        /// </summary>
        /// <param name="amt"></param>
        public void ExpendEnergy(uint amt) => ((IPowerSource)Cells).ExpendEnergy(amt);
        /// <summary>
        /// Regenerate energy on the cells
        /// </summary>
        public void Regen() => DoRegen( );
        /// <summary>
        /// Decay the cells
        /// </summary>
        public void Decay() => DoDecay( );
    }
}