using System;
using System.Windows.Forms;
using EomApp1.Components;
using EomApp1.Events;
using System.Drawing;
using System.Collections;
using System.Dynamic;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using EomApp1.Formss.Accounting.Forms;
using EomApp1.Formss.Accounting.Data;
using System.Data;
using System.Collections.Generic;

namespace EomApp1.Formss.Accounting.Controls
{
    public partial class AccountingSheet : UserControl, EomApp1.Formss.Accounting.Controls.IFormOverData
    {
        public AccountingSheet()
        {
            InitializeComponent();
            if (!DesignMode) Load += new EventHandler(AccountingSheet_Load);
        }

        public void Fill()
        {
            _monitorCellChange = false;
            _tableAdapterManager.PROC_SELECT_Eddy_Accounting_View_2TableAdapter.Fill(eddyAccountingView.PROC_SELECT_Eddy_Accounting_View_2);
            _monitorCellChange = true;
        }

        public void Save()
        {
            this.Validate();
            this._bindingSource.EndEdit();
            var db = new AccountingDataClassesDataContext(true);
            HashSet<int> itemIds = new HashSet<int>();
            foreach (EomApp1.Formss.Accounting.Data.EddyAccountingView.PROC_SELECT_Eddy_Accounting_View_2Row row
                in eddyAccountingView.GetChanges(DataRowState.Modified).Tables[0].Rows)
            {
                decimal origCost = (decimal)row["Cost/Unit", DataRowVersion.Original];
                decimal origRev = (decimal)row["Rev/Unit", DataRowVersion.Original];
                decimal curCost = row._Cost_Unit;
                decimal curRev = row._Rev_Unit;
                var itemsToUpdate =
                    from c in db.Items
                    where c.Affiliate.name2 == (string)row["Publisher", DataRowVersion.Original]
                    && c.cost_per_unit == origCost
                    && c.revenue_per_unit == origRev
                    select c;
                foreach (var item in itemsToUpdate)
                {
                    int itemID = item.id;
                    if (itemIds.Contains(itemID))
                    {
                        throw new Exception("One or more changes cause the same Item to change - try pressing save between edits");
                    }
                    else
                    {
                        itemIds.Add(item.id);
                    }
                    if (origCost != curCost)
                        item.cost_per_unit = curCost;
                    if (origRev != curRev)
                        item.revenue_per_unit = curRev;
                }
            }
            db.SubmitChanges();
            ReFill();
        }

        // Save the changes via a stored procedure that updates the individual items
        void Save(object sender, EventArgs e)
        {
            Save();
        }

        public void ReFill()
        {
            Fill();
        }

        void AccountingSheet_Load(object sender, EventArgs e)
        {
            InitGridColumnWidths();
            InitTypingTracker();
        }

        // Auto filter list on keypress.
        void InitTypingTracker()
        {
            _typingTracker1 = new TypingTracker();
            _typingTracker1.Tracking += new EventHandler(typingTracker1_Tracking);
            _typingTracker1.BindControl(pROC_SELECT_Eddy_Accounting_View_2DataGridView);
        }

        // Whatever the typing tracker gives us we make that the current filter.
        void typingTracker1_Tracking(object sender, EventArgs e)
        {
            Filter((e as TextEventArgs).Text);
        }

        void Filter(string text)
        {
            _bindingSource.Filter = "Publisher like '%{0}%'".xFormat(text.ForNullOrWhiteSpace("*"));
        }

        TypingTracker _typingTracker1;

        // Provide visual feedback when a cell changes.
        bool _monitorCellChange = false;

        // Save the column widths on per-user basis.
        bool _watchingColWidths;

        void InitGridColumnWidths()
        {
            var xd = this.UserColWidths;
            if (xd == null)
            {
                // initialize all column widths to 75
                foreach (DataGridViewColumn column in pROC_SELECT_Eddy_Accounting_View_2DataGridView.Columns)
                {
                    column.Width = 75;
                }
            }
            else
            {
                var cols = xd.Elements("col");
                foreach (DataGridViewColumn column in pROC_SELECT_Eddy_Accounting_View_2DataGridView.Columns)
                {
                    string val = cols.First(c => c.Attribute("name").Value == column.DataPropertyName).Attribute("val").Value;
                    column.Width = Convert.ToInt32(val);
                }
            }

            // TODO: watch for column width changes
        }

