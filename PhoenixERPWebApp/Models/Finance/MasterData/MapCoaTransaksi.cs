using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class MapCoaTransaksi : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Module Code")]
        public string ModuleCode { get; set; }

        [DisplayName("Mainservice Category Id")]
        public string MainserviceCategoryId { get; set; }

        //[DisplayName("Sharedservice Id")]
        //public string SharedserviceId { get; set; }

        [DisplayName("Line Code")]
        public string LineCode { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Account Name (Level 5 COA)")]
        public string CoaId { get; set; }

        [DisplayName("Condition")]
        public string DK { get; set; }

        [DisplayName("ASF")]
        public bool? IsAsf { get; set; }

        [DisplayName("Name Setting")]
        public string NameSetting { get; set; }

        [DisplayName("Pce Type Cross Billing")]
        public bool? PceTypeCrossBilling { get; set; }

        [DisplayName("Invoice Type")]
        public string InvoiceType { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        //NotMapped
        [DisplayName("Account Name (Level 5 COA)")]
        public string AccountName { get; set; }

        [DisplayName("Account ID")]
        public string CodeRec { get; set; }

        public string Condition { get { return DK == "d" ? "Debit" : DK == "k" ? "Credit" : ""; } }
        public string ASFName { get { return (IsAsf ?? false) ? "Yes" : "No"; } }
        public bool? isEdit { get; set; }
    }
}