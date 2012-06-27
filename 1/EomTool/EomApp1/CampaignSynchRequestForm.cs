using System;
using System.Windows.Forms;

namespace EomApp1
{
    public partial class CampaignSynchRequestForm : Form
    {
        private void CampaignSynchRequestForm_Load(object sender, EventArgs e)
        {
            accountManagerTableAdapter.Fill(dataSet11.AccountManager);
            zRequestTableAdapter.Fill(dataSet1.zRequest);
            campaignTableAdapter.Fill(dataSet1.Campaign);
        }
        public CampaignSynchRequestForm()
        {
            InitializeComponent();
            Shown += (a, b) => timer1.Start();
            FormClosing += (a, b) => timer1.Stop();
            comboBox1.MouseEnter += (a, b) => comboBox1.Focus();
            campaignDataGridView.MouseEnter += (a, b) => campaignDataGridView.Focus();
        }
        private void FillCampaigns()
        {
            int? sv = (int?)comboBox1.SelectedValue;
            if (sv != null)
            {
                campaignTableAdapter.FillByAccountManagerId(dataSet1.Campaign, sv.Value);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            int? sv = (int?)comboBox1.SelectedValue;
            if (sv != null && sv.Value != lastValue)
            {
                timer1.Stop();
                FillCampaigns();
                lastValue = sv.Value;
                timer1.Start();
            }
        }
        private int lastValue { get; set; }
    }
}
