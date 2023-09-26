using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("TrMappingFolderOneDrive", Schema = "Um")]
    public class TrMappingFolderOneDrive : EntityBase
    {
        [Key, Required]
        [MaxLength(128)]
        [Column(nameof(Id)), StringLength(128)]
        public string Id { get; set; }

        [Required]
        [MaxLength(256)]
        [Column(nameof(ItemId)), StringLength(256)]
        public string ItemId { get; set; }

        [Required]
        [MaxLength(128)]
        [Column(nameof(RefId)), StringLength(128)]
        public string RefId { get; set; }

        [Required]
        public RefTypeMapping RefType { get; set; }
    }

    public enum RefTypeMapping
    {
        Group = 1,
        User = 2
    }
}
