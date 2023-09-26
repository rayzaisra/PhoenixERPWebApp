using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoenixERPWebApp.Models
{   
    public class MasterLocationProcurement : FinanceEntityBase
    {
        [DisplayName("ID Master Location Procurement")]
        public string Id { get; set; }

        [DisplayName("Location Name")]
        public string LocationStore { get; set; }

        [DisplayName("Status")]
        public string status { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }
    }
}
