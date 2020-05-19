using Shared.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure.PagingHelper
{
    public class UserQueryParameters : QueryStringParameters
    {
        public string OrderBy { get; set; }
        public int Id { get; set; }
        public string gender { get; set; }
        public int minAge { get; set; } = 18;
        public int maxAge { get; set; } = 99;
        public string loggedInUserGender { get; set; }


        public UserQueryParameters()
        { 
            OrderBy = "username";
            
        }
    }
    
}
