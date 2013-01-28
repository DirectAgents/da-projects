using System;
using System.Windows.Forms;
using EomApp1.Screens.PubRep1.Controls;

namespace EomApp1.Screens.PubRep1.Forms
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
            publisherListView1.PayModeChanged += publisherListView1_PayModeChanged;

            publisherDetails1.RelatedItemCountChanged += publisherDetails1_RelatedItemCountChanged;

            //lastRefreshTimeStatusValue.Text = "Loaded at " + DateTime.Now.ToString();
        }

        void publisherDetails1_RelatedItemCountChanged(object sender, EventArgs e)
        {
            publisherListView1.RefreshPublisherRelatedItemCounts();
        }

        void publisherListView1_PayModeChanged(bool payMode)
        {
            pubReport1.PayMode = payMode;
        }

        private void PublisherListViewPublisherSelected(string publisher)
        {
            accountingView1.Publisher = publisher;
            pubReport1.Publisher = publisher;
            publisherDetails1.Publisher = publisher;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            splitContainer3.Panel2Collapsed = !splitContainer3.Panel2Collapsed;
        }
    }
}
