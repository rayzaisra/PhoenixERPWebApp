
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.PhoenixERP.Finance
{
    public partial class PettyCashDetailsDTO
    {
        public petty_cashDTO PettyCash { get; set; }
        public List<petty_cash_detailWithFileDTO> Details { get; set; }
    }

    public partial class petty_cash_detailWithFileDTO : petty_cash_detailDTO {
        public ICollection<IFormFile> Files { get; set; }
    }

    public partial class VwPettyCashDetailsDTO
    {
        public vw_petty_cashDTO PettyCash { get; set; }
        public List<vw_petty_cash_detailDTO> Details { get; set; }
    }
}
