using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class PeFileFolderDto
    {

        public string parent_id { get; set; }
        public string parent_entity { get; set; }
        public string folder_path { get; set; }
    }

    public class DamFolderMasterDto
    {
        public string folder_name { get; set; }
        public string root_name { get; set; }
        public string application_entity_id { get; set; }

    }
}
