using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("training_share_session", Schema ="hr")]
    public partial class TrainingShareSession : GenericEntity<string>
    {
        public string training_id { get; set; }
        public string training_reg_payment_id { get; set; }
        public string employee_basic_info_id { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string note { get; set; }
        public string training_requisition_id { get; set; }

    }
}