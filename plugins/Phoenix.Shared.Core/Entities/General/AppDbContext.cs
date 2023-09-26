using Microsoft.EntityFrameworkCore;
using Phoenix.Shared.Core.Contexts;
namespace Phoenix.Shared.Core.Entities
{
    public partial class AppDbContext : EFContext
    {
        public virtual DbSet<vUserList> vUserList { get; set; }
        public virtual DbSet<vTeam> vTeam { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<Actor> Actor { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<NotificationType> NotificationType { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<Filemaster> Filemaster { get; set; }
        public virtual DbSet<Country> Country { get; set; }

        public virtual DbSet<Approval> Approval { get; set; }
        public virtual DbSet<TeamRole> TeamRole { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
 
        public virtual DbSet<Affiliation> Affiliation { get; set; }
        public virtual DbSet<AuditTrail> AuditTrail { get; set; }
        public virtual DbSet<RoleAccess> RoleAccess { get; set; }
        public virtual DbSet<TeamMember> TeamMember { get; set; }

        public virtual DbSet<LegalEntity> LegalEntity { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }

        public virtual DbSet<BusinessUnit> BusinessUnit { get; set; }
        public virtual DbSet<ItemCategory> ItemCategory { get; set; }
        public virtual DbSet<SharedRecord> SharedRecord { get; set; }
        public virtual DbSet<ApprovalGroup> ApprovalGroup { get; set; }

        public virtual DbSet<ApprovalDetail> ApprovalDetail { get; set; }
        public virtual DbSet<ApprovalStatus> ApprovalStatus { get; set; }

        public virtual DbSet<ApplicationMenu> ApplicationMenu { get; set; }
        public virtual DbSet<ApplicationRole> ApplicationRole { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
        public virtual DbSet<CentralResource> CentralResource { get; set; }
        public virtual DbSet<RoleAccessMenu> RoleAccessMenu { get; set; }

        public virtual DbSet<TemporaryManager> TemporaryManager { get; set; }

        public virtual DbSet<AffiliationMember> AffiliationMember { get; set; }
        public virtual DbSet<ApplicationEntity> ApplicationEntity { get; set; }
        public virtual DbSet<BusinessUnitType> BusinessUnitType { get; set; }

        public virtual DbSet<ApplicationMenuGroup> ApplicationMenuGroup { get; set; }

        public virtual DbSet<CentralResourcesStaff> CentralResourcesStaff { get; set; }
        public virtual DbSet<ApplicationMenuCategory> ApplicationMenuCategory { get; set; }


        //approval get data
        public virtual DbSet<TrTemplateApproval> TrTemplateApproval { get; set; }
        public virtual DbSet<TmTemplateApproval> TmTemplateApproval { get; set; }
        public virtual DbSet<TrUserApproval> TrUserApproval { get; set; }
        public virtual DbSet<TmUserApproval> TmUserApproval { get; set; }
        public virtual DbSet<TmMenu> TmMenu { get; set; }
        public virtual DbSet<TmUserApp> TmUserApp { get; set; }
        public virtual DbSet<TmEmailTemplate> TmEmailTemplate { get; set; }

    }
}