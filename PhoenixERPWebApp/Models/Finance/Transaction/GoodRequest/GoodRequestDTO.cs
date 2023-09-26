using System.Collections.Generic;

namespace PhoenixERPWebApp.Models.Finance
{
    public class GoodRequestDTO
    {
        public GoodRequest Header { get; set; }
        public List<GoodRequestDetail> Details { get; set; }
    }
}
