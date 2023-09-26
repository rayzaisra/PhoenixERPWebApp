using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("cost_sharing_allocation_detail", Schema = "fn")]
    public class CostSharingAllocationDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("cost_sharing_allocation_id"), StringLength(50)]
        public string CostSharingAllocationId { get; set; }

        //[Required]
        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [Column("cost_sharing_allocation_percent")]
        public decimal? CostSharingAllocationPercent { get; set; }

        [Column("monthly_description")]
        public string MonthlyDescription { get; set; }
        

        [NotMapped]
        public string BusinessUnitName { get; set; }
    }
}