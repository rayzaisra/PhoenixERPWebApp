using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance.Transaction.FixedAssetOutMovement
{
    [Table("fixed_asset_out", Schema = "fn")]
    public class FixedAssetOutMovement : FinanceEntityBase
    {
        [Key]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("faa_number")]
        public string FaNumber { get; set; }
        [Column("faa_datetime")]
        public DateTime? FaDatetime { get; set; }
        [Column("fixed_asset_out_category_id")]
        public string FixedAssetOutCategoryId { get; set; }
        [Column("location_id")]
        public string LocationId { get; set; }
        [Column("execution_date")]
        public DateTime? ExecutionDate { get; set; }
        [Column("business_unit_id")]
        public string BusinessUnitId { get; set; }
        [Column("legal_entity_id")]
        public string LegalEntityId { get; set; }
        [Column("affiliation_id")]
        public string AffiliationId { get; set; }
        [Column("remark")]
        public string Remark { get; set; }
        [Column("approval_id")]
        public string ApprovalId { get; set; }
        [NotMapped]
        public string ApprovalName { get; set; }
        [NotMapped]
        public string AffiliationName { get; set; }
        [NotMapped]
        public string LegalEntityName { get; set; }
        [NotMapped]
        public string BusinessUnitName { get; set; }
        [NotMapped]
        public string LocationName { get; set; }
        [NotMapped]
        public string FixedAssetOutCategoryName { get; set; }
        [Column("status")]
        public int? Status { get; set; }
        [NotMapped]
        public string StatusName { get; set; }
        [Column("requestor_id")]
        public string RequestorId { get; set; }
        [NotMapped]
        public string RequestorName { get; set; }
        [NotMapped]
        public string DivisionRequestorName { get; set; }
        [Column("division_requestor_id")]
        public string DivisionRequestorId { get; set; }
        [Column("on_behalf_id")]
        public string OnBehalfId { get; set; }
        [NotMapped]
        public string OnBehalfName { get; set; }
        [NotMapped]
        public string DivisionOnBehalfName { get; set; }
        [Column("division_on_behalf_id")]
        public string DivisionOnBehalfId { get; set; }
        [NotMapped]
        public string LogDescription { get; set; }
    }
}









