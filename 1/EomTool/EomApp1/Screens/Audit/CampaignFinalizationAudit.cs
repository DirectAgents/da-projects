using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using EomAppControls;

namespace EomApp1.Screens.Audit
{
    public partial class CampaignFinalizationAudit : CampaignFinalizationAuditBase
    {
        public CampaignFinalizationAudit()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                InitGrid();
            }
        }

        private void InitGrid()
        {
            this.DataTable = new DataTable()
            {
                TableName = this.TableName
            };

            this.DataSet = new DataSet();
            this.DataSet.Tables.Add(this.DataTable);

            this.BindingSource = new BindingSource(this.DataSet, this.TableName);

            this.GridView = new ExtendedDataGridView()
            {
                AutoGenerateColumns = true,
                DataSource = this.BindingSource,
                Dock = DockStyle.Fill,
                Visible = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ColumnSelectorEnabled = false,
                AllowUserToOrderColumns = true,
                RowHeadersVisible = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                ColumnFiltersEnabled = true
            };
            this.GridView.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
            this.GridView.ColumnAdded += new DataGridViewColumnEventHandler(dataGridView_ColumnAdded);

            toolStripContainer1.ContentPanel.Controls.Add(this.GridView);

            this.DataAdapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(string.Format("SELECT * FROM {0}", this.TableName), new SqlConnection(this.ConnectionString))
            };

            Fill();
        }

        private void Fill()
        {
            this.DataTable.Clear();
            this.DataAdapter.FillSchema(this.DataTable, SchemaType.Source);
            this.DataAdapter.Fill(this.DataTable);
        }

        void dataGridView_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            if (e.Column.HeaderText == "Campaign")
            {
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                e.Column.FillWeight = 100;
            }
            else
            {
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void toolStripButton1_Click(object sender, System.EventArgs e)
        {
            Fill();
        }
    }
}
