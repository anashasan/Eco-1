using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Helper
{
    public class EncoderAgent
    {
        public static string EncryptString(string data)
        {
            try
            {
                var encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                var encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }

        public static string DecryptString(string data)
        {
            try
            {
                var encoder = new System.Text.UTF8Encoding();
                var utf8Decode = encoder.GetDecoder();

                var todecode_byte = Convert.FromBase64String(data);
                var charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                var decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                var result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }
    }
}
