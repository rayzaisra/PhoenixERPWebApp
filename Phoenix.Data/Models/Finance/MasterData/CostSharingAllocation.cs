using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("cost_sharing_allocation", Schema = "fn")]
    public class CostSharingAllocation : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(400)]
        [Column("cost_sharing_allocation_name"), StringLength(400)]
        public string CostSharingAllocationName { get; set; }
        
        [MaxLength(50)]
        [Column("coa_id"), StringLength(50)]
        public string CoaId { get; set; }

        [Column("total_cost_allocation_sharing")]
        public decimal? TotalCostAllocationSharing { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [Column("is_monthly")]
        public bool? IsMonthly { get; set; }

        [NotMapped]
        public string CodeRec { get; set; }

        [NotMapped]
        public string CoaName { get; set; }

        [NotMapped]
        public List<CostSharingAllocationDetail> CostSharingAllocationDetails { get; set; }
    }
}