using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public static class ConverterHelper
    {
        public static ValueConverter<bool, int> boolToNumberConverter = new ValueConverter<bool, int>(
            value => value ? 1 : 0,
            value => value == 1);

        public static ValueConverter dateTimeToTimestampConverter = new ValueConverter<DateTime, DateTimeOffset>(
                v => new DateTimeOffset(v, TimeZoneInfo.Local.GetUtcOffset(v)),
                v => v.UtcDateTime);
    }
}
