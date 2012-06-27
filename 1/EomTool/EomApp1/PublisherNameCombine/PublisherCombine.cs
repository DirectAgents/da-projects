using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.PublisherNameCombine
{
    public partial class PublisherCombine : Form
    {
        public PublisherCombine()
        {
            InitializeComponent();
        }

        private void publisherBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.publisherBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.publisherCombineDataSet);

        }

        private void PublisherCombine_Load(object sender, EventArgs e)
        {
            this.publisherTableAdapter.Fill(this.publisherCombineDataSet.Publisher);

        }
    }
}
