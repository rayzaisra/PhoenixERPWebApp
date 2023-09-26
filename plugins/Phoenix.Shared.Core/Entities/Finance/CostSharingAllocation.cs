using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("cost_sharing_allocation",Schema ="fn")]
    public partial class CostSharingAllocation : GenericEntity<string>
    {
        public string cost_sharing_allocation_name { get; set; }
        public DateTime? end_date { get; set; }
        public string organization_id { get; set; }
        public DateTime? start_date { get; set; }
    }
}