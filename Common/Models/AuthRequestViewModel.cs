using System;
using System.ComponentModel.DataAnnotations;

namespace TinyDemo.Common.Models
{
    public class AuthRequestViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
