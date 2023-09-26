using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class PcaTask : PmEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Pca Id")]
        public string PcaId { get; set; }

        [DisplayName("Task")]
        public string TaskName { get; set; }

        [DisplayName("Qty")]
        public int? Quantity { get; set; }

        [DisplayName("Unit")]
        public string UomId { get; set; }

        [DisplayName("Unit Cost")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Price { get; set; }

        [DisplayName("Total Cost (Unit Cost * Qty)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalPrice { get; set; }

        [DisplayName("Total used")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalPriceUsed { get; set; }

        [DisplayName("Remaining Budget")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalPriceBalance { get; set; }

        [DisplayName("Use PPN")]
        public bool? IsPpn { get; set; }

        [DisplayName("PPN")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Ppn { get; set; }

        [DisplayName("Use ASF")]
        public bool? IsAsf { get; set; }

        [DisplayName("ASF")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Asf { get; set; }

        [DisplayName("PPH")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Pph { get; set; }

        [DisplayName("Total Cost")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Cost { get; set; }

        [DisplayName("Percentage (%)")]
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

        [DisplayName("Use Ratecard")]
        public bool IsRateCard { get; set; }

        [DisplayName("Parent Id")]
        public string ParentId { get; set; }

        [DisplayName("Status")]
        public int Status { get; set; }

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

        public bool IsSelect { get; set; }

        [DisplayName("Status")]
        public string StatusDesc { get { return PCAStatus.StatusName(Status); } }

        //[DisplayName("Unit")]
        //public string UomName { get; set; }

        [DisplayName("Unit")]
        public string UomSymbol { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencySymbolRequest { get; set; }
    }
}