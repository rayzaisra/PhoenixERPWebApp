using System.ComponentModel;

namespace PhoenixERPWebApp.Models
{
    public class MasterConditionProcurement : FinanceEntityBase
    {
        [DisplayName("ID Master Condition Procurement")]
        public string Id { get; set; }

        [DisplayName("Condition Name")]
        public string ConditionName { get; set; }

        [DisplayName("Status")]
        public string status { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }
    }
}
