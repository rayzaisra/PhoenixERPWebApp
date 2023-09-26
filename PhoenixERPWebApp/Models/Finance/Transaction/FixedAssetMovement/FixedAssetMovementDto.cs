using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance
{
    public class FixedAssetMovementDto
    {
        public FixedAssetMovement Header { get; set; }
        public List<FixedAssetMovementDetail> Details { get; set; }
    }
}
