using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Engine.Exceptions {
    /// <summary>
    /// Describes when a resource is missing from a collection
    /// </summary>
    public class LackingResourceException : Exception {
        public LackingResourceException() {
        }

        public LackingResourceException(string message) : base(message) {
        }

        public LackingResourceException(string message, Exception innerException) : base(message, innerException) {
        }

        protected LackingResourceException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
