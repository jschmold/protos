using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Engine.Exceptions {
    public class PopulationExceedsMaximumException : Exception {
        public PopulationExceedsMaximumException() {
        }

        public PopulationExceedsMaximumException(string message) : base(message) {
        }

        public PopulationExceedsMaximumException(string message, Exception innerException) : base(message, innerException) {
        }

        protected PopulationExceedsMaximumException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
    public class UnsupportedException : Exception {
        public UnsupportedException() {
        }

        public UnsupportedException(string message) : base(message) {
        }

        public UnsupportedException(string message, Exception innerException) : base(message, innerException) {
        }

        protected UnsupportedException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
