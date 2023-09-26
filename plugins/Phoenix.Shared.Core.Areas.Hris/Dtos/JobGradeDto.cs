using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class JobGradeDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string grade_code { get; set; }
        [StringLength(100)]
        public string grade_name { get; set; }
        [StringLength(100)]
        public string joblevelid { get; set; }
        public DateTime? created_on { get; set; }
    }
    public class JobGradeNew
    {
        [StringLength(50)]
        public string grade_code { get; set; }
        [StringLength(100)]
        public string grade_name { get; set; }
        [StringLength(100)]
        public string joblevelid { get; set; }
    }            
    public class JobGradeSearch : SearchParameter
    {
        public string Search_grade_code { get; set; }
        public string Search_grade_name { get; set; }
        public string Search_joblevelid { get; set; }
        
    }
}