using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BaseApi.Domain.Entity.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BaseApi.Security.Token
{
    public class TokenHelper : ITokenHelper
    {
        private readonly TokenOptions _tokenOptions;

        public TokenHelper(IOptions<TokenOptions> tokenOptions)
        {
            _tokenOptions = tokenOptions.Value;
        }

        /// <summary>
        /// Get <see cref="TokenOptions"/>
        /// </summary>
        /// <returns>Token options.</returns>
        public TokenOptions GetTokenOptions()
        {
            return _tokenOptions;
        }

        /// <summary>
        /// Creates an <see cref="AccessToken"/> that includes <see cref="JwtSecurityToken"/>
        /// based on the security key, signing credentials and user claims and the expiration date.
        /// </summary>
        /// <param name="user"> for token payload data.</param>
        /// <returns><see cref="AccessToken"/> that includes access and refresh token and expiration date</returns>
        public AccessToken CreateAccessToken(User user)
        {
            DateTime accessTokenExpiration = DateTime.Now.AddDays(_tokenOptions.AccessTokenExpiration);
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now, //start using token when initilize
                claims: GetClaims(user),
                signingCredentials: signingCredentials
            );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string token = handler.WriteToken(jwtSecurityToken);

            AccessToken accessToken = new AccessToken
            {
                Token = token,
                Expration = accessTokenExpiration,
                RefreshToken = CreateRefreshToken()
            };

            return accessToken;

        }

        /// <summary>
        /// Returns claim list about the user. For security do not return email,
        /// full name etc. if not necessary.
        /// </summary>
        /// <param name="user">The user that will contain claim values.</param>
        /// <returns>Claim list that includes key and value pairs.</returns>
        private IEnumerable<Claim> GetClaims(User user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.ID.ToString()),
                //new Claim(ClaimTypes.Email,user.Email), 
                //new Claim(ClaimTypes.Name,user.FullName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
        }

        /// <summary>
        /// Creates a random base64 string that contains alphanumeric characters.
        /// </summary>
        /// <returns>Base64 string.</returns>
        private string CreateRefreshToken()
        {
            byte[] numberBytes = new byte[32];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(numberBytes);
                return Convert.ToBase64String(numberBytes);
            }
        }

    }
}
