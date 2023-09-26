using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models.Finance.Transaction.FixedAssetOutMovement
{
    public class FixedAssetOutMovementDto
    {
        public FixedAssetOutMovement Header { get; set; }
        public List<FixedAssetOutMovementDetail> Details { get; set; }
    }
}
