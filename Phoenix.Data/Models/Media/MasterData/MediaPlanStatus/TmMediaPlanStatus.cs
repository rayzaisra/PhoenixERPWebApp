using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Media.MasterData.MediaPlanStatus
{
    [Table("TmMediaPlanStatus", Schema = "media")]
    public class TmMediaPlanStatus : MediaEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(255)]
        [Column("media_plan_status_name"), StringLength(255)]
        public string MediaPlanStatusName { get; set; }
    }
}
