using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class Journal : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Transaction Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TransactionDate { get; set; }

        [DisplayName("Reference Id")]
        public string ReferenceId { get; set; }

        [DisplayName("Ref Number")]
        public string ReferenceNumber { get; set; }

        [DisplayName("Reference Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReferenceDate { get; set; }

        //[DisplayName("Reference To")]
        //public string ReferenceTo { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Default Currency Id")]
        public string DefaultCurrencyId { get; set; }

        [DisplayName("Currency Id")]
        public string CurrencyId { get; set; }

        //[DisplayName("Qty")]
        //public int? Qty { get; set; }

        //[DisplayName("Unit")]
        //public string Unit { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        //[DisplayName("Price")]
        //public int? Price { get; set; }

        //[DisplayName("Budgetdebit")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal? Budgetdebit { get; set; }

        //[DisplayName("Budgetcredit")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal? Budgetcredit { get; set; }

        //[DisplayName("Budgetgballance")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal? Budgetgballance { get; set; }

        //[DisplayName("Beginningdebit")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal? Beginningdebit { get; set; }

        //[DisplayName("Beginningcredit")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal? Beginningcredit { get; set; }

        //[DisplayName("Beginningballance")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal? Beginningballance { get; set; }

        [DisplayName("Original Currency")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? OriginalAmount { get; set; }

        [DisplayName("Debit")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Debit { get; set; }

        [DisplayName("Credit")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Credit { get; set; }

        //[DisplayName("Balance")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal? Balance { get; set; }

        //[DisplayName("Endingdebet")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal? Endingdebet { get; set; }

        //[DisplayName("Endingcredit")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal? Endingcredit { get; set; }

        //[DisplayName("Endingbalance")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal? Endingbalance { get; set; }

        //[DisplayName("Rcondition")]
        //public string Rcondition { get; set; }

        //[DisplayName("Flag")]
        //public bool? Flag { get; set; }

        //[DisplayName("Fdate")]
        //[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? Fdate { get; set; }

        //[DisplayName("Ftime")]
        //public TimeSpan? Ftime { get; set; }

        [DisplayName("Pce Id")]
        public string PceId { get; set; }

        [DisplayName("Pca Id")]
        public string PcaId { get; set; }

        [DisplayName("Job Id")]
        public string JobId { get; set; }

        //[DisplayName("External Id")]
        //public string ExternalId { get; set; }

        //[DisplayName("Internal Id")]
        //public string InternalId { get; set; }

        [DisplayName("Account Id")]
        public string AccountId { get; set; }

        //[DisplayName("Invoice Id")]
        //public string InvoiceId { get; set; }

        [DisplayName("Po Id")]
        public string PoId { get; set; }

        [DisplayName("Type Of Expense Id")]
        public string TypeOfExpenseId { get; set; }

        [DisplayName("Mainservice Category Id")]
        public string MainserviceCategoryId { get; set; }

        [DisplayName("Shareservice Id")]
        public string ShareserviceId { get; set; }

        [DisplayName("Business Unit")]
        public string DivisiId { get; set; }

        [DisplayName("Legal Id")]
        public string LegalId { get; set; }

        [DisplayName("Affiliate Id")]
        public string AffiliateId { get; set; }

        //[DisplayName("Is Posted")]
        //public string IsPosted { get; set; }

        //[DisplayName("Account Code")]
        //public string AccountCode { get; set; }

        [DisplayName("Line Code")]
        public string LineCode { get; set; }


        //[DisplayName("Is Job Closed")]
        //public string IsJobClosed { get; set; }

        //[DisplayName("Pca Task Id")]
        //public string PcaTaskId { get; set; }

        //[MaxLength(50)]
        //[Column("trx_source"), StringLength(50)]
        public string TrxSource { get; set; }

        [DisplayName("Fixed Asset")]
        public string FixedAssetId { get; set; }

        public string TaskId { get; set; }

        public string ItemTypeId { get; set; }

        //NotMapped
        [DisplayName("Account Name")]
        public string AccountName { get; set; }

        [DisplayName("Job ID-Name")]
        public string JobIdName { get; set; }

        [DisplayName("Business Unit")]
        public string DivisiName { get; set; }

        [DisplayName("Affiliation")]
        public string AfiliationName { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }

        [DisplayName("Account ID")]
        public string CodeRec { get; set; }

        [DisplayName("Pca Task")]
        public string PcaTaskName { get; set; }
        public string FilemasterId { get; set; }
        public string FilemasterName { get; set; }
        public string File { get; set; }
        public string JobName { get; set; }

        [DisplayName("Currency")]
        public string CurrencyName { get; set; }

        public string CurrencySymbol { get; set; }

        public string CurrencySymbolOriginal { get; set; }

        [DisplayName("Fixed Asset")]
        public string FixedAssetName { get; set; } 
    }
}