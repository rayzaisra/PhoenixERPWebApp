using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class SubgroupDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(100)]
        public string group_id { get; set; }
        [StringLength(50)]
        public string subgroup_code { get; set; }
        [StringLength(100)]
        public string subgroup_name { get; set; }
    }
    public class SubgroupNew
    {
        [StringLength(100)]
        public string group_id { get; set; }
        [StringLength(50)]
        public string subgroup_code { get; set; }
        [StringLength(100)]
        public string subgroup_name { get; set; }
    }            
    public class SubgroupSearch : SearchParameter
    {
        public string Search_group_id { get; set; }
        public string Search_subgroup_code { get; set; }
        public string Search_subgroup_name { get; set; }
        
    }
}