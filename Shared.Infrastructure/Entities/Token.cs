using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure.Entities
{
   public class Token
    {
        public int TokenId { get; set; }
        public string AccessToken { get; set; }
        public DateTime  AccessTokenExpiry { get; set; }
        public string refreshToken { get; set; }
        public DateTime refreshTokenExpiry { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

    }
}
