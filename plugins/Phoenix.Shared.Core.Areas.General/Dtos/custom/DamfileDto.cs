using Microsoft.AspNetCore.Http;


namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class DamfileDto
    {

    }
    public class DamUpload
    {
        public string damUploadId { get; set; }
        public string name { get; set; }
        public string tags { get; set; }
        public IFormFile file { get; set; }
        public string application_entity_id { get; set; }
        public string record_id { get; set; }
    }

    public class UploadFileDto
    {
        public string uploadid { get; set; }
        public string name { get; set; }
        public string tags { get; set; }
        public string folderpath { get; set; }
        public IFormFile file { get; set; }
    }
    public class FileMasterDto
    {
        public string name { get; set; }
        public string tags { get; set; }
        public string id { get; set; }
    }
}
