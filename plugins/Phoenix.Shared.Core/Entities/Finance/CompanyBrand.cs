using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("company_brand",Schema ="fn")]
    public partial class CompanyBrand : GenericEntity<string>
    {
        public string brand_name { get; set; }
        public string company_id { get; set; }
        public string sub_brand { get; set; }
        public string account_name { get; set; }
        public string division_id { get; set; }
        public string team_id { get; set; }
        public int? asf_value { get; set; }
    }
}