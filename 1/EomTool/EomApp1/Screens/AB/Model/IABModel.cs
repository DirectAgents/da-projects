using System;
using System.Collections.Generic;
namespace EomApp1.Screens.AB.Model
{
    interface IABModel
    {
        List<Data.Advertiser> GetAdvertisers();
        List<Data.ABItem> GetABItems(string advertiserName, bool convertAmountsToStartingBalanceCurrency);
    }
}
