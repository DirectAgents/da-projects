using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Final
{
    public partial class CampaignNotes : Form
    {
        public CampaignNotes()
        {
            InitializeComponent();
        }

        private void CampaignNotes_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'finalizeDataSet1.CampaignNotes' table. You can move, or remove it, as needed.
            this.campaignNotesTableAdapter.Fill(this.finalizeDataSet1.CampaignNotes);
            // TODO: This line of code loads data into the 'finalizeDataSet1.Campaign' table. You can move, or remove it, as needed.
            this.campaignTableAdapter.Fill(this.finalizeDataSet1.Campaign);

        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.campaignCampaignNotesBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(finalizeDataSet1);
        }
    }
}
