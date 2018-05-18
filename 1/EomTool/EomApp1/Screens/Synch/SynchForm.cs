using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DAgents.Common;
using DAgents.Synch;
using System.Threading;
using Mainn.Controls.Logging;
using EomApp1.Screens.Synch.Models.Eom;
using System.Drawing;
namespace EomApp1.Screens.Synch
{
    public partial class SynchForm : Form, ILogger
    {
        public SynchForm()
        {
            InitializeComponent();
            AddEvents();
            ApplySettings();
        }

        #region ILogger Members
        object logLock = new object();

        List<string> logMessages = new List<string>();

        List<string> logErrors = new List<string>();

        public void Log(string message)
        {
            lock (logLock)
            {
                if (message.StartsWith("!"))
                {
                    loggerBox1.Log(message);
                }
                else
                {
                    loggerBox1.Log(message);
                }
            }
        }

        public void LogError(string message)
        {
            lock (logLock)
            {
                loggerBox1.LogError(message);
            }
        }
        #endregion

        private void ApplySettings()
        {
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }

        private void AddEvents()
        {
            Shown += (s, e) =>
            {
                PopulateEndDay();
            };

            this.FormClosing += (s, e) =>
            {
                SaveSettings();
            };
        }

        private void PopulateEndDay()
        {
            int days = DaysToSynch();
            toDayTextBox.Text = days.ToString();
        }

        // When current month is same as target month, then return the current day of the month.
        // Else return the number of days in the target month.
        private static int DaysToSynch()
        {
            var now = DateTime.Now;
            int result = now.Day;
            int month = Properties.Settings.Default.StatsMonth;
            int year = Properties.Settings.Default.StatsYear;
            if (!((year == now.Year) && (month == now.Month)))
            {
                result = DateTime.DaysInMonth(year, month);
            }
            return result;
        }

        private List<int> PidList
        {
            get
            {
                List<int> pids;
                if (SynchAllActiveCampaigns)
                {
                    PopulateActiveCampaigns();
                    pids = CampaignList.ActiveCampaigns().Select(c => c.CampaignId).ToList();
                }
                else
                {
                    ExtractIntegerItemsFromSpaceAndLineDelimitedList(_pidsText, out pids);
                }
                return pids;
            }
        }

        private void PopulateActiveCampaigns()
        {
            richTextBox1.Text = string.Join("\n",
                                    CampaignList
                                        .ActiveCampaigns()
                                        .Select(c => c.CampaignId)
                                        .OrderBy(c => c)
                                        .ToArray());
        }

        private void ExtractIntegerItemsFromSpaceAndLineDelimitedList(string s, out List<int> list)
        {
            list = new List<int>();
            var split = s.Split(new char[] {' ', '\n', ','}, StringSplitOptions.RemoveEmptyEntries);
            int tmpInt;
            foreach (var pid in split)
            {
                if (int.TryParse(pid, out tmpInt))
                    list.Add(tmpInt);
            }
        }

        private string _pidsText;

        private void CampaignPrepareForStatsButton_Click(object sender, EventArgs e)
        {
            _pidsText = richTextBox1.Text;
            campaignPrepareForStatsBackgroundWorker.RunWorkerAsync(); // calls Synch(object, DoWorkEventArgs)
        }

        private void Synch(object sender, DoWorkEventArgs e)
        {
            PreSynch();
            Synch();
        }

        private void PreSynch()
        {
            if (ShouldUpdateCampaigns)
            {
                //UpdateCampaigns();
            }
            if (ShouldUpdateAMADMappings)
            {
                UpdateAMADMappings();
            }
        }

        private void Synch()
        {
            List<int> pidList = this.PidList;

            Func<int> today = () => DateTime.Now.Day;

            int currentDay = today();

            bool didOnce = false;

            // if looping or not yet looped once..
            while (ShouldLoop || !didOnce)
            {
                didOnce = true;

                // if looping and it is no longer today..
                if (ShouldLoop && (currentDay != today()))
                {
                    Log("looping and current day has changed, refreshing..");
                    currentDay = today();
                    PopulateEndDay();
                }

                DateTime start = DateTime.Now;
                Log("synch start time is " + start);

                SynchCampaigns(pidList);

                // if looping, re-get the list of pids to synch
                if (ShouldLoop)
                {
                    PauseBetweenLoops(start);

                    Log("looping, refreshing pid list..");
                    pidList = PidList;
                }
            }
        }

