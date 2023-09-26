using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class PaDetailDto
    {

        [Required]
        public string task_id { get; set; }
        public string sub_task_id { get; set; }
        [Required]
        public string sub_task { get; set; }
        public string category { get; set; }
        public string sub_brand { get; set; }
        public decimal? price_per_quantity { get; set; }
        public decimal? quantity { get; set; }
        public string remarks { get; set; }
        public decimal? ppn { get; set; }
        public decimal? asf { get; set; }
        public decimal? pph { get; set; }
        public string uom { get; set; }
        public string brand { get; set; }
        public string category_name { get; set; }
    }
    public class PaDetailNew
    {
        public string task_id { get; set; }
        public string sub_task_id { get; set; }
        public string category { get; set; }
        public string sub_brand { get; set; }
        public float price_per_quantity { get; set; }
        public float quantity { get; set; }
        public string remarks { get; set; }
        public float ppn { get; set; }
        public float asf { get; set; }
        public float pph { get; set; }
        public string uom { get; set; }
        public string brand { get; set; }
        public string category_name { get; set; }
    }            
    public class PaDetailSearch : SearchParameter
    {
        
    }
}