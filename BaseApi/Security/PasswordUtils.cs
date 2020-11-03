using System;
using System.Text;

namespace BaseApi.Security
{
    public static class PasswordUtils
    {
        /// <summary>
        /// Generates MD5 hash string from an input string.
        /// </summary>
        /// <param name="input">A string that includes ASCII characters.</param>
        /// <returns>Hashed MD5 string.</returns>
        public static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
