using System;

namespace DirectTrack
{
    public class ApiInfo
    {
        public Uri BaseUrl { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
        public string ClientId { get; set; }
        public string AccessId { get; set; }

        public ApiInfo()
        {
            this.Login = "directagents";
            this.Password = "Il1K3m0N3y";
            this.ClientId = "1137";
            this.AccessId = "1";
            this.BaseUrl = new Uri("https://da-tracking.com/apifleet/rest/" + ClientId + "/");
        }
    }
}
