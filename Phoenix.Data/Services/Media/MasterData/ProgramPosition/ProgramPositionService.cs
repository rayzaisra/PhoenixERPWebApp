using Phoenix.Data.Models.Media.MasterData.ProgramPosition;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Media.MasterData.ProgramPosition
{
    public interface IProgramPositionService : IDataService<TmProgramPosition>
    {
        Task<dynamic> GetList();
    }

    public class ProgramPositionService : IProgramPositionService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterMappingRequest
        /// </summary>
        /// <param name="context">Database context</param>
        public ProgramPositionService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TmProgramPosition entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TmProgramPosition[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TmProgramPosition entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TmProgramPosition[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TmProgramPosition entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TmProgramPosition[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TmProgramPosition entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TmProgramPosition entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<TmProgramPosition>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<dynamic> GetList()
        {
            var sql = await (from a in context.TmProgramPositions
                             where a.IsDeleted != true
                             select new
                             {
                                 a.Id,
                                 a.ProgramPositionName
                             }
                            ).ToListAsync();
            return sql;
        }

        public async Task<TmProgramPosition> Get(string Id) => await context.TmProgramPositions.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).AsNoTracking().FirstOrDefaultAsync();

        Task<List<TmProgramPosition>> IDataService<TmProgramPosition>.Get()
        {
            throw new NotImplementedException();
        }
    }
}
