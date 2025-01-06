using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
using SMFG_API_New.Models;
using System.Linq;
using System;
using SMFG_API_New.RequestModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Security.Cryptography.Xml;
using Azure;
namespace SMFG_API_New.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly SmfgApiContext _context; 
        RSAKeyHelper rsaobj = new RSAKeyHelper();
        private IExceptionHandling _exceptionHandling;

        public UserController(SmfgApiContext context, IExceptionHandling exceptionHandling)
        {
            _context = context;
            _exceptionHandling = exceptionHandling;
        }

        //[HttpPost("authenticate")]
        //public IActionResult Authenticate([FromBody] UserRequestModel user)
        //{
        //    var existingUser = _context.SysUsers
        //        .FirstOrDefault(u => u.Userid == user.UserName && u.Pwd == user.Password);

        //    if (existingUser == null)

        //        return BadRequest(new
        //        {
        //            status = false,
        //            message = "Invalid username or password",
        //            data = (object)null
        //        });
        //    // return Unauthorized("Invalid UserID or Password");

        //    var token = JwtTokenHelper.GenerateToken(user.UserName);

        //    return Ok(new
        //    {
        //        status = true,
        //        message = "Authentication successful",
        //        data = new
        //        {
        //            token = token,
        //            expiresIn = 3600 // Token expiration in seconds
        //        }
        //    });


        //}


       // [Authorize]
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_context.SysUsers.ToList());
        }

       // [Authorize]
        [HttpPost]
        private IActionResult CreateUser([FromBody] SysUser user)
        {
            _context.SysUsers.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserRequestModelEny request)
        {
        //    string IV = "ZWF5t8H9a2NUkdhN+"; // 128-bit IV

            try
            {
                string encryptedDataRequest = request.DataRequest;
                string encryptedAESKeyRequest = request.AESKey;
                string AESIvRequest = request.AESIv;

                string IVDeykey = RSAKeyHelper.DecryptAESKeyWithRSA(encryptedAESKeyRequest, rsaobj.privatekye); 

                string encryptedRequest = request.DataRequest.ToString();
                string decryptedData = RSAKeyHelper.DecryptAES(encryptedDataRequest, IVDeykey, AESIvRequest);

                //var decryptedRequest = EncryptionHelper.Decrypt(encryptedRequest, IV); 
                
                var userRequest = JsonConvert.DeserializeObject<UserRequestModel>(decryptedData);

                var token = JwtTokenHelper.GenerateToken(userRequest.UserName);

                // Prepare response payload
                var response = new
                {
                    status = true,
                    message = "Authentication successful",
                    data = new
                    {
                        token = token,
                        expiresIn = 3600
                    }
                };
                 
            return Ok(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                _exceptionHandling.LogError(ex);
                var errorResponse = new
                {
                    status = false,
                    message = "In Valid UserID and Password",
                    data = (object)null
                }; 
                return BadRequest(JsonConvert.SerializeObject(errorResponse));
            }
        }
         
       // [HttpPost("DecryptData")]
        private IActionResult DecryptData([FromBody] UserRequestModelEny request)
        {
             //RSAKeyHelper.GenerateKeys(out string publicKeyy, out string privateKeyy);

            // publicKey 
            string publickey = "MIIBCgKCAQEAoPupWgxxbyZgNea8tKrwFr+jg/+ve3BMsr/RoKeQYNUx3FOhm0j0HMTpP5Sj6Ly554o3OF/Mlzdne2gUpIiyb7AxGtCeeyd5fXsZfYIkgsVXsuS4CfTQyoRCxp3hyZS7t2R/miiTpIZ75yorwUiIpl5WSFw+uMbG/0Y+RVrSxD5KZWr6+13Gp4XDm+WB7jxrAICyPJhbzNWvLai14V61TFwCb2zR4sPMun+rmaye10+lJExMLFl1dZh5eGJJUHRhLRnx0BdLXV6pxdvV+X5W+GZIqNRdh+IQpqXtDLj06JMp5YEIr8onEE6GxNN2ekUzXE7wfrayI4ikIDXFatdqcQIDAQAB";
            //Private key
            string privatekye = "MIIEpAIBAAKCAQEAoPupWgxxbyZgNea8tKrwFr+jg/+ve3BMsr/RoKeQYNUx3FOhm0j0HMTpP5Sj6Ly554o3OF/Mlzdne2gUpIiyb7AxGtCeeyd5fXsZfYIkgsVXsuS4CfTQyoRCxp3hyZS7t2R/miiTpIZ75yorwUiIpl5WSFw+uMbG/0Y+RVrSxD5KZWr6+13Gp4XDm+WB7jxrAICyPJhbzNWvLai14V61TFwCb2zR4sPMun+rmaye10+lJExMLFl1dZh5eGJJUHRhLRnx0BdLXV6pxdvV+X5W+GZIqNRdh+IQpqXtDLj06JMp5YEIr8onEE6GxNN2ekUzXE7wfrayI4ikIDXFatdqcQIDAQABAoIBAQCBTQGeSUZHL3yxbo39UxsaOny8xf6N2ApSvE1Ei7IkhLjSrf2suzEPdn2eSuZIRmXPeq2VwySSBLdCYLvL19vyZ4NZdV0FsBQd3GUB8SkgPB+PmbPUk2WsdAUqgJ9dCPCpgbgX+yVzTW1xo+aXDYpsjnMhFa5vPYGSgjn7/NIzOkrNx/738g+TkBGqNeMbmHMPl6eTA2n8cT0NE+MnGdmvRW6PkPXTcKwy2G4hfAJI6w0OM1KxPl+MHXtA8W4+nRX9LsA55brhcEYhVIMQzSbn0X+wKPXPLqIs5Xp1GSlxZcYhswHefRvw+FC2WZDrsAY8IsW0+9FmAzIpdJ7xCCENAoGBAMWYCNUdaUsulJa6kWQA8v3FXHitsb9MaGEI6HANC/I+JyMYxIkBgIvxPRWN98+KnDOMHjD1uHR7mRJj9tL2nRs7wpG4Jz1g4T8HUvezoc7j4T61j7V/0xpK8JfjRWYp5j018RRp5yzwTnXS7Oj66vX8Tgda8htRcp3Hme8IdrfLAoGBANCRR1ZyatGdABwEiUpz+7tnWvNuStggaNLgX4fVl7SP7hU0X5JFL9IuogEkJM2kPmw73iEuyzaqSG7pmY5OsowiNsoHJlcMCsL2PGVEan49546zcZpwFSRRn53CODyz7m5Kcs95iP5xeCDdOuuRPDRMnkxMjJOVHxqPTDpYtsczAoGAJrHJOQLTddefXY0Xn7/X1f5qR2+sWUv7PNVjv12uszecrnDRPAtBQyZw0eHFX61DPYz49JmKD7WMml9dHJ8S0Rx409R+SrTIJ3Glu8A/taZGm+MuS1rG2mVGjFgDZShbYC1KErdSgChnFQfDQTSyAo3wMdyLgPIIQgGukXLU3NUCgYEAraF0URxZnv1kHO8N2ISr+by2c9fKyRhaC8ws22lOrUvxOYfrVFryz7hwuCB93xCvwu0oJFnPZUfnmyYv5s/PRmgpUpEXMvpcbygM6YVGXqhsgFkU5ywN/blR90S8CpUElp6169FS4fhWuI1UQs4a37M1SXGkyiwnw7WuERjPuQ8CgYBbXoOytYD4vxuquYFro4tV008dCYHaivzcaSyT7uUWLqNQoIBS1MGh+5pFzY4jxyDPmSRldb/lwURtKMN6ZxUmbju6upcPpFKdI3IoCzRkKwYm+yYhiiVs+EW4GPh80DZvOFDV7BWYbrpP+Mv15Q4VfIUekpMFI60TmnJ9Tw4PaA==";


            var (key, iv) = RSAKeyHelper.GenerateAESKeyAndIV();

            // Generate AES Key and IV
            //  string aesKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("WnO0TNjDOLydqELRKuy/GvENuGCDnsiSE5rSaKcKY1Y=")); // Ensure base64 encoding
            // string aesIV = Convert.ToBase64String(Encoding.UTF8.GetBytes("fvMa9opS33bpPbkonQsCNg=="));

            // Generate AES Key and IV
            string aesKey = key;   //"WnO0TNjDOLydqELRKuy/GvENuGCDnsiSE5rSaKcKY1Y="; // Base64-encoded AES Key
            string aesIV = iv;  //"fvMa9opS33bpPbkonQsCNg=="; // Base64-encoded AES IV

            string IVEnykey = RSAKeyHelper.EncryptAESKeyWithRSA(aesKey, publickey);
            string IVDeykey = RSAKeyHelper.DecryptAESKeyWithRSA(IVEnykey, privatekye);

            string IV = "ZWF5t8H9a2NUkdhN+"; // 128-bit IV

            try
            { 
               var response = new
                {
                    
                   DataRequest = new
                    {
                        UserName = "admin",
                        Password = "Rajeshwar@123",
                    }
                };

                // Encrypt and Decrypt Data Using AES
               // string plainText = response;
                string encryptedData = RSAKeyHelper.EncryptAES(JsonConvert.SerializeObject(response), aesKey, aesIV);

                string decryptedData = RSAKeyHelper.DecryptAES(encryptedData, aesKey, aesIV);

                //   var encryptedResponse = EncryptionHelper.EncryptAES(JsonConvert.SerializeObject(response), IV);


                //var response = new
                //{
                //    UserName="admin",
                //    Password= "Rajeshwar@123", 
                //};
                // Encrypt the response

                      var encryptedResponse = EncryptionHelper.Encrypt(JsonConvert.SerializeObject(response), IV);
                //EncryptAES


                //// RSAKeyHelper.GenerateKeys(out string publicKey, out string privateKey);
                //// Save or use publicKey and privateKey as needed
                ////   string decryptedResponse = DecryptAES(encryptedResponse, aesKey, iv);

                //// Decrypt the incoming request
                ////  string encryptedRequest = request["DataRequest"].ToString();
                //string encryptedRequest = request.DataRequest.ToString();
                //var decryptedRequest = EncryptionHelper.Decrypt(encryptedRequest);
                //var userRequest = JsonConvert.DeserializeObject<UserRequestModel>(decryptedRequest);

                //var token = JwtTokenHelper.GenerateToken(userRequest.UserName);

                //// Prepare response payload

                //var response = new
                //{
                //    status = true,
                //    message = "Authentication successful",
                //    data = new
                //    {
                //        token = token,
                //        expiresIn = 3600
                //    }
                //};

                //// Encrypt the response
                //var encryptedResponse = EncryptionHelper.Encrypt(JsonConvert.SerializeObject(response));

                return Ok(encryptedResponse);
            }
            catch (Exception ex)
            {
                // Handle errors and return an encrypted error response
                var errorResponse = new
                {
                    status = false,
                    message = "An error occurred: " + ex.Message,
                    data = (object)null
                };

                var encryptedErrorResponse = EncryptionHelper.Encrypt(JsonConvert.SerializeObject(errorResponse), IV);
                return BadRequest(encryptedErrorResponse);
            }
        }
         

    }
}
