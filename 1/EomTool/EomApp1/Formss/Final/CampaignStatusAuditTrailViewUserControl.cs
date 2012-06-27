using System;
using System.Windows.Forms;

namespace EomApp1.Formss.Final
{
    public partial class CampaignStatusAuditTrailViewUserControl : UserControl
    {
        public CampaignStatusAuditTrailViewUserControl()
        {
            InitializeComponent();
        }

        public void Fill()
        {
            campaignStatusAuditTrailViewTableAdapter.Fill(dADatabaseR1DataSet.CampaignStatusAuditTrailView);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Fill();
        }
    }
}