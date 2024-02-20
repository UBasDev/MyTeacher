﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.Entities.Common
{
    public interface ISoftDelete //While you are implementing this interface, don't forget to set initial value for "IsActive" and "IsDeleted" properties.
    {
        public DateTimeOffset? UpdatedAt { get;  }
        public DateTimeOffset? DeletedAt { get;  }
        public bool IsActive { get; }
        public bool IsDeleted { get; }
    }
}
