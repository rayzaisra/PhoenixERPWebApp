using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class ApplicationMenuGroupDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string group_name { get; set; }
        [StringLength(50)]
        public string organization_id { get; set; }
    }
    public class ApplicationMenuGroupNew
    {
        [StringLength(50)]
        public string group_name { get; set; }
        [StringLength(50)]
        public string organization_id { get; set; }
    }            
    public class ApplicationMenuGroupSearch : SearchParameter
    {
        public string Search_group_name { get; set; }
        public string Search_organization_id { get; set; }
        
    }
}