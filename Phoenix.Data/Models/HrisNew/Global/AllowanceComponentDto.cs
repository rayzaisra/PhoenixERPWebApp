using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models.HrisNew.Global
{
    public class AllowanceComponentDto
    {
        public string Id { get; set; }
        public string SubComponentName { get; set; }
        public decimal Percent { get; set; }
        public decimal Value { get; set; }
    }
}
