using Bhbk.Lib.Common.Primitives;
using System.Security.Cryptography;
using System.Text;

namespace Bhbk.Lib.Cryptography.Hashing
{
    public class SHA512
    {
        public static string Create(string input)
        {
            var algo = new SHA512CryptoServiceProvider();

            byte[] byteValue = Encoding.UTF8.GetBytes(input);
            byte[] byteHash = algo.ComputeHash(byteValue);

            return Strings.GetHexString(byteHash);
        }
    }
}
