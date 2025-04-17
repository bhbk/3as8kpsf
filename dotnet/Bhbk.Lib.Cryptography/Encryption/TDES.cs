using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Bhbk.Lib.Cryptography.Encryption
{
    public class TDES
    {
        public string EncryptString(string plainText, string secret)
        {
            byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };
            byte[] inputByteArray;

            try
            {
                var key = Encoding.UTF8.GetBytes(secret);
                var tdes = new TripleDESCryptoServiceProvider();
                inputByteArray = Encoding.UTF8.GetBytes(plainText);

                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, tdes.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DecryptString(string cipherText, string secret)
        {
            byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };
            byte[] inputByteArray = new byte[cipherText.Length];

            try
            {
                var key = Encoding.UTF8.GetBytes(secret);
                var tdes = new TripleDESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(cipherText);

                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, tdes.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                var encoding = Encoding.UTF8;

                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
