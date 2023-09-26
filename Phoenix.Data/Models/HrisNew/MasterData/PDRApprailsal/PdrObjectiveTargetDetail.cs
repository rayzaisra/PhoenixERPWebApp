using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("pdr_objective_target_detail", Schema = "hr")]
    public class PdrObjectiveTargetDetail : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("pdr_objective_target_id"), StringLength(50)]
        public string PdrObjectiveTargetId { get; set; }

        [MaxLength(50)]
        [Column("pdr_objective_setting_id"), StringLength(50)]
        public string PdrObjectiveSettingId { get; set; }

        [Column(nameof(Target))]
        public decimal? Target { get; set; }

        [NotMapped]
        public string PdrObjectiveSettingItem { get; set; }
    }
}