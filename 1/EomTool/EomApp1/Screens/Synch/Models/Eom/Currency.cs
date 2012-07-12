using System;
using System.Collections.Generic;
using System.Linq;

namespace EomApp1.Screens.Synch.Models.Eom
{
    public partial class Currency
    {
        public static List<Currency> List { get { return _List.Value; } }

        private static Lazy<List<Currency>> _List = new Lazy<List<Currency>>(() => {
            using (var db = EomDatabaseEntities.Create())
            {
                return db.Currencies.ToList();
            }
        }, true);
    }
}
