using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("cost_sharing_allocation_detail",Schema ="fn")]
    public partial class CostSharingAllocationDetail : GenericEntity<string>
    {
        public string business_unit_id { get; set; }
        public string cost_sharing_allocation_id { get; set; }
        public decimal cost_sharing_allocation_percent { get; set; }
    }
}