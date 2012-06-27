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
    public partial class NotesListForm1 : Form
    {
        public NotesListForm1()
        {
            InitializeComponent();
        }

        // note: unity for ITxns..
        internal void Fill(Services.ITxns txns, int campaignId)
        {
            listView1.Clear();
            var items = txns.GetCampaignNotes(campaignId).Reverse();
            bool flip = true;
            foreach (var item in items)
            {
                flip = !flip;
                var a = new ListViewItem(item.note);
                a.BackColor = flip ? Color.AliceBlue : Color.White;
                listView1.Items.Add(a);
            }
        }

        private void NotesListForm1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
