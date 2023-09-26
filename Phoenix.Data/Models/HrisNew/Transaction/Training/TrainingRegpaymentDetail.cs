using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{

    [Table("training_regpayment_detail", Schema = "hr")]
    public class TrainingRegpaymentDetail
    {
        [Key]
        [MaxLength(50)]
        [Column("id"), StringLength(50)]
        public string Id { get; set; }

        [Column("reg_payment_id"), StringLength(50)]
        public string RegPaymentId { get; set; }

        [Column("master_training_request_id")]
        public string MasterTrainingRequestId { get; set; }

        [Column("status_request")]
        public string StatusRequest { get; set; }

        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("approval_id")]
        public string ApprovalId { get; set; }

        [Column("any_needs")]
        public string AnyNeeds { get; set; }

        [Column("business_unit_finance_id")]
        public string BussinessUnitFinanceId { get; set; }
        [MaxLength(50)]
        [Column("created_by")]
        public string CreatedBy { get; set; }
        [Column("created_on")]
        public DateTime? CreatedOn { get; set; }
        [MaxLength(50)]
        [Column("modified_by")]
        public string ModifiedBy { get; set; }
        [Column("modified_on")]
        public DateTime? ModifiedOn { get; set; }
        [MaxLength(50)]
        [Column("approved_by")]
        public string ApprovedBy { get; set; }
        [Column("approved_on")]
        public DateTime? ApprovedOn { get; set; }
        [Column("is_active")]
        public bool? IsActive { get; set; }
        [Column("is_locked")]
        public bool? IsLocked { get; set; }
        [Column("is_default")]
        public bool? IsDefault { get; set; }
        [Column("is_deleted")]
        public bool? IsDeleted { get; set; }
        [Column("owner_id")]
        public string OwnerId { get; set; }
        [Column("deleted_by")]
        public string DeletedBy { get; set; }
        [Column("deleted_on")]
        public DateTime? DeletedOn { get; set; }

        [NotMapped]
        public string MasterTrainingRequestName { get; set; }

    }
}
