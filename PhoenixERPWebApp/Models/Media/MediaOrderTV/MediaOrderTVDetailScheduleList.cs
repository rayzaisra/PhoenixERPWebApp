using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media
{
    public class MediaOrderTVDetailScheduleList : MediaEntityBase
    {
        public string Id { get; set; }
        [DisplayName("Media Order Detail")]
        public string TrMediaOrderDetailId { get; set; }
        
        public string Bulan { get; set; }
        
        public string Tanggal { get; set; }
        
        public string Tahun { get; set; }
        [DisplayName("Spot Value")]
        public string SpotValue { get; set; }
        
        public string Reason { get; set; }
    }
}
