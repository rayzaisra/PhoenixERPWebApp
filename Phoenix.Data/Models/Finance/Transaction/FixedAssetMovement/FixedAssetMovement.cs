using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    [Table("fixed_asset_movement", Schema = "fn")]
    public class FixedAssetMovement : FinanceEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("fixed_asset_id"), StringLength(50)]
        public string FixedAssetId { get; set; }
        [Column("faa_number"), StringLength(50)]
        public string FaaNumber { get; set; }
        [Column("faa_datetime")]
        public DateTime? FaaDateTime { get; set; }
        [Column("from_location_id"), StringLength(50)]
        public string FromLocationID { get; set; }
        [Column("to_location_id"), StringLength(50)]
        public string ToLocationID { get; set; }
        [Column("from_business_unit_id"), StringLength(50)]
        public string FromBusinessUnitID { get; set; }
        [Column("to_business_unit_id"), StringLength(50)]
        public string ToBusinessUnitID { get; set; }
        [Column("execution_date")]
        public DateTime? ExecutionDate { get; set; }
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitID { get; set; }
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityID { get; set; }
        [Column("affiliation_id"), StringLength(50)]
        public string AffiliationID { get; set; }
        [Column("fixed_asset_movement_status_id"), StringLength(50)]
        public string FixedAssetMovementStatusID { get; set; }
        [Column("remark"), StringLength(450)]
        public string Remark { get; set; }
        [Column("approval_id"), StringLength(50)]
        public string ApprovalID { get; set; }
        [Column("condition"), StringLength(50)]
        public string Condition { get; set; }
        [Column("employee"), StringLength(50)]
        public string Employee { get; set; }

        [NotMapped]
        public string LocationName { get; set; }
        [NotMapped]
        public string ConditionName { get; set; }
        [NotMapped]
        public string EmployeeName { get; set; }
        [NotMapped]
        public string Division { get; set; }
        [NotMapped]
        public string LegalEntityName { get; set; }
        [NotMapped]
        public string AffiliationName { get; set; }
        [NotMapped]
        public string StatusName { get; set; }
        [Column("status")]
        public int? Status { get; set; }
        [NotMapped]
        public string FromLocationName { get; set; }
        [NotMapped]
        public string ToLocationName { get; set; }
    }
}
