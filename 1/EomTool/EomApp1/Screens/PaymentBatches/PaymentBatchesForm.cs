using EomApp1.UI;
using EomAppControls.DataGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Screens.PaymentBatches
{
    public partial class PaymentBatchesForm : Form
    {
        const int BATCH_STATE_SENT = 3;

        public PaymentBatchesForm()
        {
            InitializeComponent();
        }

        void PaymentBatchesForm_Load(object sender, EventArgs e)
        {
            this.Fill();
            this.DisableComponents();
            this.ClearGridSelections();
            this.paymentBatchGrid.DataError += paymentBatchGrid_DataError;
            this.publisherPaymentBatchesGrid.DataError += publisherPaymentBatchesGrid_DataError;
        }

        void Fill()
        {
            this.FillLists();
            this.FillEntities();
        }

        void Refill()
        {
            this.FillEntities();
            this.ClearGridSelections();
        }

        void FillLists()
        {
            this.affiliatePaymentMethodTableAdapter.Connection.ConnectionString = EomAppCommon.EomAppSettings.ConnStr;
            this.affiliatePaymentMethodTableAdapter.Fill(this.paymentBatchesDataSet.AffiliatePaymentMethod);

            this.paymentBatchStateTableAdapter.Connection.ConnectionString = EomAppCommon.EomAppSettings.ConnStr;
            this.paymentBatchStateTableAdapter.Fill(this.paymentBatchesDataSet.PaymentBatchState);
        }

        void FillEntities()
        {
            this.paymentBatchTableAdapter.Connection.ConnectionString = EomAppCommon.EomAppSettings.ConnStr;
            this.paymentBatchTableAdapter.Fill(this.paymentBatchesDataSet.PaymentBatch);

            this.campaignPublisherPaymentBatchesSummaryTableAdapter.Connection.ConnectionString = EomAppCommon.EomAppSettings.ConnStr;
            this.campaignPublisherPaymentBatchesSummaryTableAdapter.Fill(this.paymentBatchesDataSet.CampaignPublisherPaymentBatchesSummary);
        }

        void paymentBatchGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        void publisherPaymentBatchesGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        void DisableComponents()
        {
            foreach (DataGridViewRow row in paymentBatchGrid.Rows)
            {
                if (row.DataBoundItem != null)
                {
                    var batchRow = (row.DataBoundItem as DataRowView).Row as PaymentBatchesDataSet.PaymentBatchRow;
                    if (batchRow.payment_batch_state_id == BATCH_STATE_SENT)
                        ((DataGridViewDisableButtonCell)row.Cells[SendDataGridViewButtonColumn.Index]).Enabled = false;
                }
            }
        }

        void ClearGridSelections()
        {
            this.paymentBatchGrid.ClearSelection();
            this.publisherPaymentBatchesGrid.ClearSelection();
        }

        private void paymentBatchGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // "Send" batch
            if (e.ColumnIndex == SendDataGridViewButtonColumn.Index && e.RowIndex >= 0 && e.RowIndex < paymentBatchGrid.NewRowIndex &&
                ((DataGridViewDisableButtonCell)paymentBatchGrid.Rows[e.RowIndex].Cells[SendDataGridViewButtonColumn.Index]).Enabled)
            {
                var batchRow = (paymentBatchGrid.Rows[e.RowIndex].DataBoundItem as DataRowView).Row as PaymentBatchesDataSet.PaymentBatchRow;
                if (batchRow.id > 0)
                {
                    var emailTemplate = new PaymentBatchesEmailTemplate()
                    {
                        UrlToOpen = "http://eomweb.directagents.local/PaymentBatches/Summary"
                    };
                    string subject = "payment batches are ready for your review";
                    string from = "accounting@directagents.com";
                    string to = "jberger@directagents.com";
                    var sendDialog = new EomApp1.Screens.MediaBuyerWorkflow.SendMailDialog(subject, from, to, emailTemplate);
                    var result = MaskedDialog.ShowDialog(this, sendDialog);
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        batchRow.payment_batch_state_id = BATCH_STATE_SENT;
                        SaveButtonClicked(null, null);
                        ((DataGridViewDisableButtonCell)paymentBatchGrid.Rows[e.RowIndex].Cells[SendDataGridViewButtonColumn.Index]).Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// Assign the items in the selected rows to the selected batch
        /// </summary>
        void AssignButtonClicked(object sender, EventArgs e)
        {
            using (var context = new EomTool.Domain.Entities.EomEntities(EomAppCommon.EomAppSettings.ConnStr, false))
            {
                var repository = new EomTool.Domain.Concrete.PaymentBatchRepository(context);
                foreach (var itemIDs in this.SelectedItemIdsAsInts)
                {
                    repository.SetPaymentBatchId(itemIDs, this.SelectedPaymentBatchId);
                }
                context.SaveChanges();
            }
            Refill();
        }

        int SelectedPaymentBatchId
        {
            get
            {
                var selectedRows = this.paymentBatchGrid.SelectedRows;
                var row = (selectedRows[0].DataBoundItem as DataRowView).Row as PaymentBatchesDataSet.PaymentBatchRow;
                return row.id;
            }
        }

        IEnumerable<string> SelectedItemIdsAsStrings
        {
            get
            {
                foreach (DataGridViewRow gridRow in this.publisherPaymentBatchesGrid.SelectedRows)
                {
                    var row = (gridRow.DataBoundItem as DataRowView).Row as PaymentBatchesDataSet.CampaignPublisherPaymentBatchesSummaryRow;
                    yield return row.ItemIds;
                }
            }
        }

        IEnumerable<int[]> SelectedItemIdsAsInts
        {
            get
            {
                return from c in this.SelectedItemIdsAsStrings.Select(c => c.Split(','))
                       select c.Select(s => int.Parse(s)).ToArray();
            }
        }

        void SaveButtonClicked(object sender, EventArgs e)
        {
            this.Validate();
            this.paymentBatchBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.paymentBatchesDataSet);
        }

        void GridSelectionChanged(object sender, EventArgs e)
        {
            this.assignButton.Enabled = this.CanAssign;
        }

        bool CanAssign
        {
            get
            {
                bool aSingleBatchIsSelected = this.paymentBatchGrid.SelectedRows.Count == 1;
                bool atLeastOnePublisherIsSelected = this.publisherPaymentBatchesGrid.SelectedRows.Count > 0;
                return aSingleBatchIsSelected && atLeastOnePublisherIsSelected;
            }
        }

        void AssignButtonEnabledChanged(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            button.BackColor = button.Enabled ? button.BackColor = Color.MediumSpringGreen : SystemColors.Control;
        }

        #region Old Code
#if COMPILE_OLD_CODE
        // Keeping this as reminder how to code ADO.NET transations
        static void AssignBatchId_old(int batchID, IEnumerable<string> itemIDStrings)
        {
            using (var connection = new SqlConnection(EomAppCommon.EomAppSettings.ConnStr))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                try
                {
                    foreach (var itemIDString in itemIDStrings)
                    {
                        command.CommandText = "UPDATE Item SET payment_batch_id = @batchID WHERE id in (" + itemIDString + ")";
                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@batchID", batchID));
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to update payment batches, rolling back." + ex.Message);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        if (!(exRollback is InvalidOperationException)) // connection closed or transaction already rolled back on the server.
                        {
                            MessageBox.Show("Failed to roll back. " + exRollback.Message);
                        }
                    }
                }
            }
        }
#endif
        #endregion
    }
}
