using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Core.CustomEntities
{
    public class Meta
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public int? PreviousPageNumber { get; set; }
        public int? NextPageNumber { get; set; }
        public string NextPageUrl { get; set; }
        public string PreviousPageUrl { get; set; }

    }
}
