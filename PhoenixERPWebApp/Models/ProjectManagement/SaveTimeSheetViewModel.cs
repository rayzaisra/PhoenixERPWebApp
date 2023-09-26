using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.ProjectManagement
{
    public class SaveTimeSheetViewModel
    {
        public string Id { get; set; }
        public string ChoosenDate { get; set; }
        public string BusinessUnitId { get; set; }
        public int Status { get; set; }
    }
}
