using Microsoft.AspNetCore.Authentication;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace Bhbk.Lib.Core.Cryptography
{
    public class RandomValues
    {
        public static string CreateBase64String(int length)
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

        public static string CreateAlphaNumericString(int length)
        {
            var randomNumber = new Random();
            var allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            return new string(Enumerable.Repeat(allowedChars, length)
              .Select(s => s[randomNumber.Next(s.Length)]).ToArray());
        }
    }
}
