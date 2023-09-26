using Phoenix.Data.Models.Um;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface IAuditTrailsService : IDataService<AuditTrails>
    {
        Task<List<AuditTrails>> GetBySearch(string key);
    }
}
