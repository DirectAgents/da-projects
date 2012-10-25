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
            if (LoginAccessId == 1 || LoginAccessId == 0)
            {
                this.Login = "directagents";
                this.Password = "Il1K3m0N3y";
                this.ClientId = "1137";
                this.AccessId = "1";
            }
            else if (LoginAccessId == 3)
            {
                this.Login = "dadt";
                this.Password = "1423qrwe";
                this.ClientId = "1137";
                this.AccessId = "3";
            }

            this.BaseUrl = new Uri("https://da-tracking.com/apifleet/rest/" + ClientId + "/");
        }

        public static int LoginAccessId { get; set; }
    }
}
