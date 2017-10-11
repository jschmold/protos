using System;
using System.Collections.Generic;
using System.Text;
using Engine.Types;
using static Engine.LangHelpers;

namespace Engine.Interfaces {
    public interface IPowerable {
        bool EnergySwitch {
            get; set;
        }
        uint EnergyMaxDraw {
            get; set;
        }
        List<RegeneratingBank> EnergySources {
            get; set;
        }
        void DrawEnergy(uint amt, RegeneratingBank energySource, Action onNotEnoughEnergy = null);
    }
}
