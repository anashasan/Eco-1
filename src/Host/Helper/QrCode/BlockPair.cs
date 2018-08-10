using System;
namespace Host.Helper.QrCode
{
    public class BlockPair
    {
        private ByteArray dataBytes;
        private ByteArray errorCorrectionBytes;

        internal BlockPair(ByteArray data, ByteArray errorCorrection)
        {
            dataBytes = data;
            errorCorrectionBytes = errorCorrection;
        }

        public ByteArray GetDataBytes()
        {
            return dataBytes;
        }

        public ByteArray GetErrorCorrectionBytes()
        {
            return errorCorrectionBytes;
        }
    }
}
