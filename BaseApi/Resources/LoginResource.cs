using System;
using System.ComponentModel.DataAnnotations;

namespace BaseApi.Resources
{
    public class LoginResource
    {
        [Required] [EmailAddress] public string Email { get; set; }
        [Required] public string PassWord { get; set; }
    }
}
