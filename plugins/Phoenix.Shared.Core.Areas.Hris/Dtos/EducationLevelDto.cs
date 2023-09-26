using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EducationLevelDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(100)]
        public string education_name { get; set; }
        public int education_seq { get; set; }
    }
    public class EducationLevelNew
    {
        [StringLength(100)]
        public string education_name { get; set; }
        public int education_seq { get; set; }
    }            
    public class EducationLevelSearch : SearchParameter
    {
        public string Search_education_name { get; set; }
        public string search_employee_basic_info_id { get; set; }
        public string Search_education_id { get; set; }

    }
}