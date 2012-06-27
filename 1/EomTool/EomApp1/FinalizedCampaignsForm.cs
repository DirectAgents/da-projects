using System;
using System.Windows.Forms;
using System.Linq;
using System.Data;
namespace EomApp1
{
    public partial class FinalizedCampaignsForm : Form
    {
        private int curPid;
        public FinalizedCampaignsForm()
        {
            InitializeComponent();
            this.Shown += new EventHandler(FinalizedCampaignsForm_Shown);
        }

        void FinalizedCampaignsForm_Shown(object sender, EventArgs e)
        {
            this.isShown = true;
        }

        private void UpdateFinalizeButton()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var r = (from c in db.CampaignStatus
                    where c.pid == curPid
                    select c.campaign_status).FirstOrDefault();
            if (r == null)
            {
                button1.Text = "Set Campaign Status to 'final'";
                button1.Enabled = true;
            }
            else
            {
                button1.Text = r;
                button1.Enabled = false;
            }
        }

        private void FinalizedCampaignsForm_Load(object sender, EventArgs e)
        {
            this.campaignTableAdapter.Fill(this.dataSet1.Campaign);
        }

        private void campaignBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)campaignBindingSource.Current;
            curPid = (int)drv.Row["pid"];
        }

        public bool isShown { get; set; }

        private void FinalizeButton_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var r = (from c in db.CampaignStatus
                     where c.pid == curPid
                     select c).FirstOrDefault();
            if (r == null)
            {
                r = new CampaignStatus();
                r.pid = curPid;
                r.campaign_status = "final";
                db.GetTable<CampaignStatus>().InsertOnSubmit(r);
                db.SubmitChanges();
                UpdateFinalizeButton();
            }
            else
            {
                MessageBox.Show("should not get here");
            }
        }
    }
}
