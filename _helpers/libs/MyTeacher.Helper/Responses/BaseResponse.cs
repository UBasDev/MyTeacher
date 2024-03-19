using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyTeacher.Helper.Responses
{
    public class BaseResponse<T>
    {
        public BaseResponse()
        {
            if (typeof(T).IsClass && typeof(T) != typeof(string)) Payload = Activator.CreateInstance<T>();
            else Payload = default;
            IsSuccessful = true;
            ErrorMessage = null;
            TraceId = string.Empty;
            ServerTime = (int)(DateTime.UtcNow.Subtract(DateTime.UnixEpoch)).TotalSeconds;
            StatusCode = HttpStatusCode.OK;
        }
        public T? Payload { get; set; }
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public string TraceId { get; set; }
        public int ServerTime { get; }
        public HttpStatusCode StatusCode { get; set; }
    }
    public class BaseResponse //If we won't send anything to client
    {
        public BaseResponse()
        {
            Payload = null;
            IsSuccessful = true;
            ErrorMessage = null;
            TraceId = string.Empty;
            ServerTime = (int)(DateTime.UtcNow.Subtract(DateTime.UnixEpoch)).TotalSeconds;
            StatusCode = HttpStatusCode.OK;
        }
        public object? Payload { get; }
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public string TraceId { get; set; }
        public int ServerTime { get; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
