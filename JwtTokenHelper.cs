using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SMFG_API_New
{
    public class JwtTokenHelper
    {
        private const string SecretKey = "+4P0HQfbB3gQrtslmN5/PcU1AbqVx8rMjvgHf6/QE3k="; //GenerateSecureKey(); //GenerateSecureKey(); //"YourSuperSecretKeyHere"; // Replace with a secure key
        private const int TokenValidityInMinutes = 60;

        public static string GenerateToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, userId)
                }),
                Expires = DateTime.UtcNow.AddMinutes(TokenValidityInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public static string GenerateSecureKey()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[32]; // 256 bits
                rng.GetBytes(key);
                return Convert.ToBase64String(key); // Use Base64 for storing keys in a readable format
            }
        }


    }
}
