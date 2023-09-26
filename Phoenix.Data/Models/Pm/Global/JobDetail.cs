using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("job_detail", Schema = "pm")]
    public class JobDetail
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("client_brief_id"), StringLength(50)]
        public string ClientBriefId { get; set; }

        [MaxLength(50)]
        [Column("job_number"), StringLength(50)]
        public string JobNumber { get; set; }

        [MaxLength(150)]
        [Column("job_name"), StringLength(150)]
        public string JobName { get; set; }

        [Column(nameof(Deadline))]
        public DateTime? Deadline { get; set; }

        [Column("final_delivery")]
        public DateTime? FinalDelivery { get; set; }

        [MaxLength(50)]
        [Column("pm_id"), StringLength(50)]
        public string PmId { get; set; }

        [Column(nameof(Pa))]
        public double? Pa { get; set; }

        [Column(nameof(Pe))]
        public double? Pe { get; set; }

        [MaxLength(50)]
        [Column("job_status_detail"), StringLength(50)]
        public string JobStatusDetail { get; set; }

        [Column("job_description"), StringLength(8000)]
        public string JobDescription { get; set; }

        [Column("cash_advance")]
        public double? CashAdvance { get; set; }

        [MaxLength(50)]
        [Column("job_status_id"), StringLength(50)]
        public string JobStatusId { get; set; }

        [MaxLength(50)]
        [Column("created_by"), StringLength(50)]
        public string CreatedBy { get; set; }

        [Column("created_on")]
        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        [Column("modified_by"), StringLength(50)]
        public string ModifiedBy { get; set; }

        [Column("modified_on")]
        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        [Column("approved_by"), StringLength(50)]
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

        [MaxLength(50)]
        [Column("owner_id"), StringLength(50)]
        public string OwnerId { get; set; }

        [MaxLength(50)]
        [Column("company_id"), StringLength(50)]
        public string CompanyId { get; set; }

        [MaxLength(50)]
        [Column("deleted_by"), StringLength(50)]
        public string DeletedBy { get; set; }

        [Column("deleted_on")]
        public DateTime? DeletedOn { get; set; }

        [Column("start_job")]
        public DateTime? StartJob { get; set; }

        [Column("finish_job")]
        public DateTime? FinishJob { get; set; }

        [MaxLength(50)]
        [Column("team_id"), StringLength(50)]
        public string TeamId { get; set; }

        [MaxLength(50)]
        [Column("type_id"), StringLength(50)]
        public string TypeId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_division_id"), StringLength(50)]
        public string BusinessUnitDivisionId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_departement_id"), StringLength(50)]
        public string BusinessUnitDepartementId { get; set; }

        [MaxLength(50)]
        [Column("job_type "), StringLength(50)]
        public string JobType { get; set; }

        [MaxLength(50)]
        [Column("extended_detail"), StringLength(50)]
        public string ExtendedDetail { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("main_service_category"), StringLength(50)]
        public string MainServiceCategory { get; set; }

        [MaxLength(50)]
        [Column("affiliation_id"), StringLength(50)]
        public string AffiliationId { get; set; }

        [MaxLength(50)]
        [Column("brand_id"), StringLength(50)]
        public string BrandId { get; set; }

        [MaxLength(50)]
        [Column("client_name"), StringLength(50)]
        public string ClientName { get; set; }

        [MaxLength(50)]
        [Column("nature_of_expense_id"), StringLength(50)]
        public string NatureOfExpenseId { get; set; }

        [MaxLength(50)]
        [Column("cost_allocation"), StringLength(50)]
        public string CostAllocation { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("master_outlet_id"), StringLength(50)]
        public string MasterOutletId { get; set; }
        
        [NotMapped]
        public string JobPaId { get; set; }
        [NotMapped]
        public string JobPaIdName { get; set; }
        [NotMapped]
        public string JobPeId { get; set; }
        [NotMapped]
        public string JobPeName { get; set; }
    }
}