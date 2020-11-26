namespace Phoenixnet.Extensions.Object
{
    public struct BasicResponse
    {
        public int code { get; set; }
        public string desc { get; set; }
    }

    public struct BasicResponse<T>
    {
        public int code { get; set; }
        public string desc { get; set; }
        public T data { get; set; }
    }

    public struct PagingResponse<T>
    {
        public int code { get; set; }
        public string desc { get; set; }
        public T data { get; set; }
        public Paging paging { get; set; }
    }
}