using CryptoHelper;
using Bhbk.Lib.Common.Primitives;

namespace Bhbk.Lib.Cryptography.Hashing
{
    public class PBKDF2
    {
        public static string Create(string password)
        {
            return Crypto.HashPassword(password);
        }

        public static bool Validate(string passwordHash, string password)
        {
            return Crypto.VerifyHashedPassword(passwordHash, password);
        }
    }
}
