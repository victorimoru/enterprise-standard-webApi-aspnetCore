using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure.PagingHelper
{
    public class UserQueryParameters : QueryStringParameters
    {
        public UserQueryParameters()
        { 
            OrderBy = "username";
            
        }
    }
    
}
