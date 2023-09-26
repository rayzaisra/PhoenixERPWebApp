using Microsoft.Extensions.DependencyInjection;

using Phoenix.Shared.Core.Areas.ProjectManagement.Services;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Api
{
    public class ConfigServicesProjectManagement
    {
        public static void Init(IServiceCollection services)
        {
            services.AddScoped<IPcaServices, PcaServices>();
            services.AddScoped<IPceServices, PceServices>();
            services.AddScoped<ITimesheetNonJobServices, TimesheetNonJobServices>();
            services.AddScoped<IMotherPcaServices,MotherPcaServices>();
            services.AddScoped<ITimelineServices, TimelineServices>();
            services.AddScoped<IPmServices, PmServices>();
            services.AddScoped<IDamServices, DamServices>();
            services.AddScoped<IBastServices, BastServices>();
            services.AddScoped<ITaskServices, TaskServices>();
            services.AddScoped<IJobPaServices, JobPaServices>();
            services.AddScoped<IJobPeServices, JobPeServices>();
            services.AddScoped<IChannelServices, ChannelServices>();
            services.AddScoped<IPackageServices, PackageServices>();
            services.AddScoped<IPeFileServices, PeFileServices>();
            services.AddScoped<IPoTypeServices, PoTypeServices>();

            services.AddScoped<IDamTypeServices, DamTypeServices>();
            services.AddScoped<IJobTypeServices, JobTypeServices>();
            services.AddScoped<ISubTaskServices, SubTaskServices>();
            services.AddScoped<IPaDetailServices, PaDetailServices>();
            services.AddScoped<IRateCardServices, RateCardServices>();
            services.AddScoped<ITaskUserServices, TaskUserServices>();
            services.AddScoped<IJobDetailServices, JobDetailServices>();
            services.AddScoped<IJobStatusServices, JobStatusServices>();
            //services.AddScoped<IPettyCashsServices, PettyCashsServices>();
            services.AddScoped<IPeTemplateServices, PeTemplateServices>();
            services.AddScoped<IReportFileServices, ReportFileServices>();
            services.AddScoped<ITaskStatusServices, TaskStatusServices>();
            services.AddScoped<IClientBriefServices, ClientBriefServices>();
            services.AddScoped<IProposeTypeServices, ProposeTypeServices>();
            services.AddScoped<ISubCampaignServices, SubCampaignServices>();
            services.AddScoped<ITaskCommentServices, TaskCommentServices>();
            services.AddScoped<IApprovalTypeServices, ApprovalTypeServices>();
            services.AddScoped<ICampaignTypeServices, CampaignTypeServices>();
            services.AddScoped<IProposalFileServices, ProposalFileServices>();
            services.AddScoped<ITaskPriorityServices, TaskPriorityServices>();
            services.AddScoped<IBussinessTypeServices, BussinessTypeServices>();
            services.AddScoped<IJobStatusSetServices, JobStatusSetServices>();
            services.AddScoped<IRateCardCodeServices, RateCardCodeServices>();
            services.AddScoped<ITaskTimesheetServices, TaskTimesheetServices>();
            services.AddScoped<ICampaignStatusServices, CampaignStatusServices>();
            services.AddScoped<IClientFeedbackServices, ClientFeedbackServices>();
            services.AddScoped<IStatusAssignedServices, StatusAssignedServices>();
            services.AddScoped<ILabelAttachmentServices, LabelAttachmentServices>();
            services.AddScoped<IQuotationVendorServices, QuotationVendorServices>();
            services.AddScoped<ICampaignProgressServices, CampaignProgressServices>();
            services.AddScoped<IJobStatusDetailServices, JobStatusDetailServices>();
            services.AddScoped<IQuotationRequestServices, QuotationRequestServices>();
            services.AddScoped<IAccountManagementServices, AccountManagementServices>();

            services.AddScoped<IJobWorkflowStatusServices, JobWorkflowStatusServices>();
            services.AddScoped<IClientBriefAccountServices, ClientBriefAccountServices>();
            services.AddScoped<IClientBriefContentServices, ClientBriefContentServices>();
            services.AddScoped<IJobWorkflowHistoryServices, JobWorkflowHistoryServices>();
            services.AddScoped<IMasterSearchContentServices, MasterSearchContentServices>();
            services.AddScoped<IClientBriefContentRefServices, ClientBriefContentRefServices>();
            services.AddScoped<ICentralResourceAssigmentServices, CentralResourceAssigmentServices>();
            services.AddScoped<IClientBriefCentralResourceServices, ClientBriefCentralResourceServices>();
            services.AddScoped<IClientBriefAccountmanagementServices, ClientBriefAccountmanagementServices>();
            services.AddScoped<ICompanyBrandServices, CompanyBrandServices>();

            services.AddScoped<ITimesheetUserServices, TimesheetUserServices>();
            services.AddScoped<ITypeOfExpenseServices, TypeOfExpenseServices>();

            services.AddScoped<IShareServicesServices, ShareServicesServices>();
            services.AddScoped<IJobUserServices, JobUserServices>();

            services.AddScoped<IJobCommentServices, JobCommentServices>();

        }
    }
}
