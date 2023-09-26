using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    [Table("cash_advance", Schema = "fn")]
    public class CashAdvance : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("ca_number"), StringLength(50)]
        public string CaNumber { get; set; }

        [Column("ca_date")]
        public DateTime CaDate { get; set; }

        [Column(nameof(Revision))]
        public int? Revision { get; set; }

        [MaxLength(50)]
        [Column("mainservice_category_id"), StringLength(50)]
        public string MainserviceCategoryId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("affiliation_id"), StringLength(50)]
        public string AffiliationId { get; set; }

        [MaxLength(50)]
        [Column("share_service_id"), StringLength(50)]
        public string ShareServiceId { get; set; }

        [Column("is_project_cost")]
        public bool? IsProjectCost { get; set; }

        [MaxLength(50)]
        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; }

        [MaxLength(50)]
        [Column("pca_id"), StringLength(50)]
        public string PcaId { get; set; }

        [MaxLength(50)]
        [Column("pce_id"), StringLength(50)]
        public string PceId { get; set; }

        [MaxLength(50)]
        [Column("client_id"), StringLength(50)]
        public string ClientId { get; set; }

        [MaxLength(400)]
        [Column("client_name"), StringLength(400)]
        public string ClientName { get; set; }

        [MaxLength(50)]
        [Column("vendor_id"), StringLength(50)]
        public string VendorId { get; set; }

        [MaxLength(450)]
        [Column("vendor_name"), StringLength(450)]
        public string VendorName { get; set; }

        [MaxLength(150)]
        [Column("payee_bank_id"), StringLength(150)]
        public string PayeeBankId { get; set; }

        [MaxLength(50)]
        [Column("payee_account_number"), StringLength(50)]
        public string PayeeAccountNumber { get; set; }

        [MaxLength(450)]
        [Column("payee_account_name"), StringLength(450)]
        public string PayeeAccountName { get; set; }

        [Column("ca_period_from")]
        public DateTime? CaPeriodFrom { get; set; }

        [Column("ca_period_to")]
        public DateTime? CaPeriodTo { get; set; }

        [MaxLength(50)]
        [Column("ca_status_id"), StringLength(50)]
        public string Status { get; set; }

        [Column(nameof(Amount))]
        public decimal? Amount { get; set; }

        [MaxLength(50)]
        [Column("client_brand_id"), StringLength(50)]
        public string ClientBrandId { get; set; }

        [MaxLength(50)]
        [Column("pce_type_id"), StringLength(50)]
        public string PceTypeId { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [Column("operator"), StringLength(50)]
        public string Operator { get; set; }

        [MaxLength(50)]
        [Column("request_business_unit_id"), StringLength(50)]
        public string RequestBusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("request_employee_id"), StringLength(50)]
        public string RequestEmployeeId { get; set; }

        [MaxLength(150)]
        [Column("request_employee_name"), StringLength(150)]
        public string RequestEmployeeName { get; set; }

        [MaxLength(50)]
        [Column("request_job_title_id"), StringLength(50)]
        public string RequestJobTitleId { get; set; }

        [MaxLength(150)]
        [Column("request_job_title_name"), StringLength(150)]
        public string RequestJobTitleName { get; set; }

        [MaxLength(50)]
        [Column("request_job_grade_id"), StringLength(50)]
        public string RequestJobGradeId { get; set; }

        [MaxLength(150)]
        [Column("request_job_grade_name"), StringLength(150)]
        public string RequestJobGradeName { get; set; }

        [MaxLength(150)]
        [Column("requested_by"), StringLength(150)]
        public string RequestedBy { get; set; }

        [Column("requested_on")]
        public DateTime? RequestedOn { get; set; }

        [MaxLength(50)]
        [Column("on_behalf_id"), StringLength(50)]
        public string OnBehalfId { get; set; }

        [NotMapped]
        public List<CashAdvanceDetail> CashAdvanceDetails { get; set; }

        //For Approval
        [NotMapped]
        public int StatusApproval { get; set; }
        [NotMapped]
        public string RemarksProcess { get; set; }


        //Vendor Information
        [NotMapped]
        public string VendorPIC { get; set; }

        [NotMapped]
        public string VendorAddress { get; set; }

        [NotMapped]
        public string VendorTaxNumber { get; set; }


        //List Information
        [NotMapped]
        public string JobName { get; set; }

        [NotMapped]
        public string MainServiceCategoryName { get; set; }

        [NotMapped]
        public string BusinessUnitName { get; set; }

        [NotMapped]
        public string LegalEntityName { get; set; }

        [NotMapped]
        public string RequestBusinessUnitName { get; set; }

        [NotMapped]
        public string SettlementNumber { get; set; }

        [NotMapped]
        public string PCACurrencyId { get; set; }

        [NotMapped]
        public string DefaultCurrencyId { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }

        [NotMapped]
        public string CurrencySymbolExchange { get; set; }


        //for list
        [NotMapped]
        public string PONumber { get; set; }

        [NotMapped]
        public string InvoiceReceivableNumber { get; set; }

        [NotMapped]
        public string TAPNumber { get; set; }

        [NotMapped]
        public string APNumber { get; set; }

        [NotMapped]
        public string StatusCASettlement { get; set; }

        [NotMapped]
        public bool IsReverse { get; set; }
    }
}
