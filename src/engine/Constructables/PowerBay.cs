using System;
using System.Linq;
using System.Collections.Generic;
using Engine.Types;
using static Engine.LangHelpers;
using static System.Math;

namespace Engine.Constructables {
    public class PowerBay : Bay {
        public CappedList<Bank> ReserveCells {
            get; private set;
        }
        public CappedList<Bank> PoolCells {
            get; private set;
        }

        
        public PowerBay(Location location, uint occupantLimit) : base(location, occupantLimit) => throw new NotImplementedException();
        /// <summary>
        /// On every cycle, the energy draw and gain are calculated
        /// </summary>
        public override void Think() => throw new NotImplementedException( );
        public override void DrawEnergy(uint amt, RegeneratingBank source, Action onNotEnoughEnergy = null) => throw new NotImplementedException( );
    }
}