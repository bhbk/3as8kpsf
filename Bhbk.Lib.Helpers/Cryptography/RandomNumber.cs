using Microsoft.AspNetCore.Authentication;
using System;
using System.Security.Cryptography;

namespace Bhbk.Lib.Helpers.Security
{
    public class RandomNumber
    {
        public static string CreateBase64(int length)
        {
            byte[] byteValue = new byte[length];
            RNGCryptoServiceProvider.Create().GetBytes(byteValue);

            return Base64UrlTextEncoder.Encode(byteValue);
        }

        public static string CreateNumberAsString(int length)
        {
            var randomNumber = new Random();
            var result = string.Empty;

            for (int i = 0; i < length; i++)
                result = String.Concat(result, randomNumber.Next(10).ToString());

            return result;
        }
    }
}
