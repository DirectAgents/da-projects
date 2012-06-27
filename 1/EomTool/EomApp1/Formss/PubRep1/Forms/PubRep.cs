using System;
using System.Windows.Forms;
using EomApp1.Formss.PubRep1.Controls;

namespace EomApp1.Formss.PubRep1.Forms
{
    public partial class PubRep : Form
    {

        private void PubRep_Load(object sender, EventArgs e)
        {
            publisherListView1.Initialize();
            accountingView1.Initialize();
            pubReport1.Initialize();
            splitContainer1.Panel2Collapsed = true;
        }

        public PubRep()
        {
            InitializeComponent();
            publisherListView1.PublisherSelected += PublisherListViewPublisherSelected;
            publisherListView1.PayModeChanged += new PayModeChanged(publisherListView1_PayModeChanged);
            lastRefreshTimeStatusValue.Text = "Loaded at " + DateTime.Now.ToString();
        }

        void publisherListView1_PayModeChanged(bool payMode)
        {
            pubReport1.PayMode = payMode;
        }

        private void PublisherListViewPublisherSelected(string publisher)
        {
            accountingView1.Publisher = publisher;
            pubReport1.Publisher = publisher;
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
        }
    }
}
