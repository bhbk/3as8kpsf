using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Bhbk.Lib.Cryptography.Encryption
{
    public static class AES
    {
        public static string EncryptString(string plainText, string secret)
        {
            if (plainText == null)
                return null;

            if (secret == null)
                secret = String.Empty;

            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var passwordBytes = Encoding.UTF8.GetBytes(secret);

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesEncrypted = AES.Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(bytesEncrypted);
        }

        public static string DecryptString(string encryptedText, string secret)
        {
            if (encryptedText == null)
                return null;

            if (secret == null)
                secret = String.Empty;

            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            var passwordBytes = Encoding.UTF8.GetBytes(secret);

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesDecrypted = AES.Decrypt(bytesToBeDecrypted, passwordBytes);

            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] secretBytes)
        {
            byte[] encryptedBytes = null;

            /*
             * Set your salt here, change it to meet your flavor. The salt bytes must be at least 8 bytes.
             */
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (var ms = new MemoryStream())
            {
                using (var aes = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(secretBytes, saltBytes, 1000);

                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);
                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] secretBytes)
        {
            byte[] decryptedBytes = null;

            /*
             * Set your salt here, change it to meet your flavor. The salt bytes must be at least 8 bytes.
             */
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (var ms = new MemoryStream())
            {
                using (var aes = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(secretBytes, saltBytes, 1000);

                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);
                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
    }
}
