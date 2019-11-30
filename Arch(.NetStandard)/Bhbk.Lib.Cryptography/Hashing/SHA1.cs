﻿using System;
using System.Security.Cryptography;

namespace Bhbk.Lib.Cryptography.Hashing
{
    public class SHA1
    {
        public static string Create(string input)
        {
            var algo = new SHA1CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] byteHash = algo.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }
    }
}