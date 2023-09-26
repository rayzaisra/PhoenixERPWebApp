using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("business_unit",Schema ="dbo")]
    public partial class BusinessUnit : GenericEntity<string>
    {
        public string business_unit_type_id { get; set; }
        public string default_team { get; set; }
        public string organization_id { get; set; }
        public string parent_unit { get; set; }
        public string unit_address { get; set; }
        public string unit_code { get; set; }
        public string unit_description { get; set; }
        public string unit_email { get; set; }
        public string unit_fax { get; set; }
        public string unit_name { get; set; }
        public string unit_phone { get; set; }

        public string affiliation_id { get; set; }

        public bool?  isfinance { get; set; }
        public bool? ishris { get; set; }
    }
}