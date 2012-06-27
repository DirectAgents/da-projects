using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Campaign
{
    public partial class CampaignSynchForm : Form
    {
        public CampaignSynchForm()
        {
            InitializeComponent();
        }

        private void CampaignSynchForm_Load(object sender, EventArgs e)
        {
            campaignSynchView1.Initialize();
        }

        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData) 
        //{ 
        //    if (keyData == (Keys.Control | Keys.F)) 
        //    {
        //        campaignSynchView1.GiveFocusToSearch();
        //        return true; 
        //    } 
        //    return base.ProcessCmdKey(ref msg, keyData);
        //}
    }
}
