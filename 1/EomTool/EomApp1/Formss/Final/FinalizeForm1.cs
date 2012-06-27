using System;
using System.Linq;
using System.Windows.Forms;
using EomApp1.Services;
using EomApp1.UI;

namespace EomApp1.Formss.Final
{
    public partial class FinalizeForm1 : AppFormBase
    {
        private EomAppService _eomAppService;

        public FinalizeForm1()
        {
            InitializeComponent();
            this.notesListForm = new NotesListForm1();
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

        // Final Button
        private void campaignDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex != campaignDataGridView.Columns["FinalizeCol"].Index) 
                return;
            var id = (int)campaignDataGridView["pidCol", e.RowIndex].Value;

            string note;
            if (ConfirmationBox.ShowConfirmationModalDialog("Finalize", out note))
            {
                FinalizeCampaign(id);
                AddCampaignNote(id, note); // model
                DoFillCampaigns();
            }
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

        // Verify Button
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string notes;
            if (e.ColumnIndex == dataGridView1.Columns["verifyCol"].Index)
            {
                var id = (int)dataGridView1["pidCol2", e.RowIndex].Value;

                if (ConfirmationBox.ShowConfirmationModalDialog("Verify", out notes))
                {
                    VerifyCampaign(id);
                    AddCampaignNote(id, notes);
                    FillCampaigns();
                }
            }
            else if (e.ColumnIndex == dataGridView1.Columns["colReview"].Index)
            {
                var id = (int)dataGridView1["pidCol2", e.RowIndex].Value;

                if (ConfirmationBox.ShowConfirmationModalDialog("Review", out notes))
                {
                    ReviewCampaign(id);
                    AddCampaignNote(id, notes);
                    FillCampaigns();
                }
            }
        }

        private void ReviewCampaign(int id)
        {
            var db = new FinalizeDataDataContext(true);

            (from c in db.Campaigns
             where c.pid == id
             select c).First().campaign_status_id = (from c in db.CampaignStatus
                                                     where c.name == "Active"
                                                     select c).First().id;

            db.SubmitChanges();
        }

        private void VerifyCampaign(int id)
        {
            var db = new FinalizeDataDataContext(true);

            (from c in db.Campaigns
             where c.pid == id
             select c).First().campaign_status_id = (from c in db.CampaignStatus
                                                     where c.name == "Verified"
                                                     select c).First().id;

            db.SubmitChanges();
        }

        private static void FinalizeCampaign(int id)
        {
            var db = new FinalizeDataDataContext(true);

            (from c in db.Campaigns
             where c.pid == id
             select c).First().campaign_status_id = (from c in db.CampaignStatus
                                                     where c.name == "Finalized"
                                                     select c).First().id;
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
    }
}