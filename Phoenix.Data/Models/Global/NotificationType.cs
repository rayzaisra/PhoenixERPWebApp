using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("notification_type", Schema = "dbo")]
    public class NotificationType : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column("id")]
        public string Id { get; set; }

        [MaxLength(255)]
        [Column("shorttext")]
        public string ShortText { get; set; }

        [Column("longtext")]
        public string LongText { get; set; }

        [Column("sendmail")]
        public bool? SendMail { get; set; }

        [MaxLength(255)]
        [Column("href")]
        public string Href { get; set; }


    }
}