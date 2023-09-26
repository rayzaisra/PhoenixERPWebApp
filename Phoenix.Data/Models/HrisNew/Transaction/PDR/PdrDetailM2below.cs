using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("pdr_detail_m2below", Schema = "hr")]
    public class PdrDetailM2below : HrisEntityBase
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

        [MaxLength(50)]
        [Column("pdr_parameter_id"), StringLength(50)]
        public string PdrParameterId { get; set; }

        [Column(nameof(Score))]
        public decimal? Score { get; set; }

        [Column(nameof(Percentage))]
        public decimal? Percentage { get; set; }

        [Column(nameof(Notes)), StringLength(2147483647)]
        public string Notes { get; set; }

        //public PdrEmployee PdrEmployee { get; set; }
        //public PdrParameter PdrParameter { get; set; }
    }
}