using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Analytics.v3;
using Google.Apis.Services;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Authentication.OAuth2;
using DotNetOpenAuth.OAuth2;
using System.Security.Cryptography.X509Certificates;

namespace CakeExtracter.Etl.SearchMarketing.Extracters
{
    public class AnalyticsApiExtracter : Extracter<Dictionary<string, string>>
    {
        private readonly string clientCustomerId;
        private readonly DateTime beginDate;
        private readonly DateTime endDate;

        public AnalyticsApiExtracter(string clientCustomerId, CakeExtracter.Common.DateRange dateRange)
        {
            this.clientCustomerId = clientCustomerId;
            this.beginDate = dateRange.FromDate;
            this.endDate = dateRange.ToDate;
        }

        protected override void Extract()
        {
            string certPath = "";
            var certificate = new X509Certificate2(certPath, "notasecret", X509KeyStorageFlags.Exportable);
            var provider = new AssertionFlowClient(GoogleAuthenticationServer.Description, certificate)
            {
                ServiceAccountId = "960492209499-cdktdkluj4sj5ee8bto7hdi7m058lfg9@developer.gserviceaccount.com",
                //Scope = DriveService
            };

            //var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description)
            //{
            //    ClientIdentifier = "",
            //    ClientSecret = ""
            //};
            //var auth = new OAuth2Authenticator<NativeApplicationClient>(provider, GetAuthorization);
            //var init = new BaseClientService.Initializer() {
            //var service = new AnalyticsService();
        }

        private static IAuthorizationState GetAuthorization(NativeApplicationClient arg)
        {
            //arg.RefreshToken(
            return null;
        }

    }
}
