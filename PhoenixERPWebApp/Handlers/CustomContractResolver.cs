using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp
{
	public class CustomContractResolver : DefaultContractResolver
	{
		private Dictionary<string, string> PropertyMappings { get; set; }

		public CustomContractResolver()
		{
			this.PropertyMappings = new Dictionary<string, string>
			{
				{"EmployeeName", "NameEmployee"},
				{ "DateEmployed", "JoinDate"},
			};
		}

		protected override string ResolvePropertyName(string propertyName)
		{
			string resolvedName = null;
			var resolved = this.PropertyMappings.TryGetValue(propertyName, out resolvedName);
			return (resolved) ? resolvedName : base.ResolvePropertyName(propertyName);
		}
	}
}
