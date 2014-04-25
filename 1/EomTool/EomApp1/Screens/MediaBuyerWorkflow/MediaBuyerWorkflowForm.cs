using System;
using System.Linq;
using System.Windows.Forms;
using EomApp1.UI;
using EomAppControls.DataGrid;
using EomAppControls.Filtering;

namespace EomApp1.Screens.MediaBuyerWorkflow
{
    public partial class MediaBuyerWorkflowForm : Form
    {
        private MediaBuyerWorkflowModel model = new MediaBuyerWorkflowModel();
        private RadioButtonPanel<FilterStatus> radioButtons;

        public MediaBuyerWorkflowForm()
        {
            InitializeComponent();
            SetupFilter();
            PublishersSubformCol.Subform = typeof(PublishersSubform);
            PublishersSubformCol.SubformShowing += new DataGridViewExtensions.DataGridViewSubformColumn.SubformShowingHandler(PublishersSubformCol_SubformShowing);
            Fill();
            this.dataGridViewExtension1.CellClick += new DataGridViewCellEventHandler(dataGridViewExtension1_CellClick);
        }

        void dataGridViewExtension1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.dataGridViewExtension1.HasRows() || e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            if (e.ColumnIndex == ApprovalActionCol.Index)
            {
                // Send Email to Media Buyer 
                if (ApprovalActionCol.Value<string>(e) == "Send")
                {
                    string mediaBuyerName = mediaBuyerNameCol.Value<string>(e).Trim();
                    string mediaBuyerFirstName = mediaBuyerName.Split(' ').First();

                    var link = EomAppCommon.EomAppSettings.Settings.EomAppSettings_MediaBuyerWorkflow_Email_Link + "?period=" + Properties.Settings.Default.StatsDate.ToShortDateString();

                    var emailTemplate = new MediaBuyerEmailTemplate()
                    {
                        MediaBuyerName = mediaBuyerFirstName,
                        UrlToOpen = link,
                        TimePeriod = Properties.Settings.Default.DADatabaseName
                    };

                    string subject = EomAppCommon.EomAppSettings.Settings.EomAppSettings_MediaBuyerWorkflow_Email_Subject;
                    string from = EomAppCommon.EomAppSettings.Settings.EomAppSettings_MediaBuyerWorkflow_Email_From;
                    string mediaBuyerEmailAddress = EomApp1.Security.User.GetEmailAddress(mediaBuyerName);

                    var sendDialog = new EomApp1.Screens.MediaBuyerWorkflow.SendMailDialog(subject, from, mediaBuyerEmailAddress, emailTemplate);

                    var result = MaskedDialog.ShowDialog(this, sendDialog);

                    // Change status to Sent
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        string itemIDs = ItemIdsCol.Value<string>(e);
                        this.model.UpdateMediaBuyerApprovalStatus("Queued", "Sent", itemIDs);
                        this.Fill();
                    }
                }
            }
        }

        private void SetupFilter()
        {
            this.radioButtons = new RadioButtonPanel<FilterStatus>();
            radioButtons.Selected += new RadioButtonPanel<FilterStatus>.SelectedEvent(radioButtons_Selected);
            this.toolStrip1.Items.Add(new ToolStripControlHost(radioButtons)
            {
                Margin = new Padding(0),
                Padding = new Padding(0)
            });
        }

        void PublishersSubformCol_SubformShowing(DataGridViewExtensions.DataGridViewSubformCell sender, DataGridViewExtensions.SubformShowingEventArgs e)
        {
            e.DataBoundItem = Tuple.Create(this.bindingSource1.Current, this.FilterStatusString);
        }

        void radioButtons_Selected(MediaBuyerWorkflowForm.FilterStatus t)
        {
            Fill();
        }

        public enum FilterStatus
        {
            Checked,
            Sent,
        }

        private void Fill()
        {
            this.mediaBuyerWorkflowDataSet1.MediaBuyers.Clear();
            this.mediaBuyerWorkflowDataSet1.MediaBuyers.Load(this.model.MediaBuyers(this.FilterStatusString).CreateDataReader());
            this.ApprovalActionCol.Text = this.ActionStatusString;
            DisableButtons();
        }

        private void DisableButtons()
        {
            this.dataGridViewExtension1.ForEachCellInColumn<DataGridViewDisableButtonCell>(ApprovalActionCol.Index, cell =>
            {
                if ((string)cell.Value != "Send")
                    cell.Enabled = false;
            });
        }

        public string FilterStatusString
        {
            get
            {
                string statusString = null;
                switch (this.radioButtons.Current)
                {
                    case FilterStatus.Checked:
                        statusString = "Queued";
                        break;
                    case FilterStatus.Sent:
                        statusString = "Sent";
                        break;
                    default:
                        break;
                }
                return statusString;
            }
        }

        public string ActionStatusString
        {
            get
            {
                string statusString = null;
                switch (this.radioButtons.Current)
                {
                    case FilterStatus.Checked:
                        statusString = "Send";
                        break;
                    case FilterStatus.Sent:
                        statusString = "Waiting...";
                        break;
                    default:
                        break;
                }
                return statusString;
            }
        }
    }
}
