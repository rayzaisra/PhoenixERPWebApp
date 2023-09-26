using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.Budget
{
    public class TypeBudgetHdr : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee Basic Info Id")]
        public string employee_basic_info_id { get; set; }

        [DisplayName("Uploader Name")]
        public string employee_name { get; set; }

        [DisplayName("Financial Year Name")]
        public int? financialyear { get; set; }

        [DisplayName("Financial Year Id")]
        public string financial_year_id { get; set; }    

        [DisplayName("Financial Month")]
        public string bulan { get; set; }

        [DisplayName("Revision")]
        public int? revision { get; set; }

        [DisplayName("Upload Date")]
        public DateTime? upload_date { get; set; }
    }
}
