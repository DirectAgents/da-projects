using System;
using System.Drawing;
using System.Windows.Forms;

namespace EomApp1.Screens.Campaign
{
    public partial class CampaignsRevenueForm : Form
    {
        public CampaignsRevenueForm()
        {
            InitializeComponent();

            var style = new System.Windows.Forms.DataGridViewCellStyle() { Format = "N2" };
            revenueDataGridViewTextBoxColumn.DefaultCellStyle = style;
            unfinalizedDataGridViewTextBoxColumn.DefaultCellStyle = style;
            finalizedDataGridViewTextBoxColumn.DefaultCellStyle = style;
            verifiedDataGridViewTextBoxColumn.DefaultCellStyle = style;
            dataGridView1.Location = new Point(0, 29);

            
        }

        private void CampaignsRevenueForm_Load(object sender, EventArgs e)
        {
            FillData();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            FillData();
        }

        private void FillData()
        {
            campaignWorkflowTableAdapter.Fill(campaignsRevenueDataSet.CampaignWorkflow);
        }
    }
}
