using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.ApiExtension
{
    public class Configuration
    {
        public FileConfig FileConfig { get; set; }
        public MsGraphSettings MsGraphSettings { get; set; }
    }

    public class FileConfig
    {
        public string Path { get; set; }
        public string PathEmployee { get; set; }
        public string ThumbnailPath { get; set; }
        public string[] ValidType { get; set; }
        public string AssetPath { get; set; }
        public string HRPath { get; set; }
        public string ImagesPath { get; set; }
        public string PMPath { get; set; }
    }

    public class MsGraphSettings
    {
        public string clientId { get; set; }
        public string tenantId { get; set; }
        public string clientSecret { get; set; }
        public string objectId { get; set; }
        public string office365Username { get; set; }
        public string office365Password { get; set; }
    }
}
