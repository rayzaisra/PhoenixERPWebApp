using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data
{
    public interface IDataService<T> where T : class
    {
        Task<List<T>> Get();
        Task<List<T>> Get(int skip, int take);
        Task<T> Get(string id);

        Task<int> AddAsync(T entity);
        Task<int> AddRangeAsync(params T[] entities);

        Task<int> EditAsync(T entity);
        Task<int> EditRangeAsync(T entity);

        Task<int> DeleteAsync(T entity);
        Task<int> DeleteRageAsync(params T[] entities);

        Task<int> DeleteSoftAsync(T entity);
        Task<int> DeleteSoftRangeAsync(params T[] entities);
    }

    public interface IDataServiceHris<T> where T : class
    {
        Task<List<T>> Get();
        Task<T> Get(string id);
        Task<int> AddAsync(T entity);
        Task<int> EditAsync(T entity);
        Task<int> DeleteSoftAsync(T entity);
        Task<int> ApproveAsync(T entity);
    }

}
