using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EomApp1.Services;
using EomApp1.UI;

namespace EomApp1.Screens.Final
{
    public partial class FinalizeForm1 : AppFormBase
    {
        private EomAppService _eomAppService;
        private DataGridViewLinkColumn[] numPubCols1;
        private DataGridViewLinkColumn[] numPubCols2;
        private int[] pubColIndicies1;
        private int[] pubColIndicies2;
        private Dictionary<string, string> pubColHeaderTextToFilter = new Dictionary<string, string>();

        public FinalizeForm1()
        {
            InitializeComponent();
            this.numPubCols1 = new[] { NumPubsToFinalizeCol, NumAffiliatesNet7, NumAffiliatesNet15, NumAffiliatesNet30, NumAffiliatesNetBiWeekly };
            this.numPubCols2 = new[] { NumPubsToVerifyCol, dataGridViewLinkColumn1, dataGridViewLinkColumn2, dataGridViewLinkColumn3, dataGridViewLinkColumn4 };
            this.pubColIndicies1 = this.numPubCols1.Select(c => c.Index).ToArray();
            this.pubColIndicies2 = this.numPubCols2.Select(c => c.Index).ToArray();
            this.pubColHeaderTextToFilter.Add("#Net7", "Net 7");
            this.pubColHeaderTextToFilter.Add("#Net15", "Net 15");
            this.pubColHeaderTextToFilter.Add("#Net30", "Net 30");
            this.pubColHeaderTextToFilter.Add("#BiWkly", "Net 7/Biweekly");
            this.notesListForm = new NotesListForm1();
            foreach (var item in this.numPubCols1.Concat(numPubCols2))
            {
                item.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            SetNetTermColumnsVisibility(false);
        }

        private void FinalizeForm1_Load(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = true;
            accountManagerTableAdapter.Fill(accountManagersForFinalDataSet1.AccountManager);
            FillCampaigns();
        }

        private void FillCampaigns()
        {
            FinalizeDataSet1.CampaignDataTable x = finalizeDataSet1.Campaign;
            campaignTableAdapter.Fill(x);
        }

        private void FillCampaigns(string am)
        {
            campaignTableAdapter.FillByAM(finalizeDataSet1.Campaign, am);
        }

        private void CellClickTop(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var grid = sender as DataGridView;
            var pid = (int)grid["pidCol", e.RowIndex].Value;

            // Final Button
            if (e.ColumnIndex == FinalizeCol.Index)
            {
                string note;
                if (ConfirmationBox.ShowConfirmationModalDialog("Finalize", out note))
                {
                    FinalizeCampaign(pid);
                    AddCampaignNote(pid, note); // model
                    DoFillCampaigns();
                }
            }
            // #Pubs Button
            else if (
                this.pubColIndicies1.Contains(e.ColumnIndex)
                && ((int)grid[e.ColumnIndex, e.RowIndex].Value) != 0 // ignore empty cell
            )
            {
                HandleNumPubsClick(e, pid, grid, Forms.PublishersForm.Mode.Finalize);
            }
        }

        // Verify Button
        private void CellClickBottom(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var grid = sender as DataGridView;
            var pid = (int)grid["pidCol2", e.RowIndex].Value;

            string note;
            // Verify Button
            if (e.ColumnIndex == grid.Columns["verifyCol"].Index)
            {
                if (ConfirmationBox.ShowConfirmationModalDialog("Verify", out note))
                {
                    VerifyCampaign(pid);
                    AddCampaignNote(pid, note);
                    FillCampaigns();
                }
            }
            // Review Button
            else if (e.ColumnIndex == grid.Columns["colReview"].Index)
            {
                if (ConfirmationBox.ShowConfirmationModalDialog("Review", out note))
                {
                    ReviewCampaign(pid);
                    AddCampaignNote(pid, note);
                    FillCampaigns();
                }
            }
            // #Pubs Button
            else if (
                this.pubColIndicies2.Contains(e.ColumnIndex)
                && ((int)grid[e.ColumnIndex, e.RowIndex].Value) != 0 // ignore empty cell
            )
            {
                HandleNumPubsClick(e, pid, grid, Forms.PublishersForm.Mode.Verify);
            }
        }


        private void HandleNumPubsClick(DataGridViewCellEventArgs e, int pid, DataGridView grid, Forms.PublishersForm.Mode mode)
        {
            string filter = null;
            string headerText = grid.Columns[e.ColumnIndex].HeaderText;
            if (this.pubColHeaderTextToFilter.ContainsKey(headerText))
            {
                filter = this.pubColHeaderTextToFilter[headerText];
            }
            var publishersForm = new Forms.PublishersForm(this, pid, mode, filter);
            MaskedDialog.ShowDialog(this, publishersForm);
        }

        private void AddCampaignNote(int campaignId, string note)
        {
            if (!string.IsNullOrEmpty(note))
            {
                TransactionScripts.AddCampaignNote(campaignId, note);
            }
        }

        ITxns TransactionScripts
        {
            get
            {
                return Services.Data.Txns;
            }
        }

        public EomAppService Services
        {
            get { return _eomAppService ?? (_eomAppService = new EomAppService(new DataService(new Txns()))); }
        }

        private void ReviewCampaign(int id)
        {
            var db = new FinalizeDataDataContext(true);

            var query = from c in db.Items
                        where c.pid == id && c.CampaignStatus.name == "Finalized"
                        select c;

            var defaultCampaignStatus = db.CampaignStatus.Single(c => c.name == "default");

            foreach (var item in query)
            {
                item.CampaignStatus = defaultCampaignStatus;
            }

            db.SubmitChanges();
        }

        private void VerifyCampaign(int id)
        {
            var db = new FinalizeDataDataContext(true);

            var query = from c in db.Items
                        where c.pid == id && c.CampaignStatus.name == "Finalized"
                        select c;

            var verifyCampaignStatus = db.CampaignStatus.Single(c => c.name == "Verified");

            foreach (var item in query)
            {
                item.CampaignStatus = verifyCampaignStatus;
            }

            db.SubmitChanges();
        }

        private static void FinalizeCampaign(int id)
        {
            var db = new FinalizeDataDataContext(true);

            var query = from c in db.Items
                        where c.pid == id && c.CampaignStatus.name == "default"
                        select c;

            var finalizeCampaignStatus = db.CampaignStatus.Single(c => c.name == "Finalized");

            foreach (var item in query)
            {
                item.CampaignStatus = finalizeCampaignStatus;
            }

            db.SubmitChanges();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (splitContainer1.Panel1Collapsed)
            {
                splitContainer1.Panel1Collapsed = false;
            }
            else
            {
                splitContainer1.Panel1Collapsed = true;
            }
        }

        string selectedAM
        {
            get
            {
                return comboBox1.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshCampaigns();
        }

        public void RefreshCampaigns()
        {
            string am = comboBox1.Text;

            campaignDataGridView.EndEdit();

            if (am == "default")
            {
                FillCampaigns();
            }
            else
            {
                FillCampaigns(am);
            }

            campaignDataGridView.Refresh();
        }

        private void ShowQCDialog(object sender, EventArgs e)
        {
            var a = new StatusMonitor();
            a.Show();
        }

        private void campaignDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == NoteDGCol.Index)
            {
                string note;
                if (ConfirmationBox.ShowConfirmationModalDialog("Message", out note))
                {
                    var campaignID = (int)campaignDataGridView["pidCol", e.RowIndex].Value;
                    AddCampaignNote(campaignID, note);
                    DoFillCampaigns();

                }
            }
        }

        private void DoFillCampaigns()
        {
            string s = this.selectedAM;
            if (s == "default")
                FillCampaigns();
            else
                FillCampaigns(this.selectedAM);
        }

        NotesListForm1 notesListForm { get; set; }

        private void button3_Click(object sender, EventArgs e)
        {
            this.notesListForm.Show();
        }

        private void campaignDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            FillNotesList(e.RowIndex);
        }

        private void FillNotesList(int rowIndex)
        {
            var campaignID = (int)campaignDataGridView["pidCol", rowIndex].Value;
            this.notesListForm.Fill(TransactionScripts, campaignID);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SetNetTermColumnsVisibility(checkBox1.Checked);
        }

        private void SetNetTermColumnsVisibility(bool visible)
        {
            foreach (var item in this.numPubCols1.Skip(1).Concat(numPubCols2.Skip(1)))
            {
                item.Visible = visible;
            }
        }

        private void FormatCell(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null && e.Value is int)
            {
                if ((int)e.Value == 0)
                {
                    e.Value = "";
                }
            }
        }
    }
}