using System.Linq;
using EomApp1.Screens.Synch.Models.Eom;

namespace EomApp1.Screens.Synch.Models.Cake
{
    public partial class CakeConversionSummary
    {
        //Cake/2012-05-18/aff:10901/offer:1539/type:CPA/paycur:EUR/paid:2/recvcur:EUR/recv:3
        public Item MatchingItem(EomDatabaseEntities eomEntities)
        {
            string name = this.Name;
            string keyPart = name.Substring(0, name.IndexOf("/paycur"));

            var matchingItem = from c in eomEntities.Items
                               where c.name.StartsWith(keyPart)
                               select c;

            return matchingItem.FirstOrDefault();
        }
    }
}
