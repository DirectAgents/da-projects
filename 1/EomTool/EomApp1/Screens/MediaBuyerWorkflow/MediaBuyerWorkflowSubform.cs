using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataGridViewExtensions;

namespace EomApp1.Screens.MediaBuyerWorkflow
{
    public partial class MediaBuyerWorkflowSubform : DataGridViewSubForm
    {
        public MediaBuyerWorkflowSubform(object DataBoundItem, DataGridViewSubformCell Cell)
            : base(DataBoundItem, Cell)
        {
            InitializeComponent();
        }

        private void MediaBuyerWorkflowSubform_Load(object sender, EventArgs e)
        {

        }

        private MediaBuyerWorkflowDataSet.MediaBuyersRow Row
        {
            get
            {
                return (this.DataBoundItem as DataRowView).Row as MediaBuyerWorkflowDataSet.MediaBuyersRow;
            }
        }
    }
}
