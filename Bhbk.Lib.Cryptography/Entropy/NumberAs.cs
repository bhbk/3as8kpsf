using System;

namespace Bhbk.Lib.Cryptography.Entropy
{
    public class NumberAs
    {
        public static string CreateString(int length)
        {
            var randomNumber = new Random();
            var result = string.Empty;

            for (int i = 0; i < length; i++)
                result = String.Concat(result, randomNumber.Next(10).ToString());

            return result;
        }
    }
}
