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
            if (this.SelectedCampaignPid != null)
                (new CampaignsModel()).FillCampaignPublishers(this.data, this.SelectedCampaignPid.Value, CampaignStatusId.Verified);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != FinalizeCampaignButtonCol.Index) return;

            (new CampaignsModel()).UpdateCampaignItemStatus(this.data, this.SelectedCampaignPid.Value, CampaignStatusId.Default);
            ReFill();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != UnfinalizeCampaignPublisherButtonCol.Index) return;

            new PublishersModel(this.SelectedCampaignPid.Value).ChangePublisherItems(CampaignStatusId.Verified, CampaignStatusId.Default, this.SelectedPublisherAffId.Value);
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

        private int? SelectedCampaignPid
        {
            get
            {
                if (campaignsBindingSource.Current == null)
                    return null;
                else
                    return ((Data.DataSet1.CampaignsRow)((DataRowView)campaignsBindingSource.Current).Row).PID;
            }
        }

        private int? SelectedPublisherAffId
        {
            get
            {
                if (publishersBindingSource.Current == null)
                    return null;
                else
                    return ((Data.DataSet1.PublishersRow)((DataRowView)publishersBindingSource.Current).Row).AffId;
            }
        }
    }
}
