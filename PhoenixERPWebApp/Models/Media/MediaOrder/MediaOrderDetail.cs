using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media.MediaOrder
{
    public class MediaOrderDetail
    {

        public string Id { get; set; }
        
        public string MediaOrderNumber { get; set; }
        
        public DateTime StartPeriode { get; set; }
        
        public DateTime EndPeriode { get; set; }
        
        public string FileMasterId { get; set; }
        
        public string FileName { get; set; }
        
        public int Revision { get; set; }
        
        public string TypeID { get; set; }

        public string TypeName { get; set; }

        public bool Campaign { get; set; }
        public bool? IsChecked { get; set; }
    }
}
