using System;
using System.Data;
using DataGridViewExtensions;

namespace EomApp1.Screens.MediaBuyerWorkflow
{
    public partial class PublishersSubform : DataGridViewSubForm
    {
        private MediaBuyerWorkflowDataSet.MediaBuyersRow row;
        private MediaBuyerWorkflowModel model = new MediaBuyerWorkflowModel();
        private string statusString;

        public PublishersSubform(object DataBoundItem, DataGridViewSubformCell Cell)
            : base(DataBoundItem, Cell)
        {
            InitializeComponent();

            this.PublisherPayoutsSubform.Subform = typeof(PublisherPayoutsSubform);
            this.PublisherPayoutsSubform.SubformShowing += new DataGridViewSubformColumn.SubformShowingHandler(PublisherPayoutsSubform_SubformShowing);

            var tuple = DataBoundItem as Tuple<object, string>;
            if (tuple != null)
            {
                this.row = (tuple.Item1 as DataRowView).Row as MediaBuyerWorkflowDataSet.MediaBuyersRow;
                this.statusString = (string)tuple.Item2;
            }
        }

        void PublisherPayoutsSubform_SubformShowing(DataGridViewSubformCell sender, SubformShowingEventArgs e)
        {
            e.DataBoundItem = Tuple.Create(this.bindingSource1.Current, this.statusString);            
        }

        private void MediaBuyerWorkflowSubform_Load_1(object sender, EventArgs e)
        {
            if (this.row != null)
            {
                var publishers = model.PublishersByMediaBuyer(row.MediaBuyerName, statusString);
                this.mediaBuyerWorkflowDataSet1.Publishers.Load(publishers.CreateDataReader());
            }
        }
    }
}
