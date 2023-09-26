using Microsoft.EntityFrameworkCore;
using Phoenix.Shared.Core.Contexts;
using Phoenix.Shared.Core.Entities.Finance;
using Phoenix.Shared.Core.Entities.ProjectManagement;

namespace Phoenix.Shared.Core.Entities
{
    public partial class AppDbContext : EFContext
    {
        public virtual DbSet<TimesheetNonJob> TimesheetNonJob { get; set; }
        public virtual DbSet<Pca> Pca { get; set; }
        public virtual DbSet<PCE> Pce { get; set; }
        public virtual DbSet<PcaTask> PcaTask { get; set; }
        public virtual DbSet<PceTask> PceTask { get; set; }
        public virtual DbSet<vTaskList> vTaskList { get; set; }
        public virtual DbSet<DamTypeStatus> DamTypeStatus { get; set; }
        public virtual DbSet<MotherPca> MotherPca { get; set; }
        public virtual DbSet<spjobnumber> spjobnumber { get; set; }
        public virtual DbSet<BrandAccount> BrandAccount { get; set; }
        public virtual DbSet<JobAccount> JobAccount { get; set; }
        public virtual DbSet<spClientBriefList> spClientBriefList { get; set; }
        public virtual DbSet<vClientBriefList> vClientBriefList { get; set; }
        public virtual DbSet<vPcatask> vPcatask { get; set; }
        public virtual DbSet<Pm> Pm { get; set; }
        public virtual DbSet<Dam> Dam { get; set; }
        public virtual DbSet<DamFileSharing> DamFileSharing { get; set; }
        public virtual DbSet<Bast> Bast { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<JobPa> JobPa { get; set; }
        public virtual DbSet<JobPe> JobPe { get; set; }
        public virtual DbSet<Channel> Channel { get; set; }
        public virtual DbSet<Package> Package { get; set; }
        public virtual DbSet<PeFile> PeFile { get; set; }
        public virtual DbSet<PeFileFolder> PeFileFolder { get; set; }
        public virtual DbSet<DamFolderMaster> DamFolderMaster { get; set; }
        public virtual DbSet<PoType> PoType { get; set; }
        public virtual DbSet<DamType> DamType { get; set; }
        public virtual DbSet<JobType> JobType { get; set; }
        public virtual DbSet<SubTask> SubTask { get; set; }
        public virtual DbSet<SubTaskUser> SubTaskUser { get; set; }
        public virtual DbSet<PaDetail> PaDetail { get; set; }
        public virtual DbSet<PaTask> PaTask { get; set; }
        public virtual DbSet<RateCard> RateCard { get; set; }
        public virtual DbSet<TaskUser> TaskUser { get; set; }
        public virtual DbSet<JobDetail> JobDetail { get; set; }
        public virtual DbSet<JobStatus> JobStatus { get; set; }
        //public virtual DbSet<PettyCashs> PettyCash { get; set; }
        public virtual DbSet<PeTemplate> PeTemplate { get; set; }
        public virtual DbSet<ReportFile> ReportFile { get; set; }
        public virtual DbSet<TaskStatus> TaskStatus { get; set; }
        public virtual DbSet<ClientBrief> ClientBrief { get; set; }
        public virtual DbSet<ProposeType> ProposeType { get; set; }
        public virtual DbSet<SubCampaign> SubCampaign { get; set; }
        public virtual DbSet<TaskComment> TaskComment { get; set; }
        public virtual DbSet<ApprovalType> ApprovalType { get; set; }
        public virtual DbSet<CampaignType> CampaignType { get; set; }
        public virtual DbSet<ProposalFile> ProposalFile { get; set; }
        public virtual DbSet<TaskPriority> TaskPriority { get; set; }
        public virtual DbSet<BussinessType> BussinessType { get; set; }
        public virtual DbSet<JobStatusSet> JobStatusSet { get; set; }
        public virtual DbSet<RateCardCode> RateCardCode { get; set; }
        public virtual DbSet<TaskTimesheet> TaskTimesheet { get; set; }
        public virtual DbSet<CampaignStatus> CampaignStatus { get; set; }
        public virtual DbSet<ClientFeedback> ClientFeedback { get; set; }
        public virtual DbSet<StatusAssigned> StatusAssigned { get; set; }
        public virtual DbSet<LabelAttachment> LabelAttachment { get; set; }
        public virtual DbSet<QuotationVendor> QuotationVendor { get; set; }
        public virtual DbSet<CampaignProgress> CampaignProgress { get; set; }
        public virtual DbSet<JobStatusDetail> JobStatusDetail { get; set; }
        public virtual DbSet<QuotationRequest> QuotationRequest { get; set; }
        public virtual DbSet<AccountManagement> AccountManagement { get; set; }
        public virtual DbSet<JobWorkflow> JobWorkflow { get; set; }
        public virtual DbSet<JobWorkflowMaster> JobWorkflowMaster { get; set; }
        public virtual DbSet<JobWorkflowStatus> JobWorkflowStatus { get; set; }
        public virtual DbSet<ClientBriefAccount> ClientBriefAccount { get; set; }
        public virtual DbSet<ClientBriefContent> ClientBriefContent { get; set; }
        public virtual DbSet<JobWorkflowHistory> JobWorkflowHistory { get; set; }
        public virtual DbSet<MasterSearchContent> MasterSearchContent { get; set; }
        public virtual DbSet<ClientBriefContentRef> ClientBriefContentRef { get; set; }
        public virtual DbSet<CentralResourceAssigment> CentralResourceAssigment { get; set; }
        public virtual DbSet<ClientBriefCentralResource> ClientBriefCentralResource { get; set; }
        public virtual DbSet<ClientBriefAccountmanagement> ClientBriefAccountmanagement { get; set; }
       
        public virtual DbSet<PcaRefrence> PcaRefrence { get; set; }
        public virtual DbSet<TypeOfExpense> TypeOfExpense { get; set; }

        public virtual DbSet<TimesheetUser> TimesheetUser { get; set; }

        public virtual DbSet<JobUser> JobUser { get; set; }

        public virtual DbSet<TimesheetUserData> TimesheetUserData { get; set; }
        public virtual DbSet<ShareServices> ShareServices { get; set; }

        /// <summary>
        /// RFQ & Ca
        /// </summary>
        public virtual DbSet<RequestForQuotation> RequestForQuotation { get; set; }
        public virtual DbSet<RequestForQuotationTask> RequestForQuotationTask { get; set; }
        public virtual DbSet<RfqCode> RfqCode { get; set; }
        public virtual DbSet<CashAdvance> CashAdvance { get; set; }
        public virtual DbSet<MainserviceCategory> MainserviceCategories { get; set; }
        public virtual DbSet<MasterDivision> MasterDivisions { get; set; }
        public virtual DbSet<MasterDepartment> MasterDepartments { get; set; }
        public virtual DbSet<JobComment> JobComment { get; set; }

        public virtual DbSet<MyDam> MyDam { get; set; }
    }
}