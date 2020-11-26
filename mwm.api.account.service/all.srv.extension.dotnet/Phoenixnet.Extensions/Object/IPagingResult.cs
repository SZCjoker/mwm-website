using System.Collections.Generic;

namespace Phoenixnet.Extensions.Object
{
    public interface IPagingResult<T>
    {
        int code { get; set; }
        string desc { get; set; }
        IEnumerable<T> data { get; set; }
        Paging paging { get; }
    }

    public class ResultPaging<T> : IPagingResult<T>
    {
        public ResultPaging(IEnumerable<T> data, Paging paging, int code, string desc)
        {
            this.paging = paging;
            this.data = data;
            this.code = code;
            this.desc = desc;
        }

        public Paging paging { get; }
        public int code { get; set; }
        public string desc { get; set; }
        public IEnumerable<T> data { get; set; }
    }
}