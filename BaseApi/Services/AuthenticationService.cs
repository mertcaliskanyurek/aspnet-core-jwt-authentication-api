using System;
using System.Threading.Tasks;
using BaseApi.Domain.Entity.Model;
using BaseApi.Domain.Response;
using BaseApi.Domain.Services;
using BaseApi.Security.Token;

namespace BaseApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IGenericService<User> _userService;
        private readonly ITokenHelper _tokenHelper;
        public AuthenticationService(IGenericService<User> userService,ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        /// <summary>
        /// Creates and saves access and refresh token by email and password related user. 
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password (must be hashed).</param>
        /// <returns><see cref="ObjectResponse{AccessToken}"/> that includes new <see cref="AccessToken"/>
        /// if success, error message otherwise.</returns>
        public async Task<ObjectResponse<AccessToken>> CreateTokenAsync(string email, string password)
        {
            ObjectResponse<User> userResponse = await _userService.FindFirstOrDefault(u => u.Email == email);
            if (userResponse.Success)
            {
                User user = userResponse.Object;
                if (user.PassWord != password) return new ObjectResponse<AccessToken>(Constants.ErrorMessages.Auth.WrongPassword);

                AccessToken token = _tokenHelper.CreateAccessToken(user);
                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenExpirationDate = DateTime.Now.AddDays(_tokenHelper.GetTokenOptions().RefreshTokenExpiration);
                await _userService.UpdateAsync(user);
                return new ObjectResponse<AccessToken>(token);
            }
            
            return new ObjectResponse<AccessToken>(Constants.ErrorMessages.Auth.WrongEmail);
        }

        /// <summary>
        /// Creates new access and refresh token by refresh token related user.
        /// Saves refresh token to database.
        /// </summary>
        /// <param name="refreshToken">Refresh token of the relevant user.</param>
        /// <returns><see cref="ObjectResponse<AccessToken>"/> that includes new <see cref="AccessToken"/>
        /// if success, error message otherwise.</returns>
        public async Task<ObjectResponse<AccessToken>> CreateTokenAsync(string refreshToken)
        {
            ObjectResponse<User> userResponse = await _userService.FindFirstOrDefault(u => u.RefreshToken == refreshToken);
            if (userResponse.Success)
            {
                User user = userResponse.Object;
                if (user.RefreshTokenExpirationDate < DateTime.Now)
                    return new ObjectResponse<AccessToken>(Constants.ErrorMessages.Auth.RefreshTokenExpired);

                AccessToken token = _tokenHelper.CreateAccessToken(user);
                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenExpirationDate = DateTime.Now.AddDays(_tokenHelper.GetTokenOptions().RefreshTokenExpiration);
                await _userService.UpdateAsync(user);
                return new ObjectResponse<AccessToken>(token);
            }
            
            return new ObjectResponse<AccessToken>(Constants.ErrorMessages.Auth.RefreshTokenNotFound);
        }

        /// <summary>
        /// Revokes refresh token from the database of the related user.
        /// </summary>
        /// <param name="refreshToken">Refresh token of the relevant user.</param>
        /// <returns><see cref="ObjectResponse<AccessToken>"/> that includes empty <see cref="AccessToken"/>
        /// if success, error message otherwise.</returns>
        public async Task<ObjectResponse<AccessToken>> RevokeRefreshTokenAsync(string refreshToken)
        {
            ObjectResponse<User> userResponse = await _userService.FindFirstOrDefault(u => u.RefreshToken == refreshToken);
            if (userResponse.Success)
            {
                User user = userResponse.Object;
                user.RefreshToken = null;
                user.RefreshTokenExpirationDate = DateTime.MinValue;
                await _userService.UpdateAsync(user);
                return new ObjectResponse<AccessToken>(new AccessToken());
            } 
            return new ObjectResponse<AccessToken>(Constants.ErrorMessages.Auth.RefreshTokenNotFound);
        }

    }
}
