using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTeacher.Helper.Responses
{
    public class BaseErrorResponse
    {
        public BaseErrorResponse() { }
        private BaseErrorResponse(string errorMessage, string traceId)
        {
            ErrorMessage = errorMessage;
            TraceId = traceId;
        }
        [JsonProperty]
        private bool IsSuccessful { get; set; } = false;
        [JsonProperty]
        private string? SuccessMessage { get; set; } = null;
        [JsonProperty]
        private string ErrorMessage { get; set; } = string.Empty;
        [JsonProperty]
        private string TraceId { get; set; } = string.Empty;
        [JsonProperty]
        private int ServerTime { get; set; } = (int)(DateTime.UtcNow.Subtract(DateTime.UnixEpoch)).TotalSeconds;
        public static BaseErrorResponse BuildBaseErrorResponse(string errorMessage, string traceId)
        {
            return new BaseErrorResponse(errorMessage, traceId);
        }
    }
}
