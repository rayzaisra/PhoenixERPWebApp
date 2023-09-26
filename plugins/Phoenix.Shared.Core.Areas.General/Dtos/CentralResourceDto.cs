using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class CentralResourceDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string business_unit_id { get; set; }
        [StringLength(200)]
        public string name { get; set; }
        public bool is_channel { get; set; }
        public string business_unit_name { get; set; }
    }
    public class CentralResourceNew
    {
        [StringLength(50)]
        public string business_unit_id { get; set; }
        public bool is_channel { get; set; }

        [StringLength(200)]
        public string name { get; set; }
    }            
    public class CentralResourceSearch : SearchParameter
    {
        public string Search_business_unit_id { get; set; }

    }
}