using System.ComponentModel;

namespace PhoenixERPWebApp.Models
{
    public class MasterDepreciation : FinanceEntityBase
    {
        [DisplayName("ID Master Depreciation")]
        public string Id { get; set; }

        [DisplayName("Depreciation Name")]
        public string DepreciationName { get; set; }

        [DisplayName("Status")]
        public string status { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }
    }
}
