using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("notification", Schema = "dbo")]
    public class Notification : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column("id")]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("notification_type_id")]
        public string NotificationTypeId { get;set;}

        [MaxLength(50)]
        [Column("user_id")]
        public string UserId { get; set; }

        [MaxLength(50)]
        [Column("item_id")]
        public string ItemId { get; set; }

        [Column("is_seen")]
        public bool? IsSeen { get; set; }

        [MaxLength(50)]
        [Column("module")]
        public string Module { get; set; }

        [NotMapped]
        public string TemplateNotify { get; set; }
    }
}