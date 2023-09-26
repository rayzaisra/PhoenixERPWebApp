using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class MasterTaxDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string bpjs_tk { get; set; }
        [StringLength(50)]
        public string bruto_calculate { get; set; }
        [StringLength(50)]
        [Required]
        public string calculatetax { get; set; }
        [StringLength(50)]
        [Required]
        public string code { get; set; }
        [StringLength(50)]
        public string percent_value { get; set; }
        [StringLength(50)]
        [Required]
        public string tax_name { get; set; }
        [StringLength(50)]
        public string tax_no { get; set; }
        [StringLength(50)]
        [Required]
        public string type { get; set; }
        [StringLength(50)]
        [Required]
        public string value { get; set; }
    }
    public class MasterTaxNew
    {
        [StringLength(50)]
        [Required]
        public string bpjs_tk { get; set; }
        [StringLength(50)]
        public string bruto_calculate { get; set; }
        [StringLength(50)]
        [Required]
        public string calculatetax { get; set; }
        [StringLength(50)]
        [Required]
        public string code { get; set; }
        [StringLength(50)]
        public string percent_value { get; set; }
        [StringLength(50)]
        [Required]
        public string tax_name { get; set; }
        [StringLength(50)]
        public string tax_no { get; set; }
        [StringLength(50)]
        [Required]
        public string type { get; set; }
        [StringLength(50)]
        [Required]
        public string value { get; set; }
    }            
    public class MasterTaxSearch : SearchParameter
    {
        public string Search_bpjs_tk { get; set; }
        public string Search_bruto_calculate { get; set; }
        public string Search_calculatetax { get; set; }
        public string Search_code { get; set; }
        public string Search_percent_value { get; set; }
        public string Search_tax_name { get; set; }
        public string Search_tax_no { get; set; }
        public string Search_type { get; set; }
        public string Search_value { get; set; }
        
    }
}