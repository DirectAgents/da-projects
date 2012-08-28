using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EomApp1.Screens.PubRep1.Data;
using EomApp1.Screens.PubRep1.Data.PublisherReportDataSet1TableAdapters;
using EomAppControls;
using Eom.Common;

namespace EomApp1.Screens.PubRep1.Controls
{
    public partial class PublisherListView : UserControl
    {
        Filter<string> _filter = new Filter<string>();
        bool _payMode;
        bool _allCampaignsMode;
        StopClock _stopClock = new StopClock();
        DateTime _timeOfLastKeyPress;
        public event PublisherSelected PublisherSelected;
        public event PayModeChanged PayModeChanged;

        enum StatusChangeMode
        {
            Promote,
            Demote
        }

        private Color LinkColor(StatusChangeMode mode)
        {
            return mode == StatusChangeMode.Promote ? Color.Blue : Color.Red;
        }

        // Status mode is based on the checked state of the Undo check box
        private StatusChangeMode StatusMode
        {
            get { return this.undoCheckBox.Checked ? StatusChangeMode.Demote : StatusChangeMode.Promote; }
        }

        public PublisherListView()
        {
            _stopClock.Mark("begin constructor");
            InitializeComponent();
            _stopClock.Mark("end constructor");
        }

        public void Initialize()
        {
            _stopClock.Mark("begin Form.Load");

            // Hide the unit of work view
            _itemsToChangeDGV.Visible = false;

            // Give the pending status updates component the datagridview so it can clear the selection when it needs to
            pendingStatusUpdates.DataGridView = _itemsToChangeDGV;

            // Hide the campaign status column (TODO: make sure this is the campaign ITEM status)
            CampaignStatusCol.Visible = false;

            // Hide the right half of the top vertical splitter
            _topVerticalSplitContainer.Panel2Collapsed = true;

            // Hide the save button
            _paySaveButton.Visible = false;

            // Hide the Undo check box
            undoCheckBox.Visible = false;

            // Visibility of Total column reflects state of Total checkbox
            totalDataGridViewTextBoxColumn.Visible = _totalCheckBox.Checked;

            // Visibility of net terms column reflects state of net terms check box
            netTermsCol.Visible = netTermsCheckBox.Checked;

            ShowUnShowStatusCols();

            // Set some misc grid properties
            _dgv.AllowUserToResizeRows = false;
            _dgv.MultiSelect = false;
            _dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _dgv.RowHeadersVisible = true;
            _dgv.KeyPress += new KeyPressEventHandler(dataGridView1_KeyPress);

            // Initialize the time of the last key press
            _timeOfLastKeyPress = DateTime.Now;

            // Hook up to binding source position change event
            _bindingSource.PositionChanged += new EventHandler(BindingSource_PositionChanged);

            Fill();

            var cs = new DataGridViewColumnSelector();
            cs.DataGridView = _dgv;
            cs.MaxHeight = 400;
            cs.Width = 150;

            _stopClock.Mark("end Form.Load");
        }

        // The status columns reflect the state of the status check box
        private void ShowUnShowStatusCols()
        {
            bool statusCheckBoxChecked = statusCheckBox.Checked;
            unverifiedColumn.Visible = statusCheckBoxChecked;
            verifiedColumn.Visible = statusCheckBoxChecked;
            approvedColumn.Visible = statusCheckBoxChecked;

            // The to be paid column is visible when the status columns are not and vice versa
            toBePaidDataGridViewTextBoxColumn.Visible = !statusCheckBoxChecked;
        }

        void BindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (_bindingSource.Current != null)
            {
                var dataRow = ((DataRowView)_bindingSource.Current).Row;
                var row = (PublisherReportDataSet1.CampaignsPublisherReportSummaryRow)dataRow;
                var publisher = row.Publisher;
                if (PublisherSelected != null)
                {
                    PublisherSelected(publisher);
                }
            }
        }

