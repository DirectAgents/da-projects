using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DGVColumnSelector;
using EomApp1.Formss.PubRep1.Data;
using System.Data.SqlClient;
using System.Drawing;
using EomApp1.Formss.PubRep1.Data.PublisherReportDataSet1TableAdapters;
using EomApp1.Formss.Common;
namespace EomApp1.Formss.PubRep1.Controls
{
    public partial class PublisherListView : UserControl
    {
        Filter<string> _filter = new Filter<string>();
        bool _payMode;
        bool _allCampaignsMode;
        StopClock _perf = new StopClock();
        DateTime _timeOfLastKeyPress;

        public event PublisherSelected PublisherSelected;
        public event PayModeChanged PayModeChanged;

        public PublisherListView()
        {
            _perf.Mark("begin constructor");
            InitializeComponent();
            _perf.Mark("end constructor");
        }

        public void Initialize()
        {
            _perf.Mark("begin Form.Load");

            //setStatusCheckColumn.Visible = false;
            //nextStatusComboColumn.Visible = false;
            //nextStatusComboColumn.ValueType = typeof(DataGridViewComboBoxCell);

            _itemsToChangeDGV.Visible = false;

            pendingStatusUpdates1.DataGridView = _itemsToChangeDGV;

            CampaignStatusCol.Visible = false;

            //_includeCampaignsDGV.Visible = false;
            splitContainer3.Panel2Collapsed = true;

            _paySaveButton.Visible = false;

            totalDataGridViewTextBoxColumn.Visible = checkBox2.Checked;

            netTermsCol.Visible = checkBox3.Checked;

            ShowUnShowStatusCols();

            _dgv.AllowUserToResizeRows = false;
            _dgv.MultiSelect = false;
            _dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _dgv.RowHeadersVisible = true;
            _dgv.KeyPress += new KeyPressEventHandler(dataGridView1_KeyPress);
            _timeOfLastKeyPress = DateTime.Now;
            _bindingSource.PositionChanged += new EventHandler(BindingSource_PositionChanged);

            Fill();

            DataGridViewColumnSelector cs = new DataGridViewColumnSelector();
            cs.DataGridView = _dgv;
            cs.MaxHeight = 400;
            cs.Width = 150;

            _perf.Mark("end Form.Load");
        }

        public void Fill()
        {
            _perf.Mark("begin Fill");
            _perf.Mark("begin FillByPublishersWithLineItemsOfVerifiedCampaigns");

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

            _perf.Mark("end FillByPublishersWithLineItemsOfVerifiedCampaigns");

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

            _perf.Mark("end Fill");
        }

