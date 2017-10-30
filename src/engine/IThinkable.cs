using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    /// <summary>
    /// Something that does processing each frame
    /// </summary>
    public interface IThinkable
    {
        /// <summary>
        /// Performed every frame
        /// </summary>
        void Think();
    }
}
