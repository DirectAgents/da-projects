using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountingBackupWeb.Models.AccountingBackup
{
    public partial class Log
    {
        public static bool Enabled = true; // todo: DI

        public static void Write(string entry)
        {
            Write(entry, eEntryTypes.Default);
        }
        public static void Write(string entry, eEntryTypes entryType)
        {
            if (!Enabled) return;

            using (var model = new AccountingBackupEntities())
            {
                var log = new Log {
                    Entry = entry,
                    EntryType_Id = EntryType.Get(entryType).Id
                };
                model.Logs.AddObject(log);
                model.SaveChanges();
            }
        }

        public static void Clear()
        {
            if (!Enabled) return;

            using (var model = new AccountingBackupEntities())
            {
                foreach (var log in model.Logs)
                {
                    model.DeleteObject(log);
                }
                model.SaveChanges();
            }
        }
    }
}
