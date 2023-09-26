using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class RptInterCompanyReconciliation
    {
        public Guid? id { get; set; }

        [DisplayName("Client / Vendor")]
        public string company_name { get; set; }

        [DisplayName("Legal Entity")]
        public string legal_entity_name { get; set; }

        [DisplayName("AP")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ap { get; set; }

        [DisplayName("AR")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ar { get; set; }

        [DisplayName("Balance")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? balance { get; set; }

        [DisplayName("remarks")]
        public string remarks { get; set; }
    }

    public class RptInterCompanyReconciliationFilter
    {
        [DisplayName("Type")]
        public string Type { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndingDate { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Legal Entity")]
        public List<string> MultiLegalEntityId { get; set; }

        [DisplayName("Client / Vendor")]
        public List<string> MultiCompanyId { get; set; }
    }
}
