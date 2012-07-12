using System;
using System.ComponentModel;
using System.Data.Objects;
using System.Windows.Forms;

namespace EomAppControls.Common
{
    public class UserControlBase : UserControl
    {
        private Func<ObjectContext> createDB;

        // Parameterless constructor keeps WinForms designer happy.
        public UserControlBase()
        {
            this.createDB = () => null;
        }

        public UserControlBase(Func<ObjectContext> createDB)
        {
            this.createDB = createDB;
        }

        protected void WithContext<T>(Action<T> action, bool saveChanges) where T : ObjectContext
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

        protected bool IsRunning
        {
            get
            {
                return (LicenseManager.UsageMode == LicenseUsageMode.Runtime);
            }
        }
    }
}
