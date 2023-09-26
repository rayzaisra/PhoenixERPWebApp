using System.ComponentModel.DataAnnotations;
 

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class MsGraphLogin
    {
        [Required]
        public string client_id { get; set; }
        [Required]
        public string client_secret { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string grant_type { get; set; }
        [Required]
        public string resource { get; set; }
        [Required]
        public string scope { get; set; }
        public string requested_token_use { get; set; }
    }
    public class MsGraphLoginresult
    {
        public string token_type { get; set; }
        public string scope { get; set; }
        public string expires_in { get; set; }
        public string ext_expires_in { get; set; }
        public string expires_on { get; set; }
        public string not_before { get; set; }
        public string resource { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string id_token { get; set; }
    }
 }
