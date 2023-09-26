using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class NotificationTypeService : INotificationTypeService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage SalaryComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public NotificationTypeService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(NotificationType entity)
        {
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params NotificationType[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(NotificationType entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params NotificationType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(NotificationType entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params NotificationType[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(NotificationType entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(NotificationType entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<NotificationType>> Get() => await context.NotificationTypes.AsNoTracking().ToListAsync();

        public Task<List<NotificationType>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<NotificationType> Get(string Id) => await context.NotificationTypes.AsNoTracking().Where(x => x.Id == Id).AsNoTracking().FirstOrDefaultAsync();
    }
}
