using System;
using System.Data;
using System.Windows.Forms;
using EomApp1.Screens.Final.Models;

namespace EomApp1.Screens.Final.UI
{
    public partial class VerifiedRevenueForm : Form
    {
        private FinalizeForm1 parentForm;

        public VerifiedRevenueForm()
        {
            InitializeComponent();
            Fill();
        }

        public VerifiedRevenueForm(FinalizeForm1 parent)
            : this()
        {
            this.parentForm = parent;
        }

        private void campaignsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (CampaignSelected)
                Publishers.Fill(Data, CampaignStatusId.Verified);
        }

        private void campaignsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (IsUnfinalizeCampaignButton(e) && ConfirmUnfinalizeCampaign())
            {
                Campaigns.UpdateCampaignItemStatus(SelectedCampaign.PID, CampaignStatusId.Default);
                ReFill();
            }
        }

        private bool ConfirmUnfinalizeCampaign()
        {
            return ConfirmationBox.Confirm(this, "Unfinalize Campaign?", "Unfinalize");
        }

        private void publisherGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (IsUnfinalizePublisherButton(e) && ConfirmUnfinalizePublisher())
            {
                Publishers.ChangeCampaignStatus(CampaignStatusId.Verified, CampaignStatusId.Default, SelectedPublisher.AffId);
                ReFill();
            }
        }

        private bool ConfirmUnfinalizePublisher()
        {
            return ConfirmationBox.Confirm(this, "Unfinalize Publisher?", "Unfinalize");
        }

        private bool IsUnfinalizeCampaignButton(DataGridViewCellEventArgs e)
        {
            return (e.RowIndex > -1 && e.ColumnIndex == UnfinalizeCampaignButtonCol.Index);
        }

        private bool IsUnfinalizePublisherButton(DataGridViewCellEventArgs e)
        {
            return (e.RowIndex > -1 && e.ColumnIndex == UnfinalizeCampaignPublisherButtonCol.Index);
        }

        private void Fill()
        {
            Campaigns.FillVerifiedCampaigns();
        }

        private void ReFill()
        {
            int selectedPID = CampaignSelected ? SelectedCampaign.PID : 0;
            Data.Clear();
            Fill();
            RefreshParent();
            if (selectedPID != 0)
            {
                int index = campaignsBindingSource.Find("PID", selectedPID);
                campaignsBindingSource.Position = index;
            }
        }

        private Data Data
        {
            get { return this.data; }
        }

        private void RefreshParent()
        {
            if (this.parentForm != null)
                this.parentForm.RefreshCampaigns();
        }

        private bool CampaignSelected
        {
            get { return (campaignsBindingSource.Current != null); }
        }

        private Data.CampaignsRow SelectedCampaign
        {
            get { return (Data.CampaignsRow)((DataRowView)campaignsBindingSource.Current).Row; }
        }

        private bool PublisherSelected
        {
            get { return (publishersBindingSource.Current != null); }
        }

        private Data.PublishersRow SelectedPublisher
        {
            get { return ((Data.PublishersRow)((DataRowView)publishersBindingSource.Current).Row); }
        }

        private CampaignPublishers Publishers
        {
            get { return new CampaignPublishers(SelectedCampaign.PID); }
        }

        private Campaigns Campaigns
        {
            get { return new Campaigns(Data); }
        }
    }
}
