using System;
using System.Collections.Generic;
using System.Text;
using Engine;
using static LangRoids;
using static System.Math;
using Engine.Exceptions;

namespace Engine.Constructables
{
    // Todo: This was never finished
    public class EducationalBay : PoweredBay {
        
        public EducationalBay(List<IPowerSource> grid, uint maxDraw) 
            : base(grid, maxDraw) 
            => DoNothing();

        public override void Think() => throw new NotImplementedException( );
    }
}
