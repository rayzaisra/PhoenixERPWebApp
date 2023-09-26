using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class ClientBriefContentDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string client_brief_id { get; set; }
        [StringLength(50)]
        public string file { get; set; }
        [StringLength(50)]
        public string name { get; set; }
        public string description { get; set; }
    }
    public class ClientBriefContentNew
    {
        public string id { get; set; }
        [StringLength(50)]
        public string client_brief_id { get; set; }
        [StringLength(50)]
        public string file { get; set; }
        [StringLength(50)]
        public string name { get; set; }
        public string description { get; set; }
    }            
    public class ClientBriefContentSearch : SearchParameter
    {
        public string Search_client_brief_id { get; set; }
        public string Search_file { get; set; }
        public string Search_name { get; set; }
        
    }
}