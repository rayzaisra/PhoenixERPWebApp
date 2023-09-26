using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("insurance_company", Schema = "hr")]
    public class InsuranceCompany : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(100)]
        [Column("name_insurance"), StringLength(100)]
        public string NameInsurance { get; set; }

        [MaxLength(255)]
        [Column(nameof(Description)), StringLength(255)]
        public string Description { get; set; }
    }
}