        public void Fill()
        {
            _stopClock.Mark("begin Fill");
            _stopClock.Mark("begin FillByPublishersWithLineItemsOfVerifiedCampaigns");

            if (_allCampaignsMode)
            {
                campaignsPublisherReportSummarySummaryByNetTermsTableAdapter.FillByAllRevenue(_dataSet.CampaignsPublisherReportSummarySummaryByNetTerms, "%");
            }
            else
            {
                campaignsPublisherReportSummarySummaryByNetTermsTableAdapter.Fill(_dataSet.CampaignsPublisherReportSummarySummaryByNetTerms, "%");
            }

            _summaryDGV.ClearSelection();

            // only fetch rows representing publishers of verified campaigns
            _tableAdapter.FillByPublishersWithLineItemsOfVerifiedCampaigns(_dataSet.CampaignsPublisherReportSummary);

            _stopClock.Mark("end FillByPublishersWithLineItemsOfVerifiedCampaigns");

            // populate the currency filter list box
            foreach (var c in _dataSet.CampaignsPublisherReportSummary.Select(c => c.PayCurrency).Distinct())
            {
                _payCurrencyFilterListBox.Items.Add(c);
            }

            // populate the net terms filter list box
            foreach (var c in _dataSet.CampaignsPublisherReportSummary.Select(c => c.NetTerms).Distinct())
            {
                _netTermsFilterListBox.Items.Add(c);
            }

            UpdateCellTags();

            _stopClock.Mark("end Fill");
        }

        private void UpdateCellTags()
        {
            bool canVerify = Security.User.Current.CanDoAccountingVerify;
            bool canApprove = Security.User.Current.CanDoAccountingApprove;
            bool canPay = Security.User.Current.CanDoAccountingPay;
            bool canUnverify = canVerify;
            bool canUnapprove = canApprove;
            bool canUnpay = canPay;
            bool promoting = (this.StatusMode == StatusChangeMode.Promote);
            bool demoting = (this.StatusMode == StatusChangeMode.Demote);

            verifiedColumn.LinkColor = LinkColor(this.StatusMode);
            approvedColumn.LinkColor = LinkColor(this.StatusMode);

            // Loop through all the rows
            foreach (DataGridViewRow dataGridViewRow in _dgv.Rows)
            {
                var row = (PublisherReportDataSet1.CampaignsPublisherReportSummaryRow)((DataRowView)dataGridViewRow.DataBoundItem).Row;
                var unverifiedCell = _dgv[unverifiedColumn.Index, dataGridViewRow.Index];
                var verifiedCell = _dgv[verifiedColumn.Index, dataGridViewRow.Index];
                var approvedCell = _dgv[approvedColumn.Index, dataGridViewRow.Index];
                var paidCell = _dgv[paidColumn.Index, dataGridViewRow.Index];

                // Verify
                if (row.Unverified > 0 && promoting && canVerify)
                    unverifiedCell.Tag = new Action(() => pendingStatusUpdates.Add(row.Publisher, "Unverified", "Verified", row.Unverified));

                // Approve/Unverify
                if (row.Verified > 0)
                {
                    string fromStatus = "Verified";
                    string toStatus = (promoting && canApprove) ? "Approved" : (demoting && canUnverify) ? "Unverified" : null;
                    if (toStatus != null)
                        verifiedCell.Tag = new Action(() => pendingStatusUpdates.Add(row.Publisher, fromStatus, toStatus, row.Verified));
                }

                // Pay/Unapprove
                if (row.Approved > 0)
                {
                    string fromStatus = "Approved";
                    string toStatus = (promoting && canPay) ? "Paid" : (demoting && canUnpay) ? "Verified" : null;
                    if (toStatus != null)
                        approvedCell.Tag = new Action(() => pendingStatusUpdates.Add(row.Publisher, fromStatus, toStatus, row.Approved));
                }

                // Unpay
                if (row.Paid > 0 && demoting && canUnpay)
                {
                    paidCell = EnableLinkCell(paidColumn.Index, dataGridViewRow.Index);
                    paidCell.Tag = new Action(() => pendingStatusUpdates.Add(row.Publisher, "Paid", "Approved", row.Paid));
                }

                // Unlink cells without actions
                if (unverifiedCell.Tag == null)
                    DisableLinkCell(unverifiedColumn.Index, dataGridViewRow.Index);
                if (verifiedCell.Tag == null)
                    DisableLinkCell(verifiedColumn.Index, dataGridViewRow.Index);
                if (approvedCell.Tag == null)
                    DisableLinkCell(approvedColumn.Index, dataGridViewRow.Index);
            }
        }

