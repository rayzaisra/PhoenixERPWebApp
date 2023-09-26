using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoenixERPWebApp.Models
{
    public class FixedAssetDepreciationJournal
    {
        public DateTime? PostingDate { get; set; }
        public string LegalEntityId { get; set; }
        public List<string> Selected { get; set; }
    }
}
