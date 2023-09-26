using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Phoenix.Shared.Core.Areas.General.Dtos;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class JobDetailDto
    {
        [Required]
        public string Id { get; set; }
        public double? cash_advance { get; set; }
        [StringLength(50)]
        public string client_brief_id { get; set; }
        [StringLength(50)]
        public string company_id { get; set; }
        public DateTime? deadline { get; set; }
        public DateTime? final_delivery { get; set; }

        public DateTime? start_job { get; set; }
        public DateTime? finish_job { get; set; }

        public string job_description { get; set; }
        [StringLength(150)]
        public string job_name { get; set; }
        [StringLength(50)]
        public string job_number { get; set; }
        [StringLength(50)]
        public string job_status_detail { get; set; }
        [StringLength(50)]
        public string job_status_id { get; set; }
        public double? pa { get; set; }
        public double? pe { get; set; }
        [StringLength(50)]
        public string pm_id { get; set; }
        public string job_status_name { get; set; }
    }
    public class JobDetailList
    {
        public string Id { get; set; }
        public string client_brief_id { get; set; }
        public string company_name { get; set; }
        public string brand_name { get; set; }
        public string brand_id { get; set; }
        public string company_id { get; set; }
        public string status_name { get; set; }
        public DateTime? deadline { get; set; }
        public DateTime? final_delivery { get; set; }
        public DateTime? start_job { get; set; }
        public DateTime? finish_job { get; set; }
        public string job_description { get; set; }
        public string job_name { get; set; }
        public string job_type { get; set; }
        public string job_type_name { get; set; }
        public string job_number { get; set; }
        public string job_status_id { get; set; }
        public string campaign_name { get; set; }
        public string team_id { get; set; }
        public string team_name { get; set; }

        public string legal_entity_id { get; set; }
        public string legal_entity_name { get; set; }
        public string business_unit_id { get; set; }

        public string business_unit_division_id { get; set; }
        public string business_unit_departement_id { get; set; }
        public string business_unit_departement_name { get; set; }
        public string main_service_category { get; set; }
        public string main_service_category_name { get; set; }
        public string pm_id { get; set; }
        public string business_unit_name { get; set; }
        public string pm { get; set; }
        public DateTime? created_on { get; set; }
        public List<string> ListTeamMember { get; set; }

    }
    public class JobDetailForm
    {
        public string Id { get; set; }
        public string client_brief_id { get; set; }
        public string company_name { get; set; }
        public string departement_id { get; set; }
        public string departement_name { get; set; }
        public string affliation_id { get; set; }
        public string affliation_name { get; set; }
        public string brand_name { get; set; }
        public string brand_id { get; set; }
        public string company_id { get; set; }
        public string status_name { get; set; }
        public DateTime? deadline { get; set; }
        public DateTime? final_delivery { get; set; }
        public DateTime? start_job { get; set; }
        public DateTime? finish_job { get; set; }
        public string job_description { get; set; }
        public string job_name { get; set; }
        public string job_number { get; set; }
        public string job_type { get; set; }
        public string job_type_name { get; set; }
        public string job_status_id { get; set; }
        public string campaign_name { get; set; }
        public string created_by { get; set; }
        public string pm_id { get; set; }
        public string business_unit_name { get; set; }
        public string pm { get; set; }
        public List<Centralresourcelist> Centralresource { get; set; }
        public List<string> Accountmanager { get; set; }
        public List<UserList> TeamMember { get; set; }
        public PcaDto pca { get; set; }
        public PceDto pce { get; set; }
        public string legalEntityID { get; set; }
        public string legalEntityName { get; set; }
        public string business_type { get; set; }
        public string business_type_name { get; set; }
    }
    public class JobDetailPca : JobDetailForm
    {

        public List<PaTaskList> Pcatask { get; set; }
        public mpcaform MotherPca { get; set; }
        public string approval_id { get; set; }

        public string department { get; set; }
        public string currency { get; set; }
        public string feetype { get; set; }
        public double? fee_percentage { get; set; }
        public double? feetotal { get; set; }
        public int? hour_per_day { get; set; }
        public string job_detail_id { get; set; }
        public string mother_pca { get; set; }

        public DateTime? pa_date { get; set; }
        public string pa_number { get; set; }
        public string rate_card_id { get; set; }
    }
    public class JobDetailFormWithTask : JobDetailForm
    {
        public List<TaskList> Tasklist { get; set; }
    }

    public class jTaskList
    {
        public string id { get; set; }

        public string task_name { get; set; }
        public List<jSubTaskList> sub_task { get; set; }

    }

    public class DashJob
    {
        public string Id { get; set; }

        public string job_name { get; set; }
    }

    public class jSubTaskList
    {
        public string id { get; set; }

        public string subtask { get; set; }

    }
    public class UserAcountList
    {

        public string Id { get; set; }

        public string app_username { get; set; }
        public string app_fullname { get; set; }
        public string user_id { get; set; }
    }
    public class Centralresourcelist
    {

        public string Id { get; set; }

        public string name { get; set; }

    }
    public class JobAssignPM
    {
        public string Id { get; set; }
        public string user_id { get; set; }
    }
    public class JobAssignStatus
    {
        public string job_detail_id { get; set; }
        public string job_status_id { get; set; }
        public List<DamStatus> content { get; set; }
    }
    public class DamStatus
    {
        public string id { get; set; }
        public string dam_type_id { get; set; }
        public string file_id { get; set; }
    }
    public class JobAssignCentral
    {
        public string Id { get; set; }
        public List<string> central_resource_id { get; set; }
    }
    public class JobAssignBoard
    {
        public string Id { get; set; }
        public string central_resource_id { get; set; }
    }
 
