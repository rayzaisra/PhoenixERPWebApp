using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class StatusDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string status_name { get; set; }
        public string description { get; set; }
        public string code { get; set; }
        public int? value { get; set; }
    }
    public class StatusNew
    {
        [StringLength(50)]
        public string status_name { get; set; }
        public int? value { get; set; }
        public string description { get; set; }
        public string code { get; set; }
    }            
    public class StatusSearch : SearchParameter
    {
        public string Search_status_name { get; set; }
        public int? Search_value { get; set; }
        public string description { get; set; }
        public string code { get; set; }
    }
}