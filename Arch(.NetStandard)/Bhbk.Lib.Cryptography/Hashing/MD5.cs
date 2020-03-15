using System;
using System.Security.Cryptography;
using System.Text;

namespace Bhbk.Lib.Cryptography.Hashing
{
    public class MD5
    {
        public static string Create(string input)
        {
            var algo = new MD5CryptoServiceProvider();

            byte[] byteValue = Encoding.UTF8.GetBytes(input);
            byte[] byteHash = algo.ComputeHash(byteValue);

            return Helpers.GetHexString(byteHash);
        }
    }
}
