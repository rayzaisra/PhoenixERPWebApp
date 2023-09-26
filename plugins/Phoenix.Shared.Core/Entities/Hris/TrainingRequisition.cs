using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("training_requisition",Schema ="hr")]
    public partial class TrainingRequisition : GenericEntity<string>
    {
        public string training_id { get; set; }
        public string employee_basic_info_id { get; set; }
        public DateTime? purpose_start_date { get; set; }
        public DateTime? purpose_end_date { get; set; }
        public string venue { get; set; }
        public string notes { get; set; }
        public string status { get; set; }
        public string code { get; set; }
        public string add_recomendation { get; set; }
    }
}