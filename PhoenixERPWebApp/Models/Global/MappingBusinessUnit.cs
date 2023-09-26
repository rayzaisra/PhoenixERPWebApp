using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class MappingBusinessUnit
    {
       public string Id { get; set; }
        public string IdGroup { get; set; }
        public string IdSubgroup { get; set; }
        public string IdDivisi { get; set; }
        public string IdDepartment { get; set; }
        public string GroupName { get; set; }
        public string SubGroupName { get; set; }
        public string DivisiName { get; set; }
        public string DepartmentName { get; set; }
    }
}
