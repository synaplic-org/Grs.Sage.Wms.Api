using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Shared.Wrapper
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }

        public PagedResponse(bool succeeded, List<T> data=default, List<string> messages = null, int count = 0, int pageNumber=1, int pageSize=10)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalRecords = count;
            this.Data = data;
            this.Message = null;
            this.Succeeded = succeeded;
            this.Errors = null;
        }

          
}
}
