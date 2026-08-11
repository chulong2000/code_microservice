using GHM.Infrastructure.Extensions;

namespace GHM.Infrastructure.Models
{
    public class ActionResultResponse
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public long Code { get; set; }

        public ActionResultResponse() { }

        public ActionResultResponse(long code, string message = "", string title = "")
        {
            Code = code;
            Title = title;
            Message = message.UpperCaseFirst();
        }
    }

    public class ActionResultResponse<T> : ActionResultResponse
    {
        public T Data { get; set; }

        public ActionResultResponse()
        {
            Code = 1;
        }

        public ActionResultResponse(long code, string message = "", string title = "", T data = default)
        {
            Code = code;
            Title = title;
            Message = message.UpperCaseFirst();
            Data = data;
        }

        public ActionResultResponse(T data)
        {
            Code = 1;
            Data = data;
        }
    }
}
