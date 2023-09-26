using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("request_for_quotation", Schema = "fn")]
    public class RequestForQuotation : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("request_for_quotation_number"), StringLength(50)]
        public string RequestForQuotationNumber { get; set; }

        [Column("request_for_quotation_date")]
        public DateTime? RequestForQuotationDate { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("affiliation_id"), StringLength(50)]
        public string AffiliationId { get; set; }

        //[MaxLength(450)]
        //[Column("brand_name"), StringLength(450)]
        //public string BrandName { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [MaxLength(50)]
        [Column("request_currency_id"), StringLength(50)]
        public string RequestCurrencyId { get; set; }

        //[Column("exchange_rate")]
        //public decimal? ExchangeRate { get; set; }

        [MaxLength(50)]
        [Column("master_location_procurement_id"), StringLength(50)]
        public string MasterLocationProcurementId { get; set; }

        [MaxLength(450)]
        [Column("delivery_place"), StringLength(450)]
        public string DeliveryPlace { get; set; }

        [Column("delivery_date")]
        public DateTime? DeliveryDate { get; set; }

        [MaxLength(50)]
        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; }

        [MaxLength(50)]
        [Column("job_pa_id"), StringLength(50)]
        public string JobPaId { get; set; }

        [MaxLength(50)]
        [Column("job_pe_id"), StringLength(50)]
        public string JobPeId { get; set; }

        [Column("is_prefered_vendor")]
        public bool? IsPreferedVendor { get; set; }

        [MaxLength(50)]
        [Column("purchase_request_id"), StringLength(50)]
        public string PurchaseRequestId { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [MaxLength(50)]
        [Column("mainservice_category_id"), StringLength(50)]
        public string MainserviceCategoryId { get; set; }

        [MaxLength(50)]
        [Column("type_of_expense_id"), StringLength(50)]
        public string TypeOfExpenseId { get; set; }

        [MaxLength(450)]
        [Column(nameof(Remarks)), StringLength(450)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        [Column("internal_or_project"), StringLength(50)]
        public string InternalOrProject { get; set; }

        [Column(nameof(Status))]
        public string Status { get; set; }

        //Not Mapped For List
        [NotMapped]
        public string MainserviceCategoryName { get; set; }
        [NotMapped]
        public string RequestorName { get; set; }
        [NotMapped]
        public string BusinessUnitName { get; set; }
        [NotMapped]
        public string LegalEntityName { get; set; }
        [NotMapped]
        public string JobName { get; set; }

        [NotMapped]
        public RequestForQuotationVendor RequestForQuotationVendor { get; set; }

        [NotMapped]
        public List<RequestForQuotationVendor> RequestForQuotationVendors { get; set; }

        [NotMapped]
        public List<RequestForQuotationDetail> RequestForQuotationDetails { get; set; }

        [NotMapped]
        public List<RequestForQuotationTask> RequestForQuotationTasks { get; set; }

        //For Approval
        [NotMapped]
        public int StatusApproval { get; set; }
        [NotMapped]
        public string RemarksProcess { get; set; }

        [NotMapped]
        public IFormFile file { get; set; }

        //Kebutuhan buat internal PR (check budget master)
        [NotMapped]
        public int? BudgetYear { get; set; }
        [NotMapped]
        public string BudgetOwnerId { get; set; }
        [NotMapped]
        public string BudgetDivisionId { get; set; }

        [MaxLength(50)]
        [Column("detail_spec_file"), StringLength(50)]
        public string DetailSpecFile { get; set; }

        [NotMapped]
        public string DetailSpecFileName { get; set; }

        [NotMapped]
        public decimal? GetVatPercent { get; set; }

        [NotMapped]
        public decimal? GrandTotal { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }
        
    }
}