using System.Collections.Generic;
using System.Linq;
using LTWeb.Common;
using LTWeb.DataAccess;
using LTWeb.Service;

namespace LTWeb
{
    public static class Session
    {
        public static void Reset()
        {
            SessionUility.Clear(SessionKeys.LTModel);
            SessionUility.Clear(SessionKeys.Admin);
        }

        public static ILendingTreeModel LTModel
        {
            get
            {
                return SessionUility.GetOrCreate<ILendingTreeModel>(SessionKeys.LTModel, () => new LendingTreeModel(AppSettings.LendingTreeServiceConfig));
            }
        }

        public static Dictionary<string, string> Admin
        {
            get
            {
                return SessionUility.GetOrCreate<Dictionary<string, string>>(SessionKeys.Admin, () =>
                {
                    using (var db = new LTWebDataContext())
                    {
                        return db.AdminSettings.ToDictionary(c => c.Name, c => c.Value);
                    }
                });
            }
        }
    }
}