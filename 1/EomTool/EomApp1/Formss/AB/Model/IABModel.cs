using System;
using System.Collections.Generic;
namespace EomApp1.Formss.AB.Model
{
    interface IABModel
    {
        List<Data.Advertiser> GetAdvertisers();
        List<Data.ABItem> GetABItems(string advertiserName, bool convertAmountsToStartingBalanceCurrency);
    }
}
