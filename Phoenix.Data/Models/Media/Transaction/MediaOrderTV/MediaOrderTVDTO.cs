using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models.Media
{
    public class MediaOrderTVDTO
    {
        public TrMediaOrderTV Header { get; set; }
        public List<TrMediaOrderTVDetail> Details { get; set; }
        public List<List<TrMediaOrderTVDetailSchedule>> Schedules { get; set; }
    }
}
