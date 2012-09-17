using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using EomApp1.Screens.Accounting.Data;

namespace EomApp1.Screens.Accounting.Forms
{
    public class AccountingSheet2Base : Form
    {
        private ToolStrip toolStrip1;
        protected ToolStrip ToolStrip1
        {
            get { return toolStrip1; }
            set { toolStrip1 = value; }
        }

        private ToolStrip toolStrip2;
        protected ToolStrip ToolStrip2
        {
            get { return toolStrip2; }
            set { toolStrip2 = value; }
        }

        private ToolStripItem saveButton;
        protected ToolStripItem SaveButton
        {
            get { return saveButton; }
            set { saveButton = value; }
        }

        private DataGridView gridView2;
        protected DataGridView GridView2
        {
            get { return gridView2; }
            set { gridView2 = value; }
        }

        private ToolStripControlHost filterZeroCostAndRevToolStripItem;
        protected ToolStripControlHost FilterZeroCostAndRevToolStripItem
        {
            get { return filterZeroCostAndRevToolStripItem; }
            set { filterZeroCostAndRevToolStripItem = value; }
        }

        private CheckBox filterZeroCostAndRevCheckBox;
        protected CheckBox FilterZeroCostAndRevCheckBox
        {
            get { return filterZeroCostAndRevCheckBox; }
            set { filterZeroCostAndRevCheckBox = value; }
        }

        private ItemsDataSet itemChanges;
        protected ItemsDataSet ItemChanges
        {
            get { return itemChanges; }
            set { itemChanges = value; }
        }

        string tableName = "[AccountingSheet2]";
        protected string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        string connectionString = EomAppCommon.EomAppSettings.ConnStr;
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        BindingSource bindingSource = null;
        protected BindingSource BindingSource
        {
            get { return bindingSource; }
            set { bindingSource = value; }
        }

        DataSet dataSet = null;
        protected DataSet DataSet
        {
            get { return dataSet; }
            set { dataSet = value; }
        }

        DataTable dataTable = null;
        protected DataTable DataTable
        {
            get { return dataTable; }
            set { dataTable = value; }
        }

        SqlDataAdapter dataAdapter = null;
        protected SqlDataAdapter DataAdapter
        {
            get { return dataAdapter; }
            set { dataAdapter = value; }
        }

        DataGridView gridView = null;
        protected DataGridView GridView
        {
            get { return gridView; }
            set { gridView = value; }
        }

        protected SplitContainer splitContainer = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Horizontal,
            SplitterDistance = 80,
            Panel2Collapsed = true,
        };
    }
}
