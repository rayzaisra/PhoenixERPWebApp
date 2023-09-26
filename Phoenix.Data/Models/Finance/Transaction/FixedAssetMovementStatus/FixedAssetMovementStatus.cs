using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    [Table("fixed_asset_movement_status", Schema = "fn")]
    public class FixedAssetMovementStatus : FinanceEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("organization_id"), StringLength(50)]
        public string OrganizationId { get; set; }
        [Column("status_name"), StringLength(50)]
        public string StatusName { get; set; }
    }
}
