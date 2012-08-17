using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EomApp1.Screens.Audit
{
    public class CampaignFinalizationAuditBase : Form
    {
        string tableName = "[CampaignFinalizationAudit]";
        public string TableName
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
        public BindingSource BindingSource
        {
            get { return bindingSource; }
            set { bindingSource = value; }
        }

        DataSet dataSet = null;
        public DataSet DataSet
        {
            get { return dataSet; }
            set { dataSet = value; }
        }

        DataTable dataTable = null;
        public DataTable DataTable
        {
            get { return dataTable; }
            set { dataTable = value; }
        }

        SqlDataAdapter dataAdapter = null;
        public SqlDataAdapter DataAdapter
        {
            get { return dataAdapter; }
            set { dataAdapter = value; }
        }

        DataGridView gridView = null;
        public DataGridView GridView
        {
            get { return gridView; }
            set { gridView = value; }
        }
    }
}
