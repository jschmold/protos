using System;
using System.Linq;
using System.Collections.Generic;
using Engine.Types;
using static Engine.LangHelpers;
using static System.Math;

namespace Engine.Constructables {
    public class PowerBay : Bay {
        public PowerBay(Location location, uint occupantLimit) : base(location, occupantLimit) => throw new NotImplementedException();
        public override void Think() => throw new NotImplementedException( );
    }
}