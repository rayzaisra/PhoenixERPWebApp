using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media
{
    public class ProgramCategoryTypeDetail : MediaEntityBase
    {
        public string Id { get; set; }
        [DisplayName("Program Category Name")]
        public string ProgramCategoryName { get; set; }
    }
}
