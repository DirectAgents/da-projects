﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using LTWeb.DataAccess;
using LTWeb.Service;

namespace LTWeb
{
    public static class Settings
    {
        static string AdminSettingsKey = "AdminSettings";
        static string LTModelKey = "LTModel";

        public static void Reset()
        {
            SessionAdminSettings = CreateAdminSettings();
            SessionLTModel = CreateLTModel();
        }

        public static Dictionary<string, string> Admin
        {
            get { return SessionAdminSettings ?? CreateAdminSettings(); }
        }

        static Dictionary<string, string> SessionAdminSettings
        {
            get { return HttpContext.Current.Session[AdminSettingsKey] as Dictionary<string, string>; }
            set { HttpContext.Current.Session[AdminSettingsKey] = value; }
        }

        static Dictionary<string, string> CreateAdminSettings()
        {
            using (var db = new LTWebDataContext())
                return db.AdminSettings.ToDictionary(c => c.Name, c => c.Value);
        }

        public static ILendingTreeModel LTModel
        {
            get { return SessionLTModel ?? CreateLTModel(); }
        }

        static ILendingTreeModel SessionLTModel
        {
            get { return HttpContext.Current.Session[LTModelKey] as ILendingTreeModel; }
            set { HttpContext.Current.Session[LTModelKey] = value; }
        }

        static ILendingTreeModel CreateLTModel()
        {
            return new LendingTreeModel("Test"); // TODO: un-hardcode
        }
    }
}