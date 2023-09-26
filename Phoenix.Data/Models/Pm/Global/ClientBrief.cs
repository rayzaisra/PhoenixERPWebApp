using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("client_brief", Schema = "pm")]
    public class ClientBrief
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("campaign_name"), StringLength(50)]
        public string CampaignName { get; set; }

        [MaxLength(50)]
        [Column("campaign_type_id"), StringLength(50)]
        public string CampaignTypeId { get; set; }

        [MaxLength(50)]
        [Column("bussiness_type_id"), StringLength(50)]
        public string BussinessTypeId { get; set; }

        [MaxLength(50)]
        [Column("external_brand_id"), StringLength(50)]
        public string ExternalBrandId { get; set; }

        [Column(nameof(Deadline))]
        public DateTime? Deadline { get; set; }

        [Column("final_delivery")]
        public DateTime? FinalDelivery { get; set; }

        [MaxLength(50)]
        [Column("nationality_id"), StringLength(50)]
        public string NationalityId { get; set; }

        [MaxLength(50)]
        [Column("job_category_id"), StringLength(50)]
        public string JobCategoryId { get; set; }

        [MaxLength(50)]
        [Column("client_brief_file_id"), StringLength(50)]
        public string ClientBriefFileId { get; set; }

        [MaxLength(50)]
        [Column("assigment_id"), StringLength(50)]
        public string AssigmentId { get; set; }

        [Column("campaign_progress_id")]
        public long? CampaignProgressId { get; set; }

        [Column("start_campaign")]
        public DateTime? StartCampaign { get; set; }

        [Column("finish_campaign")]
        public DateTime? FinishCampaign { get; set; }

        [Column("responsible_person")]
        public long? ResponsiblePerson { get; set; }

        [Column("job_pe_id")]
        public long? JobPeId { get; set; }

        [Column("proposal_file_id")]
        public long? ProposalFileId { get; set; }

        [Column("campaign_status_id")]
        public long? CampaignStatusId { get; set; }

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
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("deleted_by"), StringLength(50)]
        public string DeletedBy { get; set; }

        [Column("deleted_on")]
        public DateTime? DeletedOn { get; set; }

        [MaxLength(50)]
        [Column("business_unit_division_id"), StringLength(50)]
        public string BusinessUnitDivisionId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_departement_id"), StringLength(50)]
        public string BusinessUnitDepartementId { get; set; }


        [Column("client_id")]
        public string ClientId { get; set; }
    }
}