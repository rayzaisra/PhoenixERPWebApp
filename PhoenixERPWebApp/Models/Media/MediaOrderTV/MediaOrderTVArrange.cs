using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media
{
    public class MediaOrderTVArrange : MediaEntityBase
    {
        public string Id { get; set; }
        [DisplayName("MO Number")]
        public string MediaOrderNo { get; set; }
        [DisplayName("Brand")]
        public string BrandId { get; set; }
        [DisplayName("Client")]
        public string ClientId { get; set; }
        [DisplayName("Revision Number")]
        public string RevisionNo { get; set; }
        [DisplayName("Target Audience")]
        public string TargetAudience { get; set; }
        [DisplayName("Media")]
        public string TmMediaTypeId { get; set; }
        [DisplayName("Media")]
        public string TmMediaOrderStatusId { get; set; }
    }
}
