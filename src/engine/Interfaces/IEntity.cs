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
    public interface IEntity : IThinkable {
        /// <summary>
        /// The health in an entity
        /// </summary>
        Bank Health {
            get; set;
        }
        /// <summary>
        /// The energy level
        /// </summary>
        Bank Energy {
            get; set;
        }
    }
}