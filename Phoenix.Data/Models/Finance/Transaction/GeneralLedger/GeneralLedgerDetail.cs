using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    //[Table("vw_general_ledger_type2", Schema = "fn")]
    //public class GeneralLedgerDetail
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    //    [Column(nameof(Id))]
    //    public Guid? Id { get; set; }

    //    [Column(nameof(Report))]
    //    public string Report { get; set; }

    //    [Column("account_id")]
    //    public string AccountId { get; set; }

    //    [Column("code_rec")]
    //    public string CodeRec { get; set; }

    //    [Column("coa_level1")]
    //    public string CoaLevel1 { get; set; }

    //    [Column("coa_level2")]
    //    public string CoaLevel2 { get; set; }

    //    [Column("coa_level3")]
    //    public string CoaLevel3 { get; set; }

    //    [Column("coa_level4")]
    //    public string CoaLevel4 { get; set; }

    //    [Column("coa_level5")]
    //    public string CoaLevel5 { get; set; }

    //    [Column("client_id")]
    //    public string ClientId { get; set; }

    //    [Column("client_name")]
    //    public string ClientName { get; set; }

    //    [Column("employee_id")]
    //    public string EmployeeId { get; set; }

    //    [Column("employee_name")]
    //    public string EmployeeName { get; set; }

    //    [Column("fiscal_year")]
    //    public int? FiscalYear { get; set; }

    //    [Column("periode_from")]
    //    public DateTime? PeriodeFrom { get; set; }

    //    [Column("periode_to")]
    //    public DateTime? PeriodeTo { get; set; }

    //    [Column(nameof(Referencedate))]
    //    public DateTime? Referencedate { get; set; }

    //    [Column("source_type")]
    //    public string SourceType { get; set; }

    //    [Column("reff_number")]
    //    public string ReffNumber { get; set; }

    //    [Column("vat_no")]
    //    public string VatNo { get; set; }

    //    [Column("job_no")]
    //    public string JobNo { get; set; }

    //    [Column("pce_no")]
    //    public string PceNo { get; set; }

    //    [Column("pca_no")]
    //    public string PcaNo { get; set; }

    //    [Column("business_unit_id")]
    //    public string BusinessUnitId { get; set; }

    //    [Column("business_unit")]
    //    public string BusinessUnit { get; set; }

    //    [Column("legal_entity_id")]
    //    public string LegalEntityId { get; set; }

    //    [Column("legal_entity")]
    //    public string LegalEntity { get; set; }

    //    [Column("curr_code")]
    //    public string CurrCode { get; set; }

    //    [Column("exchange_rate")]
    //    public decimal? ExchangeRate { get; set; }

    //    [Column(nameof(Debit))]
    //    public decimal? Debit { get; set; }

    //    [Column(nameof(Credit))]
    //    public decimal? Credit { get; set; }

    //    [Column(nameof(Adjustment))]
    //    public decimal? Adjustment { get; set; }

    //    [Column(nameof(Description))]
    //    public string Description { get; set; }
    //}

    public class GeneralLedgerDetail
    {
        public Guid? Id { get; set; }

        public string Report { get; set; }

        public string AccountId { get; set; }

        public string CodeRec { get; set; }

        //public string CoaLevel1 { get; set; }

        //public string CoaLevel2 { get; set; }

        //public string CoaLevel3 { get; set; }

        //public string CoaLevel4 { get; set; }

        //public string CoaLevel5 { get; set; }
        public string CoaLevel { get; set; }

        public string ClientId { get; set; }

        public string ClientName { get; set; }

        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public int? FiscalYear { get; set; }

        public DateTime? PeriodeFrom { get; set; }

        public DateTime? PeriodeTo { get; set; }

        public DateTime? Referencedate { get; set; }

        public string SourceType { get; set; }

        public string ReffNumber { get; set; }

        public string VatNo { get; set; }

        public string JobNo { get; set; }

        public string PceNo { get; set; }

        public string PcaNo { get; set; }

        public string BusinessUnitId { get; set; }

        public string BusinessUnit { get; set; }

        public string LegalEntityId { get; set; }

        public string LegalEntity { get; set; }

        public string CurrCode { get; set; }

        public decimal? ExchangeRate { get; set; }

        public decimal? Debit { get; set; }

        public decimal? Credit { get; set; }

        public decimal? Adjustment { get; set; }

        public string Description { get; set; }

        public string PoNumber { get; set; }

        public string InvoiceVendor { get; set; }

        public string ApproveBy { get; set; }

        public DateTime? ApproveDate { get; set; }
    }


    public class GeneralLedgerDetailSearch
    {
        public string LegalEntityId { get; set; }
        public DateTime? PeriodeFrom { get; set; }
        public DateTime? PeriodeTo { get; set; }
        public string EvdNo { get; set; }
        public string BusinessUnitId { get; set; }
        public int Level { get; set; }
        public string AccountId { get; set; }
    }
}
