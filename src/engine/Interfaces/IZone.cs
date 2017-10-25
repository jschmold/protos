using System;
using System.Collections.Generic;
using System.Text;
using Engine.Types;

namespace Engine.Interfaces {
    public interface IZone {
        Bound3d Location {
            get; set;
        }
    }
}
