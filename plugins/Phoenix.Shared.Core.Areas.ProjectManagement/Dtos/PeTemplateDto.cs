using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class PeTemplateDto
    {
        [Required]
            public string Id { get; set; }
        public int? pe_cost_perqty { get; set; }
        public int? pe_item_qty { get; set; }
        public int? pe_line_item { get; set; }
        public int? pe_number { get; set; }
    }
    public class PeTemplateNew
    {
        public int? pe_cost_perqty { get; set; }
        public int? pe_item_qty { get; set; }
        public int? pe_line_item { get; set; }
        public int? pe_number { get; set; }
    }            
    public class PeTemplateSearch : SearchParameter
    {
        public int? Search_pe_cost_perqty { get; set; }
        public int? Search_pe_item_qty { get; set; }
        public int? Search_pe_line_item { get; set; }
        public int? Search_pe_number { get; set; }
        
    }
}