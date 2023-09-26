using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.ViewModel
{
    [Table("vw_aging_ar", Schema = "fn")]
    public class VwAgingAr
    {
        [Key]
        public string Id { get; set; }

        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        [Column("invoice_received_date")]
        public DateTime? InvoiceReceivedDate { get; set; }

        [Column("legal_entity_id")]
        public string LegalEntityId { get; set; }

        [Column("legal_entity_name")]
        public string LegalEntityName { get; set; }

        [Column("business_unit_id")]
        public string BusinessUnitId { get; set; }

        [Column("unit_name")]
        public string BusinessUnitName { get; set; }

        [Column("client_id")]
        public string ClientId { get; set; }

        [Column("client_name")]
        public string ClientName { get; set; }

        [Column("mainservice_category_id")]
        public string MainserviceCategoryId { get; set; }

        [Column("mainservice_category_name")]
        public string MainserviceCategoryName { get; set; }

        [Column("job_id")]
        public string JobId { get; set; }

        [Column(nameof(Jobs))]
        public string Jobs { get; set; }

        [Column("type_of_expense_id")]
        public string TypeOfExpenseId { get; set; }

        [Column("type_of_expense_name")]
        public string TypeOfExpenseName { get; set; }
        [Column("invoice_number")]
        public string InvoiceNumber { get; set; }

        [Column("currency_id")]
        public string CurrencyId { get; set; }

        [Column("currency_name")]
        public string CurrencyName { get; set; }
        
        [Column("currency_symbol")]
        public string CurrencySymbol { get; set; }

        [Column("brand_name")]
        public string BrandName { get; set; }

        [Column("delivery_note")]
        public string DeliveryNote { get; set; }

        [Column("invoice_delivery_date")]
        public DateTime? InvoiceDeliveryDate { get; set; }

        [Column("po_client_no")]
        public string PoClientNo { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [Column(nameof(Days))]
        public int Days { get; set; }

        [Column("amount")]
        public decimal? Amount { get; set; }

        [Column("current")]
        public decimal? Current { get; set; }

        [Column("01-30")]
        public decimal? Value0130 { get; set; }

        [Column("31-60")]
        public decimal? Value3160 { get; set; }

        [Column("61-90")]
        public decimal? Value6190 { get; set; }

        [Column(">90")]
        public decimal? ValueMoreThan90 { get; set; }

    }

    public class VwAgingArFilter
    {
        public string ClientId { get; set; }
        public string LegalEntityId { get; set; }
        public string MainserviceCategoryId { get; set; }
        public string JobId { get; set; }
        public string TypeOfExpenseId { get; set; }
        public string CurrencyId { get; set; }
        public decimal? OriginalAmount { get; set; }
    }
}
