using System;
using System.Data;
using System.Windows.Forms;
using EomApp1.Screens.Final.Models;

namespace EomApp1.Screens.Final.Forms
{
    public partial class VerifiedRevenueForm : Form
    {
        private FinalizeForm1 finalizeForm;

        public VerifiedRevenueForm()
        {
            InitializeComponent();
            Fill();
        }

        public VerifiedRevenueForm(FinalizeForm1 finalizeForm)
            : this()
        {
            this.finalizeForm = finalizeForm;
        }

        private void Fill()
        {
            (new CampaignsModel()).FillVerifiedCampaigns(this.data);
        }

        private void campaignsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            (new CampaignsModel()).FillCampaignPublishers(this.data, this.SelectedCampaignPid);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != FinalizeCampaignButtonCol.Index) return;

            (new CampaignsModel()).UpdateCampaignItemStatus(this.data, this.SelectedCampaignPid);
            ReFill();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != UnfinalizeCampaignPublisherButtonCol.Index) return;

            new PublishersModel(this.SelectedCampaignPid).ChangePublisherItems(CampaignStatusId.Verified, CampaignStatusId.Default, this.SelectedPublisherAffId);
            ReFill();
        }

        private void ReFill()
        {
            this.data.Clear();
            Fill();
            if (this.finalizeForm != null)
            {
                this.finalizeForm.RefreshCampaigns();
            }
        }

        private int SelectedCampaignPid
        {
            get { return ((Data.DataSet1.CampaignsRow)((DataRowView)campaignsBindingSource.Current).Row).PID; }
        }

        private int SelectedPublisherAffId
        {
            get { return ((Data.DataSet1.PublishersRow)((DataRowView)publishersBindingSource.Current).Row).AffId; }
        }
    }
}
