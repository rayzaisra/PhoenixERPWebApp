using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class ClientBriefContentRefDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        [StringLength(50)]
        public string name { get; set; }
        public int? seq { get; set; }
    }
    public class ClientBriefContentRefNew
    {
        [StringLength(255)]
        public string description { get; set; }
        [StringLength(50)]
        public string name { get; set; }
        public int? seq { get; set; }
    }            
    public class ClientBriefContentRefSearch : SearchParameter
    {
        public string Search_description { get; set; }
        public string Search_name { get; set; }
        public int? Search_seq { get; set; }
        
    }
}