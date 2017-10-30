using System;
using System.Collections.Generic;
using System.Text;
using Engine;
using Engine.Math;

namespace Engine.Constructables {
    public interface IZone {
        Bound3d Location {
            get; set;
        }
    }
}
