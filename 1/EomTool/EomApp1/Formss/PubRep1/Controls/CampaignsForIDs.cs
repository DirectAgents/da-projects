using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace EomApp1.Formss.PubRep1.Controls
{
    public partial class CampaignsForIDs : UserControl
    {
        DataTable _dt;
        
        public CampaignsForIDs()
        {
            InitializeComponent();
        }

        public void Initialize(string IDs)
        {
            ItemIDsTextBox.Text = IDs;
            _dt = new DataTable("Campaigns");
            var includeCol = _dt.Columns.Add("Include", typeof(bool));
            var campaignCol = _dt.Columns.Add("Campaign", typeof(string));
            foreach (var item in IDs.Split(','))
            {
                _dt.Rows.Add(true, item);
            }
            dataGridView1.DataSource = _dt;
        }
    }
}
