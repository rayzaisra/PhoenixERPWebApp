using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{

    [Table(nameof(MyDam), Schema = "pm")]
    public class MyDam : PmEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }


        [MaxLength(250)]
        [Column("file_name"), StringLength(250)]
        public string FileName { get; set; }

        [MaxLength(250)]
        [Column("file_type"), StringLength(250)]
        public string FileType { get; set; }

        [Column("file_size")]
        public long FileSize { get; set; }

        [MaxLength(250)]
        [Column("file_path"), StringLength(250)]
        public string FilePath { get; set; }

        [MaxLength(250)]
        [Column("mimetype"), StringLength(250)]
        public string MimeType { get; set; }

        [Column("parent_id")]
        public string ParentId { get; set; }

        [NotMapped]
        public List<GridFilter> Filters { get; set; }

        [NotMapped]
        public List<dynamic> dynamicList { get; set; }
        //[NotMapped]
        //public IPagedList dynamicListMetaData { get; set; }
        [NotMapped]
        public int? page { get; set; }
        [NotMapped]
        public string sortOrder { get; set; }
        [NotMapped]
        public string fieldName { get; set; }
        [NotMapped]
        public string SearchText { get; set; }
        [NotMapped]
        public int StaticPageSize { get; set; }
        [NotMapped]
        public string ViewType { get; set; }
        [NotMapped]
        public int? Status { get; set; }
        [NotMapped]
        public string RequestBy { get; set; }
        [NotMapped]
        public string RequestByName { get; set; }
        [NotMapped]
        public string LogDescription { get; set; }

        [NotMapped]
        public DateTime? RequestDate { get; set; }
        [NotMapped]
        public string JobName { get; set; }
        [NotMapped]
        public string JobId { get; set; }
        [NotMapped]
        public string ClientBriefId { get; set; }
        [NotMapped]
        public string ClientName { get; set; }
        [NotMapped]
        public string ClientId { get; set; }
        [NotMapped]
        public string CampaignName { get; set; }
        [NotMapped]
        public DateTime? JobCreateOn { get; set; }

    }
    
    public class GridFilter
    {
        public string Value { get; set; }
        public string ColumnName { get; set; }
        public string Condition { get; set; }
    }

    public class RequestMyDam
    {
        public string SelectedId { get; set; }
        public int? Status { get; set; }
        public DateTime? RequestDate { get; set; }
        public string RequestBy { get; set; }
        public string FolderId { get; set; }
    }
}
