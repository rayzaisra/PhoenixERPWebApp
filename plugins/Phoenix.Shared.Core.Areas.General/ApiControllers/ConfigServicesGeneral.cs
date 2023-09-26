using Microsoft.Extensions.DependencyInjection;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.Areas.General.Services;

namespace Phoenix.Shared.Core.Areas.General.Api
{
    public class ConfigServicesGeneral
    {
        public static void Init(IServiceCollection services)
        {
            services.AddTransient<ICurrencyServices, CurrencyServices>();
            services.AddTransient<INotificationServices, NotificationServices>();
            services.AddScoped<IFileServices, FileServices>();
            services.AddScoped<ICityServices, CityServices>();
            services.AddScoped<IItemServices, ItemServices>();
            services.AddScoped<ITeamServices, TeamServices>();
            services.AddScoped<ICountryServices, CountryServices>();
            services.AddScoped<IApprovalServices, ApprovalServices>();
            services.AddScoped<ITeamRoleServices, TeamRoleServices>();
            services.AddScoped<IUserRoleServices, UserRoleServices>();
            services.AddScoped<IAffiliationServices, AffiliationServices>();
            services.AddScoped<IAuditTrailServices, AuditTrailServices>();
            services.AddScoped<IRoleAccessServices, RoleAccessServices>();
            services.AddScoped<ITeamMemberServices, TeamMemberServices>();

            services.AddScoped<ILegalEntityServices, LegalEntityServices>();
            services.AddScoped<IOrganizationServices, OrganizationServices>();

            services.AddScoped<IBusinessUnitServices, BusinessUnitServices>();
            services.AddScoped<IItemCategoryServices, ItemCategoryServices>();
            services.AddScoped<ISharedRecordServices, SharedRecordServices>();
            services.AddScoped<IApprovalGroupServices, ApprovalGroupServices>();

            services.AddScoped<IApprovalDetailServices, ApprovalDetailServices>();
            services.AddScoped<IApprovalStatusServices, ApprovalStatusServices>();

            services.AddScoped<IApplicationMenuServices, ApplicationMenuServices>();
            services.AddScoped<IApplicationRoleServices, ApplicationRoleServices>();
            services.AddScoped<IApplicationUserServices, ApplicationUserServices>();
            services.AddScoped<ICentralResourceServices, CentralResourceServices>();
            services.AddScoped<IRoleAccessMenuServices, RoleAccessMenuServices>();

            services.AddScoped<ITemporaryManagerServices, TemporaryManagerServices>();

            services.AddScoped<IAffiliationMemberServices, AffiliationMemberServices>();
            services.AddScoped<IApplicationEntityServices, ApplicationEntityServices>();
            services.AddScoped<IBusinessUnitTypeServices, BusinessUnitTypeServices>();

            services.AddScoped<IApplicationMenuGroupServices, ApplicationMenuGroupServices>();

            services.AddScoped<ICentralResourcesStaffServices, CentralResourcesStaffServices>();
            services.AddScoped<IApplicationMenuCategoryServices, ApplicationMenuCategoryServices>();

            services.AddScoped<ILegalEntityServices, LegalEntityServices>();

            //approval get data
            services.AddScoped<ITrTemplateApprovalServices, TrTemplateApprovalServices>();
            services.AddScoped<ITrUserApprovalServices, TrUserApprovalServices>();

            services.AddScoped<ITmTemplateApprovalServices, TmTemplateApprovalServices>();
            services.AddScoped<ITmUserApprovalServices, TmUserApprovalServices>();

            services.AddScoped<IChatInfoService, ChatInfoService>();

        }
    }
}
