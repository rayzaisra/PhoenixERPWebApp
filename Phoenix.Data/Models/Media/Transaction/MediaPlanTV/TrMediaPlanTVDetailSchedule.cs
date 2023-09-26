using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Media.Transaction.MediaPlanTV
{
    [Table("TrMediaPlanTvDetailSchedule", Schema = "media")]
    public class TrMediaPlanTVDetailSchedule : MediaEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("TrMediaPlanDetail_id"), StringLength(50)]
        public string TrMediaPlanDetailId { get; set; }

        [MaxLength(50)]
        [Column("bulan"), StringLength(50)]
        public string Bulan { get; set; }

        [MaxLength(50)]
        [Column("tanggal"), StringLength(50)]
        public string Tanggal { get; set; }

        [MaxLength(50)]
        [Column("tahun"), StringLength(50)]
        public string Tahun { get; set; }

        [MaxLength(50)]
        [Column("spot_value"), StringLength(50)]
        public string SpotValue { get; set; }
    }
}
