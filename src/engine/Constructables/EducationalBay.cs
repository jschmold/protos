using System;
using System.Collections.Generic;
using System.Text;
using Engine;
using static Engine.LangHelpers;
using static System.Math;
using Engine.Interfaces;
using Engine.Exceptions;
using Engine.Types;

namespace Engine.Constructables
{
    public class EducationalBay : Bay, IPowerable {
        public EducationalBay(Location loc, uint occLimit) : base(loc, occLimit) {
        }

        public List<IPowerSource> EnergySources {
            get;
            set;
        }

        public uint DrawEnergy(uint amt, IPowerSource energySource, Action onNotEnoughEnergy = null) => throw new NotImplementedException( );
        public uint DrawEnergy(uint amt, Action onNotEnoughEnergy = null) => throw new NotImplementedException( );
        public override void Think() => throw new NotImplementedException( );
    }
}
