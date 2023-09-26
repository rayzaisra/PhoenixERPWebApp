using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("employee_checklist", Schema = "hr")]
    public class EmployeeChecklist
    {
        [Key, Required]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [MaxLength(255)]
        [Column(nameof(Checklistitem)), StringLength(255)]
        public string Checklistitem { get; set; }

        [Column(nameof(Description)), StringLength(2147483647)]
        public string Description { get; set; }

        [Column(nameof(Whentoalert))]
        public DateTime? Whentoalert { get; set; }

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
        [Column("business_unit_division_id"), StringLength(50)]
        public string BusinessUnitDivisionId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_departement_id"), StringLength(50)]
        public string BusinessUnitDepartementId { get; set; }

        [MaxLength(50)]
        [Column("employee_checklist_category_id"), StringLength(50)]
        public string EmployeeChecklistCategoryId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_pic_id"), StringLength(50)]
        public string BusinessUnitPicId { get; set; }

        //public EmployeeBasicInfo EmployeeBasicInfo { get; set; }
        //public EmployeeChecklistCategory EmployeeChecklistCategory { get; set; }
    }
}