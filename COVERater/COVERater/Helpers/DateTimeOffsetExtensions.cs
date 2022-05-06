using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Helpers
{
    public static class DateTimeOffsetExtensions
    {
        public static int GetCurrentAge(this DateTime? dateTimeOffset)
        {
            if (dateTimeOffset == null)
                return 99;

            var currentDate = DateTime.UtcNow;
            int age = currentDate.Year - dateTimeOffset.Value.Year;

            if (currentDate < dateTimeOffset.Value.AddYears(age))
            {
                age--;
            }

            return age;
        }
    }
}
