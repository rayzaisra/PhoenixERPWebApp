using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class RequestForQuotationVendor : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Request For Quotation Id")]
        public string RequestForQuotationId { get; set; }

        [DisplayName("Vendor Name")]
        public string VendorId { get; set; }

        [DisplayName("Sub Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? SubTotalVendor { get; set; }

        [DisplayName("Vat Percent")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? VatPercent { get; set; }

        [DisplayName("Vat")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Vat { get; set; }

        [DisplayName("Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalAmountVendor { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Is Request Purchase Order")]
        public bool? IsRequestPurchaseOrder { get; set; }

        [DisplayName("Filemaster Id")]
        public string FilemasterId { get; set; }

        //NotMapped
        [DisplayName("Vendor Name")]
        public string VendorName { get; set; }

        [DisplayName("File")]
        public string FilemasterName { get; set; }

        public List<RequestForQuotationDetail> RequestForQuotationDetails { get; set; }
        public IFormFile File { get; set; }


        [DisplayName("Currency")]
        public string CurrencyRequestId { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRateRequest { get; set; }

        [DisplayName("Operator")]
        public string OperatorRequest { get; set; }

        public string CurrencySymbol { get; set; }
    }
}
