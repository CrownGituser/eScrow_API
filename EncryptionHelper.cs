using Microsoft.AspNetCore.Mvc;
using SMFG_API_New.RequestModel;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SMFG_API_New
{
    public class EncryptionHelper
    {
        private static readonly string Key = "uKfo4VzUxGp9sEaB3rZWY1mLkWdoVR/62Tkcs+QwSeE="; // 256-bit key
       
        public static string Encrypt(string plainText,string IV)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = Encoding.UTF8.GetBytes(IV);

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (var writer = new StreamWriter(cs))
                            {
                                writer.Write(plainText);
                            }
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string IV)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = Encoding.UTF8.GetBytes(IV);

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (var reader = new StreamReader(cs))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }


        public static string Decrypt_New(string cipherText, string IV)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    // Decode the Base64 encoded key and IV into byte arrays
                    aesAlg.Key = Convert.FromBase64String(Key); // 32 bytes (256-bit key)
                    aesAlg.IV = Convert.FromBase64String(IV);   // 16 bytes (128-bit IV)

                    // Check if key and IV lengths are correct
                    if (aesAlg.Key.Length != 32)
                        throw new CryptographicException("Invalid key length. Expected 256-bit (32 bytes).");
                    if (aesAlg.IV.Length != 16)
                        throw new CryptographicException("Invalid IV length. Expected 128-bit (16 bytes).");

                    // Create the decryptor
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // Perform decryption
                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CryptographicException("Error during decryption", ex);
            }
        }

        //public static string Encrypt_New(string plainText)
        //{
        //    using (Aes aesAlg = Aes.Create())
        //    {
        //        aesAlg.Key = Convert.FromBase64String(Key);
        //        aesAlg.IV = Convert.FromBase64String(IV);

        //        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        //        using (MemoryStream msEncrypt = new MemoryStream())
        //        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        //        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
        //        {
        //            swEncrypt.Write(plainText);
        //        }

        //        return Convert.ToBase64String(msEncrypt.ToArray());  // Return encrypted text as Base64 string
        //    }
        //}


    }
}
