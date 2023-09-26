using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class AchievementDto
    {
        [Required]
            public string Id { get; set; }
        public DateTime? achievement_date { get; set; }
        [StringLength(2147483647)]
        public string achievement_description { get; set; }
        [StringLength(200)]
        public string achievement_title { get; set; }
        public long? employee_basic_info_id { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
    }
    public class AchievementNew
    {
        public DateTime? achievement_date { get; set; }
        [StringLength(2147483647)]
        public string achievement_description { get; set; }
        [StringLength(200)]
        public string achievement_title { get; set; }
        public long? employee_basic_info_id { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
    }            
    public class AchievementSearch : SearchParameter
    {
        public DateTime? Search_achievement_date { get; set; }
        public string Search_achievement_description { get; set; }
        public string Search_achievement_title { get; set; }
        public long? Search_employee_basic_info_id { get; set; }
        public string Search_notes { get; set; }
        
    }
}