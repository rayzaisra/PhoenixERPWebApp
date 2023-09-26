using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
     public class PcaDto : PcaNew
    {
        public string Id { get; set; }
        public string job_name { get; set; }
        public string mother_pca_name { get; set; }
        public string type_of_expense_name { get; set; }
        public string type_of_expense_id { get; set; }
        public string rate_card_name { get; set; }

        public string currency_name { get; set; }
        public string brand_name { get; set; }
        public string company_name { get; set; }
        public string shareservice_name { get; set; }
        public string mainservice_name { get; set; }
        public string StatusApproved { get; set; }
        public string pca_task_id { get; set; }
        public List<PcaSubTask> sub_task { get; set; }
    }
    public class PcaList : PcaDto
    {
    }
        public class PcaNew
    {
       
        public string code { get; set; }
        public string type_of_expense_id { get; set; }
        public string pca_id { get; set; }
        public string pca_refrence { get; set; }
        public string job_id { get; set; }
        public List<PcaTaskDto> detail { get; set; }
        public string name { get; set; }
        public string revision { get; set; }
        public decimal? total { get; set; }
        public string currency { get; set; }
        public string mother_pca_id { get; set; }
        public string rate_card_id { get; set; }
        public decimal? subtotal { get; set; }
        public decimal? other_fee { get; set; }
        public string other_fee_name { get; set; }
        public decimal? other_fee_percentage { get; set; }
        public decimal? vat { get; set; }
        public string status { get; set; }
        public string shareservices_id { get; set; }
        public string mainservice_category_id { get; set; }
        public decimal? termofpayment1 { get; set; }
        public decimal? termofpayment2 { get; set; }
        public decimal? termofpayment3 { get; set; }
    }
    public class PcaTaskDto
    {
        public string Id { get; set; }
        public decimal? price { get; set; }
        public string task_name { get; set; }
        public string departement { get; set; }
        public string departement_name { get; set; }
        public int quantity { get; set; }
        public string unit { get; set; }

        public decimal? cost { get; set; }
        public int percentage { get; set; }
        public decimal? ppn { get; set; }
        public decimal? asf { get; set; }
        public decimal? pph { get; set; }
        public decimal? margin { get; set; }
        public decimal? total { get; set; }
        public List<PcaSubTask> sub_task { get; set; }
    }
    public class PcaSubTask
    {
        public string id { get; set; }
        public string brand { get; set; }
        public string category_id { get; set; }
        public string category_name { get; set; }
        public long price { get; set; }
        public int quantity { get; set; }
        public string remarks { get; set; }
        public string subbrand { get; set; }
        public string subtask { get; set; }

        public long total { get; set; }

    }
    public class PcaCa
    {
        public string Id { get; set; }
        public string ca_number { get; set; }
        public DateTime? ca_date { get; set; }
        public int revision { get; set; }
        public string requested_by { get; set; }
        public string requested_on { get; set; }
        public string legal_entity_id { get; set; }
        public string business_unit_id { get; set; }
        public bool is_project_cost { get; set; }
        public string purchase_order_id { get; set; }
        public string purchase_order_number { get; set; }
        public string job_id { get; set; }
        public string job_number { get; set; }
        public string client_name { get; set; }
        public string project_activity { get; set; }
        public string project_area { get; set; }
        public string vendor_id { get; set; }
        public string vendor_name { get; set; }
        public string employee_id { get; set; }
        public string notes { get; set; }
        public string payee_bank_id { get; set; }
        public string payee_account_number { get; set; }
        public string payee_account_name { get; set; }
        public string payee_bank_branch { get; set; }
        public DateTime? ca_period_from { get; set; }
        public DateTime? ca_period_to { get; set; }
        public string approval_id { get; set; }
        public string ca_status_id { get; set; }

    }
    public class PcaRfq
    {
        public string Id { get; set; }
        public string job_pa_id { get; set; }
        public string request_for_quotation_number { get; set; }
        public string legal_entity_id { get; set; }
        public string business_unit_id { get; set; }
        public string brand_name { get; set; }
        public string currency_id { get; set; }
        public decimal? exchange_rate { get; set; }
        public DateTime? delivery_date { get; set; }
        public DateTime? closing_date { get; set; }
        public string term_of_payment { get; set; }
        public string job_id { get; set; }
        public bool? is_prefered_vendor { get; set; }
        public List<PcaRfqDetail> details { get; set; }
  
    }

    public class PcaRfqDetail
    {
        public string request_for_quotation_id { get; set; }
        public string item_id { get; set; }
        public string item_name { get; set; }
        public string item_type { get; set; }
        public string uom_id { get; set; }
        public string subbrand_name { get; set; }
        public string description { get; set; }
        public decimal qty { get; set; }
        public int? qty_shift { get; set; }
        public int? qty_day { get; set; }
        public int? qty_week { get; set; }
        public int? qty_city { get; set; }
        public decimal unit_price { get; set; }
        public string currency_id { get; set; }
        public decimal exchange_rate { get; set; }
        public decimal? amount { get; set; }
        public string category_id { get; set; }
        public string filemaster_id { get; set; }
        public string task { get; set; }
        public string subtask { get; set; }
        public int revision_status_pca { get; set; }
    }

    public class PcaRfqTask
    {
        public string Id { get; set; }
        public string request_for_quotation_id { get; set; }
        public string task { get; set; }
        public string subtask { get; set; }
        public int quantity { get; set; }
    }

    public class PcaSubmit
    {
        public string Id { get; set; }
        public string RefId { get; set; }
        public string TemplateName { get; set; }
        public string SubGroupId { get; set; }
        public string DivisionId { get; set; }
        public bool? IsNotifyByEmail { get; set; }
        public bool? IsNotifyByWeb { get; set; }
        public int? DueDate { get; set; }
        public int? Reminder { get; set; }
        public int? ApprovalCategory { get; set; }
        public int? ApprovalType { get; set; }
        public int? StatusApproved { get; set; }
        public string RemarkRejected { get; set; }
        public string DetailLink { get; set; }
        public string FormReqName { get; set; }
        public string RejectedBy { get; set; }
        public DateTime? RejectedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
    }

    public class MainServiceCategoryDto
    {
        public string id { get; set; }
        public string  name { get; set; }
    }

    public class PcaSearch : SearchParameter
    {
 
        
    }
}