using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using System.Collections;

namespace EomApp1.Screens.Final.Controls
{
    public partial class PublishersView : UserControl
    {
        private List<int> affIDs = new List<int>();

        public PublishersView()
        {
            InitializeComponent();
        }

        public void SelectAll()
        {
            grid.SelectAll();
        }

        internal void SelectNone()
        {
            grid.ClearSelection();
        }

        private void SelectionChanged(object sender, EventArgs e)
        {

            this.affIDs.Clear();
            DataGridViewSelectedRowCollection selectedRows = this.grid.SelectedRows;

            foreach (DataGridViewRow row in selectedRows)
            {
                var rowView = row.DataBoundItem as DataRowView;
                affIDs.Add(((Data.DataSet1.PublishersRow)rowView.Row).AffId);
            }

            actionButton.Enabled = (grid.SelectedRows.Count > 0);
        }

        // Select All
        private void SelectAllClicked(object sender, EventArgs e)
        {
            grid.SelectAll();
        }

        // Select None
        private void SelectNoneClicked(object sender, EventArgs e)
        {
            grid.ClearSelection();
        }

        // Selected
        private void ActionButtonClicked(object sender, EventArgs e)
        {
            if (PublishersActionInvoked != null)
            {
                PublishersActionInvoked(this, new PublishersEventArgs(affIDs.ToArray()));
            }
        }

        public void InitializeNetTermsDropdown()
        {
            var items = this.finalizePublishersDataSet.Publishers.AsEnumerable().Select(p => p.NetTerms).Distinct().OrderBy(n => n);
            if (items.Count() > 1)
            {
                netTermsComboBox.Items.Add("All");
                netTermsComboBox.Items.AddRange(items.ToArray());
                netTermsComboBox.SelectedIndex = 0;
            }
            else
            {
                netTermsLabel.Enabled = false;
                netTermsComboBox.Enabled = false;
            }
        }

        public Data.DataSet1 PublishersDataSet
        {
            get { return this.finalizePublishersDataSet; }
        }

        public string ActionButtonText
        {
            get { return this.actionButton.Text; }
            set { this.actionButton.Text = value; }
        }

        public event EventHandler<PublishersEventArgs> PublishersActionInvoked;

        private void netTermsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var affIDsBeforeFilter = this.affIDs.ToList();
            int selectedIndex = netTermsComboBox.SelectedIndex;
            if (selectedIndex == 0)
            {
                this.finalizePublishersBindingSource.Filter = "NetTerms LIKE '%'";
            }
            else if (selectedIndex > 0)
            {
                this.finalizePublishersBindingSource.Filter = "NetTerms LIKE '%" + netTermsComboBox.Items[selectedIndex] + "%'";
            }
            grid.ClearSelection();

            var rows = this.grid.Rows;
            foreach (DataGridViewRow row in rows)
            {
                var rowView = row.DataBoundItem as DataRowView;
                int affID = ((Data.DataSet1.PublishersRow)rowView.Row).AffId;
                if (affIDsBeforeFilter.Contains(affID))
                    row.Selected = true;
            }
        }

        public void SetNetTermFilter(string netTermName)
        {
            if (this.netTermsComboBox.Items.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(netTermName))
                    this.netTermsComboBox.SelectedIndex = 0;
                else
                    this.netTermsComboBox.SelectedIndex = this.netTermsComboBox.Items.IndexOf(netTermName);
            }
        }
    }
}
