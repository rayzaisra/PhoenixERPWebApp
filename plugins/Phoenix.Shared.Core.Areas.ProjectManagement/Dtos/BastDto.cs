using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class BastDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string bast_number { get; set; }
        [StringLength(50)]
        public string external_id { get; set; }
        [StringLength(50)]
        public string po_id { get; set; }
    }
    public class BastNew
    {
        [StringLength(50)]
        public string bast_number { get; set; }
        [StringLength(50)]
        public string external_id { get; set; }
        [StringLength(50)]
        public string po_id { get; set; }
    }            
    public class BastSearch : SearchParameter
    {
        public string Search_bast_number { get; set; }
        public string Search_external_id { get; set; }
        public string Search_po_id { get; set; }
        
    }
}