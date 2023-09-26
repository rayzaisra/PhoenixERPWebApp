using System.Collections.Generic;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class FileConfig
    {
        public string Path { get; set; }
        public List<string> ValidType { get; set; }
        public FileConfig() { }
    }
}
