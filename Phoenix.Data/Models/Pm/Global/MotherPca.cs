using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("mother_pca", Schema = "pm")]
    public class MotherPca
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column(nameof(Code)), StringLength(50)]
        public string Code { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [Column(nameof(Detail)), StringLength(2147483647)]
        public string Detail { get; set; }

        [Column(nameof(Description)), StringLength(2147483647)]
        public string Description { get; set; }

        [Column(nameof(Total))]
        public double? Total { get; set; }

        [MaxLength(100)]
        [Column(nameof(Name)), StringLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        [Column(nameof(Revision)), StringLength(50)]
        public string Revision { get; set; }

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
        [Column(nameof(Currency)), StringLength(50)]
        public string Currency { get; set; }

        [MaxLength(50)]
        [Column("business_unit_division_id"), StringLength(50)]
        public string BusinessUnitDivisionId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_departement_id"), StringLength(50)]
        public string BusinessUnitDepartementId { get; set; }
    }
}