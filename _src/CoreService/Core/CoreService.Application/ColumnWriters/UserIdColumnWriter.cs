using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.ColumnWriters
{
    public class UserIdColumnWriter : ColumnWriterBase
    {
        public UserIdColumnWriter() : base(NpgsqlDbType.Varchar)
        {

        }
        public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
        {
            var (userId, value) = logEvent.Properties.FirstOrDefault(p => p.Key == "user_id"); //Contexte pushladığımız `user_id` propertysinin valuesini çekeceğiz ve databasedeki serilog tablosuna basacağız.
            if (value != null) return value.ToString()[1..^1];
            return null;
        }
    }
}
