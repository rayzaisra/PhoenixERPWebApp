using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    public class FixedAssetMovementDto
    {
        public FixedAssetMovement Header { get; set; }
        public List<FixedAssetMovementDetail> Details { get; set; }
    }
}
