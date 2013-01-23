using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountingBackupWeb.Models.AccountingBackup
{
    public partial class EntryType
    {
        static Lazy<Dictionary<eEntryTypes, EntryType>> EntryTypes = new Lazy<Dictionary<eEntryTypes, EntryType>>(() =>
        {
            using (var model = new AccountingBackupEntities())
            {
                return model.EntryTypes.ToDictionary(et => (eEntryTypes)Enum.Parse(typeof(eEntryTypes), et.Name));
            }
        });

        public static EntryType Get(eEntryTypes entryType)
        {
            var result = EntryTypes.Value[entryType];
            return result;
        }
    }
}
