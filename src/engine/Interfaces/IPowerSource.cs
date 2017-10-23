using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Interfaces
{
    public interface IPowerSource
    {
        uint PowerCapacity {
            get;
        }
        uint PowerAvailable {
            get;
        }
        bool IsFull {
            get;
        }
        bool IsEmpty {
            get;
        }
        void ExpendEnergy(uint amt);
        void Regen();
        void Decay();
    }
}
