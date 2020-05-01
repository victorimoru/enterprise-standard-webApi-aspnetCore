using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.DTOs
{
    public class UserDetailsDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public string Created { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
       // public string LookingFor { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
      //  public string photoUrl { get; set; }
        public List<PhotoDetailDto> Photos { get; set; }

    }
}
