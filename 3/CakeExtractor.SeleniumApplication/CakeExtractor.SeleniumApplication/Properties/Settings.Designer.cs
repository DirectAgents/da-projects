﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CakeExtractor.SeleniumApplication.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.8.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://advertising.amazon.com/sign-in/")]
        public string SignInPageUrl {
            get {
                return ((string)(this["SignInPageUrl"]));
            }
            set {
                this["SignInPageUrl"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://advertising.amazon.com/cm/campaigns")]
        public string CampaignsPageUrl {
            get {
                return ((string)(this["CampaignsPageUrl"]));
            }
            set {
                this["CampaignsPageUrl"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("email")]
        public string EMail {
            get {
                return ((string)(this["EMail"]));
            }
            set {
                this["EMail"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("password")]
        public string EMailPassword {
            get {
                return ((string)(this["EMailPassword"]));
            }
            set {
                this["EMailPassword"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WebDriverDownloads")]
        public string DownloadsDirectoryName {
            get {
                return ((string)(this["DownloadsDirectoryName"]));
            }
            set {
                this["DownloadsDirectoryName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("CAMPAIGN_*.csv")]
        public string FilesNameTemplate {
            get {
                return ((string)(this["FilesNameTemplate"]));
            }
            set {
                this["FilesNameTemplate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30")]
        public int WaitPageTimeoutInMinuts {
            get {
                return ((int)(this["WaitPageTimeoutInMinuts"]));
            }
            set {
                this["WaitPageTimeoutInMinuts"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2018-11-23")]
        public global::System.DateTime StartExtractionDateTime {
            get {
                return ((global::System.DateTime)(this["StartExtractionDateTime"]));
            }
            set {
                this["StartExtractionDateTime"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int ExtractionIntervalsInDays {
            get {
                return ((int)(this["ExtractionIntervalsInDays"]));
            }
            set {
                this["ExtractionIntervalsInDays"] = value;
            }
        }
    }
}
