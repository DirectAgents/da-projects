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
#region EDM Relationship Metadata

[assembly: EdmRelationshipAttribute("EomToolSecurityModel", "RoleGroup", "Group", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(WebApplication1.Group), "Role", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(WebApplication1.Role))]
[assembly: EdmRelationshipAttribute("EomToolSecurityModel", "RolePermission", "Permission", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(WebApplication1.Permission), "Role", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(WebApplication1.Role))]

#endregion

namespace WebApplication1
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class EomToolSecurityEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new EomToolSecurityEntities object using the connection string found in the 'EomToolSecurityEntities' section of the application configuration file.
        /// </summary>
        public EomToolSecurityEntities() : base("name=EomToolSecurityEntities", "EomToolSecurityEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new EomToolSecurityEntities object.
        /// </summary>
        public EomToolSecurityEntities(string connectionString) : base(connectionString, "EomToolSecurityEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new EomToolSecurityEntities object.
        /// </summary>
        public EomToolSecurityEntities(EntityConnection connection) : base(connection, "EomToolSecurityEntities")
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
        public ObjectSet<Group> Groups
        {
            get
            {
                if ((_Groups == null))
                {
                    _Groups = base.CreateObjectSet<Group>("Groups");
                }
                return _Groups;
            }
        }
        private ObjectSet<Group> _Groups;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Permission> Permissions
        {
            get
            {
                if ((_Permissions == null))
                {
                    _Permissions = base.CreateObjectSet<Permission>("Permissions");
                }
                return _Permissions;
            }
        }
        private ObjectSet<Permission> _Permissions;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Role> Roles
        {
            get
            {
                if ((_Roles == null))
                {
                    _Roles = base.CreateObjectSet<Role>("Roles");
                }
                return _Roles;
            }
        }
        private ObjectSet<Role> _Roles;

        #endregion
        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Groups EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToGroups(Group group)
        {
            base.AddObject("Groups", group);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Permissions EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToPermissions(Permission permission)
        {
            base.AddObject("Permissions", permission);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Roles EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToRoles(Role role)
        {
            base.AddObject("Roles", role);
        }

        #endregion
    }
    

    #endregion
    
    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="EomToolSecurityModel", Name="Group")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Group : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Group object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="name">Initial value of the Name property.</param>
        /// <param name="windowsIdentity">Initial value of the WindowsIdentity property.</param>
        public static Group CreateGroup(global::System.Int32 id, global::System.String name, global::System.String windowsIdentity)
        {
            Group group = new Group();
            group.Id = id;
            group.Name = name;
            group.WindowsIdentity = windowsIdentity;
            return group;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String WindowsIdentity
        {
            get
            {
                return _WindowsIdentity;
            }
            set
            {
                OnWindowsIdentityChanging(value);
                ReportPropertyChanging("WindowsIdentity");
                _WindowsIdentity = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("WindowsIdentity");
                OnWindowsIdentityChanged();
            }
        }
        private global::System.String _WindowsIdentity;
        partial void OnWindowsIdentityChanging(global::System.String value);
        partial void OnWindowsIdentityChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String EmailAddress
        {
            get
            {
                return _EmailAddress;
            }
            set
            {
                OnEmailAddressChanging(value);
                ReportPropertyChanging("EmailAddress");
                _EmailAddress = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("EmailAddress");
                OnEmailAddressChanged();
            }
        }
        private global::System.String _EmailAddress;
        partial void OnEmailAddressChanging(global::System.String value);
        partial void OnEmailAddressChanged();

        #endregion
    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("EomToolSecurityModel", "RoleGroup", "Role")]
        public EntityCollection<Role> Roles
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<Role>("EomToolSecurityModel.RoleGroup", "Role");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<Role>("EomToolSecurityModel.RoleGroup", "Role", value);
                }
            }
        }

        #endregion
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="EomToolSecurityModel", Name="Permission")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Permission : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Permission object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="name">Initial value of the Name property.</param>
        /// <param name="tag">Initial value of the Tag property.</param>
        public static Permission CreatePermission(global::System.Int32 id, global::System.String name, global::System.String tag)
        {
            Permission permission = new Permission();
            permission.Id = id;
            permission.Name = name;
            permission.Tag = tag;
            return permission;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Tag
        {
            get
            {
                return _Tag;
            }
            set
            {
                OnTagChanging(value);
                ReportPropertyChanging("Tag");
                _Tag = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Tag");
                OnTagChanged();
            }
        }
        private global::System.String _Tag;
        partial void OnTagChanging(global::System.String value);
        partial void OnTagChanged();

        #endregion
    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("EomToolSecurityModel", "RolePermission", "Role")]
        public EntityCollection<Role> Roles
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<Role>("EomToolSecurityModel.RolePermission", "Role");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<Role>("EomToolSecurityModel.RolePermission", "Role", value);
                }
            }
        }

        #endregion
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="EomToolSecurityModel", Name="Role")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Role : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Role object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="name">Initial value of the Name property.</param>
        public static Role CreateRole(global::System.Int32 id, global::System.String name)
        {
            Role role = new Role();
            role.Id = id;
            role.Name = name;
            return role;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();

        #endregion
    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("EomToolSecurityModel", "RoleGroup", "Group")]
        public EntityCollection<Group> Groups
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<Group>("EomToolSecurityModel.RoleGroup", "Group");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<Group>("EomToolSecurityModel.RoleGroup", "Group", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("EomToolSecurityModel", "RolePermission", "Permission")]
        public EntityCollection<Permission> Permissions
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<Permission>("EomToolSecurityModel.RolePermission", "Permission");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<Permission>("EomToolSecurityModel.RolePermission", "Permission", value);
                }
            }
        }

        #endregion
    }

    #endregion
    
}
