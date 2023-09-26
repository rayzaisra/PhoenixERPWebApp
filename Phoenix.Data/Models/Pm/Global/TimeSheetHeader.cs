using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("timesheet_header", Schema = "pm")]
    public class TimeSheetHeader
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("status"), StringLength(50)]
        public string Status { get; set; }

        [MaxLength(50)]
        [Column("business_unit_division_id"), StringLength(50)]
        public string BusinessUnitDivisionId { get; set; }

        [MaxLength(50)]
        [Column("departement_id"), StringLength(50)]
        public string DepartmentId { get; set; }

        [MaxLength(50)]
        [Column("user_id"), StringLength(50)]
        public string UserId { get; set; }

        [MaxLength(50)]
        [Column("username"), StringLength(50)]
        public string Username { get; set; }

        [Column("start_date_weekly")]
        public DateTime? StartDateWeekly { get; set; }

        [Column("end_date_weekly")]
        public DateTime? EndDateWeekly { get; set; }


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

      
    }
}