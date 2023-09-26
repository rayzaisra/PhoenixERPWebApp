using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
   
    public class TrainingRequisitionDTO
    {
        public TrainingRequisition Header { get; set; }
        public List<TrainingRequisitionDetail> Details { get; set; }
    }
}
