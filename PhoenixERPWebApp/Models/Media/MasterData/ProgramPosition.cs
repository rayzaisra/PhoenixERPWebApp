using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media.MasterData
{
    public class ProgramPosition : MediaEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }
        [DisplayName("Program Position Name")]
        public string ProgramPositionName { get; set; }
    }
}
