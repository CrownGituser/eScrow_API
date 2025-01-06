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
using System.Net;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SMFG_API_New.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataUploadController : ControllerBase
    {
        private static int ResponseCounter = 1000000000;
        private readonly SmfgApiContext _context; 
        private readonly string _connectionString; 
        RSAKeyHelper rsaobj = new RSAKeyHelper();
        private IExceptionHandling _exceptionHandling;

        public DataUploadController(SmfgApiContext context , IConfiguration configuration, IExceptionHandling exceptionHandling)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _exceptionHandling = exceptionHandling;

        }
         
        [HttpPost("DataUpload")]
        public IActionResult DataUpload([FromBody] DataUploadRequestModelEny request)
        {
            string publickey = "MIIBCgKCAQEAoPupWgxxbyZgNea8tKrwFr+jg/+ve3BMsr/RoKeQYNUx3FOhm0j0HMTpP5Sj6Ly554o3OF/Mlzdne2gUpIiyb7AxGtCeeyd5fXsZfYIkgsVXsuS4CfTQyoRCxp3hyZS7t2R/miiTpIZ75yorwUiIpl5WSFw+uMbG/0Y+RVrSxD5KZWr6+13Gp4XDm+WB7jxrAICyPJhbzNWvLai14V61TFwCb2zR4sPMun+rmaye10+lJExMLFl1dZh5eGJJUHRhLRnx0BdLXV6pxdvV+X5W+GZIqNRdh+IQpqXtDLj06JMp5YEIr8onEE6GxNN2ekUzXE7wfrayI4ikIDXFatdqcQIDAQAB";

            try
            {
                string encryptedDataRequest = request.DataRequest;
                string encryptedAESKeyRequest = request.AESKey;
                string AESIvRequest = request.AESIv;

                string responseId = GenerateResponseId();

                if (string.IsNullOrEmpty(encryptedAESKeyRequest))
                {
                    var plainResponse = new
                    {
                        Id = responseId,
                        Status = "false",
                        StatusCode = "500",
                        Message = "AES key is missing.",
                        Data = (object)null
                    };

                    //var (aesKey, aesIV) = RSAKeyHelper.GenerateAESKeyAndIVEncrypt();
                    //string encryptedData = RSAKeyHelper.EncryptAES(JsonConvert.SerializeObject(plainResponse), aesKey, aesIV);

                    //return Ok(JsonConvert.SerializeObject(encryptedData));
                    return Ok(JsonConvert.SerializeObject(plainResponse));

                }
                else if (string.IsNullOrEmpty(AESIvRequest))
                {
                    var plainResponse = new
                    {
                        RefId = responseId,
                        Status = "false",
                        StatusCode = "500",
                        Message = "AES IV key is missing.",
                        Data = (object)null
                    };

                    //var (aesKey, aesIV) = RSAKeyHelper.GenerateAESKeyAndIVEncrypt();
                    //string encryptedData = RSAKeyHelper.EncryptAES(JsonConvert.SerializeObject(plainResponse), aesKey, aesIV);

                    return Ok(JsonConvert.SerializeObject(plainResponse));
                   // return Ok(JsonConvert.SerializeObject(encryptedData));
                }
                else if (string.IsNullOrEmpty(encryptedDataRequest))
                {
                    var plainResponse = new
                    {
                        RefId = responseId,
                        Status = "false",
                        StatusCode = "500",
                        Message = "Data request is missing.",
                        Data = (object)null
                    };

                    //var (aesKey, aesIV) = RSAKeyHelper.GenerateAESKeyAndIVEncrypt();
                    //string encryptedData = RSAKeyHelper.EncryptAES(JsonConvert.SerializeObject(plainResponse), aesKey, aesIV);

                    return Ok(JsonConvert.SerializeObject(plainResponse));

                }
                else
                {
                    string IVDeykey = RSAKeyHelper.DecryptAESKeyWithRSA(encryptedAESKeyRequest, rsaobj.privatekye);

                    string encryptedRequest = request.DataRequest.ToString();
                    string decryptedData = RSAKeyHelper.DecryptAES(encryptedDataRequest, IVDeykey, AESIvRequest);

                    //var decryptedRequest = EncryptionHelper.Decrypt(encryptedRequest, IV); 

                    DURequestModelEntity uploadData = JsonConvert.DeserializeObject<DURequestModelEntity>(decryptedData);

                    if (uploadData != null && uploadData.DataRequest != null)
                    {
                        var uploadResult = IwardDumpUpload_New(uploadData);

                        var plainResponse = new
                        {
                            Data = uploadResult
                        };

                        var (key, iv) = RSAKeyHelper.GenerateAESKeyAndIV();

                        string aesKey = key;  
                        string aesIV = iv;  

                        string IVEnykey = RSAKeyHelper.EncryptAESKeyWithRSA(aesKey, publickey);
                        //var (aesKeys, AESIV) = RSAKeyHelper.GenerateAESKeyAndIVEncrypt();
                        //  string aesEnyKeys = RSAKeyHelper.EncryptAESKeyWithRSA(aesKeys, publickey);
                        //string IVEnykey = RSAKeyHelper.EncryptAESKeyWithRSA(aesKey, publickey);


                        string encryptedData = RSAKeyHelper.EncryptAES(JsonConvert.SerializeObject(plainResponse), aesKey, aesIV);
                        //string encryptedData = RSAKeyHelper.EncryptAES(JsonConvert.SerializeObject(plainResponse), aesKeys, AESIV);

                        var EnyResponse = new
                        {
                            RefId = responseId,
                            Status = "true",
                            StatusCode = "200",
                            aesKey= aesKey,
                            aesIV = aesIV,
                            dataRequest = encryptedData
                        };

                        return Ok(JsonConvert.SerializeObject(EnyResponse));


                        //string encryptedResponse = _encryption.AESEncrypt(JsonConvert.SerializeObject(plainResponse), encryptedData.PublicKey);
                        //return Request.CreateResponse(HttpStatusCode.OK, new
                        //{
                        //    Id = responseId,
                        //    Status = "true",
                        //    StatusCode = "200",

                        //    EncryptedData = encryptedResponse
                        //});
                    }

                    var errorResponse = new
                    {
                        status = false,
                        message = "No Data Found",
                        data = (object)null
                    };
                    return BadRequest(JsonConvert.SerializeObject(errorResponse));

                    //return Ok(JsonConvert.SerializeObject(response));


                }


            }
            catch (Exception ex)
            {
                _exceptionHandling.LogError(ex);
                var errorResponse = new
                {
                    status = false,
                    message = "No Data Found",
                    data = (object)null
                };
                return BadRequest(JsonConvert.SerializeObject(errorResponse));
            }
        }
         
        private string GenerateResponseId()
        {
            // Increment counter and generate the response ID
            return "REF" + (Interlocked.Increment(ref ResponseCounter));
        }
         
        private List<object> IwardDumpUpload_New(DURequestModelEntity entity)
        {
            var resultList = new List<object>();

            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var record in entity.DataRequest)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@BranchCode", record.BranchCode);
                    parameters.Add("@BranchName", record.BranchName);
                    parameters.Add("@CustomerID", record.CustomerID);
                    parameters.Add("@GroupID", record.GroupID);
                    parameters.Add("@AppRefNo", record.AppRefNo);
                    parameters.Add("@LAN", record.LAN);
                    parameters.Add("@CustomerName", record.CustomerName);
                    parameters.Add("@Disperseddate", record.Disperseddate);
                    parameters.Add("@ProductCode", record.ProductCode);
                    parameters.Add("@ProductName", record.ProductName);
                    parameters.Add("@LoanAmount", record.LoanAmount);
                    parameters.Add("@MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    // Execute stored procedure
                    SqlMapper.Execute(connection, "InwardDumpUpload", param: parameters, commandType: CommandType.StoredProcedure);

                    // Retrieve output parameter value
                    string resultMessage = parameters.Get<string>("@MSG");

                    resultList.Add(new
                    {
                        LAN = record.LAN,
                        Message = resultMessage
                    });
                }
            }

            return resultList;
        }
         
      //  [HttpPost("DecryptData")]
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
           // string IVDeykey = RSAKeyHelper.DecryptAESKeyWithRSA(IVEnykey, privatekye);

            string IV = "ZWF5t8H9a2NUkdhN+"; // 128-bit IV


            var response1 = new
            {
                dataRequest = new[]
      {
        new
        {
            BranchCode = "B001",
            BranchName = "Branch One",
            CustomerID = "C001",
            GroupID = "G001",
            AppRefNo = "APP001",
            LAN = "LAN001",
            CustomerName = "John Doe",
            Disperseddate = "2024-10-01",
            ProductCode = "P001",
            ProductName = "Personal Loan",
            LoanAmount = "10000"
        },
        new
        {
            BranchCode = "B002",
            BranchName = "Branch Two",
            CustomerID = "C002",
            GroupID = "G002",
            AppRefNo = "APP002",
            LAN = "LAN002",
            CustomerName = "Jane Smith",
            Disperseddate = "2024-10-02",
            ProductCode = "P002",
            ProductName = "Home Loan",
            LoanAmount = "20000"
        }
    }
            };



            try
            {
                 

                // Encrypt and Decrypt Data Using AES
                // string plainText = response;
                string encryptedData = RSAKeyHelper.EncryptAES(JsonConvert.SerializeObject(response1), aesKey, aesIV);


                //  string decryptedData = RSAKeyHelper.DecryptAES(encryptedData, aesKey, aesIV);

                var response = new
                {
                    AESKey = IVEnykey,
                    AESIV = aesIV,
                    DataRequest = encryptedData,
                };

                //  var encryptedResponse = EncryptionHelper.Encrypt(JsonConvert.SerializeObject(response), IV);

                return Ok(response);
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
         
      //  [HttpPost("EnyLAN")]
        private IActionResult EnyLAN([FromBody] UserRequestModelEny request)
        {
            //RSAKeyHelper.GenerateKeys(out string publicKeyy, out string privateKeyy);

            // publicKey 
            string publickey = "MIIBCgKCAQEAoPupWgxxbyZgNea8tKrwFr+jg/+ve3BMsr/RoKeQYNUx3FOhm0j0HMTpP5Sj6Ly554o3OF/Mlzdne2gUpIiyb7AxGtCeeyd5fXsZfYIkgsVXsuS4CfTQyoRCxp3hyZS7t2R/miiTpIZ75yorwUiIpl5WSFw+uMbG/0Y+RVrSxD5KZWr6+13Gp4XDm+WB7jxrAICyPJhbzNWvLai14V61TFwCb2zR4sPMun+rmaye10+lJExMLFl1dZh5eGJJUHRhLRnx0BdLXV6pxdvV+X5W+GZIqNRdh+IQpqXtDLj06JMp5YEIr8onEE6GxNN2ekUzXE7wfrayI4ikIDXFatdqcQIDAQAB";

            //Private key
            string privatekye = "MIIEpAIBAAKCAQEAoPupWgxxbyZgNea8tKrwFr+jg/+ve3BMsr/RoKeQYNUx3FOhm0j0HMTpP5Sj6Ly554o3OF/Mlzdne2gUpIiyb7AxGtCeeyd5fXsZfYIkgsVXsuS4CfTQyoRCxp3hyZS7t2R/miiTpIZ75yorwUiIpl5WSFw+uMbG/0Y+RVrSxD5KZWr6+13Gp4XDm+WB7jxrAICyPJhbzNWvLai14V61TFwCb2zR4sPMun+rmaye10+lJExMLFl1dZh5eGJJUHRhLRnx0BdLXV6pxdvV+X5W+GZIqNRdh+IQpqXtDLj06JMp5YEIr8onEE6GxNN2ekUzXE7wfrayI4ikIDXFatdqcQIDAQABAoIBAQCBTQGeSUZHL3yxbo39UxsaOny8xf6N2ApSvE1Ei7IkhLjSrf2suzEPdn2eSuZIRmXPeq2VwySSBLdCYLvL19vyZ4NZdV0FsBQd3GUB8SkgPB+PmbPUk2WsdAUqgJ9dCPCpgbgX+yVzTW1xo+aXDYpsjnMhFa5vPYGSgjn7/NIzOkrNx/738g+TkBGqNeMbmHMPl6eTA2n8cT0NE+MnGdmvRW6PkPXTcKwy2G4hfAJI6w0OM1KxPl+MHXtA8W4+nRX9LsA55brhcEYhVIMQzSbn0X+wKPXPLqIs5Xp1GSlxZcYhswHefRvw+FC2WZDrsAY8IsW0+9FmAzIpdJ7xCCENAoGBAMWYCNUdaUsulJa6kWQA8v3FXHitsb9MaGEI6HANC/I+JyMYxIkBgIvxPRWN98+KnDOMHjD1uHR7mRJj9tL2nRs7wpG4Jz1g4T8HUvezoc7j4T61j7V/0xpK8JfjRWYp5j018RRp5yzwTnXS7Oj66vX8Tgda8htRcp3Hme8IdrfLAoGBANCRR1ZyatGdABwEiUpz+7tnWvNuStggaNLgX4fVl7SP7hU0X5JFL9IuogEkJM2kPmw73iEuyzaqSG7pmY5OsowiNsoHJlcMCsL2PGVEan49546zcZpwFSRRn53CODyz7m5Kcs95iP5xeCDdOuuRPDRMnkxMjJOVHxqPTDpYtsczAoGAJrHJOQLTddefXY0Xn7/X1f5qR2+sWUv7PNVjv12uszecrnDRPAtBQyZw0eHFX61DPYz49JmKD7WMml9dHJ8S0Rx409R+SrTIJ3Glu8A/taZGm+MuS1rG2mVGjFgDZShbYC1KErdSgChnFQfDQTSyAo3wMdyLgPIIQgGukXLU3NUCgYEAraF0URxZnv1kHO8N2ISr+by2c9fKyRhaC8ws22lOrUvxOYfrVFryz7hwuCB93xCvwu0oJFnPZUfnmyYv5s/PRmgpUpEXMvpcbygM6YVGXqhsgFkU5ywN/blR90S8CpUElp6169FS4fhWuI1UQs4a37M1SXGkyiwnw7WuERjPuQ8CgYBbXoOytYD4vxuquYFro4tV008dCYHaivzcaSyT7uUWLqNQoIBS1MGh+5pFzY4jxyDPmSRldb/lwURtKMN6ZxUmbju6upcPpFKdI3IoCzRkKwYm+yYhiiVs+EW4GPh80DZvOFDV7BWYbrpP+Mv15Q4VfIUekpMFI60TmnJ9Tw4PaA==";


            var (key, iv) = RSAKeyHelper.GenerateAESKeyAndIV(); 
             
            string aesKey = key;   //"WnO0TNjDOLydqELRKuy/GvENuGCDnsiSE5rSaKcKY1Y="; // Base64-encoded AES Key
            string aesIV = iv;  //"fvMa9opS33bpPbkonQsCNg=="; // Base64-encoded AES IV

            string EnyAESkey = RSAKeyHelper.EncryptAESKeyWithRSA(aesKey, publickey);
             
            var LanResponse = new
            {
                LAN = "LAN001", 
            };
              
            try
            {
                string encryptedData = RSAKeyHelper.EncryptAES(JsonConvert.SerializeObject(LanResponse), aesKey, aesIV);

                var response = new
                {
                    AESKey = EnyAESkey,
                    AESIV = aesIV,
                    DataRequest = encryptedData,
                };


                return Ok(response);
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

                var encryptedErrorResponse = EncryptionHelper.Encrypt(JsonConvert.SerializeObject(errorResponse), aesIV);
                return BadRequest(encryptedErrorResponse);
            }
        }
         
        [HttpPost("GetLanStatusDetails")]
        public IActionResult GetLanStatusDetails([FromBody] DataUploadRequestModelEny request)
        {
            string publickey = "MIIBCgKCAQEAoPupWgxxbyZgNea8tKrwFr+jg/+ve3BMsr/RoKeQYNUx3FOhm0j0HMTpP5Sj6Ly554o3OF/Mlzdne2gUpIiyb7AxGtCeeyd5fXsZfYIkgsVXsuS4CfTQyoRCxp3hyZS7t2R/miiTpIZ75yorwUiIpl5WSFw+uMbG/0Y+RVrSxD5KZWr6+13Gp4XDm+WB7jxrAICyPJhbzNWvLai14V61TFwCb2zR4sPMun+rmaye10+lJExMLFl1dZh5eGJJUHRhLRnx0BdLXV6pxdvV+X5W+GZIqNRdh+IQpqXtDLj06JMp5YEIr8onEE6GxNN2ekUzXE7wfrayI4ikIDXFatdqcQIDAQAB";

            try
            {
                string encryptedDataRequest = request.DataRequest;
                string encryptedAESKeyRequest = request.AESKey;
                string AESIvRequest = request.AESIv;

                string responseId = GenerateResponseId();

                if (string.IsNullOrEmpty(encryptedAESKeyRequest))
                {
                    var plainResponse = new
                    {
                        Id = responseId,
                        Status = "false",
                        StatusCode = "500",
                        Message = "AES key is missing.",
                        Data = (object)null
                    };

                    //var (aesKey, aesIV) = RSAKeyHelper.GenerateAESKeyAndIVEncrypt();
                    //string encryptedData = RSAKeyHelper.EncryptAES(JsonConvert.SerializeObject(plainResponse), aesKey, aesIV);

                    //return Ok(JsonConvert.SerializeObject(encryptedData));
                    return Ok(JsonConvert.SerializeObject(plainResponse));

                }
                else if (string.IsNullOrEmpty(AESIvRequest))
                {
                    var plainResponse = new
                    {
                        RefId = responseId,
                        Status = "false",
                        StatusCode = "500",
                        Message = "AES IV key is missing.",
                        Data = (object)null
                    };

                    //var (aesKey, aesIV) = RSAKeyHelper.GenerateAESKeyAndIVEncrypt();
                    //string encryptedData = RSAKeyHelper.EncryptAES(JsonConvert.SerializeObject(plainResponse), aesKey, aesIV);

                    return Ok(JsonConvert.SerializeObject(plainResponse));
                    // return Ok(JsonConvert.SerializeObject(encryptedData));
                }
                else if (string.IsNullOrEmpty(encryptedDataRequest))
                {
                    var plainResponse = new
                    {
                        RefId = responseId,
                        Status = "false",
                        StatusCode = "500",
                        Message = "Data request is missing.",
                        Data = (object)null
                    };

                    //var (aesKey, aesIV) = RSAKeyHelper.GenerateAESKeyAndIVEncrypt();
                    //string encryptedData = RSAKeyHelper.EncryptAES(JsonConvert.SerializeObject(plainResponse), aesKey, aesIV);

                    return Ok(JsonConvert.SerializeObject(plainResponse));

                }
                else
                {
                    string IVDeykey = RSAKeyHelper.DecryptAESKeyWithRSA(encryptedAESKeyRequest, rsaobj.privatekye);

                    string encryptedRequest = request.DataRequest.ToString();
                    string decryptedData = RSAKeyHelper.DecryptAES(encryptedDataRequest, IVDeykey, AESIvRequest);

                    //var decryptedRequest = EncryptionHelper.Decrypt(encryptedRequest, IV); 

                    LANEntityDetails Landata = JsonConvert.DeserializeObject<LANEntityDetails>(decryptedData);
                    
                    var data = GetLanStatusDetails(Landata.LAN);


                    if (data != null || !data.Any())
                    {
                        
                        var plainResponse = new
                        {
                            Data = data
                        };

                        var (key, iv) = RSAKeyHelper.GenerateAESKeyAndIV();

                        string aesKey = key;
                        string aesIV = iv;

                        string IVEnykey = RSAKeyHelper.EncryptAESKeyWithRSA(aesKey, publickey);
                         

                        string encryptedData = RSAKeyHelper.EncryptAES(JsonConvert.SerializeObject(plainResponse), aesKey, aesIV);
                        //string encryptedData = RSAKeyHelper.EncryptAES(JsonConvert.SerializeObject(plainResponse), aesKeys, AESIV);

                        var EnyResponse = new
                        {
                            RefId = responseId,
                            Status = "true",
                            StatusCode = "200",
                            aesKey = aesKey,
                            aesIV = aesIV,
                            dataRequest = encryptedData
                        };

                        return Ok(JsonConvert.SerializeObject(EnyResponse));
 
                    }

                    var errorResponse = new
                    {
                        status = false,
                        message = "No Data Found",
                        data = (object)null
                    };
                    return BadRequest(JsonConvert.SerializeObject(errorResponse));

                    //return Ok(JsonConvert.SerializeObject(response));


                }


            }
            catch (Exception ex)
            {
                _exceptionHandling.LogError(ex);
                var errorResponse = new
                {
                    status = false,
                    message = "No Data Found",
                    data = (object)null
                };
                return BadRequest(JsonConvert.SerializeObject(errorResponse));
            }
        }
         
        private IEnumerable<LanDetails> GetLanStatusDetails(string Lan)
        {
            try
            {

                using (var connection = new SqlConnection(_connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Lan_no", Lan);

                    return SqlMapper.Query<LanDetails>(connection, "GetLanStatusDetails", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
            
            }
            catch (Exception ex)
            {
                _exceptionHandling.LogError(ex);
                throw new Exception("Error fetching LAN status details", ex);
            }
        }
         
    }
}
