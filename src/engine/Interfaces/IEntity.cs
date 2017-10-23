using System;
using System.Linq;
using System.Collections.Generic;
using static System.Math;
using static LangRoids;
using Engine.Types;

namespace Engine.Interfaces {
    /// <summary>
    /// Describing an object that can exist in the game directly. 
    /// </summary>
    public interface IEntity {
        Bank Health {
            get; set;
        }
        Bank Energy {
            get; set;
        }

        void Think();
    }
}