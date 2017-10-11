using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Engine.Exceptions {
    public class VolumeExceededException : Exception {
        public VolumeExceededException() {
        }

        public VolumeExceededException(string message) : base(message) {
        }

        public VolumeExceededException(string message, Exception innerException) : base(message, innerException) {
        }

        protected VolumeExceededException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
    public class NotEnoughOfCargoKindException : Exception {
        public NotEnoughOfCargoKindException() {
        }

        public NotEnoughOfCargoKindException(string message) : base(message) {
        }

        public NotEnoughOfCargoKindException(string message, Exception innerException) : base(message, innerException) {
        }

        protected NotEnoughOfCargoKindException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
