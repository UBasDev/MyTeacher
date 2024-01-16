using CoreService.Domain.Entities.Common;
using CoreService.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.Entities.Profile
{
    public class ProfileEntity : BaseEntity<Guid>
    {
        private ProfileEntity(UInt16 age)
        {
            Age = age;
        }
        public UInt16 Age { get; set; }
        public static ProfileEntity CreateNewProfile(UInt16 age)
        {
            return new ProfileEntity(age);
        }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
