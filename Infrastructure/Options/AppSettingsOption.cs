using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMarvel.Infrastructure.Options
{
    [Serializable]
    public class AppSettingsOption
    {
        public string ApiServerUrl { get; set; }
        public string WebServerUrl { get; set; }
        public string SocketServerUrl { get; set; }
    }
}
