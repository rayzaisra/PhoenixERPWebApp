using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Organization), Schema = "dbo")]
    public class Organization : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("organization_name"), StringLength(50)]
        public string OrganizationName { get; set; }

        [MaxLength(50)]
        [Column("parent_organization_id"), StringLength(50)]
        public string ParentOrganizationId { get; set; }

        [Column("is_default_organization")]
        public bool? IsDefaultOrganization { get; set; }

        [MaxLength(450)]
        [Column("smtp_server"), StringLength(450)]
        public string SmtpServer { get; set; }

        [Column("smtp_port")]
        public int? SmtpPort { get; set; }

        [MaxLength(450)]
        [Column("smtp_username"), StringLength(450)]
        public string SmtpUsername { get; set; }

        [MaxLength(450)]
        [Column("smtp_password"), StringLength(450)]
        public string SmtpPassword { get; set; }

        [MaxLength(450)]
        [Column("smtp_from_address"), StringLength(450)]
        public string SmtpFromAddress { get; set; }
    }
}