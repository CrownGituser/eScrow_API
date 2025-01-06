using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SMFG_API_New
{
    public class RSAKeyHelper
    {

        public string publickey = "MIIBCgKCAQEAoPupWgxxbyZgNea8tKrwFr+jg/+ve3BMsr/RoKeQYNUx3FOhm0j0HMTpP5Sj6Ly554o3OF/Mlzdne2gUpIiyb7AxGtCeeyd5fXsZfYIkgsVXsuS4CfTQyoRCxp3hyZS7t2R/miiTpIZ75yorwUiIpl5WSFw+uMbG/0Y+RVrSxD5KZWr6+13Gp4XDm+WB7jxrAICyPJhbzNWvLai14V61TFwCb2zR4sPMun+rmaye10+lJExMLFl1dZh5eGJJUHRhLRnx0BdLXV6pxdvV+X5W+GZIqNRdh+IQpqXtDLj06JMp5YEIr8onEE6GxNN2ekUzXE7wfrayI4ikIDXFatdqcQIDAQAB";

        public string privatekye ="MIIEpAIBAAKCAQEAoPupWgxxbyZgNea8tKrwFr+jg/+ve3BMsr/RoKeQYNUx3FOhm0j0HMTpP5Sj6Ly554o3OF/Mlzdne2gUpIiyb7AxGtCeeyd5fXsZfYIkgsVXsuS4CfTQyoRCxp3hyZS7t2R/miiTpIZ75yorwUiIpl5WSFw+uMbG/0Y+RVrSxD5KZWr6+13Gp4XDm+WB7jxrAICyPJhbzNWvLai14V61TFwCb2zR4sPMun+rmaye10+lJExMLFl1dZh5eGJJUHRhLRnx0BdLXV6pxdvV+X5W+GZIqNRdh+IQpqXtDLj06JMp5YEIr8onEE6GxNN2ekUzXE7wfrayI4ikIDXFatdqcQIDAQABAoIBAQCBTQGeSUZHL3yxbo39UxsaOny8xf6N2ApSvE1Ei7IkhLjSrf2suzEPdn2eSuZIRmXPeq2VwySSBLdCYLvL19vyZ4NZdV0FsBQd3GUB8SkgPB+PmbPUk2WsdAUqgJ9dCPCpgbgX+yVzTW1xo+aXDYpsjnMhFa5vPYGSgjn7/NIzOkrNx/738g+TkBGqNeMbmHMPl6eTA2n8cT0NE+MnGdmvRW6PkPXTcKwy2G4hfAJI6w0OM1KxPl+MHXtA8W4+nRX9LsA55brhcEYhVIMQzSbn0X+wKPXPLqIs5Xp1GSlxZcYhswHefRvw+FC2WZDrsAY8IsW0+9FmAzIpdJ7xCCENAoGBAMWYCNUdaUsulJa6kWQA8v3FXHitsb9MaGEI6HANC/I+JyMYxIkBgIvxPRWN98+KnDOMHjD1uHR7mRJj9tL2nRs7wpG4Jz1g4T8HUvezoc7j4T61j7V/0xpK8JfjRWYp5j018RRp5yzwTnXS7Oj66vX8Tgda8htRcp3Hme8IdrfLAoGBANCRR1ZyatGdABwEiUpz+7tnWvNuStggaNLgX4fVl7SP7hU0X5JFL9IuogEkJM2kPmw73iEuyzaqSG7pmY5OsowiNsoHJlcMCsL2PGVEan49546zcZpwFSRRn53CODyz7m5Kcs95iP5xeCDdOuuRPDRMnkxMjJOVHxqPTDpYtsczAoGAJrHJOQLTddefXY0Xn7/X1f5qR2+sWUv7PNVjv12uszecrnDRPAtBQyZw0eHFX61DPYz49JmKD7WMml9dHJ8S0Rx409R+SrTIJ3Glu8A/taZGm+MuS1rG2mVGjFgDZShbYC1KErdSgChnFQfDQTSyAo3wMdyLgPIIQgGukXLU3NUCgYEAraF0URxZnv1kHO8N2ISr+by2c9fKyRhaC8ws22lOrUvxOYfrVFryz7hwuCB93xCvwu0oJFnPZUfnmyYv5s/PRmgpUpEXMvpcbygM6YVGXqhsgFkU5ywN/blR90S8CpUElp6169FS4fhWuI1UQs4a37M1SXGkyiwnw7WuERjPuQ8CgYBbXoOytYD4vxuquYFro4tV008dCYHaivzcaSyT7uUWLqNQoIBS1MGh+5pFzY4jxyDPmSRldb/lwURtKMN6ZxUmbju6upcPpFKdI3IoCzRkKwYm+yYhiiVs+EW4GPh80DZvOFDV7BWYbrpP+Mv15Q4VfIUekpMFI60TmnJ9Tw4PaA==";
         
        public static void GenerateKeys(out string publicKey, out string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
                privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
            }
        }

        //public static string EncryptAESKeyWithRSA(string aesKey, string rsaPublicKey)
        //{
        //    using (var rsa = new RSACryptoServiceProvider())
        //    {
        //        rsa.ImportRSAPublicKey(Convert.FromBase64String(rsaPublicKey), out _);
        //        var encryptedKey = rsa.Encrypt(Encoding.UTF8.GetBytes(aesKey), RSAEncryptionPadding.OaepSHA256);
        //        return Convert.ToBase64String(encryptedKey);
        //    }
        //}

        //public static string EncryptAESKeyWithRSA(string aesKey, string rsaPublicKey)
        //{
        //    using (var rsa = new RSACryptoServiceProvider())
        //    {
        //        rsa.ImportRSAPublicKey(Convert.FromBase64String(rsaPublicKey), out _);
        //        var encryptedKey = rsa.Encrypt(Convert.FromBase64String(aesKey), RSAEncryptionPadding.OaepSHA256);
        //        return Convert.ToBase64String(encryptedKey);
        //    }
        //}

        public static string EncryptAESKeyWithRSA(string aesKey, string rsaPublicKey)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(Convert.FromBase64String(rsaPublicKey), out _);
                byte[] keyBytes = Convert.FromBase64String(aesKey); // Convert AES key from Base64 to bytes
                byte[] encryptedKey = rsa.Encrypt(keyBytes, RSAEncryptionPadding.OaepSHA256);
                return Convert.ToBase64String(encryptedKey); // Return the encrypted key as Base64
            }
        }

        public static string DecryptAESKeyWithRSA(string encryptedAESKey, string rsaPrivateKey)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(rsaPrivateKey), out _);
                byte[] encryptedKeyBytes = Convert.FromBase64String(encryptedAESKey); // Convert encrypted key from Base64 to bytes
                byte[] decryptedKeyBytes = rsa.Decrypt(encryptedKeyBytes, RSAEncryptionPadding.OaepSHA256);
                return Convert.ToBase64String(decryptedKeyBytes); // Return the decrypted key as Base64
            }
        }

        //public static string DecryptAESKeyWithRSA(string encryptedAESKey, string rsaPrivateKey)
        //{
        //    using (var rsa = new RSACryptoServiceProvider())
        //    {
        //        rsa.ImportRSAPrivateKey(Convert.FromBase64String(rsaPrivateKey), out _);
        //        var decryptedKey = rsa.Decrypt(Convert.FromBase64String(encryptedAESKey), RSAEncryptionPadding.OaepSHA256);
        //        return Encoding.UTF8.GetString(decryptedKey);
        //    }
        //}

        public static string DecryptAES(string encryptedData, string aesKey, string iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(aesKey);
                aes.IV = Convert.FromBase64String(iv);

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (var ms = new MemoryStream(Convert.FromBase64String(encryptedData)))
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

        //public static string EncryptAES(string plainText, string aesKey, string iv)
        //{
        //    using (var aes = Aes.Create())
        //    {
        //        aes.Key = Convert.FromBase64String(aesKey);
        //        aes.IV = Convert.FromBase64String(iv);

        //        using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
        //        {
        //            using (var ms = new MemoryStream())
        //            {
        //                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        //                {
        //                    using (var writer = new StreamWriter(cs))
        //                    {
        //                        writer.Write(plainText);
        //                    }
        //                }
        //                return Convert.ToBase64String(ms.ToArray());
        //            }
        //        }
        //    }
        //}


        public static string EncryptAES(string plainText, string aesKey, string iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(aesKey);
                aes.IV = Convert.FromBase64String(iv);

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


        public static (string Key, string IV) GenerateAESKeyAndIV()
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 256; // Ensure 256-bit key size
                aes.GenerateKey();
                aes.GenerateIV();

                string key = Convert.ToBase64String(aes.Key);
                string iv = Convert.ToBase64String(aes.IV);

                return (key, iv);
            }
        }


        public static (string Key, string IV) GenerateAESKeyAndIVEncrypt()
        {

            string publickey = "MIIBCgKCAQEAoPupWgxxbyZgNea8tKrwFr+jg/+ve3BMsr/RoKeQYNUx3FOhm0j0HMTpP5Sj6Ly554o3OF/Mlzdne2gUpIiyb7AxGtCeeyd5fXsZfYIkgsVXsuS4CfTQyoRCxp3hyZS7t2R/miiTpIZ75yorwUiIpl5WSFw+uMbG/0Y+RVrSxD5KZWr6+13Gp4XDm+WB7jxrAICyPJhbzNWvLai14V61TFwCb2zR4sPMun+rmaye10+lJExMLFl1dZh5eGJJUHRhLRnx0BdLXV6pxdvV+X5W+GZIqNRdh+IQpqXtDLj06JMp5YEIr8onEE6GxNN2ekUzXE7wfrayI4ikIDXFatdqcQIDAQAB";
            //Private key
            string privatekye = "MIIEpAIBAAKCAQEAoPupWgxxbyZgNea8tKrwFr+jg/+ve3BMsr/RoKeQYNUx3FOhm0j0HMTpP5Sj6Ly554o3OF/Mlzdne2gUpIiyb7AxGtCeeyd5fXsZfYIkgsVXsuS4CfTQyoRCxp3hyZS7t2R/miiTpIZ75yorwUiIpl5WSFw+uMbG/0Y+RVrSxD5KZWr6+13Gp4XDm+WB7jxrAICyPJhbzNWvLai14V61TFwCb2zR4sPMun+rmaye10+lJExMLFl1dZh5eGJJUHRhLRnx0BdLXV6pxdvV+X5W+GZIqNRdh+IQpqXtDLj06JMp5YEIr8onEE6GxNN2ekUzXE7wfrayI4ikIDXFatdqcQIDAQABAoIBAQCBTQGeSUZHL3yxbo39UxsaOny8xf6N2ApSvE1Ei7IkhLjSrf2suzEPdn2eSuZIRmXPeq2VwySSBLdCYLvL19vyZ4NZdV0FsBQd3GUB8SkgPB+PmbPUk2WsdAUqgJ9dCPCpgbgX+yVzTW1xo+aXDYpsjnMhFa5vPYGSgjn7/NIzOkrNx/738g+TkBGqNeMbmHMPl6eTA2n8cT0NE+MnGdmvRW6PkPXTcKwy2G4hfAJI6w0OM1KxPl+MHXtA8W4+nRX9LsA55brhcEYhVIMQzSbn0X+wKPXPLqIs5Xp1GSlxZcYhswHefRvw+FC2WZDrsAY8IsW0+9FmAzIpdJ7xCCENAoGBAMWYCNUdaUsulJa6kWQA8v3FXHitsb9MaGEI6HANC/I+JyMYxIkBgIvxPRWN98+KnDOMHjD1uHR7mRJj9tL2nRs7wpG4Jz1g4T8HUvezoc7j4T61j7V/0xpK8JfjRWYp5j018RRp5yzwTnXS7Oj66vX8Tgda8htRcp3Hme8IdrfLAoGBANCRR1ZyatGdABwEiUpz+7tnWvNuStggaNLgX4fVl7SP7hU0X5JFL9IuogEkJM2kPmw73iEuyzaqSG7pmY5OsowiNsoHJlcMCsL2PGVEan49546zcZpwFSRRn53CODyz7m5Kcs95iP5xeCDdOuuRPDRMnkxMjJOVHxqPTDpYtsczAoGAJrHJOQLTddefXY0Xn7/X1f5qR2+sWUv7PNVjv12uszecrnDRPAtBQyZw0eHFX61DPYz49JmKD7WMml9dHJ8S0Rx409R+SrTIJ3Glu8A/taZGm+MuS1rG2mVGjFgDZShbYC1KErdSgChnFQfDQTSyAo3wMdyLgPIIQgGukXLU3NUCgYEAraF0URxZnv1kHO8N2ISr+by2c9fKyRhaC8ws22lOrUvxOYfrVFryz7hwuCB93xCvwu0oJFnPZUfnmyYv5s/PRmgpUpEXMvpcbygM6YVGXqhsgFkU5ywN/blR90S8CpUElp6169FS4fhWuI1UQs4a37M1SXGkyiwnw7WuERjPuQ8CgYBbXoOytYD4vxuquYFro4tV008dCYHaivzcaSyT7uUWLqNQoIBS1MGh+5pFzY4jxyDPmSRldb/lwURtKMN6ZxUmbju6upcPpFKdI3IoCzRkKwYm+yYhiiVs+EW4GPh80DZvOFDV7BWYbrpP+Mv15Q4VfIUekpMFI60TmnJ9Tw4PaA==";


            var (key, iv) = RSAKeyHelper.GenerateAESKeyAndIV();

            string aesKey = key;  
            string aesIV = iv;

            string AESEnykey = RSAKeyHelper.EncryptAESKeyWithRSA(aesKey, publickey);
            //    string IVDeykey = RSAKeyHelper.DecryptAESKeyWithRSA(IVEnykey, privatekye);

            return (AESEnykey, iv);

        }

    }

}
