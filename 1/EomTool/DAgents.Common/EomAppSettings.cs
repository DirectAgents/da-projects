using System.Data.SqlClient;
using System.Collections.Generic;

namespace EomAppCommon
{
    public static class EomAppSettings
    {
        public static string ConnStr { get; set; }
        
        public static string MasterDatabaseListConnectionString { get; set; }
        
        public static string SettingsConnectionString { get; set; }
        
        public static string AdminGroupName = @"DIRECTAGENTS\Biz App Admins";
        
        public static class Screens
        {
            public static class Workflow
            {
                public static class ApprovalColumn
                {
                    public static string TextOnButtonWhenMediaBuyerApprovalStatusIsDefault = "Check";
                }
            }
        }

        public static Settings Settings
        {
            get
            {
                if(_Settings == null)
                    _Settings = new Settings();
                return _Settings;
            }
        }
        private static Settings _Settings;
    }

    public class Settings
    {
        private static Dictionary<string, string> SettingsDictionary;
        
        public string EomAppSettings_MediaBuyerWorkflow_Email_From { get { return GetSetting("EomAppSettings_MediaBuyerWorkflow_Email_From"); } }
        
        public string EomAppSettings_MediaBuyerWorkflow_Email_Subject { get { return GetSetting("EomAppSettings_MediaBuyerWorkflow_Email_Subject"); } }
        
        private string GetSetting(string settingName)
        {
            if (SettingsDictionary == null)
            {
                SettingsDictionary = new Dictionary<string, string>();
                using (var con = new SqlConnection(EomAppSettings.SettingsConnectionString))
                using (var cmd = new SqlCommand("select SettingName, SettingValue from Settings", con))
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.NextResult())
                        {
                            SettingsDictionary[(string)reader["SettingName"]] = (string)reader["SettingValue"];
                        }
                    }
                }
            }

            using (var con = new SqlConnection(EomAppSettings.SettingsConnectionString))
            using (var cmd = new SqlCommand("select SettingValue from Settings where SettingName = '" + settingName + "'", con))
            {
                con.Open();
                return (string)cmd.ExecuteScalar();
            }
        }
    }
}
