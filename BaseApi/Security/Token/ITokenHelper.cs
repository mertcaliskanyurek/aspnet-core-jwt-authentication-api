using System;
using BaseApi.Domain.Entity.Model;

namespace BaseApi.Security.Token
{
    public interface ITokenHelper
    {
        //for login
        AccessToken CreateAccessToken(User user);
        TokenOptions GetTokenOptions();
    }
}
