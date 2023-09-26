using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.Um;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface IEmailTemplateService : IDataService<TmEmailTemplate>
    {
        Task<List<TmEmailTemplate>> GetListTemplateName();
    }

    public class EmailTemplateService : IEmailTemplateService
    {
        readonly DataContext context;

        public EmailTemplateService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TmEmailTemplate entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TmEmailTemplate[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TmEmailTemplate entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TmEmailTemplate[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TmEmailTemplate entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TmEmailTemplate[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TmEmailTemplate entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TmEmailTemplate entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TmEmailTemplate>> Get() => await context.TmEmailTemplates.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();
        public async Task<List<TmEmailTemplate>> GetListTemplateName()
        {
            return await (from c in context.TmEmailTemplates.AsNoTracking().Where(x => !(x.IsDeleted ?? false)) select new TmEmailTemplate
            {
                Id = c.Id,
                Subject = c.Subject
            }).ToListAsync();
        }

        public Task<List<TmEmailTemplate>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TmEmailTemplate> Get(string id) => await context.TmEmailTemplates.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).AsNoTracking().FirstOrDefaultAsync();
    }
}
