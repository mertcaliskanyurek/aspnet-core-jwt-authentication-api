using System;
using System.ComponentModel.DataAnnotations;

namespace BaseApi.Domain.Entity.Model
{
    public class User
    {
        [Key] public long ID { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PassWord { get; set; }
        public DateTime SignDate { get; set; }

        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationDate { get; set; }

        public User()
        {
            SignDate = DateTime.Now;
        }

    }
}
