using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("pdr_apprailsal_m1", Schema = "hr")]
    public class PdrApprailsalM1 : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("pdr_apprailsal_m1_header_id"), StringLength(50)]
        public string PdrApprailsalM1HeaderId { get; set; }

        [MaxLength(50)]
        [Column("pdr_objective_setting_id"), StringLength(50)]
        public string PdrObjectiveSettingId { get; set; }

        [Column(nameof(Target))]
        public decimal? Target { get; set; }

        [Column(nameof(Achievement))]
        public decimal? Achievement { get; set; }

        [Column(nameof(Weight))]
        public decimal? Weight { get; set; }

        [Column("rating_score")]
        public decimal? RatingScore { get; set; }

        [NotMapped]
        public decimal? WeightedScore { get; set; }
        [NotMapped]
        public string PdrObjectiveSettingItem { get; set; }
    }
}