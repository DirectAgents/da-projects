using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace EomApp1.Formss.DBView.Forms
{
    public enum TableType 
    {
        Table,
        View
    }
    public partial class DBTableViewerForm1 : Form
    {
        BindingSource _bs = null;
        DataSet _ds = null;
        DataTable _dt = null;
        SqlDataAdapter _da = null;
        DataGridView _dgv = null;
        public DBTableViewerForm1(string connectionString, string tableName, TableType tableType)
        {
            InitializeComponent();
            if (!DesignMode)
            {
                components = new Container();

                // Create the DataSet
                _ds = new DataSet();

                // Create the DataTable
                _dt = new DataTable();

                // Create the BindingSource
                _bs = new BindingSource(components);

                // Create a SqlDataAdapter for a query that returns all rows in the table
                _da = new SqlDataAdapter();
                
                // Create the DataGridView
                _dgv = new DataGridView();

                ((ISupportInitialize)(_ds)).BeginInit();
                ((ISupportInitialize)(_bs)).BeginInit();
                ((ISupportInitialize)(_dgv)).BeginInit();
                SuspendLayout();

                // Configure the DataSet
                _ds.DataSetName = "DataSet";
                _ds.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
                _ds.Tables.Add(_dt);

                // Configure the DataTable
                _dt.TableName = tableName;

                // Configure the DataAdapter
                _da.SelectCommand = new SqlCommand(string.Format("select * from {0}", tableName), new SqlConnection(connectionString));

                // Configure the BindingSource
                _bs.DataMember = tableName;
                _bs.DataSource = _ds;

                // Configure the DataGridView
                _dgv.AutoGenerateColumns = true;
                _dgv.DataSource = _bs;
                _dgv.Dock = DockStyle.Fill;

                // Add the DataGridView to the Form
                _grid.Visible = false;
                toolStripContainer1.ContentPanel.Controls.Add(_dgv);

                ((ISupportInitialize)(_ds)).EndInit();
                ((ISupportInitialize)(_bs)).EndInit();
                ((ISupportInitialize)(_dgv)).EndInit();
                ResumeLayout(false);
                PerformLayout();

                // Load the Data
                _da.FillSchema(_dt, SchemaType.Source);
                _da.Fill(_dt);

                _saveMenuItem.Enabled = true;

                //// Set the DataGridView's DataSource property to the BindingSource instance
                //_grid.DataSource = _bs;

                //// Set the BindingSource's DataSource property to the DataTable instance
                //_bs.DataSource = _dt;


                //// Add the table to the DataSet
                //_dt = _ds.Tables.Add("DataTable_For_" + tableName);

                //// Set the BindingSource's DataMember property to the table name
                //_bs.DataMember = "DataTable_For_" + tableName;

                //// fill the schema?
                ////_da.FillSchema(_ds, SchemaType.Source);

                //// clear the datatable?
                ////_dt.Clear();

                //// Tell the SqlDataAdapter to fill the DataTable with result rows
                //_da.Fill(_dt);

                //// ?? fill the schema?
                
                //// Wire events
                //_dt.RowChanging += new DataRowChangeEventHandler(_dt_RowChanging);
                //_da.RowUpdating += new SqlRowUpdatingEventHandler(_da_RowUpdating);
                //_da.RowUpdated += new SqlRowUpdatedEventHandler(_da_RowUpdated);
            }
        }
        void _dt_RowChanging(object sender, DataRowChangeEventArgs e)
        {
            _saveMenuItem.Enabled = true;
        }
        void _da_RowUpdating(object sender, SqlRowUpdatingEventArgs e)
        {
            MessageBox.Show("row updating");
        }
        void _da_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            MessageBox.Show("row updated");
        }
        private void schemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(builder);
            _ds.WriteXmlSchema(writer);
            MessageBox.Show(writer.ToString());
        }
        private void _saveMenuItem_Click(object sender, EventArgs e)
        {
            Validate();
            _bs.EndEdit();
            _da.Update(_dt);
        }
    }
}
