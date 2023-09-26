using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("AzurePortalSetup", Schema = "Um")]
    public class AzurePortalSetup
    {
        [Key, Required]
        [MaxLength(128)]
        [Column(nameof(Id)), StringLength(128)]
        public string Id { get; set; }

        [MaxLength(250)]
        [Column(nameof(TenantId)), StringLength(250)]
        public string TenantId { get; set; }

        [MaxLength(250)]
        [Column(nameof(ClientId)), StringLength(250)]
        public string ClientId { get; set; }

        [MaxLength(250)]
        [Column(nameof(EmailAccount)), StringLength(250)]
        public string EmailAccount { get; set; }
        public string SecurityPword { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }

}
