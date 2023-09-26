using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employee_checklist_category",Schema ="hr")]
    public partial class EmployeeChecklistCategory : GenericEntity<string>
    {
        public string checklist_category {get;set;}
    }
}