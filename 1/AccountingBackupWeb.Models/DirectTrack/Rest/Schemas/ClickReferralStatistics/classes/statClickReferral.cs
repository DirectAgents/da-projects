﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.1.
// 
namespace AccountingBackupWeb.Models.DirectTrack.Schemas.Classes.StatClickReferral {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/statClickReferral/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/statClickReferral/v1_0", IsNullable=false)]
    public partial class statClickReferral {
        
        private uint numClicksField;
        
        private uint numDomainsField;
        
        private uint numIPsField;
        
        private uint numCampaignsField;
        
        private uint numAffiliatesField;
        
        private uint numLeadsField;
        
        private uint numSalesField;
        
        private decimal revenueField;
        
        private string locationField;
        
        /// <remarks/>
        public uint numClicks {
            get {
                return this.numClicksField;
            }
            set {
                this.numClicksField = value;
            }
        }
        
        /// <remarks/>
        public uint numDomains {
            get {
                return this.numDomainsField;
            }
            set {
                this.numDomainsField = value;
            }
        }
        
        /// <remarks/>
        public uint numIPs {
            get {
                return this.numIPsField;
            }
            set {
                this.numIPsField = value;
            }
        }
        
        /// <remarks/>
        public uint numCampaigns {
            get {
                return this.numCampaignsField;
            }
            set {
                this.numCampaignsField = value;
            }
        }
        
        /// <remarks/>
        public uint numAffiliates {
            get {
                return this.numAffiliatesField;
            }
            set {
                this.numAffiliatesField = value;
            }
        }
        
        /// <remarks/>
        public uint numLeads {
            get {
                return this.numLeadsField;
            }
            set {
                this.numLeadsField = value;
            }
        }
        
        /// <remarks/>
        public uint numSales {
            get {
                return this.numSalesField;
            }
            set {
                this.numSalesField = value;
            }
        }
        
        /// <remarks/>
        public decimal revenue {
            get {
                return this.revenueField;
            }
            set {
                this.revenueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
        public string location {
            get {
                return this.locationField;
            }
            set {
                this.locationField = value;
            }
        }
    }
}
