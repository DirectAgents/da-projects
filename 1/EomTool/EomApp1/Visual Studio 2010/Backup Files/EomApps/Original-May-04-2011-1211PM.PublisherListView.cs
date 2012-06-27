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
namespace EomApp1.Formss.PubRep1.Controls
{
    public partial class PublisherListView : UserControl
    {
        private StopClock _perf = new StopClock();

        private DateTime _timeOfLastKeyPress;

        public event PublisherSelected PublisherSelected;

        public PublisherListView()
        {
            _perf.Mark("begin constructor");

            InitializeComponent();

            _perf.Mark("end constructor");
        }

        public void Initialize()
        {
            _perf.Mark("begin Form.Load");

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

            campaignsPublisherReportSummarySummaryByNetTermsTableAdapter.Fill(
                _dataSet.CampaignsPublisherReportSummarySummaryByNetTerms, "%");

            //publisherRepSummaryTotalsTableAdapter.Fill(_dataSet.PublisherRepSummaryTotals);

            dataGridView1.ClearSelection();

            _tableAdapter.FillByPublishersWithLineItemsOfVerifiedCampaigns(_dataSet.CampaignsPublisherReportSummary);

            _perf.Mark("end FillByPublishersWithLineItemsOfVerifiedCampaigns");

            foreach (var c in _dataSet.CampaignsPublisherReportSummary.Select(c => c.PayCurrency).Distinct())
            {
                _payCurrencyFilterListBox.Items.Add(c);
            }

            foreach (var c in _dataSet.CampaignsPublisherReportSummary.Select(c => c.NetTerms).Distinct())
            {
                _netTermsFilterListBox.Items.Add(c);
            }

            _perf.Mark("end Fill");
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
            if (ci > 2)
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

            campaignsPublisherReportSummarySummaryByNetTermsTableAdapter.Fill(
                _dataSet.CampaignsPublisherReportSummarySummaryByNetTerms, filter);

            dataGridView1.ClearSelection();
        }

        Filter<string> _filter = new Filter<string>();

        private void perfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _perf.Show();
        }

        // Filter
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        {
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
                (
                    lb.SelectedIndices
                        .Cast<int>()
                            .Select(c => lb.Items[c])
                )
                .Cast<string>()
                    .ToArray();
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

    public delegate void PublisherSelected(string publisher);
}
