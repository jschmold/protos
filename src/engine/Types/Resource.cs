using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Types {
    public class Resource {
        public string Name {
            get; set;
        }
        public uint Volume {
            get; set;
        }
        public uint Mass {
            get; set;
        }
        public uint Identifier {
            get; set;
        }
    }
}
