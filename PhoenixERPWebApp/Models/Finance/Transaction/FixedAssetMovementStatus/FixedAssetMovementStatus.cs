using PhoenixERPWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoenixERPWebApp.Models
{
    public class FixedAssetMovementStatus : FinanceEntityBase
    {       
        public string Id { get; set; }
    
        public string StatusName { get; set; }
    }
}
