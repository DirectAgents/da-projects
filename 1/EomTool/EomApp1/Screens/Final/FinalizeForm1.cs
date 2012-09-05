﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DAgents.Common;
using EomApp1.UI;
using EomAppCommon;
using EomAppControls.DataGrid;
using System.Data;
using System.Drawing;

namespace EomApp1.Screens.Final
{
    public partial class FinalizeForm1 : AppFormBase
    {
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
            SetNetTermColumnsVisibility(false);
            SetNumPubsColumnsStyle();
            SetRevBreakdownColumnsVisibility(false);
            SetRevBreakdownColumnsStyle();

            // Security
            campaignsToFinalizeGrid.Sorted += (s, e) => DisableFinalizeButtons();
            campaignBindingSource.ListChanged += (s, e) => DisableFinalizeButtons();
            DisableVerifyAndReview();
        }

        private void DisableVerifyAndReview()
        {
            if (!Security.User.Current.CanDoWorkflowVerify)
            {
                verifyCol.Visible = false;
                colReview.Visible = false;
                ApprovalCol.Visible = false;
                mbApprovalButton.Visible = false;
            }
        }

        private void SetNumPubsColumnsStyle()
        {
            System.Windows.Forms.DataGridViewCellStyle style = new System.Windows.Forms.DataGridViewCellStyle();
            style.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            style.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            foreach (var item in this.numPubCols1)
            {
                item.DefaultCellStyle = style;
            }
        }

        private void SetRevBreakdownColumnsStyle()
        {
            System.Windows.Forms.DataGridViewCellStyle style = new System.Windows.Forms.DataGridViewCellStyle();
            style.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            style.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            style.Format = "N2";
            this.RevDefault.DefaultCellStyle = style;
            this.RevFinalized.DefaultCellStyle = this.RevDefault.DefaultCellStyle;
            this.RevVerified.DefaultCellStyle = this.RevDefault.DefaultCellStyle;
        }

        private void SetRevBreakdownColumnsVisibility(bool visible)
        {
            this.RevDefault.Visible = visible;
            this.RevFinalized.Visible = visible;
            this.RevVerified.Visible = visible;
        }

        private void FinalizeForm1_Load(object sender, EventArgs e)
        {
            accountManagerTableAdapter.Fill(accountManagersForFinalDataSet1.AccountManager);
            FillCampaigns();
        }

        private void FillCampaigns()
        {
            FinalizeDataSet1.CampaignDataTable x = finalizeDataSet1.Campaign;
            campaignTableAdapter.Fill(x);

            // Security
            DisableFinalizeButtons();

            DisableButtons(campaignsToVerifyGrid, row => row.MediaBuyerApprovalStatus != "default", ApprovalCol);
            DisableButtons(campaignsToVerifyGrid, row => row.MediaBuyerApprovalStatus != "Approved", verifyCol);
            DisableButtons(campaignsToVerifyGrid, row => row.MediaBuyerApprovalStatus != "default", colReview);
        }

        // Security        
        private void DisableFinalizeButtons()
        {
            if (campaignsToFinalizeGrid.Rows.Count > 0)
                foreach (var button in from row in campaignsToFinalizeGrid.Rows.Cast<DataGridViewRow>()
                                       let am = ((row.DataBoundItem as DataRowView).Row as FinalizeDataSet1.CampaignRow).AM
                                       where !Security.User.Current.CanDoWorkflowFinalize(am)
                                       select (DataGridViewDisableButtonCell)row.Cells[FinalizeCol.Index])
                    button.Enabled = false;
        }

        static private void DisableButtons(DataGridView grid, Func<FinalizeDataSet1.CampaignRow, bool> condition, DataGridViewDisableButtonColumn buttonCol)
        {
            if (grid.Rows.Count > 0)
                foreach (var button in from gridRow in grid.Rows.Cast<DataGridViewRow>()
                                       let dataRow = ((gridRow.DataBoundItem as DataRowView).Row as FinalizeDataSet1.CampaignRow)
                                       where condition(dataRow)
                                       select (DataGridViewDisableButtonCell)gridRow.Cells[buttonCol.Index])
                    button.Enabled = false;
        }

