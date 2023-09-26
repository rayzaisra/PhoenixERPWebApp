using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("pdr_overall_score", Schema = "hr")]
    public class PdrOverallScore : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("pdr_employee_id"), StringLength(50)]
        public string PdrEmployeeId { get; set; }

        [MaxLength(50)]
        [Column("pdr_category_id"), StringLength(50)]
        public string PdrCategoryId { get; set; }

        [Column("average_score")]
        public decimal? AverageScore { get; set; }

        [Column(nameof(Total))]
        public decimal? Total { get; set; }

        [Column(nameof(Percentage))]
        public decimal? Percentage { get; set; }
        //public PdrEmployee PdrEmployee { get; set; }
        //public PdrCategory PdrCategory { get; set; }
    }
}