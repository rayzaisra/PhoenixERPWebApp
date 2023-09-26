using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Media.MasterData.MediaOrderStatus
{
    [Table("TmMediaOrderStatus", Schema = "media")]
    public class TmMediaOrderStatus : MediaEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(255)]
        [Column("media_order_status_name"), StringLength(255)]
        public string MediaOrderStatusName { get; set; }
    }
}
