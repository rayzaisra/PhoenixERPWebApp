using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployeeTrainingRequisitionDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
    }
    public class EmployeeTrainingRequisitionNew
    {
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
    }            
    public class EmployeeTrainingRequisitionSearch : SearchParameter
    {
        public string Search_employee_basic_info_id { get; set; }
        
    }
}