using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.DTOs
{
    public class RefreshTokenServiceResponse :AuthenticationResponse
    {
        public string ErrorMsg { get; set; }
       
    }
}
