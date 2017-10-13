using System;
using System.Collections.Generic;
using System.Text;
using Engine.Interfaces;
using static Engine.LangHelpers;
using Engine.Exceptions;
using static System.Math;
namespace Engine.Helpers
{
    public static class IPowerableUtils
    {
        public static IPowerSource FirstWithEnough(uint amt, List<IPowerSource> sources) => sources.Find(src => src.PowerAvailable >= amt);
        public static uint DrawFromManySources(uint amt, List<IPowerSource> sources, Action onNotEnoughEnergy = null) {
            uint amtDrawn = 0;
            int iter = 0;
            while (amtDrawn < amt && iter < sources.Count) {
                uint draw = Min(amt, sources[iter].PowerAvailable);
                sources[iter].ExpendEnergy(draw);
                amtDrawn += draw;
                iter += 1;
            }
            DoOrThrow(amtDrawn < amt, onNotEnoughEnergy, new NotEnoughEnergyException( ));
            return amtDrawn;
        }
        public static uint Draw(uint amt, IPowerSource src, Action onNotEnoughEnergy = null) {
            try {
                src.ExpendEnergy(amt);
            } catch(NotEnoughEnergyException) {
                DoOrThrow(onNotEnoughEnergy, new NotEnoughEnergyException( ));
            }
            return amt;
        }
        public static uint PowerAvailable(List<IPowerSource> sources) {
            uint amt = 0;
            sources.ForEach(src => amt += src.PowerAvailable);
            return amt;
        }
        public static uint PowerCapacity(List<IPowerSource> sources) {
            uint amt = 0;
            sources.ForEach(src => amt += src.PowerCapacity);
            return amt;
        }
    }
}
