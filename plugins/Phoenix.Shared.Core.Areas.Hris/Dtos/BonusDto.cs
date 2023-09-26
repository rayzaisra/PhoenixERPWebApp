using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class BonusDto
    {
        [Required]
            public string Id { get; set; }
        public int? amount { get; set; }
        [StringLength(2147483647)]
        public string bonus_description { get; set; }
        public DateTime? date_bonus { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        [StringLength(200)]
        public string title { get; set; }
    }
    public class BonusNew
    {
        public int? amount { get; set; }
        [StringLength(2147483647)]
        public string bonus_description { get; set; }
        public DateTime? date_bonus { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        [StringLength(200)]
        public string title { get; set; }
    }            
    public class BonusSearch : SearchParameter
    {
        public int? Search_amount { get; set; }
        public string Search_bonus_description { get; set; }
        public DateTime? Search_date_bonus { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public string Search_notes { get; set; }
        public string Search_title { get; set; }
        
    }
}