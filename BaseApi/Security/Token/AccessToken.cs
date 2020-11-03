using System;
namespace BaseApi.Security.Token
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime Expration { get; set; }
        public string RefreshToken { get; set; }

    }
}
