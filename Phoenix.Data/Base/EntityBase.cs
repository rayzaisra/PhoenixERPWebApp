using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data
{
    public class EntityBase
    {
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class FinanceEntityBase
    {
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
        
    }

    public class HrisEntityBase
    {
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
        [Column("business_unit_division_id")]
        public string BusinessUnitDivisionId { get; set; }
        [Column("business_unit_departement_id")]
        public string BusinessUnitDepartementId { get; set; }
    }
    public class EntityBaseUm
    {
        [Key, Required]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive{ get; set; }
        public bool? IsDeleted { get; set; }
        public string SubGroupId { get; set; }
        public string DivisionId { get; set; }
    }
    public class MediaEntityBase
    {
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
        [MaxLength(50)]
        [Column("business_unit_division_id")]
        public string DivisionId { get; set; }
        [MaxLength(50)]
        [Column("business_unit_departement_id")]
        public string DepartementId { get; set; }
    }

    public class MediaPlanEntityBase
    {
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
        
    }

    public class PmEntityBase
    {
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
        [Column("business_unit_division_id")]
        public string BusinessUnitDivisionId { get; set; }
        [Column("business_unit_departement_id")]
        public string BusinessUnitDepartementId { get; set; }
    }
}
