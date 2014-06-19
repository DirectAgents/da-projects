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
                    public static string TextOnButtonWhenMediaBuyerApprovalStatusIsDefault = "Queue";
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
//        private static Dictionary<string, string> SettingsDictionary;
        
        public string EomAppSettings_MediaBuyerWorkflow_Email_From { get { return GetSetting("EomAppSettings_MediaBuyerWorkflow_Email_From"); } }
        public string EomAppSettings_MediaBuyerWorkflow_Email_Subject { get { return GetSetting("EomAppSettings_MediaBuyerWorkflow_Email_Subject"); } }
        public string EomAppSettings_MediaBuyerWorkflow_Email_Link { get { return GetSetting("EomAppSettings_MediaBuyerWorkflow_Email_Link"); } }

        public int? EomAppSettings_PaymentWorkflow_First_Batch_Threshold { get { return GetIntSetting("EomAppSettings_PaymentWorkflow_First_Batch_Threshold"); } }
        public string EomAppSettings_PaymentWorkflow_First_Batch_Approver { get { return GetSetting("EomAppSettings_PaymentWorkflow_First_Batch_Approver"); } }
        public string EomAppSettings_PaymentWorkflow_Second_Batch_Approver { get { return GetSetting("EomAppSettings_PaymentWorkflow_Second_Batch_Approver"); } }

        public decimal? EomAppSettings_FinalizationWorkflow_MinimumMargin { get { return GetDecimalSetting("EomAppSettings_FinalizationWorkflow_MinimumMargin"); } }

        private int? GetIntSetting(string settingName)
        {
            var settingVal = GetSetting(settingName);
            int intVal;
            if (int.TryParse(settingVal, out intVal))
                return intVal;
            else
                return null;
        }
        private decimal? GetDecimalSetting(string settingName)
        {
            var settingVal = GetSetting(settingName);
            decimal decimalVal;
            if (decimal.TryParse(settingVal, out decimalVal))
                return decimalVal;
            else
                return null;
        }

        private string GetSetting(string settingName)
        {
/*
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
*/
            using (var con = new SqlConnection(EomAppSettings.SettingsConnectionString))
            using (var cmd = new SqlCommand("select SettingValue from Settings where SettingName = '" + settingName + "'", con))
            {
                con.Open();
                return (string)cmd.ExecuteScalar();
            }
        }
    }
}
