using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class Pca : PmEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("Job")]
        public string JobId { get; set; }

        [DisplayName("Revision")]
        public string Revision { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Mother PCA")]
        public string MotherPcaId { get; set; }

        [DisplayName("Sub Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Subtotal { get; set; }

        [DisplayName("Other Fee")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? OtherFee { get; set; }

        [DisplayName("Other Fee Name")]
        public string OtherFeeName { get; set; }

        [DisplayName("Other Fee Percentage")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? OtherFeePercentage { get; set; }

        [DisplayName("VAT (10%)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Vat { get; set; }

        [DisplayName("Grand Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Total { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Type")]
        public string TypeOfExpenseId { get; set; }

        [DisplayName("Share Services")]
        public string ShareservicesId { get; set; }

        [DisplayName("Main Service Category")]
        public string MainserviceCategoryId { get; set; }

        [DisplayName("I")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Termofpayment1 { get; set; }

        [DisplayName("II")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Termofpayment2 { get; set; }

        [DisplayName("III")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Termofpayment3 { get; set; }

        [DisplayName("Rate Card")]
        public string RateCardId { get; set; }

        [DisplayName("Margin")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Margin { get; set; }

        [DisplayName("Cost Analysis")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? CostAnalysis { get; set; }

        [DisplayName("Total Price")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalTaskPrice { get; set; }

        [DisplayName("Total Price Used")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalTaskPriceUsed { get; set; }

        [DisplayName("Total Price Balance")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalTaskPriceBalance { get; set; }

        //Not Mapped
        public string CompanyId { get; set; }

        [DisplayName("Client Name")]
        public string CompanyName { get; set; }

        [DisplayName("Client brand")]
        public string ClientBrand { get; set; }

        [DisplayName("Job Number")]
        public string JobNumber { get; set; }

        [DisplayName("Job Name")]
        public string JobName { get; set; }

        [DisplayName("Brand")]
        public string BrandName { get; set; }

        [DisplayName("Status")]
        public int? StatusApproval { get; set; }

        [DisplayName("Remarks")]
        public string RemarkRejected { get; set; }

        [DisplayName("Remarks")]
        public string RemarkRFQ { get; set; }

        [DisplayName("Detail Spec")]
        public string DetailSpecFile { get; set; }

        public List<PcaTask> PcaTasks { get; set; }

        public string StatusPCE { get; set; }
        public bool IsPrefered { get; set; }

        [DisplayName("Due Date")]
        public DateTime? DueDate { get; set; }

        public double? TotalMotherPCA { get; set; }

        [DisplayName("Outlet Name")]
        public string MasterOutletId { get; set; }

        [DisplayName("Outlet Location")]
        public string MasterOutletLocation { get; set; }

        [DisplayName("PCE ID")]
        public string PceId { get; set; }
        public string JobStatusId { get; set; }

        //FOR RFQ

        [DisplayName("Currency")]
        public string CurrencyRequestId { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRateRequest { get; set; }

        [DisplayName("Operator")]
        public string OperatorRequest { get; set; }

        public List<PcaTaskRequest> PcaTaskRequest { get; set; }

        public string DefaultCurrencyId { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencySymbolExchange { get; set; }
        public bool IsJobOwner { get; set; }
    }

    public class PcaTaskRequest
    {
        [DisplayName("Task Id")]
        public string TaskId { get; set; }

        [DisplayName("Name")]
        public string Task { get; set; }

        [DisplayName("Qty")]
        public int? Quantity { get; set; }

        [DisplayName("Uom")]
        public string UomId { get; set; }

        [DisplayName("Unit Price")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? UnitPrice { get; set; }

        [DisplayName("Total Price")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalPrice { get; set; }

        [DisplayName("Job Id")]
        public string JobId { get; set; }

        [DisplayName("Uom")]
        public string UomName { get; set; }

        [DisplayName("Remaining Budget")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? RemainingBudget { get; set; }

        [DisplayName("Total Request")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalRequest { get; set; }

        [DisplayName("Total Exchange")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalExchangeRequest { get; set; }
        
        public string CurrencySymbol { get; set; }

        public string CurrencySymbolExchange { get; set; }
    }
}