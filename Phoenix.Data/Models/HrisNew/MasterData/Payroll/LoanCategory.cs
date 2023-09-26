using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("loan_category", Schema = "hr")]
    public class LoanCategory : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column(nameof(Name)), StringLength(50)]
        public string Name { get; set; }

        [Column("interest_month")]
        public decimal InterestMonth { get; set; }

        [MaxLength(100)]
        [Column(nameof(Description)), StringLength(100)]
        public string Description { get; set; }
        [Column("is_Corporate_Loan")]
        public bool? is_Corporate_Loan { get; set; }
        [NotMapped]
        public string CorporateLoanTypeDesc { get; set; }

    }
}