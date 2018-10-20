using System;
using System.Security.Cryptography;

namespace Bhbk.Lib.Core.Cryptography
{
    public class Hasher
    {
        public static string CreateSHA1(string input)
        {
            var algo = new SHA1CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] byteHash = algo.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        public static string CreateSHA256(string input)
        {
            var algo = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] byteHash = algo.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        public static string CreateSHA384(string input)
        {
            var algo = new SHA384CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] byteHash = algo.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        public static string CreateSHA512(string input)
        {
            var algo = new SHA512CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] byteHash = algo.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }
    }
}
