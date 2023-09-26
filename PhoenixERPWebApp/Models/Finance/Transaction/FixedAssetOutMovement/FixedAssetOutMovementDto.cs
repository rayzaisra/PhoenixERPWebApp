using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.FixedAssetOutMovement
{
    public class FixedAssetOutMovementDto
    {
        public FixedAssetOutMovement Header { get; set; }
        public List<FixedAssetOutMovementDetail> Details { get; set; }
    }
}
