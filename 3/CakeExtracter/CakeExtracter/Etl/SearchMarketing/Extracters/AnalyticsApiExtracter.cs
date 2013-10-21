using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Services;
using Google.Apis.Util;
using System;
using System.Collections.Generic;
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
            string certPath = "271266501f61f25ee49c19a3c5cf3bb9a769ba00-privatekey.p12";
            var certificate = new X509Certificate2(certPath, "notasecret", X509KeyStorageFlags.Exportable);
            var provider = new AssertionFlowClient(GoogleAuthenticationServer.Description, certificate)
            {
                //ServiceAccountId = "kevin@directagents.com",
                ServiceAccountId = "960492209499-cdktdkluj4sj5ee8bto7hdi7m058lfg9@developer.gserviceaccount.com",
                Scope = AnalyticsService.Scopes.AnalyticsReadonly.GetStringValue()
            };
            var auth = new OAuth2Authenticator<AssertionFlowClient>(provider, AssertionFlowClient.GetState);
            var service = new AnalyticsService(new BaseClientService.Initializer()
            {
                Authenticator = auth,
                ApplicationName = "DA Client Portal"
            });
            string profileId = "ga:14958389";
            string startDate = "2013-10-07";
            string endDate = "2013-10-13";
            string metrics = "ga:visits";
            DataResource.GaResource.GetRequest request = service.Data.Ga.Get(profileId, startDate, endDate, metrics);
            //request.Dimensions = "ga:date";
            GaData data = request.Execute();

            int x = 0;

            //var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description)
            //{
            //    ClientIdentifier = "",
            //    ClientSecret = ""
            //};
            //var auth = new OAuth2Authenticator<NativeApplicationClient>(provider, GetAuthorization);
        }

        //private static IAuthorizationState GetAuthorization(NativeApplicationClient arg)
        //{
        //    //arg.RefreshToken(
        //    return null;
        //}

    }
}
