using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure.PagingHelper
{
   public abstract class QueryStringParameters
    {
        const int maxPageSize = 19;
        private int pageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        public string OrderBy { get; set; }


        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
