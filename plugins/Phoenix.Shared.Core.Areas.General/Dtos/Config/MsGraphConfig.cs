namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class MsGraphConfig
    {
        public bool use_mslogin { get; set; }
        public string tenant_id { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string resource { get; set; }
        public string scope { get; set; }
        public string login_url { get; set; }
        public MsGraphConfig() { }
    }
}
