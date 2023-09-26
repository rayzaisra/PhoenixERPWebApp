using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CommonTool.QueryBuilder.Model
{
    public class JsonRule : IRule, IGroup
    {
        #region IRule

        public string id { get; set; }

        public string field { get; set; }

        public string type { get; set; }

        public string input { get; set; }

        public string @operator { get; set; }

        public object value { get; set; }

        public dynamic data { get; set; }

        #endregion

        #region IGroup

        public string condition { get; set; }

        public bool not { get; set; }

        public List<JsonRule> rules { get; set; }

        #endregion


        #region Constructor
        public JsonRule()
        {
            rules = new List<JsonRule>();
        }
        #endregion

        public bool IsGroup()
        {
            return !string.IsNullOrEmpty(condition);
        }

        public IRule ToRule()
        {
            return this;
        }

        public IGroup ToGroup()
        {
            return this;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