        private void UpdateCellTags()
        {
            foreach (DataGridViewRow dataGridViewRow in _dgv.Rows)
            { 
                var row = (PublisherReportDataSet1.CampaignsPublisherReportSummaryRow)
                    ((DataRowView)dataGridViewRow.DataBoundItem).Row;
                
                if (row.Unverified > 0)
                {
                    _dgv[unverifiedColumn.Index, dataGridViewRow.Index].Tag = new Action(() =>
                    {
                        pendingStatusUpdates1.Add(row.Publisher, "Unverified", "Verified", row.Unverified);
                    });
                }
                if (row.Verified > 0)
                {
                    _dgv[verifiedColumn.Index, dataGridViewRow.Index].Tag = new Action(() =>
                    {
                        pendingStatusUpdates1.Add(row.Publisher, "Verified", "Approved", row.Verified);
                    });
                }
                if (row.Approved > 0)
                {
                    _dgv[approvedColumn.Index, dataGridViewRow.Index].Tag = new Action(() =>
                    {
                        pendingStatusUpdates1.Add(row.Publisher, "Approved", "Paid", row.Approved);
                    });
                }
            }
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

        void BindingSource_PositionChanged(object sender, EventArgs e)
        {
            var drv = ((DataRowView)_bindingSource.Current).Row;
            var r = (PublisherReportDataSet1.CampaignsPublisherReportSummaryRow)drv;
            var p = r.Publisher;
            PublisherSelected(p);
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
            var lb = (ListBox)sender;
            string[] selected = lb.GetSelectedStrings();
            _filter.Add((string)lb.Tag, selected);
            _bindingSource.Filter = _filter.ToString();

            string filter = (selected.Length != 1 ? "%" : selected[0]);

            //campaignsPublisherReportSummarySummaryByNetTermsTableAdapter.Fill(
            //    _dataSet.CampaignsPublisherReportSummarySummaryByNetTerms, filter);

            if (_allCampaignsMode)
            {
                campaignsPublisherReportSummarySummaryByNetTermsTableAdapter.FillByAllRevenue(_dataSet.CampaignsPublisherReportSummarySummaryByNetTerms, filter);
            }
            else
            {
                campaignsPublisherReportSummarySummaryByNetTermsTableAdapter.Fill(_dataSet.CampaignsPublisherReportSummarySummaryByNetTerms, filter);
            }

            UpdateCellTags(); // TODO: move this call to an event that occurs when the table adapter is filled?
            // NOTE: if using DI container, could contral the table adapter class, and put an override of Fill?

            _summaryDGV.ClearSelection();
        }

        // Enter/Leave pay mode
        private void _payModeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _payMode = _payModeCheckBox.Checked;

            if (_payMode)
            {
                _paySaveButton.Enabled = false;

                //setStatusCheckColumn.Visible = true;
                //nextStatusComboColumn.Visible = true;

                _dgv.ReadOnly = false;

                foreach (DataGridViewColumn item in _dgv.Columns)
                {
                    if (!object.Equals(item, nextStatusComboColumn))
                    {
                        item.ReadOnly = true;
                    }
                }

                _dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;

                
                
                //unverifiedLinkColumn.Visible = true;
                //unverifiedTextColumn.Visible = false;
                //unverifiedLinkColumn.LinkBehavior = LinkBehavior.NeverUnderline;
                //unverifiedLinkColumn.LinkColor = Color.Black;
                //unverifiedLinkColumn.ActiveLinkColor = Color.Black;
                //unverifiedLinkColumn.TrackVisitedState = false;
            }
            else
            {

                //setStatusCheckColumn.Visible = false;
                //nextStatusComboColumn.Visible = false;

                //DataGridViewCellStyle st = new DataGridViewCellStyle();
                //st.BackColor = Color.Gray;
                //if (!nextStatusComboColumn.HasDefaultCellStyle)
                //{
                //    nextStatusComboColumn.DefaultCellStyle = new DataGridViewCellStyle(st);
                //}
                //nextStatusComboColumn.DefaultCellStyle.ApplyStyle(st);

                //unverifiedLinkColumn.Visible = false;
                //unverifiedTextColumn.Visible = true;
            }

            _itemsToChangeDGV.Visible = _payMode;
            splitContainer2.Panel1Collapsed = _payMode;
            _paySaveButton.Visible = _payMode;
            
            //_includeCampaignsDGV.Visible = false;
            splitContainer3.Panel2Collapsed = true;

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
                //int? currentValue = dataGridView[columnIndex, rowIndex].Value as int?;
                //int? newValue = currentValue == null ? (int?)1 : null;
                //dataGridView[columnIndex, rowIndex].Value = newValue;

                //if (newValue == null)
                //{
                //    // TODO: support unchecking a pending update item
                //    MessageBox.Show("TODO: support unchecking a pending update item");
                //}
            //    else if (newValue == 1)
            //    {
            //        // Fill in the cell with a drop down list of choices
            //        // based on the available status transitions

            //        // Data Table to bind to the drop down that goes into the DGV cell
            //        //var comboBoxDataSourceTable = new DataTable("NextStatusTable");
            //        //var dataColumn = comboBoxDataSourceTable.Columns.Add("name", typeof(string));

            //        // DGV row that was clicked
            //        var row = 
            //            (PublisherReportDataSet1.CampaignsPublisherReportSummaryRow)((DataRowView)_bindingSource[rowIndex]).Row;

            //        // Populate rows of drop down
            //        //bool addedDefaultAction = false;

            //        if (row.Unverified > 0)
            //        {
            //            //comboBoxDataSourceTable.Rows.Add("Verify " + FormatCurrency(row.PayCurrency, row.Unverified));
            //            //if (!addedDefaultAction)
            //            //{
            //            //    pendingStatusUpdates1.Add(row.Publisher, "Unverified", "Verified", row.Unverified);
            //            //    addedDefaultAction = true;
            //            //}
            //            dataGridView[unverifiedColumn.Index, rowIndex].Tag = new Action(() =>
            //            {
            //                pendingStatusUpdates1.Add(row.Publisher, "Unverified", "Verified", row.Unverified);
            //            });
            //        }
            //        if (row.Verified > 0)
            //        {
            //            //comboBoxDataSourceTable.Rows.Add("Approve " + FormatCurrency(row.PayCurrency, row.Verified));
            //            //if (!addedDefaultAction)
            //            //{
            //            //    pendingStatusUpdates1.Add(row.Publisher, "Verified", "Approved", row.Verified);
            //            //    addedDefaultAction = true;
            //            //}
            //            dataGridView[verifiedColumn.Index, rowIndex].Tag = new Action(() =>
            //            {
            //                pendingStatusUpdates1.Add(row.Publisher, "Verified", "Approved", row.Verified);
            //            });
            //        }
            //        if (row.Approved > 0)
            //        {
            //            //comboBoxDataSourceTable.Rows.Add("Pay " + FormatCurrency(row.PayCurrency, row.Approved));
            //            //if (!addedDefaultAction)
            //            //{
            //            //    pendingStatusUpdates1.Add(row.Publisher, "Approved", "Paid", row.Approved);
            //            //    addedDefaultAction = true;
            //            //}
            //            dataGridView[approvedColumn.Index, rowIndex].Tag = new Action(() =>
            //            {
            //                pendingStatusUpdates1.Add(row.Publisher, "Approved", "Paid", row.Approved);
            //            });
            //        }

            //        // Always give the option to put payment on Hold
            //        //comboBoxDataSourceTable.Rows.Add("Hold");

            //        // Create the combo box cell and place it in the DGV cell
            //        //var dataGridViewComboBoxCell = new DataGridViewComboBoxCell();
            //        //dataGridViewComboBoxCell.ValueMember = "name";
            //        //dataGridViewComboBoxCell.DisplayMember = "name";
            //        //dataGridViewComboBoxCell.DataSource = comboBoxDataSourceTable;
            //        //dataGridViewComboBoxCell.Value = comboBoxDataSourceTable.Rows[0].Field<string>("name");
            //        //dataGridViewComboBoxCell.FlatStyle = FlatStyle.Flat;

            //        // Push the combo box cell into the DataGridView
            //        //dataGridView[columnIndex + 1, rowIndex] = dataGridViewComboBoxCell;
            //    }
            }
            else if (e.ColumnIndex == nextStatusComboColumn.Index)
            {
                // TODO!: handle the case where the combo box drop down changes
                MessageBox.Show("TODO: support changing drop down value");
            }
            else if (e.ColumnIndex == approvedColumn.Index
                || e.ColumnIndex == unverifiedColumn.Index
                || e.ColumnIndex == verifiedColumn.Index)
            {
                object tag = dataGridView[columnIndex, rowIndex].Tag;
                if (tag is Action)
                {
                    ((Action)tag)();
                }
            }
        }

        // Save pending changes
        private void button1_Click(object sender, EventArgs e)
        {
            pendingStatusUpdates1.Save(string.Join(",", ExcludedItemIDs));
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
                if (splitContainer3.Panel2Collapsed != false)
                {
                    splitContainer3.Panel2Collapsed = false;
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

        private void ShowUnShowStatusCols()
        {
            bool b = checkBox1.Checked;
            unverifiedColumn.Visible = b;
            verifiedColumn.Visible = b;
            approvedColumn.Visible = b;
            toBePaidDataGridViewTextBoxColumn.Visible = !b;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            totalDataGridViewTextBoxColumn.Visible = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            netTermsCol.Visible = checkBox3.Checked;
        }
    }

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

    public delegate void PublisherSelected(string publisher);
    public delegate void PayModeChanged(bool payMode);

    
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

    public class WatchableBool
    {
        private bool _value;

        public WatchableBool(bool b)
        {
            _value = b;
        }

        static public implicit operator bool(WatchableBool wb)
        {
            return wb._value;
        }
    }
}
