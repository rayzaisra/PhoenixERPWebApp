using Microsoft.EntityFrameworkCore;
using Phoenix.Shared.Core.Contexts;
namespace Phoenix.Shared.Core.Entities
{
    public partial class AppDbContext : EFContext
    {
        
        public virtual DbSet<RequestForQuotationDetail> RequestForQuotationDetail { get; set; }
        public virtual DbSet<Bank> Bank { get; set; }
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<SalesOrder> SalesOrder { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<AccountGroup> AccountGroup { get; set; }
        public virtual DbSet<CompanyBrand> CompanyBrand { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrder { get; set; }

        public virtual DbSet<SalesOrderDetail> SalesOrderDetail { get; set; }
        public virtual DbSet<TransactionDetail> TransactionDetail { get; set; }
        public virtual DbSet<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }
        public virtual DbSet<CostSharingAllocation> CostSharingAllocation { get; set; }
        public virtual DbSet<CostSharingAllocationDetail> CostSharingAllocationDetail { get; set; }
        public virtual DbSet<FinancialYear> FinancialYear { get; set; }
    }
}