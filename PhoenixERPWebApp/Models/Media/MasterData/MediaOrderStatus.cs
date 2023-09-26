using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media.MasterData
{
    public class MediaOrderStatus : MediaEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }
        [DisplayName("Media Order Status Name")]
        public string MediaOrderStatusName { get; set; }
    }
}
