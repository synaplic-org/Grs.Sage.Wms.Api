using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Shared.Wrapper
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(List<T> data)
        {
            Succeeded = true;
            Message = null;
            Errors = null;
            Data = data;
        }
        public List<T> Data { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public List<string> Message { get; set; }
    }
}