        private void DisableLinkCell(int colIndex, int rowIndex)
        {
            var textCell = new DataGridViewTextBoxCell()
            {
                Value = _dgv[colIndex, rowIndex].Value,
            };
            _dgv[colIndex, rowIndex] = textCell;
        }

        private DataGridViewLinkCell EnableLinkCell(int colIndex, int rowIndex)
        {
            var linkCell = new DataGridViewLinkCell()
            {
                Value = _dgv[colIndex, rowIndex].Value,
                LinkColor = LinkColor(this.StatusMode),
                LinkBehavior = LinkBehavior.NeverUnderline
            };
            _dgv[colIndex, rowIndex] = linkCell;
            return linkCell;
        }

        public void ReFill()
        {
            if (_allCampaignsMode)
            {
                campaignsPublisherReportSummarySummaryByNetTermsTableAdapter.FillByAllRevenue(_dataSet.CampaignsPublisherReportSummarySummaryByNetTerms, "%");
            }
            else
            {
                campaignsPublisherReportSummarySummaryByNetTermsTableAdapter.Fill(_dataSet.CampaignsPublisherReportSummarySummaryByNetTerms, "%");
            }

            if (_allCampaignsMode)
            {
                CampaignStatusCol.Visible = true;
                _tableAdapter.FillByPublishersWithLineItemsOfVerifiedCampaigns(_dataSet.CampaignsPublisherReportSummary);
                _tableAdapter.ClearBeforeFill = false;
                _tableAdapter.FillByPublishersWithLineItemsOfNotVerifiedCampaigns(_dataSet.CampaignsPublisherReportSummary);
                _tableAdapter.ClearBeforeFill = true;
            }
            else
            {
                CampaignStatusCol.Visible = false;
                _tableAdapter.FillByPublishersWithLineItemsOfVerifiedCampaigns(_dataSet.CampaignsPublisherReportSummary);
            }

            UpdateCellTags();

            _summaryDGV.ClearSelection();
        }

        void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char key = e.KeyChar;

            if (char.IsLetter(key) || char.IsNumber(key) || key == 32)
            {
                DateTime now = DateTime.Now;

                if (now - _timeOfLastKeyPress > TimeSpan.FromMilliseconds(1000))
                {
                    publisherFilterTextBox.Text = e.KeyChar.ToString();

                    _timeOfLastKeyPress = now;
                }
                else
                {
                    publisherFilterTextBox.AppendText(e.KeyChar.ToString());
                }
            }

