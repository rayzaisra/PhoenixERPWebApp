using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class SaveTimeSheetDto
    {
        public string Id { get; set; }
        public string ChoosenDate { get; set; }
        public string BusinessUnitId { get; set; }
        public int Status { get; set; }
        public string Userid { get; set; }
        public string SubGroupId { get; set; }
        public string divisionId { get; set; }
    }
}
