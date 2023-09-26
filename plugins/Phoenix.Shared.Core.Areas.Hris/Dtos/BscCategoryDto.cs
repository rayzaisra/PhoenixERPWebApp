using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class BscCategoryDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(255)]
        public string bsc_category { get; set; }
        [StringLength(255)]
        public string bsc_description { get; set; }
        public int? status_id { get; set; }
    }
    public class BscCategoryNew
    {
        [StringLength(255)]
        public string bsc_category { get; set; }
        [StringLength(255)]
        public string bsc_description { get; set; }
        public int? status_id { get; set; }
    }            
    public class BscCategorySearch : SearchParameter
    {
        public string Search_bsc_category { get; set; }
        public string Search_bsc_description { get; set; }
        public int? Search_status_id { get; set; }
        
    }
}