using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.ttt
{
    public partial class TttGameForm1 : Form
    {
        public TttGameForm1()
        {
            InitializeComponent();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var db = new TttDataClasses1DataContext();
            var rows = db.TttState1s.Select(c => c);
            foreach (var row in rows)
            {
                row.state = 1;
            }
            db.SubmitChanges();
        }
    }
}
