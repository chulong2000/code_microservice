namespace GHM.Infrastructure.ViewModels
{
    public class SearchResult<T> where T : class
    {
        public int TotalRows { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }

        public SearchResult()
        {
            Code = 1;
        }

        public SearchResult(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public SearchResult(int totalRows = 0, dynamic data = null)
        {
            Code = 1;
            TotalRows = totalRows;
            Data = data;
        }
    }
}
