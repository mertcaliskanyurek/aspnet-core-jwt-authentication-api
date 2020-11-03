using System;
using System.Threading.Tasks;
using BaseApi.Domain.Response;
using BaseApi.Security.Token;

namespace BaseApi.Domain.Services
{
    public interface IAuthenticationService
    {
        Task<ObjectResponse<AccessToken>> CreateTokenAsync(string email, string password);
        Task<ObjectResponse<AccessToken>> CreateTokenAsync(string refreshToken);
        Task<ObjectResponse<AccessToken>> RevokeRefreshTokenAsync(string refreshToken);
    }
}
