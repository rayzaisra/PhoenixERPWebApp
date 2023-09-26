using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("brand_detail", Schema = "pm")]
    public partial class BrandDetail : GenericEntity<string>
    {
        public string brand_name { get; set; }
        public string subbrand1 { get; set; }
        public string subbrand2 { get; set; }
        public string account_name { get; set; }
        public string legal_enitity_id { get; set; }
        public string name { get; set; }
        public string division_id { get; set; }
    }
}
