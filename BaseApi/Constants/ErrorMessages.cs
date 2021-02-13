using System;
namespace BaseApi.Constants
{
    public static class ErrorMessages
    {
        public static class User
        {
            public const string UserNotFound = "User not found.";
            public const string EmailExist = "Email already exist.";
        }

        public static class Auth
        {

            public const string WrongEmail = "Wrong email.";
            public const string WrongPassword = "Wrong password.";
            public const string RefreshTokenExpired = "Refresh Token Expired.";
            public const string RefreshTokenNotFound = "Refresh Token Not Found.";
            public const string AuthProblem = "You are not allowed to do this.";
        }
    }
}
