using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Engine.Exceptions
{
    public class InappropriateEquippableException : Exception {
        public InappropriateEquippableException() {
        }

        public InappropriateEquippableException(string message) : base(message) {
        }

        public InappropriateEquippableException(string message, Exception innerException) : base(message, innerException) {
        }

        protected InappropriateEquippableException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
    
}
