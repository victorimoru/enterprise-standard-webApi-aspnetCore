using Shared.Infrastructure.Entities;
using Shared.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure.PagingHelper
{
    public class UserQueryParameters : QueryStringParameters, ISortCriteria
    {
        public string OrderBy { get; set; }
        public int Id { get; set; }
        public string gender { get; set; }
        public int minAge { get; set; } = 18;
        public int maxAge { get; set; } = 99;
        public string loggedInUserGender { get; set; }
        public bool IsSortAscending { get; set; }
        public string SortBy { get; set; }

        public UserQueryParameters()
        {
            IsSortAscending = false;
            
        }
    }
    
}
