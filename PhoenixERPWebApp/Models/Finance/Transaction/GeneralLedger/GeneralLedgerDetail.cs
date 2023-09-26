using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class GeneralLedgerDetail
    {
        [DisplayName("Id")]
        public Guid? Id { get; set; }

        [DisplayName("Report")]
        public string Report { get; set; }

        [DisplayName("Account Id")]
        public string AccountId { get; set; }

        [DisplayName("COA No")]
        public string CodeRec { get; set; }

        //[DisplayName("Level 1 COA")]
        //public string CoaLevel1 { get; set; }

        //[DisplayName("Level 2 COA")]
        //public string CoaLevel2 { get; set; }

        //[DisplayName("Level 3 COA")]
        //public string CoaLevel3 { get; set; }

        //[DisplayName("Level 4 COA")]
        //public string CoaLevel4 { get; set; }

        //[DisplayName("Level 5 COA")]
        //public string CoaLevel5 { get; set; }

        [DisplayName("COA")]
        public string CoaLevel { get; set; }

        [DisplayName("Client Name")]
        public string ClientId { get; set; }

        [DisplayName("Client Name")]
        public string ClientName { get; set; }

        [DisplayName("Employee Name")]
        public string EmployeeId { get; set; }

        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        [DisplayName("Fiscal Year")]
        public int? FiscalYear { get; set; }

        [DisplayName("From")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PeriodeFrom { get; set; }

        [DisplayName("To")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PeriodeTo { get; set; }

        [DisplayName("Date Transaction")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Referencedate { get; set; }

        [DisplayName("Source Type")]
        public string SourceType { get; set; }

        [DisplayName("Reff Number")]
        public string ReffNumber { get; set; }

        [DisplayName("Vat No")]
        public string VatNo { get; set; }

        [DisplayName("JOB NO")]
        public string JobNo { get; set; }

        [DisplayName("PCE ID")]
        public string PceNo { get; set; }

        [DisplayName("PCA ID")]
        public string PcaNo { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnit { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntity { get; set; }

        [DisplayName("Curr Code")]
        public string CurrCode { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Debit")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Debit { get; set; }

        [DisplayName("Credit")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Credit { get; set; }

        [DisplayName("Adjustment")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Adjustment { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("PO Number")]
        public string PoNumber { get; set; }

        [DisplayName("Invoice Vendor")]
        public string InvoiceVendor { get; set; }

        [DisplayName("Approved By")]

        public string ApproveBy { get; set; }

        [DisplayName("Approved Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ApproveDate { get; set; }
    }
    //public class GeneralLedgerDetail
    //{
    //    [DisplayName("Id")]
    //    public Guid? Id { get; set; }

    //    [DisplayName("Report")]
    //    public string Report { get; set; }

    //    [DisplayName("RPOTYPE")]
    //    public string Rpotype { get; set; }

    //    [DisplayName("DESC1")]
    //    public string Desc1 { get; set; }

    //    [DisplayName("FDESC1")]
    //    public string Fdesc1 { get; set; }

    //    [DisplayName("ACC NAME")]
    //    public string Accname { get; set; }

    //    [DisplayName("ACC DESC")]
    //    public string Accdesc { get; set; }

    //    [DisplayName("CLIENT")]
    //    public int? ClientId { get; set; }

    //    [DisplayName("REMARK")]
    //    public string Description { get; set; }

    //    [DisplayName("Periode From")]
    //    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime? PeriodeFrom { get; set; }

    //    [DisplayName("Periode To")]
    //    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime? PeriodeTo { get; set; }

    //    [DisplayName("YOUR REFF")]
    //    public int? YourReff { get; set; }

    //    [DisplayName("Reference No")]
    //    public string EvdNo { get; set; }

    //    [DisplayName("Reference Date")]
    //    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime? Referencedate { get; set; }

    //    [DisplayName("JOB NO")]
    //    public string JobId { get; set; }

    //    [DisplayName("Mainservice Category")]
    //    public string MainserviceCategory { get; set; }

    //    [DisplayName("PCE ID")]
    //    public string PceId { get; set; }

    //    [DisplayName("PCA ID")]
    //    public string PcaId { get; set; }

    //    [DisplayName("DESC")]
    //    public string TrxDescription { get; set; }

    //    [DisplayName("Division")]
    //    public string Division { get; set; }

    //    [DisplayName("Currency")]
    //    public string CurrCode { get; set; }

    //    [DisplayName("Exchange")]
    //    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    //    public decimal? ExchangeRate { get; set; }

    //    [DisplayName("Debit")]
    //    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    //    public decimal? Debit { get; set; }

    //    [DisplayName("Credit")]
    //    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    //    public decimal? Credit { get; set; }

    //    [DisplayName("Adjustment")]
    //    public decimal? Adjustment { get; set; }

    //    [DisplayName("Currency")]
    //    public string CurrencyId { get; set; }

    //    [DisplayName("Legal Entity")]
    //    public string LegalEntityId { get; set; }

    //    [DisplayName("PERIOD")]
    //    public string period { get { return Convert.ToDateTime(PeriodeFrom).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(PeriodeTo).ToString("dd-MMM-yyyy"); } }
    //}

    public class GeneralLedgerDetailSearch
    {
        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Periode From")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PeriodeFrom { get; set; }

        [DisplayName("Periode To")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PeriodeTo { get; set; }

        [DisplayName("Reference No")]
        public string EvdNo { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("COA Level")]
        public int Level { get; set; }

        [DisplayName("COA")]
        public string AccountId { get; set; }
    }
}
