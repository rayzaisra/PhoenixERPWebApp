using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class Pce : PmEntityBase
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

        [DisplayName("PCA Id")]
        public string PcaId { get; set; }

        //[DisplayName("Total Invoicing")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public decimal? CurrentInvoice { get; set; }

        //[DisplayName("Total PCE Balance")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public decimal? Balance { get; set; }

        [DisplayName("PCE Reference")]
        public string PceReference { get; set; }

        [DisplayName("PO Client Number")]
        public string PoClient { get; set; }

        [DisplayName("Total PO Client")]
        public decimal? TotalPoClient { get; set; }

        //Not Mapped
        public string CompanyId { get; set; }

        [DisplayName("Client Name")]
        public string CompanyName { get; set; }

        [DisplayName("Client brand")]
        public string ClientBrand { get; set; }

        [DisplayName("Job Name")]
        public string JobName { get; set; }

        [DisplayName("Brand")]
        public string BrandName { get; set; }

        [DisplayName("Status")]
        public int? StatusApproval { get; set; }

        [DisplayName("Remarks")]
        public string RemarkRejected { get; set; }

        [DisplayName("Pce Margin")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? PceMargin { get; set; }

        [DisplayName("Total Invoicing")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalUsedInvoice { get; set; }

        [DisplayName("Total PCE Balance")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalBalanceInvoice { get; set; }

        [DisplayName("Sub Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? SubtotalOriginal { get; set; }

        [DisplayName("Other Fee")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? OtherFeeOriginal { get; set; }

        [DisplayName("VAT (10%)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? VatOriginal { get; set; }

        [DisplayName("Grand Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalOriginal { get; set; }

        [DisplayName("Pce Margin")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? PceMarginOriginal { get; set; }


        //Not Mapped
        public string MainServicesName { get; set; }

        public string external_brand_id { get; set; }
        public List<PceTask> PceTasks { get; set; }

        [DisplayName("Outlet Name")]
        public string MasterOutletId { get; set; }

        [DisplayName("Outlet Location")]
        public string MasterOutletLocation { get; set; }
        public bool IsPM { get; set; }

        [DisplayName("Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? SubTotalOtherFee { get { return Subtotal + OtherFee; } }

        [DisplayName("Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? SubTotalOtherFeeOriginal { get { return SubtotalOriginal + OtherFeeOriginal; } }

        [DisplayName("Currency")]
        public string CurrencySymbol { get; set; }

        [DisplayName("Currency")]
        public string CurrencySymbolOriginal { get; set; }
    }
}