public class JobStatusForm
{
    public string Id { get; set; }
    public string job_status_id { get; set; }
    public List<job_status_file> dam_file { get; set; }
}
    public class job_status_file
    {
        public string id { get; set; }
        public string file_id { get; set; }
        public string dam_type_id { get; set; }
        public string dam_type_name { get; set; }
    }
    public class MasterDivisionList
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class MasterDepartmentList
    {
        public string id { get; set; }
        public string name { get; set; }

        
    }

    public class BrandListDto
    {
        public string id { get; set; }
        public string brand_name { get; set; }
    }
    public class MainServiceList
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class JobDetailNew
    {
 
        public string job_number { get; set; }
        [Required,StringLength(50)]
        public string client_brief_id { get; set; }
 
        public DateTime? deadline { get; set; }
        public DateTime? final_delivery { get; set; }
        public DateTime? start_job { get; set; }
        [StringLength(4000)]
        public string job_description { get; set; }
        [Required, StringLength(50)]
        public string job_type { get; set; }
        [Required, StringLength(50)]
        public string team_id { get; set; }
        [Required, StringLength(150)]
        public string job_name { get; set; }
        public string legal_entity_id { get; set; }
        public string business_unit_id { get; set; }
        public string business_unit_division_id { get; set; }
        public string business_unit_departement_id { get; set; }

        public string main_service_category { get; set; }
        public string brand_id { get; set; }

    }
    public class JobDetailUpdate
    {
        [Required, StringLength(50)]
        public string id { get; set; }

        public DateTime? deadline { get; set; }
        public DateTime? final_delivery { get; set; }
        public DateTime? start_job { get; set; }
        [StringLength(4000)]
        public string job_description { get; set; }
        //[Required, StringLength(50)]
        //public string job_type { get; set; }
        //[Required, StringLength(50)]
        public string job_type { get; set; }
        //[Required, StringLength(50)]
        //public string team_id { get; set; }
        //[Required, StringLength(50)]
        public string team_id { get; set; }
        [Required, StringLength(150)]
        public string job_name { get; set; }


    }
    public class JobPMSearch : SearchParameter
    {
        

    }
    public class JobDetailSearch : SearchParameter
    {

        public string Search_client_brief_id { get; set; }
        public string Search_company_id { get; set; }
        public string Search_company_name { get; set; }
        public string Search_job_name { get; set; }
        public string Search_brand_name { get; set; }
        public string Search_job_number { get; set; }
        public string Search_job_status_detail { get; set; }
        public string Search_job_status_id { get; set; }
        public string Search_pm_id { get; set; }
        public string group_id { get; set; }
        public string user_id { get; set; }

    }

}