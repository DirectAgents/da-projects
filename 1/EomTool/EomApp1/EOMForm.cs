﻿using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DAgents.Common;
using DAgents.Synch;
using EomApp1.UI;
using System.Drawing;

namespace EomApp1
{
    public partial class EOMForm : AppFormBase, ILogger
    {
        public EOMForm()
        {
            InitializeComponent();

            // Show the connection string when hovering over the database label (Test Mode Only)
            if (Properties.Settings.Default.IsTestMode())
                this.toolTip1.SetToolTip(this.databaseLabel, EomAppCommon.EomAppSettings.ConnStr);

            // Security
            DisableMenus();
        }

        private void DisableMenus()
        {
            // Disable individual menu items
            foreach (var menuItem in this.TaggedToolStripMenuItems())
                menuItem.Enabled = Security.User.Current.CanDoMenuItem((string)menuItem.Tag);

            // Apply disabled color to top level menus that have all their items disabled
            foreach (var menu in menuStrip1.DisabledMenus())
                menu.ForeColor = SystemColors.GrayText;
        }

        // Campaigns Workflow
        private void finalizedCampaignsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Screens.Final.FinalizeForm1();
            a.Show();
        }

        // Accounting PublisherReports
        private void publisherReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Screens.PubRep1.Forms.PubRep();
            a.Show();
        }

        // Accounting Currencies
        private void currenciesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Screens.Accounting.Forms.CurrencyForm();
            a.Show();
        }

        // Campaigns ExtraItems
        private void extraItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Screens.Extra.ExtraItemsForm();
            a.Show();
        }

        // ttt
        private void button1_Click(object sender, EventArgs e)
        {
            var a = new Screens.ttt.TttGameForm1();
            a.Show();
        }

        // Campaigns Synch
        private void synchToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var a = new Screens.Synch.SynchForm();
            a.Show();
        }

        // Affiliate Affiliates
        private void affiliatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Screens.Affiliate.Forms.AffiliatesForm1();
            a.Show();
        }

        // Affiliate PublisherMap
        private void publisherMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Screens.PublisherNameCombine.PublisherCombine();
            a.Show();
        }

        // Campaigns Campaigns
        private void campaignsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Screens.Campaign.CampaignsForm();
            a.Show();
        }

        // Settings SelectDatabase
        private void selectDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Screens.Settings.SelectDatabaseForm(Properties.Settings.Default.DADatabaseName);
            a.SelectedARow += new EventHandler(SelectedDatabase);
            a.ShowDialog();
        }

        void SelectedDatabase(object sender, EventArgs e)
        {
            Properties.Settings.Default.DADatabaseName = ((Screens.Settings.SelectDatabaseForm)sender).SelectedDatabaseName;
            Properties.Settings.Default.Save();
            Application.Restart();
        }

        // Short cut for Settings->Select Database
        private void label1_Click(object sender, EventArgs e)
        {
            selectDatabaseToolStripMenuItem_Click(sender, e);
        }

        // Advertisers Advertisers
        private void advertisersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var a = new Screens.Advertiser2.Forms.AdvertisersForm();
            a.Show();
        }

        // Utilities UpdateAMAndADMappings
        private void updateCampaignAMAndADFromDirectTrackToDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SynchUtility.SynchCampaignGroups((ILogger)this);
        }

        // DB TablesAndViews
        private void tablesViewsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Screens.DBView.Forms.DBViewForm1();
            a.Show();
        }

        // DB Sql
        private void sQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Screens.SqlExecute();
            a.Show();
        }

        // Attempt at Excel plugin installer
        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://aaron/nov10eom/EomAppService1/setup.exe");
        }

        // Utilities SynchMediaBuyers
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

        // Accounting AccountingSheet
        private void accountingSheetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //var a = new Screens.Accounting.Forms.AccountingSheetForm();
            //a.Show();

            LaunchForm<Screens.Accounting.Forms.AccountingSheet2>();
        }

        // Campaigns Notes
        private void notesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Screens.Final.CampaignNotes();
            a.Show();
        }

        // Accounting VerifiedRevenue
        private void finalizedRevenueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new Screens.Final.UI.VerifiedRevenueForm();
            a.Show();
        }

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

        private void sToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //new Screens.Security.Forms.SecuritySetupForm().Show();
        }

        // Campaigns Revenue Summary
        private void revenueSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LaunchForm<Screens.Campaign.CampaignsRevenueForm>();
        }

        private static void LaunchForm<T>() where T : Form, new()
        {

            var form = Application.OpenForms.OfType<T>().FirstOrDefault();
            if (form != null)
            {
                if (form.WindowState == FormWindowState.Minimized)
                    form.WindowState = FormWindowState.Normal;
                form.BringToFront();
            }
            else
            {
                form = new T();
                form.Show();
            }
        }

        private void securityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LaunchForm<Screens.Settings.SecurityForm>();
        }

        private void testWpfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LaunchForm<Screens.Wpf.Test>();
        }

        private void itemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LaunchForm<Screens.Audit.CampaignFinalizationAudit>();
        }

        private void mBApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LaunchForm<Screens.MediaBuyerWorkflow.MediaBuyerWorkflowForm>();
        }

        private void paymentBatchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LaunchForm<Screens.PaymentBatches.PaymentBatchesForm>();
        }

        private void quickBooksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LaunchForm<Screens.QB.QuickBooksSynchForm>();
        }
    }
}
