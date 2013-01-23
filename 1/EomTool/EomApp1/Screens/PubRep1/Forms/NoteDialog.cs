using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Screens.PubRep1.Forms
{
    public partial class NoteDialog : Form
    {
        public NoteDialog()
        {
            InitializeComponent();
        }

        public string NoteText
        {
            get
            {
                return htmlEditorControl1.InnerText;
            }
        }

        private void OkClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
