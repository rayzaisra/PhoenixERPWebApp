using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class ChannelDto
    {
        [Required]
            public string Id { get; set; }
        public long? central_resource_id { get; set; }
        [StringLength(250)]
        public string channel { get; set; }
        [StringLength(250)]
        public string note { get; set; }
        public double? value { get; set; }
    }
    public class ChannelNew
    {
        public long? central_resource_id { get; set; }
        [StringLength(250)]
        public string channel { get; set; }
        [StringLength(250)]
        public string note { get; set; }
        public double? value { get; set; }
    }            
    public class ChannelSearch : SearchParameter
    {
        public long? Search_central_resource_id { get; set; }
        public string Search_channel { get; set; }
        public string Search_note { get; set; }
        public double? Search_value { get; set; }
        
    }
}