using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployeeChecklistDto
    {
        [Required]
            public string Id { get; set; }
        public string employee_basic_info_id {get;set;}
		public string checklistitem {get;set;}
		public string description {get;set;}
		public DateTime? whentoalert {get;set;}
        public DateTime? created_on { get; set; }
    }
    public class EmployeeChecklistNew
    {
		public string Id { get; set; }
        public string employee_basic_info_id {get;set;}
		public string checklistitem {get;set;}
		public string description {get;set;}
		public DateTime? whentoalert {get;set;}
        public DateTime? created_on { get; set; }
    }            
    public class SearchEmployeeChecklist : SearchParameter
    {
        
        
    }
}