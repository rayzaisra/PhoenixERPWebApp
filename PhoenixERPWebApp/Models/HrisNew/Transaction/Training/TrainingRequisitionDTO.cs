using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class TrainingRequisitionDTO
    {
        public TrainingRequisition Header { get; set; }
        public List<TrainingRequisitionDetail> Details { get; set; }
    }
}
