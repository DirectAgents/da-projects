﻿using System.Linq;
namespace EomApp1.Screens.PubRep1.Data
{
    partial class Vendor
    {
        internal static int GetOrCreate(string name)
        {
            PRDataDataContext db = new PRDataDataContext(EomAppCommon.EomAppSettings.ConnStr);
            var query = from c in db.Vendors
                        where c.name == name
                        select c;
            Vendor v = query.FirstOrDefault();
            if (v == null)
            {
                v = new Vendor();
                db.Vendors.InsertOnSubmit(v);
            }
            v.name = name;
            db.SubmitChanges();
            return v.id;
        }
    }
}
