using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class InformationDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string body { get; set; }
        public DateTime? date_information { get; set; }
        public int? flag_active { get; set; }
        [StringLength(50)]
        public string title { get; set; }
    }
    public class InformationNew
    {
        [StringLength(50)]
        public string body { get; set; }
        public DateTime? date_information { get; set; }
        public int? flag_active { get; set; }
        [StringLength(50)]
        public string title { get; set; }
    }            
    public class InformationSearch : SearchParameter
    {
        public string Search_body { get; set; }
        public DateTime? Search_date_information { get; set; }
        public int? Search_flag_active { get; set; }
        public string Search_title { get; set; }
        
    }
}