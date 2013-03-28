using System.Xml.Serialization;

namespace QuickBooks
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class MetaData
    {

        private QBMetaDataTable[] tableField;

        private QBMetaDataColumn[] columnField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Table")]
        public QBMetaDataTable[] Tables
        {
            get
            {
                return this.tableField;
            }
            set
            {
                this.tableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Column")]
        public QBMetaDataColumn[] Columns
        {
            get
            {
                return this.columnField;
            }
            set
            {
                this.columnField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class QBMetaDataTable
    {

        private string qUALIFIERNAMEField;

        private string tABLENAMEField;

        private string tYPENAMEField;

        private string rEMARKSField;

        private byte dELETEABLEField;

        private byte vOIDABLEField;

        private byte iNSERT_ONLYField;

        /// <remarks/>
        public string QUALIFIERNAME
        {
            get
            {
                return this.qUALIFIERNAMEField;
            }
            set
            {
                this.qUALIFIERNAMEField = value;
            }
        }

        /// <remarks/>
        public string TABLENAME
        {
            get
            {
                return this.tABLENAMEField;
            }
            set
            {
                this.tABLENAMEField = value;
            }
        }

        /// <remarks/>
        public string TYPENAME
        {
            get
            {
                return this.tYPENAMEField;
            }
            set
            {
                this.tYPENAMEField = value;
            }
        }

        /// <remarks/>
        public string REMARKS
        {
            get
            {
                return this.rEMARKSField;
            }
            set
            {
                this.rEMARKSField = value;
            }
        }

        /// <remarks/>
        public byte DELETEABLE
        {
            get
            {
                return this.dELETEABLEField;
            }
            set
            {
                this.dELETEABLEField = value;
            }
        }

        /// <remarks/>
        public byte VOIDABLE
        {
            get
            {
                return this.vOIDABLEField;
            }
            set
            {
                this.vOIDABLEField = value;
            }
        }

        /// <remarks/>
        public byte INSERT_ONLY
        {
            get
            {
                return this.iNSERT_ONLYField;
            }
            set
            {
                this.iNSERT_ONLYField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class QBMetaDataColumn
    {

        private string qUALIFIERNAMEField;

        private string tABLENAMEField;

        private string cOLUMNNAMEField;

        private sbyte tYPEField;

        private string tYPENAMEField;

        private ushort pRECISIONField;

        private ushort lENGTHField;

        private byte sCALEField;

        private byte rADIXField;

        private bool rADIXFieldSpecified;

        private byte nULLABLEField;

        private string dEFAULTField;

        private sbyte dATATYPEField;

        private byte dATETIME_SUBTYPEField;

        private bool dATETIME_SUBTYPEFieldSpecified;

        private ushort oCTET_LENGTHField;

        private bool oCTET_LENGTHFieldSpecified;

        private byte oRDINAL_POSITIONField;

        private string iS_NULLABLEField;

        private byte qUERYABLEField;

        private byte uPDATEABLEField;

        private byte iNSERTABLEField;

        private byte rEQUIRED_ON_INSERTField;

        private string fORMATField;

        private string rELATES_TOField;

        private string jUMPIN_TYPEField;

        private byte fORCE_UNOPTIMIZEDField;

        private byte aDVANCEDField;

        private string sDK_QB_NAMEField;

        private string sDK_QRY_AGG_NAMEField;

        private string sDK_QRY_AGG_NAME2Field;

        private string sDK_QRY_AGG_NAME3Field;

        private string sDK_ADD_AGG_NAMEField;

        private string sDK_ADD_AGG_NAME2Field;

        private string sDK_ADD_AGG_NAME3Field;

        private string sDK_MOD_AGG_NAMEField;

        private string sDK_MOD_AGG_NAME2Field;

        private string sDK_MOD_AGG_NAME3Field;

        private string cOLUMNFULLNAMEField;

        private string cOLUMNSHORTNAMEField;

        /// <remarks/>
        public string QUALIFIERNAME
        {
            get
            {
                return this.qUALIFIERNAMEField;
            }
            set
            {
                this.qUALIFIERNAMEField = value;
            }
        }

        /// <remarks/>
        public string TABLENAME
        {
            get
            {
                return this.tABLENAMEField;
            }
            set
            {
                this.tABLENAMEField = value;
            }
        }

        /// <remarks/>
        public string COLUMNNAME
        {
            get
            {
                return this.cOLUMNNAMEField;
            }
            set
            {
                this.cOLUMNNAMEField = value;
            }
        }

        /// <remarks/>
        public sbyte TYPE
        {
            get
            {
                return this.tYPEField;
            }
            set
            {
                this.tYPEField = value;
            }
        }

        /// <remarks/>
        public string TYPENAME
        {
            get
            {
                return this.tYPENAMEField;
            }
            set
            {
                this.tYPENAMEField = value;
            }
        }

        /// <remarks/>
        public ushort PRECISION
        {
            get
            {
                return this.pRECISIONField;
            }
            set
            {
                this.pRECISIONField = value;
            }
        }

        /// <remarks/>
        public ushort LENGTH
        {
            get
            {
                return this.lENGTHField;
            }
            set
            {
                this.lENGTHField = value;
            }
        }

        /// <remarks/>
        public byte SCALE
        {
            get
            {
                return this.sCALEField;
            }
            set
            {
                this.sCALEField = value;
            }
        }

        /// <remarks/>
        public byte RADIX
        {
            get
            {
                return this.rADIXField;
            }
            set
            {
                this.rADIXField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RADIXSpecified
        {
            get
            {
                return this.rADIXFieldSpecified;
            }
            set
            {
                this.rADIXFieldSpecified = value;
            }
        }

        /// <remarks/>
        public byte NULLABLE
        {
            get
            {
                return this.nULLABLEField;
            }
            set
            {
                this.nULLABLEField = value;
            }
        }

        /// <remarks/>
        public string DEFAULT
        {
            get
            {
                return this.dEFAULTField;
            }
            set
            {
                this.dEFAULTField = value;
            }
        }

        /// <remarks/>
        public sbyte DATATYPE
        {
            get
            {
                return this.dATATYPEField;
            }
            set
            {
                this.dATATYPEField = value;
            }
        }

        /// <remarks/>
        public byte DATETIME_SUBTYPE
        {
            get
            {
                return this.dATETIME_SUBTYPEField;
            }
            set
            {
                this.dATETIME_SUBTYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DATETIME_SUBTYPESpecified
        {
            get
            {
                return this.dATETIME_SUBTYPEFieldSpecified;
            }
            set
            {
                this.dATETIME_SUBTYPEFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ushort OCTET_LENGTH
        {
            get
            {
                return this.oCTET_LENGTHField;
            }
            set
            {
                this.oCTET_LENGTHField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OCTET_LENGTHSpecified
        {
            get
            {
                return this.oCTET_LENGTHFieldSpecified;
            }
            set
            {
                this.oCTET_LENGTHFieldSpecified = value;
            }
        }

        /// <remarks/>
        public byte ORDINAL_POSITION
        {
            get
            {
                return this.oRDINAL_POSITIONField;
            }
            set
            {
                this.oRDINAL_POSITIONField = value;
            }
        }

        /// <remarks/>
        public string IS_NULLABLE
        {
            get
            {
                return this.iS_NULLABLEField;
            }
            set
            {
                this.iS_NULLABLEField = value;
            }
        }

        /// <remarks/>
        public byte QUERYABLE
        {
            get
            {
                return this.qUERYABLEField;
            }
            set
            {
                this.qUERYABLEField = value;
            }
        }

        /// <remarks/>
        public byte UPDATEABLE
        {
            get
            {
                return this.uPDATEABLEField;
            }
            set
            {
                this.uPDATEABLEField = value;
            }
        }

        /// <remarks/>
        public byte INSERTABLE
        {
            get
            {
                return this.iNSERTABLEField;
            }
            set
            {
                this.iNSERTABLEField = value;
            }
        }

        /// <remarks/>
        public byte REQUIRED_ON_INSERT
        {
            get
            {
                return this.rEQUIRED_ON_INSERTField;
            }
            set
            {
                this.rEQUIRED_ON_INSERTField = value;
            }
        }

        /// <remarks/>
        public string FORMAT
        {
            get
            {
                return this.fORMATField;
            }
            set
            {
                this.fORMATField = value;
            }
        }

        /// <remarks/>
        public string RELATES_TO
        {
            get
            {
                return this.rELATES_TOField;
            }
            set
            {
                this.rELATES_TOField = value;
            }
        }

        /// <remarks/>
        public string JUMPIN_TYPE
        {
            get
            {
                return this.jUMPIN_TYPEField;
            }
            set
            {
                this.jUMPIN_TYPEField = value;
            }
        }

        /// <remarks/>
        public byte FORCE_UNOPTIMIZED
        {
            get
            {
                return this.fORCE_UNOPTIMIZEDField;
            }
            set
            {
                this.fORCE_UNOPTIMIZEDField = value;
            }
        }

        /// <remarks/>
        public byte ADVANCED
        {
            get
            {
                return this.aDVANCEDField;
            }
            set
            {
                this.aDVANCEDField = value;
            }
        }

        /// <remarks/>
        public string SDK_QB_NAME
        {
            get
            {
                return this.sDK_QB_NAMEField;
            }
            set
            {
                this.sDK_QB_NAMEField = value;
            }
        }

        /// <remarks/>
        public string SDK_QRY_AGG_NAME
        {
            get
            {
                return this.sDK_QRY_AGG_NAMEField;
            }
            set
            {
                this.sDK_QRY_AGG_NAMEField = value;
            }
        }

        /// <remarks/>
        public string SDK_QRY_AGG_NAME2
        {
            get
            {
                return this.sDK_QRY_AGG_NAME2Field;
            }
            set
            {
                this.sDK_QRY_AGG_NAME2Field = value;
            }
        }

        /// <remarks/>
        public string SDK_QRY_AGG_NAME3
        {
            get
            {
                return this.sDK_QRY_AGG_NAME3Field;
            }
            set
            {
                this.sDK_QRY_AGG_NAME3Field = value;
            }
        }

        /// <remarks/>
        public string SDK_ADD_AGG_NAME
        {
            get
            {
                return this.sDK_ADD_AGG_NAMEField;
            }
            set
            {
                this.sDK_ADD_AGG_NAMEField = value;
            }
        }

        /// <remarks/>
        public string SDK_ADD_AGG_NAME2
        {
            get
            {
                return this.sDK_ADD_AGG_NAME2Field;
            }
            set
            {
                this.sDK_ADD_AGG_NAME2Field = value;
            }
        }

        /// <remarks/>
        public string SDK_ADD_AGG_NAME3
        {
            get
            {
                return this.sDK_ADD_AGG_NAME3Field;
            }
            set
            {
                this.sDK_ADD_AGG_NAME3Field = value;
            }
        }

        /// <remarks/>
        public string SDK_MOD_AGG_NAME
        {
            get
            {
                return this.sDK_MOD_AGG_NAMEField;
            }
            set
            {
                this.sDK_MOD_AGG_NAMEField = value;
            }
        }

        /// <remarks/>
        public string SDK_MOD_AGG_NAME2
        {
            get
            {
                return this.sDK_MOD_AGG_NAME2Field;
            }
            set
            {
                this.sDK_MOD_AGG_NAME2Field = value;
            }
        }

        /// <remarks/>
        public string SDK_MOD_AGG_NAME3
        {
            get
            {
                return this.sDK_MOD_AGG_NAME3Field;
            }
            set
            {
                this.sDK_MOD_AGG_NAME3Field = value;
            }
        }

        /// <remarks/>
        public string COLUMNFULLNAME
        {
            get
            {
                return this.cOLUMNFULLNAMEField;
            }
            set
            {
                this.cOLUMNFULLNAMEField = value;
            }
        }

        /// <remarks/>
        public string COLUMNSHORTNAME
        {
            get
            {
                return this.cOLUMNSHORTNAMEField;
            }
            set
            {
                this.cOLUMNSHORTNAMEField = value;
            }
        }
    }

}