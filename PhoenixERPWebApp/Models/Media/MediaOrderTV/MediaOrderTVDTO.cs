using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media
{
    public class MediaOrderTVDTO
    {
        public MediaOrderTVDetail Header { get; set; }
        public List<MediaOrderTVDetailList> Details { get; set; }
    }
}
