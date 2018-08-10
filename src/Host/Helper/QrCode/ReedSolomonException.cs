using System;

namespace Host.Helper.QrCode
{
    [Serializable]
    public sealed class ReedSolomonException : Exception
    {

        public ReedSolomonException(String message)
            : base(message)
        {
        }

        protected ReedSolomonException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
