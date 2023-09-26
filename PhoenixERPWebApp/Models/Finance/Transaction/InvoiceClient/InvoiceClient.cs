using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class InvoiceClient : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("PCE Id")]
        public string PceId { get; set; }

        [DisplayName("Pce Seq")]
        public int? PceSeq { get; set; }

        [DisplayName("Brand Id")]
        public string BrandId { get; set; }

        [DisplayName("Client Id")]
        public string ClientId { get; set; }

        [DisplayName("Alternate Client Id")]
        public string AlternateClientId { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Invoice Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceDate { get; set; }

        [DisplayName("Invoice Number")]
        public string InvoiceNumber { get; set; }

        [DisplayName("Start Period")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartPeriod { get; set; }

        [DisplayName("End Period")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndPeriod { get; set; }

        //[DisplayName("Due Date")]
        //[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? DueDate { get; set; }

        [DisplayName("Vat")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Vat { get; set; }

        [DisplayName("Vat Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? VatExchangeRate { get; set; }

        [DisplayName("Sub Total Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalAmount { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Job Name")]
        public string JobName { get; set; }

        public string JobId { get; set; }

        [DisplayName("Description")]
        public string Remarks { get; set; }

        [DisplayName("Estimate Number")]
        public string EstimateNumber { get; set; }

        [DisplayName("Ref Number")]
        public string RefNumber { get; set; }

        [DisplayName("Your Ref")]
        public string YourRef { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Account")]
        public string AccountId { get; set; }

        [DisplayName("ASF")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Asf { get; set; }

        [DisplayName("ASF Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AsfAmount { get; set; }

        [DisplayName("Grand Total Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? GrandTotal { get; set; }

        public string MultiPceId { get; set; }

        [DisplayName("Description")]
        public string PayDescription { get; set; }

        public string PoDocFileId { get; set; }

        public string VatDocFileId { get; set; }

        [DisplayName("Main Service Category")]
        public string MainServiceCategory { get; set; }

        public string LegalId { get; set; }

        [DisplayName("Affiliation")]
        public string AffiliationId { get; set; }

        public string BusinessUnitId { get; set; }

        [DisplayName("PO Client No")]
        public string POClientNo { get; set; }

        [DisplayName("Billed")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? InvoiceBilled { get; set; }

        [DisplayName("Pce Type Cross Billing")]
        public bool? PceTypeCrossBilling { get; set; }


        //============= Not Mapped =================
        public string PoDocFileName { get; set; }
        public string VatDocFileName { get; set; }


        //For List Invoice Client
        [DisplayName("Client Name")]
        public string ClientName { get; set; }

        public string AccountBankName { get; set; }
        public string AccountBankNumber { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }

        [DisplayName("Business Unit")]
        public string DivisionName { get; set; }

        [DisplayName("Main Service Category")]
        public string MainServiceCategoryName { get; set; }

        [DisplayName("Currency")]
        public string CurrencyName { get; set; }

        [DisplayName("Status")]
        public string StatusDesc { get { return InvoiceClientStatus.StatusName(Status); } }


        //For Detail Invoice Client - Process Crud
        public List<InvoiceClientItemOther> items_other { get; set; }


        //======== Start Detail Invoice Client ============
        //For Approval 
        public string LogDescription { get; set; }


        //For Client Information
        [DisplayName("Client Brand")]
        public string ClientBrandName { get; set; }
        public string ClientInvAddress { get; set; }
        public string ClientTaxNumber { get; set; }

        [DisplayName("PIC")]
        public string ClientPIC { get; set; }

        public int? TermOfPayment { get; set; }


        //For Invoice Detail
        [DisplayName("Total PCE")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalPCE { get; set; }

        [DisplayName("Pce Balance")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? PceBalance { get; set; }

        [DisplayName("VAT (%)")]
        public decimal? VAtPercentage { get; set; }

        [DisplayName("VAT No")]
        public string VatNo { get; set; }

        [DisplayName("Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? totalamtplusasf { get; set; }

        [DisplayName("Invoice")]
        public string InvoiceTo { get; set; }

        //For Job Detail 
        [DisplayName("Job Status")]
        public string JobStatus { get; set; }
        //======== End Detail Invoice Client ============ 


        //For Form AR 
        [DisplayName("Invoice Delivery Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceDeliveryDate { get; set; }

        public string InvoiceDeliveryNo { get; set; }


        //For Form Multiple Invoice PCE
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public decimal? BilledAsfPercent { get; set; }
        public List<InvoiceClient> PceListMultiple { get; set; }

        public List<InvoiceClientItemOther> InvoiceMultipleListOther { get; set; }


        //For Mapping Data
        public string SourceOption { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencyIdDefault { get; set; }

        public string CurrencySymbolDefault { get; set; }

        public List<MultipleBrand> MultipleBrand { get; set; }


        [DisplayName("Amount Exchange")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? GrandTotalExchange { get { return GrandTotal * ExchangeRate; } }


        [DisplayName("CR Number")]
        public string BankReceivedNumber { get; set; }

        [DisplayName("CR Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? BankReceivedAmount { get; set; }

        [DisplayName("CR Status")]
        public string BankReceivedStatus { get; set; }

        [DisplayName("CR Status")]
        public string BankReceivedStatusDesc { get { return PaymentRequestStatus.GetStatus(BankReceivedStatus); } }

        public string BankReceivedCurrencySymbol { get; set; }
    }

    public class MultipleBrand
    {
        [DisplayName("Client Brand")]
        public string BrandId { get; set; }

        [DisplayName("Client Brand")]
        public string ClientBrandName { get; set; }

        [DisplayName("Address")]
        public string ClientInvAddress { get; set; }

        [DisplayName("PIC")]
        public string ClientPIC { get; set; }

        [DisplayName("Term Of Payment")]
        public int? TermOfPayment { get; set; }
    }

    public class FilterInvoiceClient
    {

        [DisplayName("Status")]
        public string Status { get; set; }

        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }

        [DisplayName("Date")]
        public string DateType { get; set; }
    }
}
