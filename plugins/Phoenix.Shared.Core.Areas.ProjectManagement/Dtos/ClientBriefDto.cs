using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Phoenix.Shared.Helpers;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class ClientBriefDto
    {
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        public string assigment_id { get; set; }
        [StringLength(50)]
        public string business_unit_id { get; set; }
        [StringLength(50)]
        public string bussiness_type_id { get; set; }
        [StringLength(50)]

        public string campaign_name { get; set; }
        public long? campaign_progress_id { get; set; }
        public long? campaign_status_id { get; set; }
        [StringLength(50)]
        public string campaign_type_id { get; set; }
        [StringLength(50)]
        public string client_brief_file_id { get; set; }
        public DateTime? deadline { get; set; }
        [StringLength(50)]
        public string external_brand_id { get; set; }
        public DateTime? final_delivery { get; set; }
        public DateTime? finish_campaign { get; set; }
        [StringLength(50)]
        public string job_category_id { get; set; }
        public long? job_pe_id { get; set; }
        [StringLength(50)]
        public string nationality_id { get; set; }
        public long? proposal_file_id { get; set; }
        public long? responsible_person { get; set; }
        public DateTime? start_campaign { get; set; }
        public string brand_name { get; set; }

        public string business_unit_name { get; set; }
        public string job_id { get; set; }
        public List<string> job_number { get; set; }
        public List<string> job_name { get; set; }
        public string created_by { get; set; }
        public DateTime? created_on { get; set; }
        public bool? is_deleted { get; set; }

    }
    public class JobClientbriefDto
    {
        public string Id { get; set; }

        public string job_name { get; set; }
    }
    public class ClientBriefNew
    {
        public string Id { get; set; }
        public List<ClientBriefContentNew> content { get; set; }
        [Required, StringLength(50)]
        public string business_unit_id { get; set; }
        [Required, StringLength(50)]
        public string bussiness_type_id { get; set; }
        [Required, StringLength(50)]
        public string campaign_name { get; set; }
        [Required, StringLength(50)]
        public string campaign_type_id { get; set; }
        [DateGreaterThan("deadline")]
        public DateTime? start_campaign { get; set; }
        [DateGreaterThan("final_delivery")]
        public DateTime? deadline { get; set; }
        [StringLength(50)]
        public string external_brand_id { get; set; }
        public DateTime? final_delivery { get; set; }
        public DateTime? finish_campaign { get; set; }
        [StringLength(50)]
        public string job_category_id { get; set; }
        public long? job_pe_id { get; set; }
        public string client_id { get; set; }

    }
    public class ClientBriefUpdate
    {

        public string Id { get; set; }
        public List<ClientBriefContentNew> content { get; set; }
        public List<string> account_management_id { get; set; }
        [StringLength(50)]
        public string assigment_id { get; set; }
        [StringLength(50)]
        public string business_unit_id { get; set; }
        [StringLength(50)]
        public string bussiness_type_id { get; set; }
        [StringLength(50)]
        public string campaign_name { get; set; }
        [StringLength(50)]
        public string campaign_type_id { get; set; }
        [StringLength(50)]
        public string client_brief_file_id { get; set; }
        public DateTime? deadline { get; set; }
        [StringLength(50)]
        public string external_brand_id { get; set; }
        public DateTime? final_delivery { get; set; }
        public DateTime? finish_campaign { get; set; }
        [StringLength(50)]
        public string job_category_id { get; set; }
        public long? job_pe_id { get; set; }
        [StringLength(50)]
        public string nationality_id { get; set; }
        public long? proposal_file_id { get; set; }
        public long? responsible_person { get; set; }
        public DateTime? start_campaign { get; set; }

        public string client_id { get; set; }
    }
    public class ClientBriefDetil
    {
        public string Id { get; set; }
        public List<ClientBriefContentDto> content { get; set; }
        public List<JobDetailDto> job_detil { get; set; }
        public List<AccountManagementList> account_management { get; set; }
        public string assigment_id { get; set; }
        public string business_unit_id { get; set; }
        public string brand_name { get; set; }
        [StringLength(50)]
        public string bussiness_type_id { get; set; }
        public string bussiness_type_name { get; set; }
        public string business_unit_name { get; set; }
        [StringLength(50)]
        public string campaign_name { get; set; }

        public long? campaign_progress_id { get; set; }
        public long? campaign_status_id { get; set; }
        [StringLength(50)]
        public string campaign_type_id { get; set; }
        public string campaign_type_name { get; set; }
        [StringLength(50)]
        public string client_brief_file_id { get; set; }
        public DateTime? deadline { get; set; }
        [StringLength(50)]
        public string external_brand_id { get; set; }
        public DateTime? final_delivery { get; set; }
        public DateTime? finish_campaign { get; set; }
        [StringLength(50)]
        public string job_category_id { get; set; }
        public long? job_pe_id { get; set; }
        [StringLength(50)]

        public DateTime? start_campaign { get; set; }

        public string client_id { get; set; }
        public string company_name { get; set; }
    }
    public class mpca : ClientBriefDetil
    {
        public string approval_id { get; set; }
        public string currency { get; set; }
        public string detail { get; set; }
        public string client_brief_id { get; set; }
        public DateTime? pa_date { get; set; }
        public string pa_number { get; set; }
        public decimal? total { get; set; }
    }

    public class CompanyClientDto
    {
        public string id { get; set; }
        public string name { get; set; }

    }

    public class mpcaform
    {
        public string approval_id { get; set; }
        public string currency { get; set; }
        public string detail { get; set; }
        public string client_brief_id { get; set; }
        public DateTime? pa_date { get; set; }
        public string pa_number { get; set; }
        public decimal? total { get; set; }
    }
    public class ClientBriefSearch : SearchParameter
    {
        public string Search_business_unit_id { get; set; }
        public string Search_bussiness_type_id { get; set; }
 
        public string Search_campaign_name { get; set; }
        public string Search_campaign_type_id { get; set; }
        public string Search_external_brand_id { get; set; }

        public string Search_client_brief_id { get; set; }
        public string Search_is_deleted { get; set; }

        public string user_id { get; set; }
        public string group_id { get; set; }
    }
}