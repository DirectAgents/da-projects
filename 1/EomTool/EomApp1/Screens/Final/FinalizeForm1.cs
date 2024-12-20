﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DAgents.Common;
using EomApp1.Screens.Final.Models;
using EomApp1.UI;
using EomAppControls.DataGrid;

namespace EomApp1.Screens.Final
{
    public partial class FinalizeForm1 : AppFormBase
    {
        private DataGridViewLinkColumn[] numPubColsTop;
        private DataGridViewLinkColumn[] numPubColsBottom;
        private Dictionary<string, string> pubColHeaderTextToFilter = new Dictionary<string, string>();

        public FinalizeForm1()
        {
            InitializeComponent();

            this.numPubColsTop = new[] { NumPubsToFinalizeCol, NumAffiliatesNet7, NumAffiliatesNet15, NumAffiliatesNet30, NumAffiliatesNet7Bi, NumAffiliatesNet15Bi, NumAffiliatesWeekly, NumAffiliatesBiWeekly };
            this.numPubColsBottom = new[] { NumPubsToVerifyCol, dataGridViewLinkColumn1, dataGridViewLinkColumn2, dataGridViewLinkColumn3, dataGridViewLinkColumn4, dataGridViewLinkColumn5, dataGridViewLinkColumn6, dataGridViewLinkColumn7 };
            this.pubColHeaderTextToFilter.Add("#Net7", "Net 7");
            this.pubColHeaderTextToFilter.Add("#Net15", "Net 15");
            this.pubColHeaderTextToFilter.Add("#Net30", "Net 30");
            this.pubColHeaderTextToFilter.Add("#Net7Bi", "Net 7/Biweekly");
            this.pubColHeaderTextToFilter.Add("#Net15Bi", "Net 15/Biweekly");
            this.pubColHeaderTextToFilter.Add("#Wkly", "Weekly");
            this.pubColHeaderTextToFilter.Add("#BiWkly", "Biweekly");
            this.notesListForm = new NotesListForm1();

            SetNetTermColumnsVisibility(false);
            SetNumPubsColumnsStyle();
            SetRevBreakdownColumnsVisibility(false);
            SetRevBreakdownColumnsStyle();

            var finalizeConcat = (!Security.User.Current.FinalizeList.Any() ? "''" :
                String.Join(",", Security.User.Current.FinalizeList.Select(x => "'" + x + "'")));

            accountManagerBindingSource.Filter = "name in ('default'," + finalizeConcat + ")";
            AddToBindingSourceFilter(campaignBindingSource, "AM in (" + finalizeConcat + ")");
            AddToBindingSourceFilter(campaignBindingSource1, "AM in (" + finalizeConcat + ")");

            // Security
            //campaignsToFinalizeGrid.Sorted += (s, e) => DisableFinalizeButtons();
            //campaignBindingSource.ListChanged += (s, e) => DisableFinalizeButtons();
            DisableVerifyAndReview();
        }
        private void AddToBindingSourceFilter(BindingSource bindingSource, string moreFilter)
        {
            string prefix = "";
            if (!String.IsNullOrWhiteSpace(bindingSource.Filter))
                prefix = bindingSource.Filter + " and ";
            bindingSource.Filter = prefix + moreFilter;
        }

        private void DisableVerifyAndReview()
        {
            if (Security.User.Current.CanDoWorkflowVerify)
            {
                campaignsToVerifyGrid.Sorted += (s, e) => DisableBottomButtons();
                campaignBindingSource1.ListChanged += (s, e) => DisableBottomButtons();
            }
            else
            {
                verifyCol.Visible = false; // verify button
                colReview.Visible = false; // review button
                ApprovalCol.Visible = false; // mediabuyer approval - send button
                mbApprovalButton.Visible = false; // link at top
                finalizedRevenueButton.Visible = false; // link at top
            }
        }

        private void SetNetTermColumnsVisibility(bool visible)
        {
            foreach (DataGridViewLinkColumn linkColumn in NumPubNetTermColumns)
                linkColumn.Visible = visible;
        }

        // The first one in each set is the total number of publishers and the rest are the beakdown by net terms.
        private IEnumerable<DataGridViewLinkColumn> NumPubNetTermColumns
        {
            get { return this.numPubColsTop.Skip(1).Concat(numPubColsBottom.Skip(1)); }
        }

        private void SetNumPubsColumnsStyle()
        {
            System.Windows.Forms.DataGridViewCellStyle style = new System.Windows.Forms.DataGridViewCellStyle();
            style.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            style.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            foreach (var item in this.numPubColsTop)
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
            DoFillCampaigns();
        }

