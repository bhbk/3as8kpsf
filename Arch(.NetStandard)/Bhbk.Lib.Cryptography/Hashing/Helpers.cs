using System.Text;

namespace Bhbk.Lib.Cryptography.Hashing
{
    public static class Helpers
    {
        public static string GetHexString(byte[] input)
        {
            var sb = new StringBuilder();

            foreach (byte b in input)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }
}
