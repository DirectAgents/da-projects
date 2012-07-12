using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Screens.Advertiser2.Forms
{
    public partial class AdvertisersForm : Form
    {
        public AdvertisersForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get advertisers from DirectTrack database
            using (var model = new DirectTrack.Rest.Models.DirectTrackContainer())
            {
                // Create table adapter for monthly database
                using (var advertiserTableAdapter = 
                    new EomApp1.Screens.Advertiser2.Data.Advertiser2DataSetTableAdapters.AdvertiserTableAdapter())
                {
                    // Get data table of advertisers from monthly database
                    var advertiserDataTable = advertiserTableAdapter.GetData();

                    // Upsert advertiser from DirectTrack database to monthly database
                    //if(
                    foreach (var advertiser in model.Resources)
                    {
                        
                    }

                    // save changes to monthly database
                    advertiserTableAdapter.Update(advertiserDataTable);
                }
            }

            // Update control
            advertisers2Control1.DoFill();
        }
    }
}