            e.Handled = true;
        }

        void publisherFilterTextBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = sender as ToolStripTextBox;
            string text = textBox.Text;
            bool isEmpty = text.Length == 0;
            var currentColor = publisherFilterTextBox.BackColor;
            publisherFilterClearToolStripMenuItem.Visible = !isEmpty;
            _bindingSource.Filter = "Publisher LIKE '%" + textBox.Text + "%'";
            UpdateCellTags();
        }

        void publisherFilterClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            publisherFilterTextBox.Clear();
        }

        string FormatCurrency(string currency, decimal amount)
        {
            var map = new Dictionary<string, string>
            {
                {"USD", "en-us"},
                {"GBP", "en-gb"},
                {"EUR", "de-de"},
                {"AUD", "en-AU"}
            };
            return string.Format(CultureInfo.CreateSpecificCulture(map[currency]), "{0:C}", amount);
        }

        void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int ci = e.ColumnIndex;
            int ri = e.RowIndex;
            if (ci > 5)
            {
                if ((decimal)_dgv[ci, ri].Value == 0.0m)
                {
                    e.Value = "-";
                }
                else
                {
                    string curr = (string)_dgv["PayCurrencyCol", ri].Value;
                    e.Value = FormatCurrency(curr, (decimal)_dgv[ci, ri].Value);
                }

                e.FormattingApplied = true;
            }
        }

        void listBox1_SelectedIndexChanged_currency(object sender, EventArgs e)
        {
            var lb = (ListBox)sender;
            _filter.Add((string)lb.Tag, lb.GetSelectedStrings());
            _bindingSource.Filter = _filter.ToString();
        }

        private void listBox1_SelectedIndexChanged_netterms(object sender, EventArgs e)
        {
            var listBox = (ListBox)sender;
            string[] selectedStrings = listBox.GetSelectedStrings();
            _filter.Add((string)listBox.Tag, selectedStrings);
            _bindingSource.Filter = _filter.ToString();
            string filter = (selectedStrings.Length != 1 ? "%" : selectedStrings[0]);
            if (_allCampaignsMode)
            {
                campaignsPublisherReportSummarySummaryByNetTermsTableAdapter.FillByAllRevenue(_dataSet.CampaignsPublisherReportSummarySummaryByNetTerms, filter);
            }
            else
            {
                campaignsPublisherReportSummarySummaryByNetTermsTableAdapter.Fill(_dataSet.CampaignsPublisherReportSummarySummaryByNetTerms, filter);
            }
            UpdateCellTags();
            _summaryDGV.ClearSelection();
        }

        // Enter/Leave pay mode
        private void _payModeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _payMode = _payModeCheckBox.Checked;

            if (_payMode)
            {
                //_paySaveButton.Enabled = false;

                _dgv.ReadOnly = false;

                foreach (DataGridViewColumn item in _dgv.Columns)
                {
                    if (!object.Equals(item, nextStatusComboColumn))
                    {
                        item.ReadOnly = true;
                    }
                }

                _dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            }

            _itemsToChangeDGV.Visible = _payMode;
            splitContainer2.Panel1Collapsed = _payMode;
            _paySaveButton.Visible = _payMode;
            undoCheckBox.Visible = _payMode;

            //_includeCampaignsDGV.Visible = false;
            _topVerticalSplitContainer.Panel2Collapsed = true;

            //_bindingSource.ResetBindings(false);

            PayModeChanged(_payMode);
        }

        private void _dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var dataGridView = (DataGridView)sender;
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            if (e.ColumnIndex == setStatusCheckColumn.Index) // Pay check box  CHECKED or UNCHECKED
            {
                // used to be a bunch of commented out lines of code...
            }
            else if (e.ColumnIndex == nextStatusComboColumn.Index)
            {
                // TODO!: handle the case where the combo box drop down changes
                MessageBox.Show("TODO: support changing drop down value");
            }
            else if (e.ColumnIndex == approvedColumn.Index
                || e.ColumnIndex == unverifiedColumn.Index
                || e.ColumnIndex == verifiedColumn.Index
                || e.ColumnIndex == paidColumn.Index)
            {
                var action = dataGridView[columnIndex, rowIndex].Tag as Action;
                if (action != null)
                    action();
            }
        }

        // Save pending changes
        private void button1_Click(object sender, EventArgs e)
        {
            pendingStatusUpdates.Save(string.Join(",", ExcludedItemIDs));
            _paySaveButton.Enabled = false;
            ReFill();
        }

        // Yield the item ids for unchecked rows of _includeCampaignsDGV
        private IEnumerable<string> ExcludedItemIDs
        {
            get
            {
                for (int i = 0; i < _includeCampaignsDGV.Rows.Count; i++)
                {
                    if ((string)_includeCampaignsDGV[0, i].Value == "no")
                    {
                        yield return (string)_includeCampaignsDGV[2, i].Value;
                    }
                }
            }
        }

        // Toggle between showing items derived from verified campaigns
        private void _allRevenueCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _allCampaignsMode = _allRevenueCheckBox.Checked;
            ReFill();
        }

        // pending updates DGV cell click
        private void _itemsToChangeDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == ItemIDs.Index) // The link column containing item IDs has been clicked
            {
                if (_topVerticalSplitContainer.Panel2Collapsed != false)
                {
                    _topVerticalSplitContainer.Panel2Collapsed = false;
                }
                var dt = new DataTable("CampaignsForItems");
                var included = dt.Columns.Add("Included");
                var campaign = dt.Columns.Add("Campaign");
                var itemIDs = dt.Columns.Add("ItemIDs");
                var o = (string)_itemsToChangeDGV[e.ColumnIndex, e.RowIndex].Value;
                var queries = new QueriesTableAdapter();
                var map = new Dictionary<string, DataRow>();
                foreach (var item in o.Split(','))
                {
                    var campaignName = queries.CampaignNameFromItemId(Convert.ToInt32(item));
                    if (!map.ContainsKey(campaignName))
                    {
                        map[campaignName] = dt.NewRow();
                        map[campaignName].SetField<bool>("Included", true);
                        map[campaignName].SetField<string>("Campaign", campaignName);
                        map[campaignName].SetField<string>("ItemIDs", item);
                        dt.Rows.Add(map[campaignName]);
                    }
                    else
                    {
                        var cur = map[campaignName].Field<string>("ItemIDs");
                        map[campaignName].SetField<string>("ItemIDs", cur + "," + item);
                    }
                }
                _includeCampaignsDGV.DataSource = dt;
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReFill();
        }

        private void _itemsToChangeDGV_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            _paySaveButton.Enabled = true;
        }

        // To Be Paid
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ShowUnShowStatusCols();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            totalDataGridViewTextBoxColumn.Visible = _totalCheckBox.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            netTermsCol.Visible = netTermsCheckBox.Checked;
        }

        private void undoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ReFill();
        }
    }

    public delegate void PublisherSelected(string publisher);
    public delegate void PayModeChanged(bool payMode);

    public class Filter<T> : Dictionary<T, T[]>
    {
        public new void Add(T k, T[] v)
        {
            if (v.Length > 0)
            {
                this[k] = v;
            }
            else
            {
                Remove(k);
            }
        }
        public override string ToString()
        {
            var x = new List<string>();
            foreach (var i in Keys)
            {
                var y = new List<string>();
                foreach (var j in this[i])
                {
                    y.Add(string.Format("{0}='{1}'", i, j));
                }
                x.Add(string.Join(" OR ", y));
            }
            return string.Join(" AND ", x);
        }
    }

    static public class MyExt
    {
        public static string[] GetSelectedStrings(this ListBox lb)
        {
            return
                (lb.SelectedIndices.Cast<int>()
                    .Select(c => lb.Items[c]))
                        .Cast<string>().ToArray();
        }
    }

    class StopClock
    {
        class Stop
        {
            public string Name { get; set; }
            public DateTime When { get; set; }
        }
        private List<Stop> _stops = new List<Stop>();
        public void Mark(string name)
        {
            var stop = new Stop();
            stop.Name = name;
            stop.When = DateTime.Now;
            _stops.Add(stop);
        }
        public void Show()
        {
            var sb = new StringBuilder();
            Form f = new Form
            {
                Width = 800,
                Height = 400
            };
            ListBox lb = new ListBox();
            lb.Dock = DockStyle.Fill;
            f.Controls.Add(lb);
            for (int i = 0; i < _stops.Count; i++)
            {
                var s = string.Format(
                    "[{0} Start: {1} Stop: {2} Elapsed: {3}]",
                    _stops[i].Name,
                    (i > 0) ? _stops[i - 1].When.ToString() : "-",
                    _stops[i].When,
                    (i > 0) ? (_stops[i].When - _stops[i - 1].When).ToString() : "-");
                lb.Items.Add(s);
            }
            f.ShowDialog();
        }
    }
}
