using System;
using CakeMarketing.WebServices.V1;

namespace CakeMarketing.Service
{
    internal class CakeWebService
    {
        private static readonly string ApiKey = "FCjdYAcwQE";
        private static readonly string GetServiceConfigurationName = "getSoap";
        private static readonly object Locker = new object();
        private currency[] currencies = null;

        internal currency[] Currencies()
        {
            lock (Locker)
            {
                if (this.currencies == null)
                {
                    var result = Client().Currencies(ApiKey);

                    if (!result.success)
                        throw new Exception("Failed to get currencies.");

                    this.currencies = result.currencies;
                }

                return this.currencies;
            }
        }

        private static getSoapClient Client()
        {
            return new WebServices.V1.getSoapClient(GetServiceConfigurationName); 
        }
    }
}
