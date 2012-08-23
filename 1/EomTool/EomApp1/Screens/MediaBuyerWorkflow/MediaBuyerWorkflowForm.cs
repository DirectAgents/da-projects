using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Screens.MediaBuyerWorkflow
{
    public partial class MediaBuyerWorkflowForm : Form
    {
        public MediaBuyerWorkflowForm()
        {
            InitializeComponent();
            Fill();
        }

        private void Fill()
        {
            using (var db = MediaBuyerWorkflowEntities.Create())
            {
                var mediaBuyerNames = db.PublisherPayouts.Select(c => c.Media_Buyer).Distinct();
                foreach (var item in mediaBuyerNames)
                {
                    this.mediaBuyerWorkflowDataSet1.MediaBuyers.AddMediaBuyersRow(item);
                }
            }
        }
    }
}
