﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EomApp1.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=aaron\\sqlexpress;Initial Catalog=DADatabaseR1;Integrated Security=Tru" +
            "e")]
        public string DADatabaseR1ConnectionString {
            get {
                return ((string)(this["DADatabaseR1ConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=aaron\\sqlexpress;Initial Catalog=Nov10Eom2;Integrated Security=True")]
        public string Nov10Eom2ConnectionString2 {
            get {
                return ((string)(this["Nov10Eom2ConnectionString2"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Dec 10")]
        public string DADatabaseName {
            get {
                return ((string)(this["DADatabaseName"]));
            }
            set {
                this["DADatabaseName"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz2\\da;Initial Catalog=DAMain1;Integrated Security=True")]
        public string DAMain1ConnectionString {
            get {
                return ((string)(this["DAMain1ConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=aaron\\sqlexpress;Initial Catalog=DADatabaseR2;Integrated Security=Tru" +
            "e")]
        public string DADatabaseR2ConnectionString {
            get {
                return ((string)(this["DADatabaseR2ConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=aaron\\sqlexpress;Initial Catalog=DADatabaseMarch11;Integrated Securit" +
            "y=True")]
        public string DADatabaseMarch11ConnectionString {
            get {
                return ((string)(this["DADatabaseMarch11ConnectionString"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("n/a")]
        public string PubReportLastRefresh {
            get {
                return ((string)(this["PubReportLastRefresh"]));
            }
            set {
                this["PubReportLastRefresh"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string AccountingSheetSettingsString {
            get {
                return ((string)(this["AccountingSheetSettingsString"]));
            }
            set {
                this["AccountingSheetSettingsString"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz2\\da;Initial Catalog=DADatabaseJul11;Integrated Security=True")]
        public string DADatabaseJul11ConnectionString {
            get {
                return ((string)(this["DADatabaseJul11ConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz2\\da;Initial Catalog=DADatabaseAug11;Integrated Security=True")]
        public string DADatabaseAug11ConnectionString {
            get {
                return ((string)(this["DADatabaseAug11ConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz2\\da;Initial Catalog=DirectTrack;Integrated Security=True")]
        public string DirectTrackConnectionString {
            get {
                return ((string)(this["DirectTrackConnectionString"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool CampaignSynchViewPayoutsChecked {
            get {
                return ((bool)(this["CampaignSynchViewPayoutsChecked"]));
            }
            set {
                this["CampaignSynchViewPayoutsChecked"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool CampaignSynchViewStatsChecked {
            get {
                return ((bool)(this["CampaignSynchViewStatsChecked"]));
            }
            set {
                this["CampaignSynchViewStatsChecked"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string CampaignSynchViewFromDay {
            get {
                return ((string)(this["CampaignSynchViewFromDay"]));
            }
            set {
                this["CampaignSynchViewFromDay"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string CampaignSynchViewToDay {
            get {
                return ((string)(this["CampaignSynchViewToDay"]));
            }
            set {
                this["CampaignSynchViewToDay"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz2\\da;Initial Catalog=DirectTrack3;Integrated Security=True")]
        public string DirectTrack3ConnectionString {
            get {
                return ((string)(this["DirectTrack3ConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz2\\da;Initial Catalog=DADatabaseApril2012_before_cake;Integrated Se" +
            "curity=True")]
        public string DADatabaseApril2012_before_cakeConnectionString {
            get {
                return ((string)(this["DADatabaseApril2012_before_cakeConnectionString"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DirectTrack")]
        public global::EomApp1.Screens.Synch.TargetSystemChoice SynchScreenTargetSystemChoice {
            get {
                return ((global::EomApp1.Screens.Synch.TargetSystemChoice)(this["SynchScreenTargetSystemChoice"]));
            }
            set {
                this["SynchScreenTargetSystemChoice"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz2\\da;Initial Catalog=zDADatabaseJuly2012Test;Integrated Security=T" +
            "rue")]
        public string OverrideConnectionString {
            get {
                return ((string)(this["OverrideConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2012-07-01")]
        public global::System.DateTime OverrideDate {
            get {
                return ((global::System.DateTime)(this["OverrideDate"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("directagents\\aanodide,directagents\\kslesinsky")]
        public string TestUserIdentities {
            get {
                return ((string)(this["TestUserIdentities"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool TestMode {
            get {
                return ((bool)(this["TestMode"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz2\\da;Initial Catalog=zDADatabaseJuly2012Test;Integrated Security=T" +
            "rue")]
        public string zDADatabaseJuly2012TestConnectionString {
            get {
                return ((string)(this["zDADatabaseJuly2012TestConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz2\\da;Initial Catalog=EomToolSecurity;Integrated Security=True")]
        public string EomToolSecurityConnectionString {
            get {
                return ((string)(this["EomToolSecurityConnectionString"]));
            }
        }
    }
}
