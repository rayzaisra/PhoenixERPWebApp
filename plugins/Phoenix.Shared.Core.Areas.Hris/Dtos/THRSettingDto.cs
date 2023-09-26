using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class THRSettingDto
    {
        [StringLength(50)]
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string month_payroll { get; set; }
        [Required]
        public int year { get; set; }
        [StringLength(50)]
        [Required]
        public string description { get; set; }
       
    }

    public class THRSettingUpdateDto
    {
        [StringLength(50)]
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string month_payroll { get; set; }
        [Required]
        public int year { get; set; }
        [StringLength(50)]
        [Required]
        public string description { get; set; }
       
        
    }
    public class THRSettingDtoNew
    {
        [StringLength(50)]
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string month_payroll { get; set; }
        [Required]
        public int year { get; set; }
        [StringLength(50)]
        [Required]
        public string description { get; set; }
        public DateTime created_on { get; set; }
    }
    public class THRSettingSearch : SearchParameter
    {
        public string month_payroll { get; set; }
        public string description { get; set; }
        //public string Search_type { get; set; }

    }
}
