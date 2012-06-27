using System;
using EomApp1.UI;

namespace EomApp1.Formss.Final
{
    public partial class StatusMonitor : AppFormBase
    {
        public StatusMonitor()
        {
            InitializeComponent();
        }

        private void StatusMonitor_Load(object sender, EventArgs e)
        {
            RefreshCampaignAuditTrail();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshCampaignAuditTrail();
        }

        private void RefreshCampaignAuditTrail()
        {
            campaignStatusAuditTrailViewUserControl1.Fill();
        }
    }
}