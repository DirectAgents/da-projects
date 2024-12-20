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

[assembly: EdmRelationshipAttribute("GoogleDocSpreadsheets", "WorksheetCell", "Worksheet", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(AccountingBackupWeb.Models.GoogleDocs.Worksheet), "Cell", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(AccountingBackupWeb.Models.GoogleDocs.Cell))]
[assembly: EdmRelationshipAttribute("GoogleDocSpreadsheets", "SpreadsheetWorksheet", "Spreadsheet", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(AccountingBackupWeb.Models.GoogleDocs.Spreadsheet), "Worksheet", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(AccountingBackupWeb.Models.GoogleDocs.Worksheet))]

#endregion

namespace AccountingBackupWeb.Models.GoogleDocs
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class GoogleDocSpreadsheetsContainer : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new GoogleDocSpreadsheetsContainer object using the connection string found in the 'GoogleDocSpreadsheetsContainer' section of the application configuration file.
        /// </summary>
        public GoogleDocSpreadsheetsContainer() : base("name=GoogleDocSpreadsheetsContainer", "GoogleDocSpreadsheetsContainer")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new GoogleDocSpreadsheetsContainer object.
        /// </summary>
        public GoogleDocSpreadsheetsContainer(string connectionString) : base(connectionString, "GoogleDocSpreadsheetsContainer")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new GoogleDocSpreadsheetsContainer object.
        /// </summary>
        public GoogleDocSpreadsheetsContainer(EntityConnection connection) : base(connection, "GoogleDocSpreadsheetsContainer")
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
        public ObjectSet<Spreadsheet> Spreadsheets
        {
            get
            {
                if ((_Spreadsheets == null))
                {
                    _Spreadsheets = base.CreateObjectSet<Spreadsheet>("Spreadsheets");
                }
                return _Spreadsheets;
            }
        }
        private ObjectSet<Spreadsheet> _Spreadsheets;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Worksheet> Worksheets
        {
            get
            {
                if ((_Worksheets == null))
                {
                    _Worksheets = base.CreateObjectSet<Worksheet>("Worksheets");
                }
                return _Worksheets;
            }
        }
        private ObjectSet<Worksheet> _Worksheets;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Cell> Cells
        {
            get
            {
                if ((_Cells == null))
                {
                    _Cells = base.CreateObjectSet<Cell>("Cells");
                }
                return _Cells;
            }
        }
        private ObjectSet<Cell> _Cells;

        #endregion
        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Spreadsheets EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToSpreadsheets(Spreadsheet spreadsheet)
        {
            base.AddObject("Spreadsheets", spreadsheet);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Worksheets EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToWorksheets(Worksheet worksheet)
        {
            base.AddObject("Worksheets", worksheet);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Cells EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToCells(Cell cell)
        {
            base.AddObject("Cells", cell);
        }

        #endregion
    }
    

    #endregion
    
    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="GoogleDocSpreadsheets", Name="Cell")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Cell : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Cell object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="row">Initial value of the Row property.</param>
        /// <param name="column">Initial value of the Column property.</param>
        /// <param name="value">Initial value of the Value property.</param>
        public static Cell CreateCell(global::System.Int32 id, global::System.Int32 row, global::System.Int32 column, global::System.String value)
        {
            Cell cell = new Cell();
            cell.Id = id;
            cell.Row = row;
            cell.Column = column;
            cell.Value = value;
            return cell;
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
        public global::System.Int32 Row
        {
            get
            {
                return _Row;
            }
            set
            {
                OnRowChanging(value);
                ReportPropertyChanging("Row");
                _Row = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Row");
                OnRowChanged();
            }
        }
        private global::System.Int32 _Row;
        partial void OnRowChanging(global::System.Int32 value);
        partial void OnRowChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Column
        {
            get
            {
                return _Column;
            }
            set
            {
                OnColumnChanging(value);
                ReportPropertyChanging("Column");
                _Column = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Column");
                OnColumnChanged();
            }
        }
        private global::System.Int32 _Column;
        partial void OnColumnChanging(global::System.Int32 value);
        partial void OnColumnChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Value
        {
            get
            {
                return _Value;
            }
            set
            {
                OnValueChanging(value);
                ReportPropertyChanging("Value");
                _Value = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Value");
                OnValueChanged();
            }
        }
        private global::System.String _Value;
        partial void OnValueChanging(global::System.String value);
        partial void OnValueChanged();

        #endregion
    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("GoogleDocSpreadsheets", "WorksheetCell", "Worksheet")]
        public Worksheet Worksheet
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Worksheet>("GoogleDocSpreadsheets.WorksheetCell", "Worksheet").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Worksheet>("GoogleDocSpreadsheets.WorksheetCell", "Worksheet").Value = value;
            }
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<Worksheet> WorksheetReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Worksheet>("GoogleDocSpreadsheets.WorksheetCell", "Worksheet");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<Worksheet>("GoogleDocSpreadsheets.WorksheetCell", "Worksheet", value);
                }
            }
        }

        #endregion
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="GoogleDocSpreadsheets", Name="Spreadsheet")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Spreadsheet : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Spreadsheet object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="title">Initial value of the Title property.</param>
        public static Spreadsheet CreateSpreadsheet(global::System.Int32 id, global::System.String title)
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.Id = id;
            spreadsheet.Title = title;
            return spreadsheet;
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
        public global::System.String Title
        {
            get
            {
                return _Title;
            }
            set
            {
                OnTitleChanging(value);
                ReportPropertyChanging("Title");
                _Title = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Title");
                OnTitleChanged();
            }
        }
        private global::System.String _Title;
        partial void OnTitleChanging(global::System.String value);
        partial void OnTitleChanged();

        #endregion
    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("GoogleDocSpreadsheets", "SpreadsheetWorksheet", "Worksheet")]
        public EntityCollection<Worksheet> Worksheets
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<Worksheet>("GoogleDocSpreadsheets.SpreadsheetWorksheet", "Worksheet");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<Worksheet>("GoogleDocSpreadsheets.SpreadsheetWorksheet", "Worksheet", value);
                }
            }
        }

        #endregion
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="GoogleDocSpreadsheets", Name="Worksheet")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Worksheet : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Worksheet object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="title">Initial value of the Title property.</param>
        public static Worksheet CreateWorksheet(global::System.Int32 id, global::System.String title)
        {
            Worksheet worksheet = new Worksheet();
            worksheet.Id = id;
            worksheet.Title = title;
            return worksheet;
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
        public global::System.String Title
        {
            get
            {
                return _Title;
            }
            set
            {
                OnTitleChanging(value);
                ReportPropertyChanging("Title");
                _Title = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Title");
                OnTitleChanged();
            }
        }
        private global::System.String _Title;
        partial void OnTitleChanging(global::System.String value);
        partial void OnTitleChanged();

        #endregion
    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("GoogleDocSpreadsheets", "WorksheetCell", "Cell")]
        public EntityCollection<Cell> Cells
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<Cell>("GoogleDocSpreadsheets.WorksheetCell", "Cell");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<Cell>("GoogleDocSpreadsheets.WorksheetCell", "Cell", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("GoogleDocSpreadsheets", "SpreadsheetWorksheet", "Spreadsheet")]
        public Spreadsheet Spreadsheet
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Spreadsheet>("GoogleDocSpreadsheets.SpreadsheetWorksheet", "Spreadsheet").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Spreadsheet>("GoogleDocSpreadsheets.SpreadsheetWorksheet", "Spreadsheet").Value = value;
            }
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<Spreadsheet> SpreadsheetReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Spreadsheet>("GoogleDocSpreadsheets.SpreadsheetWorksheet", "Spreadsheet");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<Spreadsheet>("GoogleDocSpreadsheets.SpreadsheetWorksheet", "Spreadsheet", value);
                }
            }
        }

        #endregion
    }

    #endregion
    
}
