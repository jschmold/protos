using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Engine.Exceptions
{
    public class NotYetCompletedException : Exception {
        public NotYetCompletedException() {
        }

        public NotYetCompletedException(string message) : base(message) {
        }

        public NotYetCompletedException(string message, Exception innerException) : base(message, innerException) {
        }

        protected NotYetCompletedException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
    public class LimitMetException : Exception {
        public LimitMetException() {
        }

        public LimitMetException(string message) : base(message) {
        }

        public LimitMetException(string message, Exception innerException) : base(message, innerException) {
        }

        protected LimitMetException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
