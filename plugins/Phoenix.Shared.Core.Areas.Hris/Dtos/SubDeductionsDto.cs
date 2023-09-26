using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class SubDeductionsDto
    {
        [StringLength(50)]
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string name { get; set; }
        [Required]
        public int value { get; set; }
        [StringLength(50)]
        [Required]
        public string description { get; set; }
        [StringLength(50)]
        public string salary_component { get; set; }

        public string salary_component_id { get; set; }
        //public DateTime created_on { get; set; }
    }

    public class SubDeductionsUpdateDto
    {
        [StringLength(50)]
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string name { get; set; }
        [Required]
        public int value { get; set; }
        [StringLength(50)]
        [Required]
        public string description { get; set; }
        [Required]
        public string salary_component_id { get; set; }
        //public DateTime created_on { get; set; }
    }
    public class SubDeductionsDtoNew
    {
        [StringLength(50)]
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string name { get; set; }
        [StringLength(50)]
        [Required]
        public string salary_component { get; set; }
        [StringLength(50)]
        [Required]
        public string description { get; set; }
        public DateTime created_on { get; set; }
    }
    public class SubDeductionsSearch : SearchParameter
    {
        public string Search_name { get; set; }
        public string Search_salary_component { get; set; }
        //public string Search_type { get; set; }

    }
}
