using System;
using System.Linq;
using System.Collections.Generic;
using Engine.Types;
using static LangRoids;
using static System.Math;
using Engine.Interfaces;

namespace Engine.Constructables {
    /// <summary>
    /// A powerbay. Use the Cells property for modifying capability.
    /// </summary>
    public class PowerBay : Bay, IPowerSource {
        public PowerCellCluster Cells {
            get; set; 
        }

        public uint PowerCapacity => ((IPowerSource)Cells).PowerCapacity;
        public uint PowerAvailable => ((IPowerSource)Cells).PowerAvailable;
        public bool IsFull => ((IPowerSource)Cells).IsFull;
        public bool IsEmpty => ((IPowerSource)Cells).IsEmpty;
        public void DoRegen() => Cells.Regen( );
        public void DoDecay() => Cells.Decay( );

        public PowerBay(uint occupantLimit, uint cellLimit) 
            : base(occupantLimit) => Cells = new PowerCellCluster(cellLimit);

        /// <summary>
        /// On every cycle, the energy draw and gain are calculated
        /// </summary>
        public override void Think() => Compose(DoRegen, DoDecay);

        public void ExpendEnergy(uint amt) => ((IPowerSource)Cells).ExpendEnergy(amt);
        public void Regen() => ((IPowerSource)Cells).Regen( );
        public void Decay() => ((IPowerSource)Cells).Decay( );
    }
}