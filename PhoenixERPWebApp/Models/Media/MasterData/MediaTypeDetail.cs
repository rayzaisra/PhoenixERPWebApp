using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media
{
    public class MediaTypeDetail : MediaEntityBase
    {
        public string Id { get; set; }
        [DisplayName("Media Type Name")]
        public string MediaTypeName { get; set; }
    }
}
