using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace EomApp1.Screens.Accounting.Forms
{
    public class AccountingSheet2Base : Form
    {
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
