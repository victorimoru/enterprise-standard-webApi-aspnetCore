using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure.PagingHelper
{
    public class PagingMetadata
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
    }
}
