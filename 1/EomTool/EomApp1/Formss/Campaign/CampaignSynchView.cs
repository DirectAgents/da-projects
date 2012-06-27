using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml.Linq;
using DAgents.Common;
using DAgents.Synch;
using DirectTrack.Rest;
using System;

namespace EomApp1.Formss.Campaign
{
    public partial class CampaignSynchView : UserControl
    {
        private bool _started;

        public CampaignSynchView()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            Fill();
            InitializeSearchControl();
        }

        private void InitializeSearchControl()
        {
            searchControl1.SearchTextChanged += (sender, searchTextEventArgs) =>
            {
                campaignSynchDataSetBindingSource.Filter = string.Format("campaign_name LIKE '%{0}%'", searchTextEventArgs.SearchText);
            };
            searchControl1.AccountManagerSelected += (sender, accountManagerSelectedEventArgs) =>
            {
                Fill(accountManagerSelectedEventArgs.AccountManagerId);
            };
        }

        private void Fill(int accountManagerId)
        {
            campaignSynchDataTableTableAdapter.FillByAccountManagerId(campaignSynchDataSet.CampaignSynchDataTable, accountManagerId);
            searchControl1.ClearSearchText();
        }

        private void Fill()
        {
            campaignSynchDataTableTableAdapter.Fill(campaignSynchDataSet.CampaignSynchDataTable);
            searchControl1.ClearSearchText();
        }

        private void _startLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!_started)
            {
                _startLink.Text = "Stop";
                _started = true;
                var loggerForm = new LoggerForm("synch", (logger) => DoSynch(logger));
                loggerForm.Show();
            }
            else
            {
                _startLink.Text = "Start";
                _started = false;
            }
        }

        private void DoSynch(ILogger logger)
        {
            int startDay;
            bool startDaySpecified = Int32.TryParse(_fromDay.Text, out startDay);

            int endDay;
            bool endDaySpecified = Int32.TryParse(_toDay.Text, out endDay);

            while (
                _started 
                && SynchNextCampaign(
                    logger,
                    Properties.Settings.Default.StatsYear,
                    Properties.Settings.Default.StatsMonth,
                    startDaySpecified ? startDay : 1,
                    endDaySpecified ? endDay : Properties.Settings.Default.StatsDaysInMonth,
                    _payoutsCheckBox.Checked,
                    _statsCheckBox.Checked
                    )
            ) ;
        }

        private bool SynchNextCampaign(ILogger logger, int year, int month, int startDay, int endDay, bool synchPayouts, bool synchStats)
        {
            // Choose campaign to Synch
            int? nextCampaignIdToSynch;
            using (var sqlConnection = new SqlConnection(global::DAgents.Common.Properties.Settings.Default.ConnStr))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand(@"
                            SELECT TOP (1) 
                                dbo.Campaign.pid,
                                DATEDIFF(mi, COALESCE (dbo.CampaignSynch.LastSynched, CONVERT(DATETIME, '2011-01-01 00:00:00', 102)), GETDATE()) AS SinceLastSynch
                            FROM 
                                dbo.Campaign 
                                INNER JOIN dbo.CampaignSynch ON dbo.Campaign.id = dbo.CampaignSynch.CampaignId
                            WHERE 
                                (dbo.CampaignSynch.Active = 1)
                            ORDER BY 
                                SinceLastSynch DESC, dbo.Campaign.pid", sqlConnection))
                {
                    nextCampaignIdToSynch = (int?)sqlCommand.ExecuteScalar();
                }
            }
            if (nextCampaignIdToSynch != null)
            {
                // Synch Payouts
                if (_payoutsCheckBox.Checked)
                {
                    SynchUtility.SynchPayoutsForCampaignPid(logger, nextCampaignIdToSynch.Value);
                }
                // Synch Stats
                if (synchStats)
                {
                    // redirect pid
                    SynchUtility.SynchStatsForPid(
                        logger,
                        nextCampaignIdToSynch.Value,
                        year,
                        month,
                        startDay,
                        endDay,
                        false,
                        0);
                }
                if (synchStats || synchPayouts)
                {
                    // Update Synch Time
                    using (var connection = new SqlConnection(global::DAgents.Common.Properties.Settings.Default.ConnStr))
                    {
                        connection.Open();
                        using (var command = new SqlCommand(@"EXEC dbo.UpdateLastCampaignSynch " + nextCampaignIdToSynch, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
                return true;
            }
            return false;
        }

        //private void _loadCampaignsLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    _saveLoadCampaignsLinkText = _loadCampaignsLink.Text;
        //    _loadCampaignsLink.Text = "Loading Campaigns...";
        //    _loadCampaignsLink.Enabled = false;
        //    backgroundWorker2.RunWorkerAsync();
        //}

        //private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        //{
        //    var db = new DirectTrackDataContext();
        //    var campaignListXML = new CampaignListXml
        //    {
        //        Content = XElement.Parse(XmlGetter.ListCampaigns())
        //    };
        //    db.CampaignListXmls.InsertOnSubmit(campaignListXML);
        //    db.SubmitChanges();
        //}

        //private void backgroundWorker2_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        //{
        //    _loadCampaignsLink.Text = _saveLoadCampaignsLinkText;
        //    _loadCampaignsLink.Enabled = true;
        //}

        private void flowLayoutPanel1_Leave(object sender, System.EventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
