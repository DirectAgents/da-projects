﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EomApp1.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Sep 2012")]
        public string DADatabaseName {
            get {
                return ((string)(this["DADatabaseName"]));
            }
            set {
                this["DADatabaseName"] = value;
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
        [global::System.Configuration.DefaultSettingValueAttribute("2014-04-01")]
        public global::System.DateTime OverrideDate {
            get {
                return ((global::System.DateTime)(this["OverrideDate"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool TestMode {
            get {
                return ((bool)(this["TestMode"]));
            }
            set {
                this["TestMode"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("directagents\\aanodide,directagents\\kslesinsky,aaron-pavillion\\aaron,levon\\aaron")]
        public string TestUserIdentities {
            get {
                return ((string)(this["TestUserIdentities"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz\\sqlexpress;Initial Catalog=DADatabaseR1;User=sa;Password=sp0ngbOb" +
            "")]
        public string DADatabaseR1ConnectionString {
            get {
                return ((string)(this["DADatabaseR1ConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz\\sqlexpress;Initial Catalog=Nov10Eom2;Integrated Security=True")]
        public string Nov10Eom2ConnectionString2 {
            get {
                return ((string)(this["Nov10Eom2ConnectionString2"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz\\sqlexpress;Initial Catalog=DAMain1;User=sa;Password=sp0ngbOb")]
        public string DAMain1ConnectionString {
            get {
                return ((string)(this["DAMain1ConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz\\sqlexpress;Initial Catalog=DADatabaseR2;Integrated Security=True")]
        public string DADatabaseR2ConnectionString {
            get {
                return ((string)(this["DADatabaseR2ConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz\\sqlexpress;Initial Catalog=DADatabaseMarch11;Integrated Security=" +
            "True")]
        public string DADatabaseMarch11ConnectionString {
            get {
                return ((string)(this["DADatabaseMarch11ConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz\\sqlexpress;Initial Catalog=DADatabaseJul11;Integrated Security=Tr" +
            "ue")]
        public string DADatabaseJul11ConnectionString {
            get {
                return ((string)(this["DADatabaseJul11ConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz\\sqlexpress;Initial Catalog=DADatabaseAug11;Integrated Security=Tr" +
            "ue")]
        public string DADatabaseAug11ConnectionString {
            get {
                return ((string)(this["DADatabaseAug11ConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz\\sqlexpress;Initial Catalog=DirectTrack;Integrated Security=True")]
        public string DirectTrackConnectionString {
            get {
                return ((string)(this["DirectTrackConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz\\sqlexpress;Initial Catalog=DirectTrack3;Integrated Security=True")]
        public string DirectTrack3ConnectionString {
            get {
                return ((string)(this["DirectTrack3ConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz\\sqlexpress;Initial Catalog=DADatabaseApril2012_before_cake;Integr" +
            "ated Security=True")]
        public string DADatabaseApril2012_before_cakeConnectionString {
            get {
                return ((string)(this["DADatabaseApril2012_before_cakeConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz\\sqlexpress;Initial Catalog=zDADatabaseApr2014Test;User=sa;Passwor" +
            "d=sp0ngbOb")]
        public string OverrideConnectionString {
            get {
                return ((string)(this["OverrideConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz\\sqlexpress;Initial Catalog=EOMToolSecurity;Integrated Security=Tr" +
            "ue")]
        public string EomToolSecurityConnectionString {
            get {
                return ((string)(this["EomToolSecurityConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz\\sqlexpress;Initial Catalog=DADatabaseR1;Integrated Security=True")]
        public string DADatabaseR1ConnectionString1 {
            get {
                return ((string)(this["DADatabaseR1ConnectionString1"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz\\sqlexpress;Initial Catalog=EomToolSecurity;Integrated Security=Tr" +
            "ue")]
        public string OverrideEomToolSecurityConnectionString {
            get {
                return ((string)(this["OverrideEomToolSecurityConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz\\sqlexpress;Initial Catalog=zDADatabaseNov2012Test;Integrated Secu" +
            "rity=True")]
        public string zDADatabaseNov2012TestConnectionString {
            get {
                return ((string)(this["zDADatabaseNov2012TestConnectionString"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=biz\\sqlexpress;Initial Catalog=DAMain1;User=sa;Password=sp0ngbOb")]
        public string SettingsConnectionString {
            get {
                return ((string)(this["SettingsConnectionString"]));
            }
            set {
                this["SettingsConnectionString"] = value;
            }
        }
    }
}
