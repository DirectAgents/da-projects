using System;
using System.Data;
using DataGridViewExtensions;

namespace EomApp1.Screens.MediaBuyerWorkflow
{
    public partial class PublisherPayoutsSubform : DataGridViewSubForm
    {
        private MediaBuyerWorkflowDataSet.PublishersRow row;
        private MediaBuyerWorkflowModel model = new MediaBuyerWorkflowModel();
        private string statusString;

        public PublisherPayoutsSubform(object DataBoundItem, DataGridViewSubformCell Cell)
            : base(DataBoundItem, Cell)
        {
            InitializeComponent();

            var tuple = DataBoundItem as Tuple<object, string>;
            if (tuple != null)
            {
                this.row = (tuple.Item1 as DataRowView).Row as MediaBuyerWorkflowDataSet.PublishersRow;
                this.statusString = (string)tuple.Item2;
            }
        }

        private void PublisherPayoutsSubform_Load(object sender, EventArgs e)
        {
            if (this.row != null)
            {
                publisherPayoutsTableAdapter.Fill(this.mediaBuyerWorkflowDataSet.PublisherPayouts, row.PublisherName, this.statusString);
            }
        }
    }
}
