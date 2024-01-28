using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTeacher.Helper.Responses
{
    public abstract class BaseResponse<T>
    {
        protected BaseResponse()
        {
            isSuccessful = true;
            if (typeof(T).IsClass && typeof(T) != typeof(string)) SuccessMessage = Activator.CreateInstance<T>();
            else SuccessMessage = default;
            ErrorMessage = null;
            StatusCode = 200;
            ServerTime = (int)(DateTime.UtcNow.Subtract(DateTime.UnixEpoch)).TotalSeconds;
        }
        private bool isSuccessful;
        public bool IsSuccessful
        {
            get => isSuccessful;
            set
            {
                isSuccessful = value;
                SuccessMessage = default;
            }
        }
        public T? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }
        public virtual int StatusCode { get; set; }
        public int ServerTime { get; set; }
    }
}