        private void PauseBetweenLoops(DateTime start)
        {
            TimeSpan elapsed = DateTime.Now - start;
            Log("synch time elapsed is " + elapsed);

            TimeSpan interval = TimeSpan.FromHours(2); //todo: unhardcode, take from text box
            Log("time between synchs is " + interval);

            if (elapsed < interval)
            {
                TimeSpan sleepFor = interval - elapsed;
                Log("sleeping unitl next synch in " + sleepFor);

                Thread.Sleep(interval - elapsed);
            }
        }

        private void UpdateAMADMappings()
        {
            Log("updating AM/AD mappings from DirectTrack..");
            SynchUtility.SynchCampaignGroups((ILogger)this);
        }

        private class PidInfo
        {
            public int Pid { get; set; }
            public int RedirectedPid { get; set; }
        }

        private void SynchCampaigns(IEnumerable<int> pidList)
        {
            Log("synch starting..");
            foreach (var pidListBatch in pidList.InBatches(5))
            {
                pidListBatch.AsParallel().ForAll(pid =>
                {
                    SynchStats(pid, 0); // redirect pid
                });
            }
            Log("synch done.");
        }

        private void SynchPayouts(int pid)
        {
            Log("synching payouts..");
            try
            {
                DAgents.Synch.SynchUtility.SynchPayoutsForCampaignPid(Logger, pid);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("(404)"))
                {
                    Logger.LogError("error for payout(pid=" + pid + "): " + ex.Message + ex.StackTrace);
                    if (ex.InnerException != null)
                    {
                        Logger.LogError("error for payout(pid=" + pid + "): " + ex.InnerException.Message + ex.InnerException.StackTrace);
                    }
                }
            }
            Log("done synching payouts.");
        }

        private void SynchStats(int pid, int redirectedPID)
        {
            Log("SynchStats begin..");
            try
            {
                int startDay = Convert.ToInt32(fromDayTextBox.Text);
                int endDay = Convert.ToInt32(toDayTextBox.Text);

                int? affId = null;
                int tmp;
                if (int.TryParse(affIdTextBox.Text, out tmp))
                    affId = tmp;

                SynchStatsFromCake(pid, startDay, endDay, affID: affId);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("(404)"))
                {
                    Logger.LogError("error for stats(pid=" + pid + "): " + ex.Message + " " + ex.StackTrace);
                    if (ex.InnerException != null)
                    {
                        Logger.LogError("INNER EXCEPTION: " + pid + "): " + ex.InnerException.Message + " " + ex.InnerException.StackTrace);
                    }
                }
            }
            Log("SynchStats end.");
        }

        private void SynchStatsFromCake(int offerID, int startDay, int endDay, int? affID = null)
        {
            int campaignID = offerID;
            try
            {
                using (var db = EomDatabaseEntities.Create())
                {
                    var campaignQuery = from c in db.Campaigns
                                        where c.external_id == offerID && c.TrackingSystem.name == "Cake Marketing"
                                        select c;
                    var campaign = campaignQuery.FirstOrDefault();
                    if (campaign != null)
                    {
                        campaignID = campaign.pid;
                    }
                }
            }
            catch
            {
            }
            var parameters = new CakeSyncher.Parameters
            {
                CampaignId = campaignID,
                CampaignExternalId = offerID,
                Year = Properties.Settings.Default.StatsYear,
                Month = Properties.Settings.Default.StatsMonth,
                FromDay = startDay,
                ToDay = endDay,
                GroupItemsToFirstDayOfMonth = _groupItemsToFirstDayOfMonthCheckBox.Checked,
                SkipZeros = _skipZerosCheckBox.Checked,
                AffiliateId = affID
            };
            var cakeSyncher = new CakeSyncher(this.Logger, parameters);
            cakeSyncher.SynchStatsForOfferId();
        }

        private void SynchStatsFromDirectTrack(int pid, int redirectedPID, int startDay, int endDay)
        {
            DAgents.Synch.SynchUtility.SynchStatsForPid(
                Logger,
                pid,
                Properties.Settings.Default.StatsYear,
                Properties.Settings.Default.StatsMonth,
                startDay,
                endDay,
                redirectedPID); // redirect pid
        }

        private void LoopCheckedChanged(object sender, EventArgs e)
        {
            var checkBox = (CheckBox)sender;
            continuouslyRadioButton.Visible = checkBox.Checked;
            everyRadioButton.Visible = checkBox.Checked;
            _loopEveryTextBox.Visible = checkBox.Checked;
            _loopEveryComboBox.Visible = checkBox.Checked;

            int additionalWidth = 160;
            if (checkBox.Checked)
            {
                this.Width += additionalWidth;
            }
            else
            {
                this.Width -= additionalWidth;
            }
        }

        private bool SynchAllActiveCampaigns
        {
            get { return _activeCampaignsCheckBox.Checked; }
        }

        private bool ShouldLoop
        {
            get { return _loopCheckBox.Checked; }
        }

        private ILogger Logger { get { return this; } }

        private void _preDeleteCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void IntervalCheckedChanged(object sender, EventArgs e)
        {
            ShowOrHideIntervalSetup = everyRadioButton.Checked;
        }

        private bool ShowOrHideIntervalSetup
        {
            set
            {
                _loopEveryTextBox.Enabled = value;
                _loopEveryComboBox.Enabled = value;
            }
        }

        private bool ShouldUpdateCampaigns
        {
            get
            {
                return _updateCampaignsCheckbox.Checked;
            }
        }

        private bool ShouldUpdateAMADMappings
        {
            get
            {
                return _updateAMADCheckbox.Checked;
            }
        }

        private void loggerBox1_Load(object sender, EventArgs e)
        {

        }

        private void targetSystem1_TargetSystemChoiceChanged(object sender, TargetSystemChoiceChangedEventArgs e)
        {
            EnableAndDisableControlsForTargetSystemChoice(e.TargetSystemChoice);
        }

        private void EnableAndDisableControlsForTargetSystemChoice(TargetSystemChoice targetSystemChoice)
        {
            switch (targetSystemChoice)
            {
                case TargetSystemChoice.DirectTrack:
                    Properties.Settings.Default.SynchScreenTargetSystemChoice = TargetSystemChoice.DirectTrack;
                    EnableAndDisableControlsForDirectTrackTargetSystemChoice();
                    break;
                case TargetSystemChoice.Cake:
                    Properties.Settings.Default.SynchScreenTargetSystemChoice = TargetSystemChoice.Cake;
                    EnableAndDisableControlsForCakeTargetSystemChoice();
                    break;
                default:
                    break;
            }
        }

        private void EnableAndDisableControlsForDirectTrackTargetSystemChoice()
        {
        }

        private void EnableAndDisableControlsForCakeTargetSystemChoice()
        {
            this.groupBox1.Enabled = false;
        }

        private void redirectsLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var form = new SetupForm();
            form.ShowDialog();
        }

        private void _groupItemsToFirstDayOfMonthCheckBox_EnabledChanged(object sender, EventArgs e)
        {
        }
    }
}

