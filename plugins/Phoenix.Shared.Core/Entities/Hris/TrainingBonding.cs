using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("training_requisition_bonding", Schema ="hr")]
    public partial class TrainingBonding : GenericEntity<string>
    {
        public string training_req_id { get; set; }
        public string employee_basic_info_id { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
    }
}