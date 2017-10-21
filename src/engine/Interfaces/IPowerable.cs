using System;
using System.Collections.Generic;
using System.Text;
using Engine.Types;


namespace Engine.Interfaces {
    public interface IPowerable {
        bool EnergySwitch {
            get; set;
        }
        uint EnergyMaxDraw {
            get; set;
        }
        List<IPowerSource> EnergySources {
            get; set;
        }
        uint DrawEnergy(uint amt, IPowerSource energySource, Action onNotEnoughEnergy = null);
        uint DrawEnergy(uint amt, Action onNotEnoughEnergy = null);
    }
}
