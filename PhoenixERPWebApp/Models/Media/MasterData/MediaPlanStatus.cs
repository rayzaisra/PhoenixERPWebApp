using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media.MasterData
{
    public class MediaPlanStatus : MediaEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }
        [DisplayName("Media Plan Status Name")]
        public string MediaPlanStatusName { get; set; }
    }
}
