using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media.MasterData
{
    public class ProgramType : MediaEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }
        [DisplayName("Program Type Name")]
        public string ProgramTypeName { get; set; }
    }
}
