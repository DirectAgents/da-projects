﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]

namespace EomApp1.Formss.Campaign2
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class Campaigns2Entities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new Campaigns2Entities object using the connection string found in the 'Campaigns2Entities' section of the application configuration file.
        /// </summary>
        public Campaigns2Entities() : base("name=Campaigns2Entities", "Campaigns2Entities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new Campaigns2Entities object.
        /// </summary>
        public Campaigns2Entities(string connectionString) : base(connectionString, "Campaigns2Entities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new Campaigns2Entities object.
        /// </summary>
        public Campaigns2Entities(EntityConnection connection) : base(connection, "Campaigns2Entities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Campaign> Campaign
        {
            get
            {
                if ((_Campaign == null))
                {
                    _Campaign = base.CreateObjectSet<Campaign>("Campaign");
                }
                return _Campaign;
            }
        }
        private ObjectSet<Campaign> _Campaign;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<AccountManager> AccountManager
        {
            get
            {
                if ((_AccountManager == null))
                {
                    _AccountManager = base.CreateObjectSet<AccountManager>("AccountManager");
                }
                return _AccountManager;
            }
        }
        private ObjectSet<AccountManager> _AccountManager;

        #endregion
        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Campaign EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToCampaign(Campaign campaign)
        {
            base.AddObject("Campaign", campaign);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the AccountManager EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToAccountManager(AccountManager accountManager)
        {
            base.AddObject("AccountManager", accountManager);
        }

        #endregion
    }
    

    #endregion
    
    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Campaigns2Model", Name="AccountManager")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class AccountManager : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new AccountManager object.
        /// </summary>
        /// <param name="id">Initial value of the id property.</param>
        /// <param name="name">Initial value of the name property.</param>
        public static AccountManager CreateAccountManager(global::System.Int32 id, global::System.String name)
        {
            AccountManager accountManager = new AccountManager();
            accountManager.id = id;
            accountManager.name = name;
            return accountManager;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    OnidChanging(value);
                    ReportPropertyChanging("id");
                    _id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("id");
                    OnidChanged();
                }
            }
        }
        private global::System.Int32 _id;
        partial void OnidChanging(global::System.Int32 value);
        partial void OnidChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String name
        {
            get
            {
                return _name;
            }
            set
            {
                OnnameChanging(value);
                ReportPropertyChanging("name");
                _name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("name");
                OnnameChanged();
            }
        }
        private global::System.String _name;
        partial void OnnameChanging(global::System.String value);
        partial void OnnameChanged();

        #endregion
    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Campaigns2Model", Name="Campaign")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Campaign : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Campaign object.
        /// </summary>
        /// <param name="id">Initial value of the id property.</param>
        /// <param name="account_manager_id">Initial value of the account_manager_id property.</param>
        /// <param name="campaign_status_id">Initial value of the campaign_status_id property.</param>
        /// <param name="ad_manager_id">Initial value of the ad_manager_id property.</param>
        /// <param name="advertiser_id">Initial value of the advertiser_id property.</param>
        /// <param name="pid">Initial value of the pid property.</param>
        /// <param name="campaign_name">Initial value of the campaign_name property.</param>
        /// <param name="modified">Initial value of the modified property.</param>
        public static Campaign CreateCampaign(global::System.Int32 id, global::System.Int32 account_manager_id, global::System.Int32 campaign_status_id, global::System.Int32 ad_manager_id, global::System.Int32 advertiser_id, global::System.Int32 pid, global::System.String campaign_name, global::System.DateTime modified)
        {
            Campaign campaign = new Campaign();
            campaign.id = id;
            campaign.account_manager_id = account_manager_id;
            campaign.campaign_status_id = campaign_status_id;
            campaign.ad_manager_id = ad_manager_id;
            campaign.advertiser_id = advertiser_id;
            campaign.pid = pid;
            campaign.campaign_name = campaign_name;
            campaign.modified = modified;
            return campaign;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    OnidChanging(value);
                    ReportPropertyChanging("id");
                    _id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("id");
                    OnidChanged();
                }
            }
        }
        private global::System.Int32 _id;
        partial void OnidChanging(global::System.Int32 value);
        partial void OnidChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 account_manager_id
        {
            get
            {
                return _account_manager_id;
            }
            set
            {
                Onaccount_manager_idChanging(value);
                ReportPropertyChanging("account_manager_id");
                _account_manager_id = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("account_manager_id");
                Onaccount_manager_idChanged();
            }
        }
        private global::System.Int32 _account_manager_id;
        partial void Onaccount_manager_idChanging(global::System.Int32 value);
        partial void Onaccount_manager_idChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 campaign_status_id
        {
            get
            {
                return _campaign_status_id;
            }
            set
            {
                Oncampaign_status_idChanging(value);
                ReportPropertyChanging("campaign_status_id");
                _campaign_status_id = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("campaign_status_id");
                Oncampaign_status_idChanged();
            }
        }
        private global::System.Int32 _campaign_status_id;
        partial void Oncampaign_status_idChanging(global::System.Int32 value);
        partial void Oncampaign_status_idChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 ad_manager_id
        {
            get
            {
                return _ad_manager_id;
            }
            set
            {
                Onad_manager_idChanging(value);
                ReportPropertyChanging("ad_manager_id");
                _ad_manager_id = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("ad_manager_id");
                Onad_manager_idChanged();
            }
        }
        private global::System.Int32 _ad_manager_id;
        partial void Onad_manager_idChanging(global::System.Int32 value);
        partial void Onad_manager_idChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 advertiser_id
        {
            get
            {
                return _advertiser_id;
            }
            set
            {
                Onadvertiser_idChanging(value);
                ReportPropertyChanging("advertiser_id");
                _advertiser_id = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("advertiser_id");
                Onadvertiser_idChanged();
            }
        }
        private global::System.Int32 _advertiser_id;
        partial void Onadvertiser_idChanging(global::System.Int32 value);
        partial void Onadvertiser_idChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 pid
        {
            get
            {
                return _pid;
            }
            set
            {
                OnpidChanging(value);
                ReportPropertyChanging("pid");
                _pid = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("pid");
                OnpidChanged();
            }
        }
        private global::System.Int32 _pid;
        partial void OnpidChanging(global::System.Int32 value);
        partial void OnpidChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String campaign_name
        {
            get
            {
                return _campaign_name;
            }
            set
            {
                Oncampaign_nameChanging(value);
                ReportPropertyChanging("campaign_name");
                _campaign_name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("campaign_name");
                Oncampaign_nameChanged();
            }
        }
        private global::System.String _campaign_name;
        partial void Oncampaign_nameChanging(global::System.String value);
        partial void Oncampaign_nameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String campaign_type
        {
            get
            {
                return _campaign_type;
            }
            set
            {
                Oncampaign_typeChanging(value);
                ReportPropertyChanging("campaign_type");
                _campaign_type = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("campaign_type");
                Oncampaign_typeChanged();
            }
        }
        private global::System.String _campaign_type;
        partial void Oncampaign_typeChanging(global::System.String value);
        partial void Oncampaign_typeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime modified
        {
            get
            {
                return _modified;
            }
            set
            {
                OnmodifiedChanging(value);
                ReportPropertyChanging("modified");
                _modified = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("modified");
                OnmodifiedChanged();
            }
        }
        private global::System.DateTime _modified;
        partial void OnmodifiedChanging(global::System.DateTime value);
        partial void OnmodifiedChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> created
        {
            get
            {
                return _created;
            }
            set
            {
                OncreatedChanging(value);
                ReportPropertyChanging("created");
                _created = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("created");
                OncreatedChanged();
            }
        }
        private Nullable<global::System.DateTime> _created;
        partial void OncreatedChanging(Nullable<global::System.DateTime> value);
        partial void OncreatedChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String dt_campaign_status
        {
            get
            {
                return _dt_campaign_status;
            }
            set
            {
                Ondt_campaign_statusChanging(value);
                ReportPropertyChanging("dt_campaign_status");
                _dt_campaign_status = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("dt_campaign_status");
                Ondt_campaign_statusChanged();
            }
        }
        private global::System.String _dt_campaign_status;
        partial void Ondt_campaign_statusChanging(global::System.String value);
        partial void Ondt_campaign_statusChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String dt_campaign_url
        {
            get
            {
                return _dt_campaign_url;
            }
            set
            {
                Ondt_campaign_urlChanging(value);
                ReportPropertyChanging("dt_campaign_url");
                _dt_campaign_url = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("dt_campaign_url");
                Ondt_campaign_urlChanged();
            }
        }
        private global::System.String _dt_campaign_url;
        partial void Ondt_campaign_urlChanging(global::System.String value);
        partial void Ondt_campaign_urlChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String dt_allowed_country_names
        {
            get
            {
                return _dt_allowed_country_names;
            }
            set
            {
                Ondt_allowed_country_namesChanging(value);
                ReportPropertyChanging("dt_allowed_country_names");
                _dt_allowed_country_names = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("dt_allowed_country_names");
                Ondt_allowed_country_namesChanged();
            }
        }
        private global::System.String _dt_allowed_country_names;
        partial void Ondt_allowed_country_namesChanging(global::System.String value);
        partial void Ondt_allowed_country_namesChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> is_email
        {
            get
            {
                return _is_email;
            }
            set
            {
                Onis_emailChanging(value);
                ReportPropertyChanging("is_email");
                _is_email = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("is_email");
                Onis_emailChanged();
            }
        }
        private Nullable<global::System.Boolean> _is_email;
        partial void Onis_emailChanging(Nullable<global::System.Boolean> value);
        partial void Onis_emailChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> is_search
        {
            get
            {
                return _is_search;
            }
            set
            {
                Onis_searchChanging(value);
                ReportPropertyChanging("is_search");
                _is_search = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("is_search");
                Onis_searchChanged();
            }
        }
        private Nullable<global::System.Boolean> _is_search;
        partial void Onis_searchChanging(Nullable<global::System.Boolean> value);
        partial void Onis_searchChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> is_display
        {
            get
            {
                return _is_display;
            }
            set
            {
                Onis_displayChanging(value);
                ReportPropertyChanging("is_display");
                _is_display = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("is_display");
                Onis_displayChanged();
            }
        }
        private Nullable<global::System.Boolean> _is_display;
        partial void Onis_displayChanging(Nullable<global::System.Boolean> value);
        partial void Onis_displayChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> is_coreg
        {
            get
            {
                return _is_coreg;
            }
            set
            {
                Onis_coregChanging(value);
                ReportPropertyChanging("is_coreg");
                _is_coreg = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("is_coreg");
                Onis_coregChanged();
            }
        }
        private Nullable<global::System.Boolean> _is_coreg;
        partial void Onis_coregChanging(Nullable<global::System.Boolean> value);
        partial void Onis_coregChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> max_scrub
        {
            get
            {
                return _max_scrub;
            }
            set
            {
                Onmax_scrubChanging(value);
                ReportPropertyChanging("max_scrub");
                _max_scrub = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("max_scrub");
                Onmax_scrubChanged();
            }
        }
        private Nullable<global::System.Int32> _max_scrub;
        partial void Onmax_scrubChanging(Nullable<global::System.Int32> value);
        partial void Onmax_scrubChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String notes
        {
            get
            {
                return _notes;
            }
            set
            {
                OnnotesChanging(value);
                ReportPropertyChanging("notes");
                _notes = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("notes");
                OnnotesChanged();
            }
        }
        private global::System.String _notes;
        partial void OnnotesChanging(global::System.String value);
        partial void OnnotesChanged();

        #endregion
    
    }

    #endregion
    
}
