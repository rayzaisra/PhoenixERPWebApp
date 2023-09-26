using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("salary_process_header", Schema = "hr")]
    public class SalaryProcessHeader : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column("id"), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("code"), StringLength(50)]
        public string Code { get; set; }

        [Column("month")]
        public int? Month { get; set; }

        [Column("year")]
        public int? Year { get; set; }

        [Column("is_tmp")]
        public bool? IsTmp { get; set; }

        [NotMapped]
        public int? Status { get; set; }

        [NotMapped]
        public string StatusDesc { get; set; }

        [NotMapped]
        public string RemarkRejected { get; set; }

        [NotMapped]
        public string RequestBy { get; set; }

        [NotMapped]
        public List<SalaryProcess> SalaryProcesss { get; set; }

        [NotMapped]
        public List<SalaryProcessComponent> SalaryProcessComponents { get; set; }
    }
}