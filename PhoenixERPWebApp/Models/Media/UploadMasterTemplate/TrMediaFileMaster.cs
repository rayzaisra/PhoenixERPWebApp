using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media.UploadMasterTemplate
{
    public class TrMediaFileMaster : MediaEntityBase
    {
        public string Id { get; set; }
        public string FileMasterId { get; set; }
        public string FileMasterName { get; set; }
        public string TrMediaPlanTvId { get; set; }
        public string TmMediaTypeId { get; set; }
        public string TmMediaTypeName { get; set; }
    }
}