//private delegate void SetPropertyThreadSafeDelegate<TResult>(Control @this, Expression<Func<TResult>> property, TResult value);
//public static void SetPropertyThreadSafe<TResult>(this Control @this, Expression<Func<TResult>> property, TResult value)
//{
//    var propertyInfo = (property.Body as MemberExpression).Member as PropertyInfo;
//    if (propertyInfo == null ||
//        !@this.GetType().IsSubclassOf(propertyInfo.ReflectedType) ||
//        @this.GetType().GetProperty(propertyInfo.Name, propertyInfo.PropertyType) == null)
//    {
//        throw new ArgumentException("The lambda expression 'property' must reference a valid property on this Control.");
//    }
//    if (@this.InvokeRequired)
//    {
//        @this.Invoke(new SetPropertyThreadSafeDelegate<TResult>(SetPropertyThreadSafe), new object[] { @this, property, value });
//    }
//    else
//    {
//        @this.GetType().InvokeMember(propertyInfo.Name, BindingFlags.SetProperty, null, @this, new object[] { value });
//    }
//}

//logMessages.Add(message);
//if (logMessages.Count % 10 == 0 || message == "EOF")
//{
//    logBox1.Log(string.Join("\n", logMessages));
//    logMessages.Clear();
//}

//private void campaignInitBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
//{
//    Log("synching campaigns");
//    DAgents.Synch.SynchUtility.SynchCampaignListFromDirectTrackToDatabase(Logger);
//    Log("synching campaign groups");
//    DAgents.Synch.SynchUtility.SynchCampaignGroups(Logger);
//    Log("synching payouts");
//    DAgents.Synch.SynchUtility.SynchPayoutsForAllCampaigns(Logger);
//    Log("done");
//}

