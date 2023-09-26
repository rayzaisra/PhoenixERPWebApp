using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media
{
    public class MediaPlanRequestTypeDetail : MediaEntityBase
    {
        public string Id { get; set; }
        [DisplayName("Request Type Name")]
        public string RequestTypeName { get; set; }
    }
}