        private void FillCampaignsAll()
        {
            FinalizeDataSet1.CampaignDataTable dataTable = finalizeDataSet1.Campaign;
            campaignTableAdapter.Fill(dataTable);

            // Security... Note: maybe it's not necessary to call these here b/c they are called during the Fill when the
            // "Sorted" and "ListChanged" events are triggered.
            //DisableFinalizeButtons();
            //DisableBottomButtons();
        }

        // Security        
        //private void DisableFinalizeButtons()
        //{
        //    if (campaignsToFinalizeGrid.Rows.Count > 0)
        //    {
        //        //campaignsToFinalizeGrid.CurrentCell = null; // so can set row.Visible to false
        //        int finalizeColIndex = FinalizeCol.Index;
        //        var noAccessRows =
        //            from row in campaignsToFinalizeGrid.Rows.Cast<DataGridViewRow>()
        //            let am = ((row.DataBoundItem as DataRowView).Row as FinalizeDataSet1.CampaignRow).AM
        //            where !Security.User.Current.CanFinalizeForAccountManager(am)
        //            select row;
        //        foreach (var row in noAccessRows)
        //        {
        //            ((DataGridViewDisableButtonCell)row.Cells[finalizeColIndex]).Enabled = false;
        //            //row.Visible = false;
        //        }
        //    }
        //}

        private void DisableBottomButtons()
        {
            DisableButtons(campaignsToVerifyGrid, row => row.MediaBuyerApprovalStatus != "default", ApprovalCol);
            DisableButtons(campaignsToVerifyGrid, row => row.MediaBuyerApprovalStatus != "Approved", verifyCol);
            DisableButtons(campaignsToVerifyGrid, row => row.MediaBuyerApprovalStatus != "default", colReview);
        }

        static private void DisableButtons(DataGridView grid, Func<FinalizeDataSet1.CampaignRow, bool> condition, DataGridViewDisableButtonColumn buttonCol)
        {
            if (grid.Rows.Count > 0)
            {
                var rows =
                    from gridRow in grid.Rows.Cast<DataGridViewRow>()
                    let dataRow = ((gridRow.DataBoundItem as DataRowView).Row as FinalizeDataSet1.CampaignRow)
                    where condition(dataRow)
                    select gridRow;
                foreach (var row in rows)
                {
                    ((DataGridViewDisableButtonCell)row.Cells[buttonCol.Index]).Enabled = false;
                }
            }
        }

        private void FillCampaigns(string am)
        {
            campaignTableAdapter.FillByAM(finalizeDataSet1.Campaign, am);

            // Security
            //DisableFinalizeButtons();
        }

        // Click handlers for TOP Pane
        private void CellClickTop(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var grid = sender as DataGridView;
            var cell = grid[e.ColumnIndex, e.RowIndex];

            if (cell is DataGridViewDisableButtonCell && !(cell as DataGridViewDisableButtonCell).Enabled)
                return;
            
            var pid = (int)grid[pidCol.Index, e.RowIndex].Value;
            var currency = (string)grid[Curr.Index, e.RowIndex].Value;

            // Final Button
            if (e.ColumnIndex == FinalizeCol.Index && (grid[FinalizeCol.Index, e.RowIndex] as DataGridViewDisableButtonCell).Enabled) // Security
            {
                string note;
                if (ConfirmationBox.ShowConfirmationModalDialog("Finalize", out note))
                {
                    if (!FinalizeUtility.CheckFinalizationMargins(pid, currency, null, null))
                        return;

                    FinalizeCampaign(pid, currency);
                    CampaignNoteUtility.AddCampaignNote(pid, note); // model
                    DoFillCampaigns();
                }
            }
            // #Pubs Button
            else if (this.numPubColsTop.Indicies().Contains(e.ColumnIndex) && ((int)grid[e.ColumnIndex, e.RowIndex].Value) != 0/*ignore empty cell*/)
            {
                HandleNumPubsClick(e, pid, currency, grid, UI.PublishersForm.Mode.Finalize, MediaBuyerApprovalStatusId.Approved);
            }
        }

