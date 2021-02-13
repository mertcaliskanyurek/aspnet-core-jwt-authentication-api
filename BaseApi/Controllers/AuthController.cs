using System;
using System.Threading.Tasks;
using BaseApi.Domain.Response;
using BaseApi.Domain.Services;
using BaseApi.Extensions;
using BaseApi.Resources;
using BaseApi.Security;
using BaseApi.Security.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Creates new access and refresh token if user credentials are correct.
        /// </summary>
        /// <param name="loginResouce">An object that includes user credentials.</param>
        /// <returns>HTTP OK response with new <see cref="AccessToken"/> object if user credentials are correct
        ///          Otherwise HTTP BadRequest with string message</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginResource loginResouce)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            string passwordMD5 = PasswordUtils.CreateMD5(loginResouce.PassWord);
            ObjectResponse<AccessToken> accessTokenResponse = await _authenticationService.CreateTokenAsync(loginResouce.Email, passwordMD5);
            if (accessTokenResponse.Success) return Ok(accessTokenResponse.Object);
            return BadRequest(accessTokenResponse.Message);
        }

        /// <summary>
        /// Creates new access token and refresh token. Saves refresh token to database.
        /// </summary>
        /// <param name="refreshToken">User's refresh token.</param>
        /// <returns>HTTP OK response with new <see cref="AccessToken"/> object if user credentials are correct
        ///          Otherwise HTTP BadRequest with string message</returns>
        [HttpPost]
        public async Task<IActionResult> RefreshTokens([FromBody] string refreshToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            ObjectResponse<AccessToken> accessTokenResponse = await _authenticationService.CreateTokenAsync(refreshToken);
            if (accessTokenResponse.Success) return Ok(accessTokenResponse.Object);
            return BadRequest(accessTokenResponse.Message);
        }

        /// <summary>
        /// Deletes refresh token from database. For logout call this function with the refresh token.
        /// </summary>
        /// <param name="refreshToken">User's refresh token.</param>
        /// <returns>HTTP OK response with new <see cref="AccessToken"/> object if user credentials are correct
        ///          Otherwise HTTP BadRequest with string message</returns>
        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] string refreshToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            ObjectResponse<AccessToken> accessTokenResponse = await _authenticationService.RevokeRefreshTokenAsync(refreshToken);
            if (accessTokenResponse.Success) return Ok(accessTokenResponse.Object);
            return BadRequest(accessTokenResponse.Message);
        }
    }
}