        private void FillCampaigns(string am)
        {
            campaignTableAdapter.FillByAM(finalizeDataSet1.Campaign, am);

            // Security
            DisableFinalizeButtons();
        }

        private void CellClickTop(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var grid = sender as DataGridView;
            var pid = (int)grid[pidCol.Index, e.RowIndex].Value;
            var currency = (string)grid[Curr.Index, e.RowIndex].Value;

            // Final Button
            if (e.ColumnIndex == FinalizeCol.Index && (grid[FinalizeCol.Index, e.RowIndex] as DataGridViewDisableButtonCell).Enabled) // Security
            {
                string note;
                if (ConfirmationBox.ShowConfirmationModalDialog("Finalize", out note))
                {
                    FinalizeCampaign(pid, currency);
                    CampaignNoteUtility.AddCampaignNote(pid, note); // model
                    DoFillCampaigns();
                }
            }
            // #Pubs Button
            else if (
                this.pubColIndicies1.Contains(e.ColumnIndex)
                && ((int)grid[e.ColumnIndex, e.RowIndex].Value) != 0 // ignore empty cell
            )
            {
                HandleNumPubsClick(e, pid, currency, grid, UI.PublishersForm.Mode.Finalize);
            }
        }

        // Click handlers for Verification Buttons and MB Workflow
        private void CellClickBottom(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var grid = sender as DataGridView;
            var pid = (int)grid[pidCol2.Index, e.RowIndex].Value;
            var currency = (string)grid[dataGridViewTextBoxColumn6.Index, e.RowIndex].Value;
            var row = (grid.Rows[e.RowIndex].DataBoundItem as DataRowView).Row as FinalizeDataSet1.CampaignRow;
            string itemIds = row.ItemIds;

            string note;
            // Ready Button
            if (e.ColumnIndex == ApprovalCol.Index)
            {
                UpdateMediaBuyerApprovalStatus("default", "Queued", itemIds);
                FillCampaigns();
            }
            // Verify Button
            else if (e.ColumnIndex == grid.Columns["verifyCol"].Index)
            {
                if (ConfirmationBox.ShowConfirmationModalDialog("Verify", out note))
                {
                    VerifyCampaign(pid, currency);
                    CampaignNoteUtility.AddCampaignNote(pid, note);
                    FillCampaigns();
                }
            }
            // Review Button
            else if (e.ColumnIndex == grid.Columns["colReview"].Index)
            {
                if (ConfirmationBox.ShowConfirmationModalDialog("Review", out note))
                {
                    ReviewCampaign(pid, currency);
                    CampaignNoteUtility.AddCampaignNote(pid, note);
                    FillCampaigns();
                }
            }
            // #Pubs Button
            else if (
                this.pubColIndicies2.Contains(e.ColumnIndex)
                && ((int)grid[e.ColumnIndex, e.RowIndex].Value) != 0 // ignore empty cell
            )
            {
                HandleNumPubsClick(e, pid, currency, grid, UI.PublishersForm.Mode.Verify);
            }
        }

        private void UpdateMediaBuyerApprovalStatus(string fromStatus, string toStatus, string itemIds)
        {
            string sql = @"
                                UPDATE Item 
                                SET media_buyer_approval_status_id = (SELECT id FROM MediaBuyerApprovalStatus WHERE name = '[[ToStatus]]') 
                                WHERE id IN ([[ItemIds]]) AND media_buyer_approval_status_id = (SELECT id FROM MediaBuyerApprovalStatus WHERE name = '[[FromStatus]]') 
                            "
                            .Replace("[[FromStatus]]", fromStatus)
                            .Replace("[[ToStatus]]", toStatus)
                            .Replace("[[ItemIds]]", itemIds)
                            .Trim();

            SqlUtility.ExecuteNonQuery(sql);
        }

