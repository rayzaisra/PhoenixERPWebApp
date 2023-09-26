using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phoenix.Shared.Helpers
{
    public class CommonHelper
    {
        public static T Cast<T>(T typeHolder, Object x)
        {
            // typeHolder above is just for compiler magic
            // to infer the type to cast x to
            return (T)x;
        }
        public List<string> Csvtolist(string csv)
        {

            List<string> xlist = new List<string>();
            if (csv != null)
            {
                xlist = csv.Split(',').Select(p => p.Trim()).ToList();
            }

            return xlist;
        }

      
    }
}
