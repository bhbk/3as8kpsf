using Bhbk.Lib.Common.Primitives;
using System.Security.Cryptography;
using System.Text;

namespace Bhbk.Lib.Cryptography.Hashing
{
    public class SHA256
    {
        public static string Create(string input)
        {
            var algo = new SHA256CryptoServiceProvider();

            byte[] byteValue = Encoding.UTF8.GetBytes(input);
            byte[] byteHash = algo.ComputeHash(byteValue);

            return Strings.GetHexString(byteHash);
        }
    }
}
