using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class StatusTaxDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string master_tax_id { get; set; }
        [StringLength(50)]
        public string tax_status { get; set; }
    }
    public class StatusTaxNew
    {
        [StringLength(50)]
        public string master_tax_id { get; set; }
        [StringLength(50)]
        public string tax_status { get; set; }
    }            
    public class StatusTaxSearch : SearchParameter
    {
        public string Search_master_tax_id { get; set; }
        public string Search_tax_status { get; set; }
        
    }
}