using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTeacher.Helper.Responses
{
    public class BaseResponse<T>
    {
        public BaseResponse()
        {
            if (typeof(T).IsClass && typeof(T) != typeof(string)) SuccessMessage = Activator.CreateInstance<T>();
            else SuccessMessage = default;
        }
        private bool isSuccessful = true;
        public bool IsSuccessful
        {
            get => isSuccessful;
            set
            {
                isSuccessful = value;
                SuccessMessage = default; //Burası sadece `false` aldığında set edileceği için `if(value == false) kontrolü koymaya gerek yoktur.`
            }
        }
        private T? successMessage;
        public T? SuccessMessage
        {
            get => successMessage;
            set
            {
                successMessage = value;
            }
        }
        private string? errorMessage = string.Empty;
        public string? ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
            }
        }
        private string traceId = string.Empty;
        public string TraceId
        {
            get => traceId;
            set
            {
                traceId = value;
            }
        }
        private int serverTime = (int)(DateTime.UtcNow.Subtract(DateTime.UnixEpoch)).TotalSeconds;
        public int ServerTime
        {
            get => serverTime;
            set
            {
                serverTime = value;
            }
        }
    }
}
