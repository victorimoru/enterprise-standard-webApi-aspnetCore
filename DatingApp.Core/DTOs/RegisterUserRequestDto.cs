using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatingApp.Core.DTOs
{
    public class RegisterUserRquestDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

    }

    public class UserLoginRquestDto : RegisterUserRquestDto { }

    public class RefreshTokenRequestDto
    {
        
        public string Token { get; set; }

        public string RefreshToken { get; set; }

    }

    public class AuthenticationResponse
    {
        
        public string Token { get; set; }
        public string RefreshToken { get; set; }

    }


}
