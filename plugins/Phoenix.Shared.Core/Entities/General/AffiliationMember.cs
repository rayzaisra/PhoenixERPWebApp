using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("affiliation_member",Schema ="dbo")]
    public partial class AffiliationMember : GenericEntity<string>
    {
        public string affiliation_id { get; set; }
        public string business_unit_id { get; set; }
    }
}