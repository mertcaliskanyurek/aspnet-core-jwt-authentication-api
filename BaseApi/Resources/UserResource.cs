using System;
using System.ComponentModel.DataAnnotations;

namespace BaseApi.Resources
{
    public class UserResource
    {
        [Required][EmailAddress] public string Email { get; set; }
        [Required] public string PassWord { get; set; }
        public string FullName { get; set; }
    }
}
