using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Bhbk.Lib.Cryptography.Encryption
{
    public static class AES
    {
        public static string Encrypt(string plainText, string password)
        {
            if (plainText == null)
            {
                return null;
            }

            if (password == null)
            {
                password = String.Empty;
            }

            // Get the bytes of the string
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesEncrypted = AES.Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(bytesEncrypted);
        }

        public static string Decrypt(string encryptedText, string password)
        {
            if (encryptedText == null)
            {
                return null;
            }

            if (password == null)
            {
                password = String.Empty;
            }

            // Get the bytes of the string
            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesDecrypted = AES.Decrypt(bytesToBeDecrypted, passwordBytes);

            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        [Obsolete]
        public static string EncryptString(String plainText, String secret)
        {
            RijndaelManaged aesalgorithm = new RijndaelManaged();
            MD5CryptoServiceProvider hashalgorithm = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();

            byte[] hash = new byte[31];
            byte[] encrypted = null;
            byte[] unencrypted = Convert.FromBase64String(plainText);

            Array.Copy(hashalgorithm.ComputeHash(utf8.GetBytes(secret)), 0, hash, 0, 16);
            Array.Copy(hashalgorithm.ComputeHash(utf8.GetBytes(secret)), 0, hash, 15, 16);

            aesalgorithm.Key = hash;
            aesalgorithm.Mode = CipherMode.ECB;
            aesalgorithm.Padding = PaddingMode.PKCS7;

            try
            {
                ICryptoTransform Encryptor = aesalgorithm.CreateEncryptor();
                encrypted = Encryptor.TransformFinalBlock(unencrypted, 0, unencrypted.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name + " "
                    + MethodBase.GetCurrentMethod().ToString());
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "EXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "STACK TRACE: " + ex.StackTrace);
            }
            finally
            {
                aesalgorithm.Clear();
                hashalgorithm.Clear();
            }

            return Convert.ToBase64String(encrypted);
        }

        [Obsolete]
        public static string DecryptString(String cipherText, String secret)
        {
            RijndaelManaged aesalgorithm = new RijndaelManaged();
            MD5CryptoServiceProvider hashalgorithm = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();

            byte[] hash = new byte[31];
            byte[] encrypted = ASCIIEncoding.ASCII.GetBytes(cipherText);
            byte[] unencrypted = null;

            Array.Copy(hashalgorithm.ComputeHash(ASCIIEncoding.ASCII.GetBytes(secret)), 0, hash, 0, 16);
            Array.Copy(hashalgorithm.ComputeHash(ASCIIEncoding.ASCII.GetBytes(secret)), 0, hash, 15, 16);

            aesalgorithm.Key = hash;
            aesalgorithm.Mode = CipherMode.ECB;
            aesalgorithm.Padding = PaddingMode.PKCS7;

            try
            {
                ICryptoTransform Decryptor = aesalgorithm.CreateDecryptor();
                unencrypted = Decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name + " "
                    + MethodBase.GetCurrentMethod().ToString());
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "EXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "STACK TRACE: " + ex.StackTrace);
            }
            finally
            {
                aesalgorithm.Clear();
                hashalgorithm.Clear();
            }

            return Convert.ToBase64String(unencrypted);
        }
    }
}
