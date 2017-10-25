using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Exceptions;
using Engine.Types;
using static LangRoids;
using Engine;
using Engine.Interfaces;
using Engine.Entities;

namespace Engine.Constructables {
    /// <summary>
    /// A purpose-driven section of the colony
    /// </summary>
    public abstract class Bay : IThinkable, IZone {

        /// <summary>
        /// A function that needs to be called every frame.
        /// </summary>
        public abstract void Think();

        public Bound3d Location {
            get; set;
        }
    }
}
