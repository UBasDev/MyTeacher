using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyTeacher.Helper.Responses
{
    public class BaseErrorResponse
    {
        public BaseErrorResponse() {
            Payload = null;
            IsSuccessful = false;
            ErrorMessage = string.Empty;
            TraceId = string.Empty;
            ServerTime = 0;
            StatusCode = 0;
        }
        private BaseErrorResponse(string errorMessage, string traceId, HttpStatusCode statusCode)
        {
            Payload = null;
            IsSuccessful = false;
            ErrorMessage = errorMessage;
            TraceId = traceId;
            ServerTime = (int)(DateTime.UtcNow.Subtract(DateTime.UnixEpoch)).TotalSeconds;
            StatusCode = statusCode;
        }
        [JsonProperty]
        private object? Payload { get; }
        [JsonProperty]
        private bool IsSuccessful { get; }
        [JsonProperty]
        private string ErrorMessage { get; }
        [JsonProperty]
        private string TraceId { get; }
        [JsonProperty]
        private int ServerTime { get; }
        [JsonProperty]
        private HttpStatusCode StatusCode { get; }
        public static BaseErrorResponse BuildBaseErrorResponse(string errorMessage, string traceId, HttpStatusCode statusCode)
        {
            return new BaseErrorResponse(errorMessage, traceId, statusCode);
        }
    }
}
