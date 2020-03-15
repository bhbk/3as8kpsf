using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Bhbk.Lib.Cryptography.Encryption
{
    public class TripleDES
    {
        public static String EncryptString(String plainText, String secret)
        {
            TripleDESCryptoServiceProvider tdesalgorithm = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashalgorithm = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();

            byte[] hash = hashalgorithm.ComputeHash(utf8.GetBytes(secret));
            byte[] encrypted = null;
            byte[] unencrypted = Convert.FromBase64String(plainText);

            tdesalgorithm.Key = hash;
            tdesalgorithm.Mode = CipherMode.ECB;
            tdesalgorithm.Padding = PaddingMode.PKCS7;

            try
            {
                ICryptoTransform Encryptor = tdesalgorithm.CreateEncryptor();
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
                tdesalgorithm.Clear();
                hashalgorithm.Clear();
            }

            return Convert.ToBase64String(encrypted);
        }

        public static String DecryptString(String cipherText, String secret)
        {
            TripleDESCryptoServiceProvider tdesalgorithm = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashalgorithm = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();

            byte[] hash = hashalgorithm.ComputeHash(utf8.GetBytes(secret));
            byte[] unencrypted = null;
            byte[] encrypted = Convert.FromBase64String(cipherText);

            tdesalgorithm.Key = hash;
            tdesalgorithm.Mode = CipherMode.ECB;
            tdesalgorithm.Padding = PaddingMode.PKCS7;

            try
            {
                ICryptoTransform Decryptor = tdesalgorithm.CreateDecryptor();
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
                tdesalgorithm.Clear();
                hashalgorithm.Clear();
            }

            return utf8.GetString(unencrypted);
        }
    }
}
