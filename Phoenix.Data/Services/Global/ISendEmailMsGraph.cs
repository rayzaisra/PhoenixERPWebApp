using Phoenix.Data.Models.ViewModel;
using System.Threading.Tasks;

namespace Phoenix.Data
{
    public interface ISendEmailMsGraph
    {
        Task SendEmail(VmEmailMsGraph vm);
    }
}
