using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("company",Schema ="fn")]
    public partial class Company : GenericEntity<string>
    {
        public string company_name { get; set; }
        public string email { get; set; }
        public string facebook { get; set; }
        public string fax { get; set; }
        public string instagram { get; set; }
        public bool is_client { get; set; }
        public bool is_vendor { get; set; }
        public string mobile_phone { get; set; }
        public string organization_id { get; set; }
        public int? payment_term_day { get; set; }
        public string phone { get; set; }
        public string registration_code { get; set; }
        public int? shipping_term_day { get; set; }
        public string tax_number { get; set; }
        public string twitter { get; set; }
        public string website { get; set; }
    }
}