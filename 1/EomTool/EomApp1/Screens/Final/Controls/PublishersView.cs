using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace EomApp1.Screens.Final.Controls
{
    public partial class PublishersView : UserControl
    {
        public PublishersView()
        {
            InitializeComponent();
        }

        public void ClearSelection()
        {
            grid.ClearSelection();
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
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
        private void CellClicked(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = this.grid.SelectedRows;
            var affIDs = new List<int>();

            foreach (DataGridViewRow row in selectedRows)
            {
                var rowView = row.DataBoundItem as DataRowView;
                affIDs.Add(((Data.PublishersDataSet.PublishersRow)rowView.Row).AffId);
            }

            if (PublishersSelected != null)
            {
                PublishersSelected(this, new PublishersEventArgs(affIDs));
            }
        }

        public Data.PublishersDataSet PublishersDataSet 
        { 
            get { return this.finalizePublishersDataSet; } 
        }

        public string ActionButtonText
        {
            get { return this.actionButton.Text; }
            set { this.actionButton.Text = value; }
        }

        public event EventHandler<PublishersEventArgs> PublishersSelected;
    }
}
