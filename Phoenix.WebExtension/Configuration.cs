using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.WebExtension
{
    public class Configuration
    {
        public AppConfiguration AppConfiguration { get; set; }
    }

    public class AppConfiguration
    {
        public string ApiServerUrl { get; set; }
        public string WebServerUrl { get; set; }
        public string SocketServerUrl { get; set; }
    }
}
