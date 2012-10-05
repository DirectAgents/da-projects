using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Model.Staging;

namespace DirectAgents.Domain.Concrete
{
    internal class CountryLookup
    {
        CakeStagingEntities cake;
        IDictionary<int, IEnumerable<string>> lookup;

        public CountryLookup(CakeStagingEntities cake)
        {
            this.cake = cake;
        }

        public IEnumerable<string> this[int offerID]
        {
            get
            {
                if (this.lookup == null)
                {
                    var offers = from o in cake.CakeOffers.ToList()
                                 let Id = o.Offer_Id
                                 let Countries = from part in SplitOfferName(o.OfferName)
                                                 where IsCountryCode(part)
                                                 select part
                                 select new { Id, Countries };

                    this.lookup = offers.ToDictionary(x => x.Id, x => x.Countries);
                }
                return this.lookup[offerID];
            }
        }

        static string[] SplitOfferName(string offerName)
        {
            var split = offerName.Split(new[] { ' ', '-', '/' }, StringSplitOptions.RemoveEmptyEntries);
            return split;
        }

        static bool IsCountryCode(string str)
        {
            bool result = str != "DA" && str.Length == 2 && str.ToCharArray().All(c => IsUpperAlpha(c));
            return result;
        }

        static bool IsUpperAlpha(char c)
        {
            return c >= 'A' && c <= 'Z';
        }
    }
}
