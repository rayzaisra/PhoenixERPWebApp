using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities.ProjectManagement
{
    [Table("mydam", Schema = "pm")]
    public class MyDam : GenericEntity<string>
    {

        [Column("file_name")]
        public string FileName { get; set; }
        [Column("file_type")]
        public string FileType { get; set; }
        [Column("file_size")]
        public long FileSize { get; set; }
        [Column("file_path")]
        public string FilePath { get; set; }
        [Column("mimetype")]
        public string MimeType { get; set; }
        [Column("parent_id")]
        public string ParentId { get; set; }
        [Column("upload_date")]
        public DateTime? upload_date { get; set; }



    }

}

