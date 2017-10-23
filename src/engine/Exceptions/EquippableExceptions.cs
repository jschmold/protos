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
    public class EquipSlotOccupiedException : Exception {
        public EquipSlotOccupiedException() {
        }

        public EquipSlotOccupiedException(string message) : base(message) {
        }

        public EquipSlotOccupiedException(string message, Exception innerException) : base(message, innerException) {
        }

        protected EquipSlotOccupiedException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
    public class NoAvailableSlotsException : Exception {
        public NoAvailableSlotsException() {
        }

        public NoAvailableSlotsException(string message) : base(message) {
        }

        public NoAvailableSlotsException(string message, Exception innerException) : base(message, innerException) {
        }

        protected NoAvailableSlotsException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
