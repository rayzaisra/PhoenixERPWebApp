using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class Bank : FinanceEntityBase
    {
        [DisplayName("ID Bank")]
        public string Id { get; set; }

        [DisplayName("Bank Account Name")]
        public string BankName { get; set; }

        [DisplayName("Code Bank")]
        public string BankCode { get; set; }

        [DisplayName("Swift Code")]
        public string SwiftCode { get; set; }

        [DisplayName("Bank Account Number")]
        public string AccountNumber { get; set; }

        [DisplayName("COA LEVEL 5")]
        public string CoaId { get; set; }

        [DisplayName("COA LEVEL 5")]
        public string CoaLvl5Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        //Not Mapped
        [DisplayName("Currency")]
        public string CurrencyName { get; set; }

        [DisplayName("COA Level 5 ID")]
        public string CodeRec { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }
    }
}