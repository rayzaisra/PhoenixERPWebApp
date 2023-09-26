using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class DivisionDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string division_code { get; set; }
        [StringLength(100)]
        public string division_name { get; set; }
        [StringLength(100)]
        public string subgroupid { get; set; }
    }
    public class DivisionNew
    {
        [StringLength(50)]
        public string division_code { get; set; }
        [StringLength(100)]
        public string division_name { get; set; }
        [StringLength(100)]
        public string subgroupid { get; set; }
    }            
    public class DivisionSearch : SearchParameter
    {
        public string Search_division_code { get; set; }
        public string Search_division_name { get; set; }
        public string Search_subgroupid { get; set; }
        
    }
}