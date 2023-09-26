using Newtonsoft.Json;
using System;


namespace Phoenix.ApiExtension
{
    public class DataConfiguration
    {
        public static void SetConfiguration(Configuration config)
        {
            var json = JsonConvert.SerializeObject(config);
            System.IO.File.WriteAllText("appsettings.json", json);
        }

        public static Configuration Configuration
        {
            get
            {
                string json = System.IO.File.ReadAllText("appsettings.json");
                var data = JsonConvert.DeserializeObject<Configuration>(json);
                return data;
            }
        }
    }
}
