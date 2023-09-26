using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("application_user", Schema = "dbo")]
    public class ApplicationUser
    {
        [Key, Required]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

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

        [MaxLength(50)]
        [Column("owner_id"), StringLength(50)]
        public string OwnerId { get; set; }

        [Required]
        [MaxLength(400)]
        [Column("app_username"), StringLength(400)]
        public string AppUsername { get; set; }

        [Required]
        [MaxLength(450)]
        [Column("app_password"), StringLength(450)]
        public string AppPassword { get; set; }

        [Required]
        [MaxLength(450)]
        [Column("app_fullname"), StringLength(450)]
        public string AppFullname { get; set; }

        [Column("is_system_administrator")]
        public bool? IsSystemAdministrator { get; set; }

        [MaxLength(450)]
        [Column(nameof(Email)), StringLength(450)]
        public string Email { get; set; }

        [MaxLength(50)]
        [Column(nameof(Phone)), StringLength(50)]
        public string Phone { get; set; }

        [MaxLength(50)]
        [Column("manager_id"), StringLength(50)]
        public string ManagerId { get; set; }

        [MaxLength(50)]
        [Column("primary_team"), StringLength(50)]
        public string PrimaryTeam { get; set; }

        [MaxLength(50)]
        [Column("access_token"), StringLength(50)]
        public string AccessToken { get; set; }

        [Column("token_expiry")]
        public DateTime? TokenExpiry { get; set; }

        [Column("thumbnail_base64"), StringLength(8000)]
        public string ThumbnailBase64 { get; set; }

        [Column("photo_base64"), StringLength(8000)]
        public string PhotoBase64 { get; set; }

        [MaxLength(50)]
        [Column("mime_type"), StringLength(50)]
        public string MimeType { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("organization_id"), StringLength(50)]
        public string OrganizationId { get; set; }

        [MaxLength(50)]
        [Column("deleted_by"), StringLength(50)]
        public string DeletedBy { get; set; }

        [Column("deleted_on")]
        public DateTime? DeletedOn { get; set; }

        [Column("is_deleted")]
        public bool? IsDeleted { get; set; }

        [Column("is_msuser")]
        public bool? IsMsuser { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        //public BusinessUnit BusinessUnit { get; set; }
        //public EmployeeBasicInfo EmployeeBasicInfo { get; set; }
    }
}