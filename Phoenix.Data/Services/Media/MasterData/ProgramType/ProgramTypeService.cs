using Phoenix.Data.Models.Media.MasterData.ProgramType;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Media.MasterData.ProgramType
{
    public interface IProgramTypeService : IDataService<TmProgramType>
    {
        Task<dynamic> GetList();
    }

    public class ProgramTypeService : IProgramTypeService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterMappingRequest
        /// </summary>
        /// <param name="context">Database context</param>
        public ProgramTypeService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TmProgramType entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TmProgramType[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TmProgramType entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TmProgramType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TmProgramType entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TmProgramType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TmProgramType entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TmProgramType entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<TmProgramType>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<dynamic> GetList()
        {
            var sql = await (from a in context.TmProgramTypes
                             where a.IsDeleted != true
                             select new
                             {
                                 a.Id,
                                 a.ProgramTypeName
                             }
                            ).ToListAsync();
            return sql;
        }

        public async Task<TmProgramType> Get(string Id) => await context.TmProgramTypes.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).AsNoTracking().FirstOrDefaultAsync();

        Task<List<TmProgramType>> IDataService<TmProgramType>.Get()
        {
            throw new NotImplementedException();
        }
    }
}
