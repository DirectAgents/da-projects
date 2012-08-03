using System;
using System.ComponentModel;
using System.Data.Objects;
using System.Windows.Forms;

namespace EomAppControls
{
    public class UserControlBase : UserControl
    {
        private Func<ObjectContext> createDB;

        public UserControlBase()
        {
            this.createDB = () => null;
        }

        public UserControlBase(Func<ObjectContext> createDB)
        {
            this.createDB = createDB;
        }

        protected void UsingDB<T>(Action<T> action, bool saveChanges) where T : ObjectContext
        {
            var db = this.createDB();
            if (db != null)
            {
                using (db)
                {
                    action((T)db);
                    if (saveChanges)
                    {
                        db.SaveChanges();
                    }
                }
            }
        }

        protected bool Running
        {
            get
            {
                return (LicenseManager.UsageMode == LicenseUsageMode.Runtime);
            }
        }
    }
}
