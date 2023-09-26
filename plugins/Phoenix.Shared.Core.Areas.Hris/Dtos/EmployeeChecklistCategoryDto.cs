using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployeeChecklistCategoryDto
    {
        [Required]
            public string Id { get; set; }
        public string checklist_category {get;set;}
        public DateTime? created_on { get; set; }
    }
    public class EmployeeChecklistCategoryNew
    {
		public string Id { get; set; }
       public string checklist_category {get;set;}
        public DateTime? created_on { get; set; }
    }            
    public class SearchEmployeeChecklistCategory : SearchParameter
    {
        
        
    }
}