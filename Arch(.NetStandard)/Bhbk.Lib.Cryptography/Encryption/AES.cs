using System;
using System.Security.Cryptography;
using System.Text;

namespace Bhbk.Lib.Cryptography.Encryption
{
    public class AES
    {
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
                Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " "
                    + System.Reflection.MethodBase.GetCurrentMethod().ToString());
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

        public static string DecryptString(String cipherText, String secret)
        {
            RijndaelManaged aesalgorithm = new RijndaelManaged();
            MD5CryptoServiceProvider hashalgorithm = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();

            byte[] hash = new byte[31];
            byte[] encrypted = System.Text.ASCIIEncoding.ASCII.GetBytes(cipherText);
            byte[] unencrypted = null;

            Array.Copy(hashalgorithm.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(secret)), 0, hash, 0, 16);
            Array.Copy(hashalgorithm.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(secret)), 0, hash, 15, 16);

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
                Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " "
                    + System.Reflection.MethodBase.GetCurrentMethod().ToString());
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
