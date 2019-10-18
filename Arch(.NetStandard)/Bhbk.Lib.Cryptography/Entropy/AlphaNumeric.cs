using System;
using System.Linq;

namespace Bhbk.Lib.Cryptography.Entropy
{
    public class AlphaNumeric
    {
        public static string CreateString(int length)
        {
            var randomNumber = new Random();
            var allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            return new string(Enumerable.Repeat(allowedChars, length)
              .Select(s => s[randomNumber.Next(s.Length)]).ToArray());
        }
    }
}
