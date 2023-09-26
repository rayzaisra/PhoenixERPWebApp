using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employee_training_requisition",Schema ="hr")]
    public partial class EmployeeTrainingRequisition : GenericEntity<string>
    {
        public string employee_basic_info_id { get; set; }
    }
}