using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("employee_contract", Schema = "hr")]
    public class EmployeeContract
    {
        [Key, Required]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("candidate_recruitment_id"), StringLength(50)]
        public string CandidateRecruitmentId { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column(nameof(Active))]
        public int? Active { get; set; }

        [Column(nameof(Notes)), StringLength(2147483647)]
        public string Notes { get; set; }

        [Column("contract_upload"), StringLength(2147483647)]
        public string ContractUpload { get; set; }

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
        [Column("deleted_by"), StringLength(50)]
        public string DeletedBy { get; set; }

        [Column("deleted_on")]
        public DateTime? DeletedOn { get; set; }

        [MaxLength(50)]
        [Column("filemaster_id"), StringLength(50)]
        public string FilemasterId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_division_id"), StringLength(50)]
        public string BusinessUnitDivisionId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_departement_id"), StringLength(50)]
        public string BusinessUnitDepartementId { get; set; }

        //public CandidateRecruitment CandidateRecruitment { get; set; }
        //public EmployeeBasicInfo EmployeeBasicInfo { get; set; }
        //public Filemaster Filemaster { get; set; }
    }
}