        // Click handlers for BOTTOM Pane
        private void CellClickBottom(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var grid = sender as DataGridView;
            var cell = grid[e.ColumnIndex, e.RowIndex];
            
            if (cell is DataGridViewDisableButtonCell && !(cell as DataGridViewDisableButtonCell).Enabled)
                return;

            var pid = (int)grid[pidCol2.Index, e.RowIndex].Value;
            var currency = (string)grid[dataGridViewTextBoxColumn6.Index, e.RowIndex].Value;
            var row = (grid.Rows[e.RowIndex].DataBoundItem as DataRowView).Row as FinalizeDataSet1.CampaignRow;
            MediaBuyerApprovalStatusId mbApprovalStatusID = (MediaBuyerApprovalStatusId)Enum.Parse(typeof(MediaBuyerApprovalStatusId), row.MediaBuyerApprovalStatus.Trim(), true);
            string itemIds = row.ItemIds;

            string note;
            // Queue Button
            if (e.ColumnIndex == ApprovalCol.Index)
            {
                UpdateMediaBuyerApprovalStatus("default", "Queued", itemIds);
                DoFillCampaigns();
            }
            // Verify Button
            else if (e.ColumnIndex == grid.Columns["verifyCol"].Index)
            {
                if (ConfirmationBox.ShowConfirmationModalDialog("Verify", out note))
                {
                    VerifyCampaign(pid, currency);
                    CampaignNoteUtility.AddCampaignNote(pid, note);
                    DoFillCampaigns();
                }
            }
            // Review Button
            else if (e.ColumnIndex == grid.Columns["colReview"].Index)
            {
                if (ConfirmationBox.ShowConfirmationModalDialog("Review", out note))
                {
                    ReviewCampaign(pid, currency);
                    CampaignNoteUtility.AddCampaignNote(pid, note);
                    DoFillCampaigns();
                }
            }
            // #Pubs Button
            else if (this.numPubColsBottom.Indicies().Contains(e.ColumnIndex) && ((int)grid[e.ColumnIndex, e.RowIndex].Value) != 0 /*ignore empty cell*/)
            {
                HandleNumPubsClick(e, pid, currency, grid, UI.PublishersForm.Mode.Verify, mbApprovalStatusID);
            }
        }

        private void UpdateMediaBuyerApprovalStatus(string fromStatus, string toStatus, string itemIds)
        {
            string fromSql = "";
            if (!string.IsNullOrWhiteSpace(fromStatus))
            {
                fromSql = " AND media_buyer_approval_status_id = (SELECT id FROM MediaBuyerApprovalStatus WHERE name = '[[FromStatus]]')";
            }
            string sql = (@"
                                UPDATE Item 
                                SET media_buyer_approval_status_id = (SELECT id FROM MediaBuyerApprovalStatus WHERE name = '[[ToStatus]]') 
                                WHERE id IN ([[ItemIds]])
                            " + fromSql)
                            .Replace("[[FromStatus]]", fromStatus)
                            .Replace("[[ToStatus]]", toStatus)
                            .Replace("[[ItemIds]]", itemIds)
                            .Trim();

            SqlUtility.ExecuteNonQuery(sql);
        }

        private void HandleNumPubsClick(DataGridViewCellEventArgs e, int pid, string currency, DataGridView grid, UI.PublishersForm.Mode mode, MediaBuyerApprovalStatusId? mediaBuyerApprovalStatus)
        {
            // Set up the Net Terms filter based on the column header that was clicked.
            string filter = null;
            string headerText = grid.Columns[e.ColumnIndex].HeaderText;
            if (this.pubColHeaderTextToFilter.ContainsKey(headerText))
            {
                filter = this.pubColHeaderTextToFilter[headerText];
            }

            var publishersForm = new UI.PublishersForm(this, pid, currency, mode, filter, mediaBuyerApprovalStatus);
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
            using (var eom = Models.Eom.Create())
            {
                var query = from c in eom.Items
                            where
                                c.pid == id && c.campaign_status_id == (int)CampaignStatusId.Finalized &&
                                c.RevenueCurrency.name == currency &&
                                c.media_buyer_approval_status_id == (int)MediaBuyerApprovalStatusId.Default
                            select c;

                var defaultCampaignStatus = eom.CampaignStatuses.Single(c => c.name == "default").id;

                foreach (var item in query)
                {
                    item.campaign_status_id = defaultCampaignStatus;
                }

                eom.SaveChanges();
            }
        }

        private void VerifyCampaign(int id, string currency)
        {
            using (var eom = Models.Eom.Create())
            {
                var query = from c in eom.Items
                            where 
                                c.pid == id && c.campaign_status_id == (int)CampaignStatusId.Finalized &&
                                c.RevenueCurrency.name == currency &&
                                c.media_buyer_approval_status_id == (int)MediaBuyerApprovalStatusId.Approved
                            select c;

                int verifyCampaignStatus = eom.CampaignStatuses.Single(c => c.name == "Verified").id;

                foreach (var item in query)
                {
                    item.campaign_status_id = verifyCampaignStatus;
                }

                eom.SaveChanges();
            }
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
            campaignsToFinalizeGrid.EndEdit();
            DoFillCampaigns();
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
            string am = this.selectedAM;
            if (am == "default")
                FillCampaignsAll();
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