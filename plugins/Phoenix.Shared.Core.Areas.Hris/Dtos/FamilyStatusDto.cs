using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class FamilyStatusDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(250)]
        public string family_status_name { get; set; }
    }
    public class FamilyStatusNew
    {
        [StringLength(250)]
        public string family_status_name { get; set; }
    }            
    public class FamilyStatusSearch : SearchParameter
    {
        public string Search_family_status_name { get; set; }
        
    }
}