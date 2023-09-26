using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Phoenix.Shared.Core.Contexts;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using Phoenix.Shared.Core.Entities.BaseEntity;
using Microsoft.AspNetCore.Http;

namespace Phoenix.Shared.Core.Entities
{
    public partial class AppDbContext : EFContext
    {

        public AppDbContext(DbContextOptions<EFContext> options) 
            : base(options)
        { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Data Source=139.255.87.235,49172;Initial Catalog=PhoenixDBUltimate;persist security info=True;user id=dev-user;password=U$3rDevelopment2019;MultipleActiveResultSets=True;ConnectRetryCount=3;ConnectRetryInterval=3;Connection Timeout=9");
                //optionsBuilder.UseSqlServer(@"Data Source=dbphoenix.database.windows.net;Initial Catalog=PhoenixDBUltimate;persist security info=True;user id=phoenix;password=P@nglima20|9;MultipleActiveResultSets=True;ConnectRetryCount=3;ConnectRetryInterval=3;Connection Timeout=9");

                //optionsBuilder.UseSqlServer(@"data source=DESKTOP-G2J88J3;Initial Catalog=PhoenixDBUltimate;Persist Security Info=True;MultipleActiveResultSets=true;User ID=phoniex;password=phoniex123;");
                //optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-EJM4VS4\SQLEXPRESS2012;Initial Catalog=nava;Integrated Security=True;Persist Security Info=True;User ID=sa;password=belajar;");
                //optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-KV8I5VP\MSSQLSERVER2012;Initial Catalog=PhoenixDev3;Integrated Security=True;Persist Security Info=True;User ID=sa;password=sa123;");
            }
        }
        //------------------------tambahan untuk soft delete--------------------------
        private const string _isDeletedProperty = "is_deleted";
        private static readonly MethodInfo _propertyMethod = typeof(EF).GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(typeof(bool));

        private static LambdaExpression GetIsDeletedRestriction(Type type)
        {
            var parm = Expression.Parameter(type, "it");
            var prop = Expression.Call(_propertyMethod, parm, Expression.Constant(_isDeletedProperty));
            var condition = Expression.MakeBinary(ExpressionType.Equal, prop, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, parm);
            return lambda;
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entity.ClrType) == true)
                {
                    modelBuilder
                    .Entity(entity.ClrType)
                    .HasQueryFilter(GetIsDeletedRestriction(entity.ClrType));
                }

            }
        }
    }
}
