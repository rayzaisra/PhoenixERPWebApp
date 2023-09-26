using Phoenix.WebExtension.RestApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.WebExtension.Extentions
{
    public static class WorkingDays
    {
        public static async Task<int> GetWorkingDays(ApiClientFactory client, DateTime from, DateTime to)
        {
            var totalDays = 0;
            var holidays = await client.Get<List<Holiday>>($"{ApiUrl.HolidayUrl}/{from.ToString("yyyyy-MM-dd")}/{to.ToString("yyyyy-MM-dd")}") ?? new List<Holiday>();
            from = from.Date;
            to = to.Date;
            for (var date = from; date <= to; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday
                    && date.DayOfWeek != DayOfWeek.Sunday
                    && holidays.Where(x => x.Date.Date == date.Date).Count() == 0)
                {
                    totalDays++;
                }
            }

            return totalDays;
        }


        private class Holiday
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public DateTime Date { get; set; }
        }
    }
}

