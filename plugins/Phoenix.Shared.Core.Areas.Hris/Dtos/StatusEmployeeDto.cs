using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class StatusEmployeeDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string compare { get; set; }
        [StringLength(50)]
        public string level_position_id { get; set; }
        [StringLength(50)]
        public string month { get; set; }
        [StringLength(50)]
        [Required]
        public string name { get; set; }
        [StringLength(50)]
        public string ptkp { get; set; }
        [StringLength(50)]
        public string tax { get; set; }
    }
    public class StatusEmployeeNew
    {
        [StringLength(50)]
        public string compare { get; set; }
        [StringLength(50)]
        public string level_position_id { get; set; }
        [StringLength(50)]
        public string month { get; set; }
        [StringLength(50)]
        [Required]
        public string name { get; set; }
        [StringLength(50)]
        public string ptkp { get; set; }
        [StringLength(50)]
        public string tax { get; set; }
    }            
    public class StatusEmployeeSearch : SearchParameter
    {
        public string Search_compare { get; set; }
        public string Search_level_position_id { get; set; }
        public string Search_month { get; set; }
        public string Search_name { get; set; }
        public string Search_ptkp { get; set; }
        public string Search_tax { get; set; }
        
    }
}