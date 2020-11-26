using System;

namespace Phoenixnet.Extensions.Method
{
    public static class ByteExtension
    {
        /// <summary>
        /// Byte array轉換為16進位字串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHex(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        /// <summary>
        /// XOR Exclusive OR運算
        /// </summary>
        /// <param name="hex1"></param>
        /// <param name="hex2"></param>
        /// <returns></returns>
        public static byte[] ToXOR(this byte[] hex1, byte[] hex2)
        {
            byte[] bHEX_OUT = new byte[hex1.Length];
            for (int i = 0; i < hex1.Length; i++)
            {
                bHEX_OUT[i] = (byte)(hex1[i] ^ hex2[i]);
            }

            return bHEX_OUT;
        }

        /// <summary>
        /// bytes 轉 Base64
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToBase64String(this byte[] bytes)
        {
            if (bytes.Length > 0)
            {
                return Convert.ToBase64String(bytes);
            }

            return string.Empty;
        }
    }
}