using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class ItemDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string item_category_id { get; set; }
        [StringLength(50)]
        public string item_code { get; set; }
        [StringLength(400)]
        [Required]
        public string item_name { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_id { get; set; }
    }
    public class ItemNew
    {
        [StringLength(50)]
        public string item_category_id { get; set; }
        [StringLength(50)]
        public string item_code { get; set; }
        [StringLength(400)]
        [Required]
        public string item_name { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_id { get; set; }
    }            
    public class ItemSearch : SearchParameter
    {
        public string Search_item_category_id { get; set; }
        public string Search_item_code { get; set; }
        public string Search_item_name { get; set; }
        public string Search_organization_id { get; set; }
        
    }
}