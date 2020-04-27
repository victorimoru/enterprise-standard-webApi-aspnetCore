using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Shared.Infrastructure.Entities
{
    public class User
    {
        public User()
        {
            PhotoSet = new Collection<Photo>();
        }
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public Gender Gender { get; set; }
        public DateTime Created { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> PhotoSet { get; set; } = new HashSet<Photo>();



    }

}
