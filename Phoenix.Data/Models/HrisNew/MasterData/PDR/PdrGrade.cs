using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("pdr_grade", Schema = "hr")]
    public class PdrGrade : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column(nameof(Result)), StringLength(50)]
        public string Result { get; set; }

        [Column("start_range")]
        public decimal? StartRange { get; set; }

        [Column("end_range")]
        public decimal? EndRange { get; set; }
    }
}