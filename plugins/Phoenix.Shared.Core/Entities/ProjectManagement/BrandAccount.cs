using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("brand_account",Schema ="pm")]
    public partial class BrandAccount : GenericEntity<string>
    {
        public string user_id { get; set; }
        public string company_brand_id { get; set; }
		public string act_as { get; set; }
    }
}