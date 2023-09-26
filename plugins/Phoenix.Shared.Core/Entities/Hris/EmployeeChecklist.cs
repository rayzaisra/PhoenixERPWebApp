using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employee_checklist",Schema ="hr")]
    public partial class EmployeeChecklist : GenericEntity<string>
    {
        public string employee_basic_info_id {get;set;}
        public string employee_checklist_category_id { get; set; }
        public string checklistitem {get;set;}
		public string description {get;set;}
		public DateTime? whentoalert {get;set;}
    }
}