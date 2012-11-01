using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiClient.Models.DirectTrack
{
    public class DirectTrackDbContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public DbSet<DirectTrackResource> DirectTrackResources { get; set; }
        public DbSet<DirectTrackApiCall> DirectTrackApiCalls { get; set; }
        public DbSet<CumulativeCampaignStat> CumulativeCampaignStats { get; set; }
        public DbSet<leadDetail> LeadDetails { get; set; }
        public DbSet<programLead> ProgramLeads { get; set; }

        public int PointsUsedInLastMinutes(int minutes, int accessID)
        {
            DateTime ago = DateTime.UtcNow.AddMinutes(-1 * minutes);

            var query = from c in DirectTrackResources
                        where c.AccessId == accessID && c.Timestamp >= ago
                        select c;

            int result = query.Sum(c => c.PointsUsed);

            return result;
        }
    }

    public class DirectTrackResource
    {
        [Key]
        [System.ComponentModel.DataAnnotations.MaxLength(500)]
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public int PointsUsed { get; set; }
        public int AccessId { get; set; }
    }

    public class DirectTrackApiCall
    {
        [Key]
        [System.ComponentModel.DataAnnotations.MaxLength(500)]
        public string Url { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
        public int PointsUsed { get; set; }
    }

    public class CumulativeCampaignStat
    {
        public virtual DirectTrackApiCall DirectTrackApiCall { get; set; }
        [Key]
        [MaxLength(500)]
        [ForeignKey("DirectTrackApiCall")]
        public string Url { get; set; }
        public int impressions { get; set; }
        public int contextualImpressions { get; set; }
        public int clicks { get; set; }
        public double clickthru { get; set; }
        public int leads { get; set; }
        public double signups { get; set; }
        public int numSales { get; set; }
        public decimal saleAmount { get; set; }
        public int numSubSales { get; set; }
        public decimal subSaleAmount { get; set; }
        public decimal theyGet { get; set; }
        public decimal weGet { get; set; }
        public double EPC { get; set; }
        public decimal revenue { get; set; }
        [MaxLength(3)]
        public string currency { get; set; }
    }

    #region Detailed Leads and Program Leads
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.digitalriver.com/directtrack/api/programLead/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.digitalriver.com/directtrack/api/programLead/v1_0", IsNullable = false)]
    public partial class affiliateResourceURL
    {

        private string locationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.digitalriver.com/directtrack/api/programLead/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.digitalriver.com/directtrack/api/programLead/v1_0", IsNullable = false)]
    public partial class campaignResourceURL
    {

        private string locationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.digitalriver.com/directtrack/api/programLead/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.digitalriver.com/directtrack/api/programLead/v1_0", IsNullable = false)]
    public partial class creativeResourceURL
    {

        private string locationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.digitalriver.com/directtrack/api/programLead/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.digitalriver.com/directtrack/api/programLead/v1_0", IsNullable = false)]
    public partial class creativeDeploymentResourceURL
    {

        private string locationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.digitalriver.com/directtrack/api/programLead/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.digitalriver.com/directtrack/api/programLead/v1_0", IsNullable = false)]
    public partial class subIDs
    {

        private string subID1Field;

        private string subID2Field;

        private string subID3Field;

        private string subID4Field;

        private string subID5Field;

        /// <remarks/>
        public string subID1
        {
            get
            {
                return this.subID1Field;
            }
            set
            {
                this.subID1Field = value;
            }
        }

        /// <remarks/>
        public string subID2
        {
            get
            {
                return this.subID2Field;
            }
            set
            {
                this.subID2Field = value;
            }
        }

        /// <remarks/>
        public string subID3
        {
            get
            {
                return this.subID3Field;
            }
            set
            {
                this.subID3Field = value;
            }
        }

        /// <remarks/>
        public string subID4
        {
            get
            {
                return this.subID4Field;
            }
            set
            {
                this.subID4Field = value;
            }
        }

        /// <remarks/>
        public string subID5
        {
            get
            {
                return this.subID5Field;
            }
            set
            {
                this.subID5Field = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.digitalriver.com/directtrack/api/programLead/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.digitalriver.com/directtrack/api/programLead/v1_0", IsNullable = false)]
    public partial class programLead
    {

        private uint numLeadsField;

        private string cookieIDField;

        private affiliateResourceURL affiliateResourceURLField;

        private campaignResourceURL campaignResourceURLField;

        private creativeResourceURL creativeResourceURLField;

        private creativeDeploymentResourceURL creativeDeploymentResourceURLField;

        private string dateField;

        private subIDs subIDsField;

        private uint landingPageIDField;

        private bool landingPageIDFieldSpecified;

        private uint poolIDField;

        private bool poolIDFieldSpecified;

        private string theyGetField;

        private string weGetField;

        private string locationField;

        /// <remarks/>
        public uint numLeads
        {
            get
            {
                return this.numLeadsField;
            }
            set
            {
                this.numLeadsField = value;
            }
        }

        /// <remarks/>
        public string cookieID
        {
            get
            {
                return this.cookieIDField;
            }
            set
            {
                this.cookieIDField = value;
            }
        }

        /// <remarks/>
        public affiliateResourceURL affiliateResourceURL
        {
            get
            {
                return this.affiliateResourceURLField;
            }
            set
            {
                this.affiliateResourceURLField = value;
            }
        }

        /// <remarks/>
        public campaignResourceURL campaignResourceURL
        {
            get
            {
                return this.campaignResourceURLField;
            }
            set
            {
                this.campaignResourceURLField = value;
            }
        }

        /// <remarks/>
        public creativeResourceURL creativeResourceURL
        {
            get
            {
                return this.creativeResourceURLField;
            }
            set
            {
                this.creativeResourceURLField = value;
            }
        }

        /// <remarks/>
        public creativeDeploymentResourceURL creativeDeploymentResourceURL
        {
            get
            {
                return this.creativeDeploymentResourceURLField;
            }
            set
            {
                this.creativeDeploymentResourceURLField = value;
            }
        }

        /// <remarks/>
        public string date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        public subIDs subIDs
        {
            get
            {
                return this.subIDsField;
            }
            set
            {
                this.subIDsField = value;
            }
        }

        /// <remarks/>
        public uint landingPageID
        {
            get
            {
                return this.landingPageIDField;
            }
            set
            {
                this.landingPageIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool landingPageIDSpecified
        {
            get
            {
                return this.landingPageIDFieldSpecified;
            }
            set
            {
                this.landingPageIDFieldSpecified = value;
            }
        }

        /// <remarks/>
        public uint poolID
        {
            get
            {
                return this.poolIDField;
            }
            set
            {
                this.poolIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool poolIDSpecified
        {
            get
            {
                return this.poolIDFieldSpecified;
            }
            set
            {
                this.poolIDFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string theyGet
        {
            get
            {
                return this.theyGetField;
            }
            set
            {
                this.theyGetField = value;
            }
        }

        /// <remarks/>
        public string weGet
        {
            get
            {
                return this.weGetField;
            }
            set
            {
                this.weGetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        [Key]
        [MaxLength(500)]
        public string location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.digitalriver.com/directtrack/api/leadDetail/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute("affiliateResourceURL", Namespace = "http://www.digitalriver.com/directtrack/api/leadDetail/v1_0", IsNullable = false)]
    public partial class affiliateResourceURL1
    {

        private string locationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.digitalriver.com/directtrack/api/leadDetail/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute("campaignResourceURL", Namespace = "http://www.digitalriver.com/directtrack/api/leadDetail/v1_0", IsNullable = false)]
    public partial class campaignResourceURL1
    {

        private string locationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.digitalriver.com/directtrack/api/leadDetail/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute("creativeResourceURL", Namespace = "http://www.digitalriver.com/directtrack/api/leadDetail/v1_0", IsNullable = false)]
    public partial class creativeResourceURL1
    {

        private string locationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.digitalriver.com/directtrack/api/leadDetail/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute("creativeDeploymentResourceURL", Namespace = "http://www.digitalriver.com/directtrack/api/leadDetail/v1_0", IsNullable = false)]
    public partial class creativeDeploymentResourceURL1
    {

        private string locationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.digitalriver.com/directtrack/api/leadDetail/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute("subIDs", Namespace = "http://www.digitalriver.com/directtrack/api/leadDetail/v1_0", IsNullable = false)]
    public partial class subIDs1
    {

        private string subID1Field;

        private string subID2Field;

        private string subID3Field;

        private string subID4Field;

        private string subID5Field;

        /// <remarks/>
        public string subID1
        {
            get
            {
                return this.subID1Field;
            }
            set
            {
                this.subID1Field = value;
            }
        }

        /// <remarks/>
        public string subID2
        {
            get
            {
                return this.subID2Field;
            }
            set
            {
                this.subID2Field = value;
            }
        }

        /// <remarks/>
        public string subID3
        {
            get
            {
                return this.subID3Field;
            }
            set
            {
                this.subID3Field = value;
            }
        }

        /// <remarks/>
        public string subID4
        {
            get
            {
                return this.subID4Field;
            }
            set
            {
                this.subID4Field = value;
            }
        }

        /// <remarks/>
        public string subID5
        {
            get
            {
                return this.subID5Field;
            }
            set
            {
                this.subID5Field = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.digitalriver.com/directtrack/api/leadDetail/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.digitalriver.com/directtrack/api/leadDetail/v1_0", IsNullable = false)]
    public partial class leadDetail
    {

        private string cookieIDField;

        private affiliateResourceURL1 affiliateResourceURLField;

        private campaignResourceURL1 campaignResourceURLField;

        private creativeResourceURL1 creativeResourceURLField;

        private creativeDeploymentResourceURL1 creativeDeploymentResourceURLField;

        private string dateField;

        private subIDs1 subIDsField;

        private string ipAddressField;

        private string refererURLField;

        private string affOptInfoField;

        private string advOptInfoField;

        private uint landingPageIDField;

        private bool landingPageIDFieldSpecified;

        private uint poolIDField;

        private bool poolIDFieldSpecified;

        private string locationField;

        /// <remarks/>
        public string cookieID
        {
            get
            {
                return this.cookieIDField;
            }
            set
            {
                this.cookieIDField = value;
            }
        }

        /// <remarks/>
        public affiliateResourceURL1 affiliateResourceURL
        {
            get
            {
                return this.affiliateResourceURLField;
            }
            set
            {
                this.affiliateResourceURLField = value;
            }
        }

        /// <remarks/>
        public campaignResourceURL1 campaignResourceURL
        {
            get
            {
                return this.campaignResourceURLField;
            }
            set
            {
                this.campaignResourceURLField = value;
            }
        }

        /// <remarks/>
        public creativeResourceURL1 creativeResourceURL
        {
            get
            {
                return this.creativeResourceURLField;
            }
            set
            {
                this.creativeResourceURLField = value;
            }
        }

        /// <remarks/>
        public creativeDeploymentResourceURL1 creativeDeploymentResourceURL
        {
            get
            {
                return this.creativeDeploymentResourceURLField;
            }
            set
            {
                this.creativeDeploymentResourceURLField = value;
            }
        }

        /// <remarks/>
        public string date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        public subIDs1 subIDs
        {
            get
            {
                return this.subIDsField;
            }
            set
            {
                this.subIDsField = value;
            }
        }

        /// <remarks/>
        public string ipAddress
        {
            get
            {
                return this.ipAddressField;
            }
            set
            {
                this.ipAddressField = value;
            }
        }

        /// <remarks/>
        public string refererURL
        {
            get
            {
                return this.refererURLField;
            }
            set
            {
                this.refererURLField = value;
            }
        }

        /// <remarks/>
        public string affOptInfo
        {
            get
            {
                return this.affOptInfoField;
            }
            set
            {
                this.affOptInfoField = value;
            }
        }

        /// <remarks/>
        public string advOptInfo
        {
            get
            {
                return this.advOptInfoField;
            }
            set
            {
                this.advOptInfoField = value;
            }
        }

        /// <remarks/>
        public uint landingPageID
        {
            get
            {
                return this.landingPageIDField;
            }
            set
            {
                this.landingPageIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool landingPageIDSpecified
        {
            get
            {
                return this.landingPageIDFieldSpecified;
            }
            set
            {
                this.landingPageIDFieldSpecified = value;
            }
        }

        /// <remarks/>
        public uint poolID
        {
            get
            {
                return this.poolIDField;
            }
            set
            {
                this.poolIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool poolIDSpecified
        {
            get
            {
                return this.poolIDFieldSpecified;
            }
            set
            {
                this.poolIDFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        [Key]
        [MaxLength(500)]
        public string location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }
    }
    #endregion
}
