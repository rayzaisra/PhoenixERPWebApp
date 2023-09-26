using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Bank), Schema = "fn")]
    public class Bank : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("bank_name"), StringLength(50)]
        public string BankName { get; set; }

        [MaxLength(50)]
        [Column("bank_code"), StringLength(50)]
        public string BankCode { get; set; }

        [MaxLength(50)]
        [Column("swift_code"), StringLength(50)]
        public string SwiftCode { get; set; }

        [MaxLength(250)]
        [Column("account_number"), StringLength(250)]
        public string AccountNumber { get; set; }

        [MaxLength(50)]
        [Column("coa_id"), StringLength(50)]
        public string CoaId { get; set; }

        [MaxLength(50)]
        [Column("coa_lvl5_name"), StringLength(50)]
        public string CoaLvl5Name { get; set; }

        [MaxLength(50)]
        [Column(nameof(Description)), StringLength(50)]
        public string Description { get; set; }

        [MaxLength(50)]
        [Column("Legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("Currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }
        
        [NotMapped]
        public string CurrencyName { get; set; }

        [NotMapped]
        public string CodeRec { get; set; }

        [NotMapped]
        public string LegalEntityName { get; set; }
    }
}