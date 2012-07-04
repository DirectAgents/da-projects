using System;
using System.Windows.Forms;
using DAgents.Common;
using DAgents.Synch;
using EomApp1.UI;
using System.Data.SqlClient;
using DirectTrack.Rest;
using System.Collections.Generic;
using System.Xml.Linq;
using EomApp1.Services;
namespace EomApp1
{
    public partial class EOMForm : AppFormBase, ILogger
    {
        public EOMForm()
        {
            InitializeComponent();

        }

        private void finalizedCampaignsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Formss.Final.FinalizeForm1();
            a.Show();
        }

        private void EOMForm_Load(object sender, EventArgs e)
        {
        }

        private void namesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Formss.Advertiser.Advertisers();
            a.Show();
        }

        private void advertisersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Formss.Advertiser.AdvertiserMapping();
            a.Show();
        }

        private void publisherReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Formss.PubRep1.Forms.PubRep();
            a.Show();
        }

        private void currenciesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Formss.Accounting.Forms.CurrencyForm();
            a.Show();
        }

        private void extraItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Formss.Extra.ExtraItemsForm();
            a.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var a = new Formss.ttt.TttGameForm1();
            a.Show();
        }

        private void synchToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var a = new Formss.Synch.SynchForm();
            a.Show();
        }

        private void affiliatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Formss.Affiliate.Forms.AffiliatesForm1();
            a.Show();
        }

        private void publisherMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new PublisherNameCombine.PublisherCombine();
            a.Show();
        }

        private void campaignsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Formss.Campaign.CampaignsForm();
            a.Show();
        }

        private void EOMForm_Shown(object sender, EventArgs e)
        {
            isShown = true;
        }

        public bool isShown { get; set; }

        private void selectDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Formss.Settings.SelectDatabaseForm(Properties.Settings.Default.DADatabaseName);
            a.SelectedARow += new EventHandler(a_SelectedARow);
            a.ShowDialog();
        }

        void a_SelectedARow(object sender, EventArgs e)
        {
            Properties.Settings.Default.DADatabaseName = ((Formss.Settings.SelectDatabaseForm)sender).SelectedDatabaseName;
            Properties.Settings.Default.Save();
            Application.Restart();
        }

        // Short cut for Settings->Select Database
        private void label1_Click(object sender, EventArgs e)
        {
            selectDatabaseToolStripMenuItem_Click(sender, e);
        }

        private void paymentsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var a = new Formss.Payment.Forms.PaymentsForm();
            a.Show();
        }

        private void advertisersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var a = new Formss.Advertiser2.Forms.AdvertisersForm();
            a.Show();
        }

        private void EOMForm_Paint(object sender, PaintEventArgs e)
        {
            //Graphics g = e.Graphics;
            //g.FillEllipse(Brushes.Red, this.ClientRectangle);
            //g.DrawEllipse(Pens.DarkBlue, this.ClientRectangle);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void getNewCampaignsFromDTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SynchUtility.SynchCampaignListFromDirectTrackToDatabase((ILogger)this);
        }

        //private string GetDADatabaseName()
        //{
        //    System.Data.DataRowView b = (System.Data.DataRowView)dADatabaseBindingSource.Current;
        //    EomApp1.DAMain1DataSet.DADatabaseRow c = (EomApp1.DAMain1DataSet.DADatabaseRow)b.Row;
        //    return c.name;
        //}

        #region ILogger Members

        void ILogger.Log(string message)
        {
            Console.WriteLine(message);
        }

        void ILogger.LogError(string message)
        {
            Console.WriteLine(message);
        }

        #endregion

        private void updateCampaignAMAndADFromDirectTrackToDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SynchUtility.SynchCampaignGroups((ILogger)this);
        }

        private void updateAffiliateMBMappingFromDirectTrackToDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SynchUtility.SynchAffiliateToMediaBuyer()
        }

        private void synchCampaignWikiToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dBToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void qBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var a = new Formss.QB.Forms.QB();
            //a.Show();
        }

        private void dTRestApiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Formss.ETL.Forms.DTRestApi();
            a.Show();
        }

        private void aBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var a = new Formss.AB.Forms.AB10();
            //a.Show();
        }

        private void tablesViewsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Formss.DBView.Forms.DBViewForm1();
            a.Show();
        }

        private void sQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Formss.SqlExecute();
            a.Show();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://aaron/nov10eom/EomAppService1/setup.exe");
        }

        private void synchMediaBuyersAndAffiliatesFromDTToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (var con = new SqlConnection(EomAppCommon.EomAppSettings.ConnStr))
            {
                var cmd = new SqlCommand("select * from Affiliate", con);
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var affid = (int)reader["affid"];
                    try
                    {
                        SynchUtility.SynchAffiliateToMediaBuyer(affid, (ILogger)this);
                    }
                    catch { }
                }
            }
        }

        private void accountingSheetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var a = new Formss.Accounting.Forms.AccountingSheetForm();
            a.Show();
        }

        private void syncher2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Formss.Campaign.CampaignSynchForm();
            a.Show();
        }

        private void syncher3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Formss.AutoSynch.Screen(new Formss.AutoSynch.Presenter(new Formss.AutoSynch.Model()));
            a.Show();
        }

        private void test1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var service = new SyncherService();
            //var action = new Action<ILogger>((logger) => service.SynchAdvertisers(logger));
            //var form = new Formss.Campaign.LoggerForm("synch ADVERTISERS", action);
            //form.Show();
        }

        private void test2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var service = new SyncherService();
            //var action = new Action<ILogger>((logger) => service.SynchCampaigns(logger));
            //var form = new Formss.Campaign.LoggerForm("synch CAMPAIGNS", action);
            //form.Show();
        }

        private void synchCampaignGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var service = new SyncherService();
            //var action = new Action<ILogger>((logger) => service.SynchCampaignGroups(logger));
            //var form = new Formss.Campaign.LoggerForm("synch CAMPAIGN GROUPS", action);
            //form.Show();
        }

        private void synchAffiliatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var service = new SyncherService();
            //var action = new Action<ILogger>((logger) => service.SynchCampaignCategories(logger));
            //var form = new Formss.Campaign.LoggerForm("synch CAMPAIGN CATEGORIES", action);
            //form.Show();
        }

        private void synchQuickBooksToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void notesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Formss.Final.CampaignNotes();
            a.Show();
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("hello");
        }

        // add Dirty bit to XmlDoc table
        // one thread upserts the location with emtpty content and dirty bit set
        // other thread updates the content and clears the dirty bit

        // other way, scales up to multiple synch clients...

        // add table synchtask
        //  id pk identity
        //  jobid varchar(100)
        //  location varchar(500)
        // create new jobid
        // one thread
        //  inserts synch task - (jobid, location)
        // other thread
        //  selects a synch task with same jobid
        //  upserts the xml doc
        //  deletes the synch task

    }
}
