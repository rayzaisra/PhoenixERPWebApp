using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Filemaster), Schema = "dbo")]
    public class Filemaster
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

        [MaxLength(50)]
        [Column("deleted_by"), StringLength(50)]
        public string DeletedBy { get; set; }

        [Column("deleted_on")]
        public DateTime? DeletedOn { get; set; }

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
        [Column("organization_id"), StringLength(50)]
        public string OrganizationId { get; set; }

        [MaxLength(255)]
        [Column(nameof(Name)), StringLength(255)]
        public string Name { get; set; }

        [MaxLength(50)]
        [Column(nameof(Type)), StringLength(50)]
        public string Type { get; set; }

        [Column(nameof(Size))]
        public long? Size { get; set; }

        [MaxLength(255)]
        [Column(nameof(Realname)), StringLength(255)]
        public string Realname { get; set; }

        [MaxLength(255)]
        [Column(nameof(Thumbnail)), StringLength(255)]
        public string Thumbnail { get; set; }

        [Column(nameof(Tags)), StringLength(8000)]
        public string Tags { get; set; }

        [MaxLength(255)]
        [Column(nameof(Filepath)), StringLength(255)]
        public string Filepath { get; set; }

        [MaxLength(255)]
        [Column(nameof(Mimetype)), StringLength(255)]
        public string Mimetype { get; set; }

        //public Organization Organization { get; set; }
    }
}