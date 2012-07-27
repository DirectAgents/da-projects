using System;
using System.ComponentModel;
using System.Windows.Forms;
using DAgents.Common;
using System.Drawing;

namespace EomToolDatabaseUpdater
{
    public partial class SqlExecuteForm : Form
    {
        private ILogger logger;

        public SqlExecuteForm()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                logger = new Mainn.Controls.Logging.LoggerBox { Dock = DockStyle.Fill, ShowLogMessages = true, ShowErrorMessages = true };
                splitContainer2.Panel2.Controls.Add((Mainn.Controls.Logging.LoggerBox)logger);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Properties.Settings.Default.Save();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.gridView.ClearSelection();
            this.textBox1.SelectionLength = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Validate();
            this.tableAdapter.Fill(this.dataSet.DADatabase);
        }

        private void executeClick(object sender, EventArgs e)
        {
            this.Validate();
            this.bindingSource.EndEdit();

            foreach (DataGridViewRow row in this.gridView.Rows)
            {
                var isChecked = (bool?)this.gridView[UpdateDatabaseCheckCol.Index, row.Index].Value;
                if (isChecked ?? false)
                {
                    string connectionString = (string)gridView[connectionStringCol.Index, row.Index].Value;
                    string sql = this.textBox1.Text;
                    ExecuteSql(connectionString, sql);
                }
            }
        }

        private void ExecuteSql(string connectionString, string sql)
        {
            SqlBatchUtil.Execute(connectionString, sql, this.logger);
        }

        // Allow CellValueChanged to fire as soon as the check box changes.
        private void gridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    this.gridView.EndEdit();
                }
                else if (e.ColumnIndex > 0)
                {
                    var cell = this.gridView[0, e.RowIndex];
                    cell.Value = !(bool?)cell.Value ?? true;
                }
                this.gridView.ClearSelection();
            }
        }

        private void gridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    bool selected = (bool)this.gridView[e.ColumnIndex, e.RowIndex].Value;
                    this.gridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = selected ? Color.Yellow : Color.White;
                }
            }
        }
    }
}
