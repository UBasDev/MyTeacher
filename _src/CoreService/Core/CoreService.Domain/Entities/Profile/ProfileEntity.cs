using CoreService.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.Entities.Profile
{
    public class ProfileEntity : BaseEntity
    {
        public ProfileEntity(UInt16 age)
        {
            Age = age;
        }
        public UInt16 Age { get; set; }
    }
}
