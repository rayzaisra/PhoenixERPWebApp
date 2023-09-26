using System.Collections.Generic;

namespace Phoenix.Data.Models.Finance
{
    public class GoodRequestDTO
    {
        public GoodRequest Header { get; set; }
        public List<GoodRequestDetail> Details { get; set; }
    }
}
