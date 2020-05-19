using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure.PagingHelper
{
   public abstract class QueryStringParameters
    {
        const int maxPageSize = 20;
        private int pageSize { get; set; } = 8;
        public int PageNumber { get; set; } = 1;
        



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
