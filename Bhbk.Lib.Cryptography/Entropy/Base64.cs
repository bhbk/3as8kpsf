using System;
using System.Security.Cryptography;

namespace Bhbk.Lib.Cryptography.Entropy
{
    public class Base64
    {
        public static string CreateString(int length)
        {
            byte[] byteValue = new byte[length];
            RNGCryptoServiceProvider.Create().GetBytes(byteValue);

            return Convert.ToBase64String(byteValue);
        }
    }
}
