using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Engine.Exceptions {
    public class NotEnoughEnergyException : Exception {
        public NotEnoughEnergyException() {
        }

        public NotEnoughEnergyException(string message) : base(message) {
        }

        public NotEnoughEnergyException(string message, Exception innerException) : base(message, innerException) {
        }

        protected NotEnoughEnergyException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
