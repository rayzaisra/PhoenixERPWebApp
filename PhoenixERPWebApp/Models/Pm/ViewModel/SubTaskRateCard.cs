using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoenixERPWebApp.Models
{
    public class SubTaskRateCard
    {
        public string id { get; set; }
        public string brand { get; set; }
        public string category_id { get; set; }
        public string category_name { get; set; }
        public decimal? price { get; set; }
        public string quantity { get; set; }
        public string remarks { get; set; }
        public string subbrand { get; set; }
        public string subtask { get; set; }
        public decimal? total { get; set; }
    }
}