        // Change the color of the cell when it is edited (visual feedback)
        void Grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (_monitorCellChange)
            //{
            //    var d = (DataGridView)sender;
            //    var c = d[e.ColumnIndex, e.RowIndex];
            //    c.Style.BackColor = Color.IndianRed;
            //}
        }

        //private void RevPerUnitTextBox_Enter(object sender, EventArgs e)
        //{
        //    var textBox = new xControl<TextBox>(sender);
        //    textBox.Tags.StartText = textBox.Value.Text;
        //}

        void Grid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (_watchingColWidths)
            {
                // create a new xdoc to store column widths in on every change
                // as opposed to updated the current one (simpler, hopefully neg. perf. impact)
                var xd = new XDocument();
                var colWidths = new XElement("colwidths");
                xd.Add(colWidths);
                foreach (DataGridViewColumn dcol in pROC_SELECT_Eddy_Accounting_View_2DataGridView.Columns)
                {
                    colWidths.Add(new XElement("col", new XAttribute("name", dcol.DataPropertyName), new XAttribute("val", dcol.Width)));
                }
                this.UserColWidths = xd;
            }
        }

        XDocument UserColWidths
        {
            get
            {
                var result = Properties.Settings.Default.AccountingSheetSettingsString;
                return result.IsNullOrWhitespace() ? null : XDocument.Parse(result);
            }
            set
            {
                Properties.Settings.Default.AccountingSheetSettingsString = value.ToString();
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Open dialog to edit number of units on individual items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (sender as DataGridView);
            if (e.ColumnIndex == UnitsColumn.Index)
            {
                var unitCountsForm = new UnitCountsForm(
                    campaign_NumberTextBox.Auto(),
                    publisherTextBox.Auto(),
                    rev_UnitTextBox.Auto(),
                    cost_UnitTextBox.Auto());
                unitCountsForm.ShowDialog(this);
                ReFill();
            }
        }

        private void pROC_SELECT_Eddy_Accounting_View_2DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_monitorCellChange)
            {
                var d = (DataGridView)sender;
                var c = d[e.ColumnIndex, e.RowIndex];
                c.Style.BackColor = Color.IndianRed;
            }
        }

        //public Func<XDocument> GetUserColWidths { get; set; }
    }

    //public class xControl<T> where T : class 
    //{
    //    public xControl(object sender)
    //    {
    //        Value = sender as T;
    //    }
    //    public T Value { get; set; }
    //    public dynamic Tags = new xConf(new Hashtable());
    //    //public static implicit operator T(Ctl<T> value) 
    //    //{
    //    //    return value as T;
    //    //}
    //}

    public class MyTextBox
    {
        private TextBox _textBox;
        public MyTextBox(TextBox t)
        {
            _textBox = t;
        }
        public static implicit operator string(MyTextBox o)
        {
            return o._textBox.Text;
        }
        public static implicit operator int(MyTextBox o)
        {
            return Convert.ToInt32(o._textBox.Text);
        }
        public static implicit operator decimal(MyTextBox o)
        {
            return Convert.ToDecimal(o._textBox.Text);
        }
    }

    public class xConf : DynamicObject
    {
        public xConf(Hashtable h)
        {
            ht = h;
        }
        Hashtable ht = new Hashtable();
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = ht[binder.Name];
            return true;
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            ht[binder.Name] = value;
            return true;
        }
    }

    public static class MyExt
    {
        //public static Conf Tags(this TextBox t)
        //{
        //    if (t.Tag == null)
        //    {
        //        t.Tag = new xConf(new Hashtable());
        //    }
        //    return (Conf)t.Tag;
        //}
        public static string xFormat(this string s, params string[] args)
        {
            return string.Format(s, args);
        }
        public static bool IsNullOrWhitespace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }
        public static Int32 ToInt32(this string s)
        {
            return Convert.ToInt32(s);
        }
        public static Decimal ToDecimal(this string s)
        {
            return Convert.ToDecimal(s);
        }
        public static MyTextBox Auto(this TextBox tb)
        {
            return new MyTextBox(tb);
        }
        public static string ForNullOrWhiteSpace(this string s1, string s2)
        {
            if (string.IsNullOrWhiteSpace(s1)) return s2;
            else return s1;
        }
    }
}
