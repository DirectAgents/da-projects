using System;
using System.Linq;
using System.Windows.Forms;
using EomApp1.Services;
using EomApp1.UI;

namespace EomApp1.Screens.Final
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

        private void campaignDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var pid = (int)campaignDataGridView["pidCol", e.RowIndex].Value;

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
            else if (e.ColumnIndex == NumPubsToFinalizeCol.Index)
            {
                var finalizePublishersForm = new Forms.PublishersForm(this, pid);
                finalizePublishersForm.ShowDialog();
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
            else if (e.ColumnIndex == NumPubsToVerifyCol.Index)
            {
                // todo: display modal workflow by publisher
            }
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
    }
}