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
        public T? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; } = null;
        public int ServerTime { get; set; } = (int)(DateTime.UtcNow.Subtract(DateTime.UnixEpoch)).TotalSeconds;
    }
}
