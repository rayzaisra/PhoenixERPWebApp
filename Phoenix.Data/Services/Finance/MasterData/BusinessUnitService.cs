using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IBusinessUnitService
    {
        Task<List<BusinessUnit>> GetFinance();
        Task<List<MappingBusinessUnit>> GetHris(int BussinesUnitLevel);
        Task<BusinessUnit> Get(string Id);
        Task<int> AddFinanceAsync(BusinessUnit entity);
        Task<int> AddHrisAsync(BusinessUnit entity);
        Task<int> DeleteFinanceSoftAsync(BusinessUnit entity);
        Task<int> DeleteHrisSoftAsync(BusinessUnit entity);
        Task<int> EditFinanceAsync(BusinessUnit entity);
        Task<int> EditHrisAsync(BusinessUnit entity);
        Task<List<MaapingGroupDto>> GetMappingBusinessUnitGroupSp();
        Task<List<MaapingSubgroupDto>> GetMappingBusinessUnitSubgroupSp();
        Task<List<MaapingDisivionDto>> GetMappingBusinessUnitDisivionSp();
        Task<List<MappingBusinessUnit>> GetMappingBusinessUnitDepartementSp();

    }

    public class BusinessUnitService : IBusinessUnitService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage BusinessUnit
        /// </summary>
        /// <param name="context">Database context</param>
        public BusinessUnitService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<MappingBusinessUnit>> GetMappingBusinessUnitDepartementSp()
        {
            try
            {
                var sqlParameters = new List<SqlParameter>
                {

                };
                SqlParameter[] param = sqlParameters.ToArray();
                var resultSet = new List<MappingBusinessUnit>();
                resultSet = await context.ExecuteStoredProcedure<MappingBusinessUnit>("dbo.usp_get_department_mapping", param);
                return resultSet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<List<MaapingGroupDto>> GetMappingBusinessUnitGroupSp()
        {
            try
            {
                var sqlParameters = new List<SqlParameter>
                {

                };
                SqlParameter[] param = sqlParameters.ToArray();
                var resultSet = new List<MaapingGroupDto>();
                resultSet = await context.ExecuteStoredProcedure<MaapingGroupDto>("dbo.usp_get_group_mapping", param);
                return resultSet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<List<MaapingSubgroupDto>> GetMappingBusinessUnitSubgroupSp()
        {
            try
            {
                var sqlParameters = new List<SqlParameter>
                {

                };
                SqlParameter[] param = sqlParameters.ToArray();
                var resultSet = new List<MaapingSubgroupDto>();
                resultSet = await context.ExecuteStoredProcedure<MaapingSubgroupDto>("dbo.usp_get_subgroup_mapping", param);
                return resultSet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<List<MaapingDisivionDto>> GetMappingBusinessUnitDisivionSp()
        {
            try
            {
                var sqlParameters = new List<SqlParameter>
                {

                };
                SqlParameter[] param = sqlParameters.ToArray();
                var resultSet = new List<MaapingDisivionDto>();
                resultSet = await context.ExecuteStoredProcedure<MaapingDisivionDto>("dbo.usp_get_division_mapping", param);
                return resultSet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<int> AddFinanceAsync(BusinessUnit entity)
        {
            var validation = await context.BusinessUnits.Where(x => x.AffiliationId == entity.AffiliationId && x.UnitName == entity.UnitName).FirstOrDefaultAsync();

            if (validation == null)
            {
                var bussnisUnitType = await context.BusinessUnitTypes.Where(x => x.BusinessUnitLevel == BusinessUnitLevelCode.Division).FirstOrDefaultAsync();

                if (bussnisUnitType != null)
                {
                    entity.BusinessUnitTypeId = bussnisUnitType.Id;
                }
                if (string.IsNullOrEmpty(entity.OrganizationId))
                {
                    entity.OrganizationId = "9EE4834225E334380667DDCF2F60F6DA";
                }

                entity.Id = Guid.NewGuid().ToString();
                entity.Isfinance = true;
                await context.PhoenixAddAsync(entity);
                return await context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("for unit name '" + entity.UnitName + "' in affiliation '" + entity.AffiliationName + "' is already in database.");
            }
            
            
        }

        public async Task<int> AddHrisAsync(BusinessUnit entity)
        {
            var affiliation = await context.Affiliations.AsNoTracking().Where(x => x.Id == entity.AffiliationId).FirstOrDefaultAsync();
            if (affiliation == null)
            {
                entity.AffiliationId = "27f5efff-e898-473b-b5d6-890429706b1c";
            }
            if (string.IsNullOrEmpty(entity.OrganizationId))
            {
                entity.OrganizationId = "9EE4834225E334380667DDCF2F60F6DA";
            }
            var bussnisUnitType = await context.BusinessUnitTypes.Where(x => x.BusinessUnitLevel == entity.bussnisUnitTypeLevel).FirstOrDefaultAsync();
            if (bussnisUnitType != null)
            {
                entity.BusinessUnitTypeId = bussnisUnitType.Id;
            }

            entity.Id = Guid.NewGuid().ToString();
            entity.Ishris = true;
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteFinanceSoftAsync(BusinessUnit entity)
        {
            entity.Isfinance = true;
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteHrisSoftAsync(BusinessUnit entity)
        {
            entity.Isfinance = false;
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditFinanceAsync(BusinessUnit entity)
        {

            entity.Isfinance = true;
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EditHrisAsync(BusinessUnit entity)
        {
            var affiliation = await context.Affiliations.AsNoTracking().Where(x => x.Id == entity.AffiliationId).FirstOrDefaultAsync();
            if (affiliation == null)
            {
                entity.AffiliationId = "27f5efff-e898-473b-b5d6-890429706b1c";
            }
            if (string.IsNullOrEmpty(entity.OrganizationId))
            {
                entity.OrganizationId = "9EE4834225E334380667DDCF2F60F6DA";
            }
            var bussnisUnitType = await context.BusinessUnitTypes.Where(x => x.BusinessUnitLevel == entity.bussnisUnitTypeLevel).FirstOrDefaultAsync();
            if (bussnisUnitType != null)
            {
                entity.BusinessUnitTypeId = bussnisUnitType.Id;
            }

            entity.Ishris = true;
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<BusinessUnit>> GetFinance()
        {
            var result = await (from a in context.BusinessUnits
                                join b in context.Affiliations on a.AffiliationId equals b.Id
                                join c in context.BusinessUnitTypes on a.BusinessUnitTypeId equals c.Id
                                where !(a.IsDeleted ?? false) && (a.Isfinance ?? false) &&
                                c.BusinessUnitLevel == BusinessUnitLevelCode.Division
                                select new BusinessUnit
                                {
                                    Id = a.Id,
                                    UnitName = a.UnitName,
                                    AffiliationId = a.AffiliationId,
                                    AffiliationName = b.AffiliationName
                                }).ToListAsync();
            return result;
        }
        public async Task<List<MappingBusinessUnit>> GetHris(int BussinesUnitLevel)
        {
            var result = new List<MappingBusinessUnit>();

            if (BussinesUnitLevel == BusinessUnitLevelCode.HoldingGroup)
            {

                result = await (from tb in context.MappingBusinessUnit
                                select new MappingBusinessUnit
                                {
                                    IdGroup = tb.IdGroup,
                                    GroupName = tb.GroupName
                                }).ToListAsync();
            }
            else if (BussinesUnitLevel == BusinessUnitLevelCode.BusinessUnitSubgroup)
            {

                result = await (from tb in context.MappingBusinessUnit
                                select new MappingBusinessUnit
                                {
                                    IdGroup = tb.IdGroup,
                                    GroupName = tb.GroupName,
                                    IdSubgroup = tb.IdSubgroup,
                                    SubGroupName = tb.SubGroupName
                                }).ToListAsync();
            }
            else if (BussinesUnitLevel == BusinessUnitLevelCode.Division)
            {

                result = await (from tb in context.MappingBusinessUnit
                                select new MappingBusinessUnit
                                {
                                    IdGroup = tb.IdGroup,
                                    GroupName = tb.GroupName,
                                    IdSubgroup = tb.IdSubgroup,
                                    SubGroupName = tb.SubGroupName,
                                    IdDivisi = tb.IdDivisi,
                                    DivisiName = tb.DivisiName
                                }).ToListAsync();
            }
            else
            {
                result = await (from tb in context.MappingBusinessUnit
                                select new MappingBusinessUnit
                                {
                                    IdGroup = tb.IdGroup,
                                    GroupName = tb.GroupName,
                                    IdSubgroup = tb.IdSubgroup,
                                    SubGroupName = tb.SubGroupName,
                                    IdDivisi = tb.IdDivisi,
                                    DivisiName = tb.DivisiName,
                                    IdDepartment = tb.IdDepartment,
                                    DepartmentName = tb.DepartmentName
                                }).ToListAsync();
            }
            return result;

        }
        public async Task<BusinessUnit> Get(string Id) => await context.BusinessUnits.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        private string GetParent(BusinessUnit Entity)
        {
            return context.BusinessUnits.Where(x => x.IsDeleted.Equals(false) && x.Id == Entity.ParentUnit).FirstOrDefault().UnitName;
        }
    }
}