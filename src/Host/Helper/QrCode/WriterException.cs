using System;

namespace Host.Helper.QrCode
{
    /**
     * A base class which covers the range of exceptions which may occur when encoding a barcode using
     * the Writer framework.
     *
     * @author dswitkin@google.com (Daniel Switkin)
     */
    [Serializable]
    public sealed class WriterException : Exception
    {

        public WriterException()
            : base()
        {
        }

        public WriterException(String message)
            : base(message)
        {
        }

        public WriterException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
