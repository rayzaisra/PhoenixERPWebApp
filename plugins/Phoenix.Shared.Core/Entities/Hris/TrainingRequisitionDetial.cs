using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("training_requisition_detail", Schema ="hr")]
    public partial class TrainingRequisitionDetial : GenericEntity<string>
    {
        public string training_requition_id { get; set; }
        public string employee_basic_info_id { get; set; }
       
    }
}