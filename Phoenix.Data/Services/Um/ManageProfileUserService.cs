using Phoenix.Data.Models;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class ManageProfileUserService : IManageProfileUserService
    {
        readonly DataContext context;
        public ManageProfileUserService(DataContext context)
        {
            this.context = context;
        }

        #region Default Interface
        public Task<int> AddAsync(TmUserApp entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddRangeAsync(params TmUserApp[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TmUserApp entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TmUserApp[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftAsync(TmUserApp entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSoftRangeAsync(params TmUserApp[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditAsync(TmUserApp entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditRangeAsync(TmUserApp entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<TmUserApp>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<List<TmUserApp>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public Task<TmUserApp> Get(string id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Custome User Profile
        public async Task<List<VmProfileInfo>> GetProfileInfo(string userid)
        {
            var parameter = new SqlParameter[]
            {
                new SqlParameter("@Key",
                    SqlDbType.NVarChar) {Value = userid}
            };
            return await context.ExecuteStoredProcedure<VmProfileInfo>("dbo.uspGetEmployeeMappingDivision", parameter);            
        }
        #endregion

    }
}
