using System;
using System.Linq;
using System.Windows.Forms;
using EomApp1.Misc.ttt;

namespace EomApp1.Screens.ttt
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