        private void HandleNumPubsClick(DataGridViewCellEventArgs e, int pid, string currency, DataGridView grid, UI.PublishersForm.Mode mode)
        {
            string filter = null;
            string headerText = grid.Columns[e.ColumnIndex].HeaderText;
            if (this.pubColHeaderTextToFilter.ContainsKey(headerText))
            {
                filter = this.pubColHeaderTextToFilter[headerText];
            }
            var publishersForm = new UI.PublishersForm(this, pid, currency, mode, filter);
            MaskedDialog.ShowDialog(this, publishersForm);
        }

        public IEnumerable<CampaignNote> GetCampaignNotes(int campaignId)
        {
            return from c in new FinalizeDataDataContext(true).CampaignNotes
                   where c.campaign_id == campaignId
                   select c;
        }

        private void ReviewCampaign(int id, string currency)
        {
            var db = new FinalizeDataDataContext(true);

            var query = from c in db.Items
                        where c.pid == id && c.CampaignStatus.name == "Finalized" && c.Currency.name == currency
                        select c;

            var defaultCampaignStatus = db.CampaignStatus.Single(c => c.name == "default");

            foreach (var item in query)
            {
                item.CampaignStatus = defaultCampaignStatus;
            }

            db.SubmitChanges();
        }

        private void VerifyCampaign(int id, string currency)
        {
            var db = new FinalizeDataDataContext(true);

            var query = from c in db.Items
                        where c.pid == id && c.CampaignStatus.name == "Finalized" && c.Currency.name == currency
                        select c;

            var verifyCampaignStatus = db.CampaignStatus.Single(c => c.name == "Verified");

            foreach (var item in query)
            {
                item.CampaignStatus = verifyCampaignStatus;
            }

            db.SubmitChanges();
        }

        private static void FinalizeCampaign(int id, string currency)
        {
            var db = new FinalizeDataDataContext(true);

            var query = from c in db.Items
                        where c.pid == id && c.CampaignStatus.name == "default" && c.Currency.name == currency
                        select c;

            var finalizeCampaignStatus = db.CampaignStatus.Single(c => c.name == "Finalized");

            foreach (var item in query)
            {
                item.CampaignStatus = finalizeCampaignStatus;
            }

            db.SubmitChanges();
        }

        private string selectedAM
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

            campaignsToFinalizeGrid.EndEdit();

            if (am == "default")
            {
                FillCampaigns();
            }
            else
            {
                FillCampaigns(am);
            }

            campaignsToFinalizeGrid.Refresh();
        }

        private void campaignDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == NoteDGCol.Index)
            {
                string note;
                if (ConfirmationBox.ShowConfirmationModalDialog("Message", out note))
                {
                    var campaignID = (int)campaignsToFinalizeGrid["pidCol", e.RowIndex].Value;
                    CampaignNoteUtility.AddCampaignNote(campaignID, note);
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

        private NotesListForm1 notesListForm { get; set; }

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
            var campaignID = (int)campaignsToFinalizeGrid["pidCol", rowIndex].Value;
            this.notesListForm.Fill(this, campaignID);
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

            if (sender == campaignsToVerifyGrid)
            {
                if (e.ColumnIndex == ApprovalCol.Index)
                {
                    if (e.Value.ToString() == "default")
                        e.Value = EomAppCommon.EomAppSettings.Screens.Workflow.ApprovalColumn.TextOnButtonWhenMediaBuyerApprovalStatusIsDefault;
                }
            }
        }

        private void finalizedRevenueButton_Click(object sender, EventArgs e)
        {
            MaskedDialog.ShowDialog(this, new Screens.Final.UI.VerifiedRevenueForm(this));
        }

        private void revStatusCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            SetRevBreakdownColumnsVisibility(checkBox.Checked);
        }

        private void mbApprovalButton_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.LaunchForm<Screens.MediaBuyerWorkflow.MediaBuyerWorkflowForm>();
        }
    }
}