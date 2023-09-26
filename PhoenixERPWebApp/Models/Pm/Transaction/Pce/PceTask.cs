using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class PceTask : PmEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Pce Id")]
        public string PceId { get; set; }

        [DisplayName("Task Name")]
        public string TaskName { get; set; }

        [DisplayName("Quantity")]
        public int? Quantity { get; set; }

        [DisplayName("Unit")]
        public string UomId { get; set; }

        [DisplayName("Unit Cost")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Price { get; set; }

        [DisplayName("Use PPN")]
        public bool? IsPpn { get; set; }

        [DisplayName("Ppn")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Ppn { get; set; }

        [DisplayName("Use ASF")]
        public bool? IsAsf { get; set; }

        [DisplayName("Asf")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Asf { get; set; }

        [DisplayName("Pph")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Pph { get; set; }

        [DisplayName("Total Cost")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Cost { get; set; }

        [DisplayName("Percentage")]
        public decimal? Percentage { get; set; }

        [DisplayName("Margin")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Margin { get; set; }

        [DisplayName("Unit Price")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? UnitPrice { get; set; }

        [DisplayName("Total Price")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Total { get; set; }

        [DisplayName("Departement")]
        public string Departement { get; set; }

        [DisplayName("Is Rate Card")]
        public bool? IsRateCard { get; set; }

        [DisplayName("Parent Id")]
        public string ParentId { get; set; }

        public decimal? TotalUsedInvoice { get; set; }

        public decimal? TotalBalanceInvoice { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Operator")]
        public string Operator { get; set; }

        [DisplayName("Total Price Exchange")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalOriginal { get; set; }

        //Not Mapped
        [DisplayName("Departement")]
        public string DepartementName { get; set; }

        //[DisplayName("Unit")]
        //public string UomName { get; set; }

        [DisplayName("Unit")]
        public string UomSymbol { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencySymbolRequest { get; set; }
    }
}