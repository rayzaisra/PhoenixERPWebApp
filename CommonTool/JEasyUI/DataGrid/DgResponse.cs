using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTool.JEasyUI.DataGrid
{
    public class DgResponse
    {
        public object Data { get; set; }
        public int Total { get; set; }
        public List<dgSort> Orders { get; set; }
    }
}
