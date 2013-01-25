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
            this.affiliatePaymentMethodTableAdapter.Fill(this.paymentBatchesDataSet.AffiliatePaymentMethod);
            this.paymentBatchStateTableAdapter.Fill(this.paymentBatchesDataSet.PaymentBatchState);
        }

        void FillEntities()
        {
            this.paymentBatchAttachmentTableAdapter.Fill(this.paymentBatchesDataSet.PaymentBatchAttachment);
            this.paymentBatchTableAdapter.Fill(this.paymentBatchesDataSet.PaymentBatch);
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
            //if (campaignsToFinalizeGrid.Rows.Count > 0)
            //    foreach (var button in from row in campaignsToFinalizeGrid.Rows.Cast<DataGridViewRow>()
            //                           let am = ((row.DataBoundItem as DataRowView).Row as FinalizeDataSet1.CampaignRow).AM
            //                           where !Security.User.Current.CanFinalizeForAccountManager(am)
            //                           select (DataGridViewDisableButtonCell)row.Cells[FinalizeCol.Index])
            //        button.Enabled = false;

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
                        UrlToOpen = "http://localhost/EomToolWeb/PaymentBatches/Summary"
                    };
                    // TODO: put in settings...
                    string subject = "payment batches subject";
                    string from = "accounting@directagents.com";
                    string to = "@directagents.com";
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

        void AttachmentsSaveButtonClicked(object sender, EventArgs e)
        {
            this.Validate();
            this.fKPaymentBatchAttachmentPaymentBatchBindingSource.EndEdit();
            this.tableAdapterManager.PaymentBatchAttachmentTableAdapter.Update(this.paymentBatchesDataSet);
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

        void AddAttachmentClicked(object sender, EventArgs e)
        {
            Stream stream = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 3;
            openFileDialog.Multiselect = false; // TODO: support multiselect
            openFileDialog.RestoreDirectory = true;

            var paymentBatchAttachmentRow = paymentBatchesDataSet.PaymentBatchAttachment.NewPaymentBatchAttachmentRow();
            bool rowAdded = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((stream = openFileDialog.OpenFile()) != null)
                    {
                        using (stream)
                        {
                            byte[] fileBytes = ReadToEnd(stream);
                            using (var connection = new SqlConnection(EomAppCommon.EomAppSettings.ConnStr))
                            using (var command = new SqlCommand("InsertPaymentBatchAttachment", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                connection.Open();
                                try
                                {
                                    string name = openFileDialog.FileName.Split('\\').Last();
                                    int paymentBatchID = this.SelectedPaymentBatchId;

                                    command.Parameters.AddWithValue("@name", name);
                                    command.Parameters.AddWithValue("@binary_content", fileBytes);
                                    command.Parameters.AddWithValue("@payment_batch_id", paymentBatchID);
                                    SqlParameter identity = command.Parameters.Add("@Identity", SqlDbType.Int, 0, "CategoryID");
                                    identity.Direction = ParameterDirection.Output;
                                    command.ExecuteNonQuery();

                                    paymentBatchAttachmentRow.id = (int)identity.Value;
                                    paymentBatchAttachmentRow.name = name;
                                    paymentBatchAttachmentRow.payment_batch_id = paymentBatchID;

                                    rowAdded = true;
                                }
                                catch (Exception insertEx)
                                {
                                    MessageBox.Show("Error adding attachment. " + insertEx.Message);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            if (rowAdded)
            {
                paymentBatchesDataSet.PaymentBatchAttachment.AddPaymentBatchAttachmentRow(paymentBatchAttachmentRow);
                paymentBatchAttachmentRow.AcceptChanges();
            }
        }

        void AttachmentClicked(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var dataGridView = (DataGridView)sender;
                if (e.ColumnIndex == nameDataGridViewTextBoxColumn.Index)
                {
                    int attachmentID = (int)dataGridView1[idDataGridViewTextBoxColumn.Index, e.RowIndex].Value;
                    string attachmentName = (string)dataGridView1[nameDataGridViewTextBoxColumn.Index, e.RowIndex].Value;
                    this.OpenAttachment(attachmentID, attachmentName);
                }
            }
        }

        /// <summary>
        /// Opens the clicked attachment on the local client.
        /// </summary>
        void OpenAttachment(int attachmentID, string attachmentName)
        {
            using (var connection = new SqlConnection(EomAppCommon.EomAppSettings.ConnStr))
            using (var command = new SqlCommand("select binary_content from PaymentBatchAttachment where id=@id", connection))
            {
                connection.Open();
                try
                {
                    command.Parameters.AddWithValue("@id", attachmentID);
                    var fileBytes = (byte[])command.ExecuteScalar();

                    // Create a temp directory unique to the attachment id
                    DirectoryInfo di = new DirectoryInfo(System.IO.Path.GetTempPath())
                                            .CreateSubdirectory("PaymentBatchAttachments")
                                            .CreateSubdirectory(attachmentID.ToString());

                    // Write the binary content into the temp file
                    string filePath = di.FullName + "\\" + attachmentName;
                    File.WriteAllBytes(filePath, fileBytes);

                    // Launch the temp file in a new process
                    System.Diagnostics.Process.Start(filePath);
                }
                catch (Exception selectAttachmentEx)
                {
                    MessageBox.Show("Error: Could not read attachment content: " + selectAttachmentEx.Message);
                }
            }
        }

        static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;
            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }
            try
            {
                byte[] readBuffer = new byte[4096];
                int totalBytesRead = 0;
                int bytesRead;
                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;
                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }
                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
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
