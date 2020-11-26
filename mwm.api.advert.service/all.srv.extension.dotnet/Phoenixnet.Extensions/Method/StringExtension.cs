using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Phoenixnet.Extensions.Method
{
    public static class StringExtension
    {
        #region Encoding

        public static byte[] ToUnicodeBytes(this string source) => Encoding.Unicode.GetBytes(source);

        public static byte[] ToUtf8Bytes(this string source) => Encoding.UTF8.GetBytes(source);

        public static byte[] ToAsciiBytes(this string source) => Encoding.ASCII.GetBytes(source);

        public static string ToBig5FromUtf8(this string utf8String)
        {
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            Encoding big5 = Encoding.GetEncoding("big5");

            // convert string to bytes
            byte[] utf8Bytes = utf8.GetBytes(utf8String);

            // convert encoding from big5 to utf8
            byte[] big5Bytes = Encoding.Convert(utf8, big5, utf8Bytes);

            char[] big5Chars = new char[big5.GetCharCount(big5Bytes, 0, big5Bytes.Length)];
            big5.GetChars(big5Bytes, 0, big5Bytes.Length, big5Chars, 0);

            return new string(big5Chars);
        }

        public static string ToUnicodeFrom(this string srcText)
        {
            StringBuilder sb = new StringBuilder();

            char[] src = srcText.ToCharArray();
            for (int i = 0; i < src.Length; i++)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(src[i].ToString());
                string str = @"\u" + bytes[1].ToString("X2") + bytes[0].ToString("X2");
                sb.Append(str);
            }

            return sb.ToString();
        }

        public static string ToUtf8FromBig5(this string big5String)
        {
            Encoding big5 = Encoding.GetEncoding("big5");
            Encoding utf8 = Encoding.GetEncoding("utf-8");

            // convert string to bytes
            byte[] big5Bytes = big5.GetBytes(big5String);

            // convert encoding from big5 to utf8
            byte[] utf8Bytes = Encoding.Convert(big5, utf8, big5Bytes);

            char[] utf8Chars = new char[utf8.GetCharCount(utf8Bytes, 0, utf8Bytes.Length)];
            utf8.GetChars(utf8Bytes, 0, utf8Bytes.Length, utf8Chars, 0);

            return new string(utf8Chars);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="plainString"></param>
        /// <returns></returns>
        /// <example>
        /// return "48656C6C6F20776F726C64" for "Hello world"
        /// </example>
        public static string ToHexFrom(this string plainString)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(plainString);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        /// <example>
        /// returns: "Hello world" for "48656C6C6F20776F726C64"
        /// </example>
        public static string ToStringFromHex(this string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.Unicode.GetString(bytes);
        }

        /// <summary>
        /// 16進位轉2進位
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] ToByteArryFromHex(this string hexString)
        {
            return Enumerable.Range(0, hexString.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                .ToArray();
        }

        /// <summary>
        /// 字串 轉 Base64
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static byte[] ToByteFromBase64(this string base64String)
        {
            return !string.IsNullOrWhiteSpace(base64String) ? Convert.FromBase64String(base64String) : new byte[] { };
        }

        public static byte[] GetBytes(string hexString)
        {
            int byteLength = hexString.Length / 2;
            byte[] bytes = new byte[byteLength];
            string hex;
            int j = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                hex = new string(new Char[] { hexString[j], hexString[j + 1] });
                bytes[i] = ToByteFromHex(hex);
                j = j + 2;
            }

            return bytes;
        }

        public static byte ToByteFromHex(string hex)
        {
            if (hex.Length > 2 || hex.Length <= 0)
                throw new ArgumentException("hex must be 1 or 2 characters in length");
            byte newByte = byte.Parse(hex, NumberStyles.HexNumber);
            return newByte;
        }

        #endregion Encoding

        #region Convert

        public static string ToSnakeCase(this string source)
        {
            return Regex.Replace(source, "(?<!_|\\b)([A-Z])", "_$1", RegexOptions.Compiled)
                        .ToLower();
        }

        public static string ToUnderscoreCase(this string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }

        /// <summary>
        /// 字串轉換Enum型別
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        #endregion Convert


        #region Hash
        public static string ToMD5(this string str)
        {
            using (var cryptoMD5 = MD5.Create())
            {
                //將字串編碼成 UTF8 位元組陣列
                var bytes = Encoding.UTF8.GetBytes(str);

                //取得雜湊值位元組陣列
                var hash = cryptoMD5.ComputeHash(bytes);

                //取得 MD5
                var md5 = BitConverter.ToString(hash)
                    .Replace("-", String.Empty)
                    .ToLower();

                return md5;
            }
        }
        

        #endregion
        public static bool ContainsAny(this string haystack, params string[] needles)
        {
            return needles.Any(haystack.Contains);
        }
    }
}