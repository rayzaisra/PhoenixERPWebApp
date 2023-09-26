using Phoenix.Data.Models.Um;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface IManageAzurePortalSetupService : IDataService<AzurePortalSetup>
    {
        Task<AzurePortalSetup> GetOnlyOne();
    }
}
