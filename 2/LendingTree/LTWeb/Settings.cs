using System.Collections.Generic;
using System.Linq;
using System.Web;
using LTWeb.DataAccess;
using LTWeb.Service;

namespace LTWeb
{
    // TODO: think about refactoring this whole Settings class as
    //       it is too complicated right now...
    public static class Settings
    {
        #region Session Keys

        static string AdminSettingsKey = "AdminSettings";
        static string LTModelKey = "LTModel"; 

        #endregion

        #region Public

        public static void Reset()
        {
            SessionAdminSettings = CreateAdminSettings();
            SessionLTModel = CreateLTModel();
        }

        public static Dictionary<string, string> Admin
        {
            get { return SessionAdminSettings ?? CreateAdminSettings(); }
        }

        public static ILendingTreeModel LTModel
        {
            get { return SessionLTModel ?? CreateLTModel(); }
        }

        #endregion

        #region Private

        static Dictionary<string, string> SessionAdminSettings
        {
            get { return HttpContext.Current.Session[AdminSettingsKey] as Dictionary<string, string>; }
            set { HttpContext.Current.Session[AdminSettingsKey] = value; }
        } 

        static Dictionary<string, string> CreateAdminSettings()
        {
            using (var db = new LTWebDataContext())
            {
                var adminSettings = db.AdminSettings.ToDictionary(c => c.Name, c => c.Value);
                SessionAdminSettings = adminSettings;
                return adminSettings;
            }
        }

        static ILendingTreeModel SessionLTModel
        {
            get { return HttpContext.Current.Session[LTModelKey] as ILendingTreeModel; }
            set { HttpContext.Current.Session[LTModelKey] = value; }
        }

        static ILendingTreeModel CreateLTModel()
        {
            var ltModel = new LendingTreeModel("Test"); // TODO: un-hardcode, should probably come from xml file at runtime
            SessionLTModel = ltModel;
            return ltModel;
        }
        #endregion
    }
}