using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Services.Finance.Transaction.Extension
{
    public static class DateTimeExtension
    {
        public static DateTime NextDayOfWeek(this DateTime dt, DayOfWeek dayOfWeek)
        {
            int offsetDays = dayOfWeek - dt.DayOfWeek;
            return dt.AddDays(offsetDays > 0 ? offsetDays : offsetDays + 7);//.AtMidnight();
        }

        public static DateTime PreviousDayOfWeek(this DateTime dt, DayOfWeek dayOfWeek)
        {
            int offsetDays = -(dt.DayOfWeek - dayOfWeek);
            return dt.AddDays(offsetDays);//.AtMidnight();
        }
    }
}
