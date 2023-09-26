using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("pca",Schema ="pm")]
    public partial class Pca : GenericEntity<string>
    {
        public string description { get; set; }
        public string code { get; set; }
        public string type_of_expense_id { get; set; }
        public string job_id { get; set; }
        public string detail { get; set; }
        public string name { get; set; }
        public string revision { get; set; }
        public decimal? total { get; set; }
        public string currency { get; set; }
        public string rate_card_id { get; set; }
        public string mother_pca_id { get; set; }
        public decimal? subtotal { get; set; }
        public decimal? other_fee { get; set; }
        public string other_fee_name { get; set; }
        public decimal? other_fee_percentage { get; set; }
        public decimal? vat { get; set; }
        public string status { get; set; }
        public string shareservices_id { get; set; }
        public string mainservice_category_id { get; set; }
        public decimal? termofpayment1 { get; set; }
        public decimal? termofpayment2 { get; set; }
        public decimal? termofpayment3 { get; set; }
    }
}