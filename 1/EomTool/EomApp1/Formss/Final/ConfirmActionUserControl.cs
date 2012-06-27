using System;
using System.Windows.Forms;

namespace EomApp1.Formss.Final
{
    public partial class ConfirmActionUserControl : UserControl
    {
        public event EventHandler Done;

        public string NotesText
        {
            get { return notesTextBox.Text; }
            set { notesTextBox.Text = value; }
        }

        public bool IsOk { get; private set; }

        public ConfirmActionUserControl()
        {
            InitializeComponent();
        }

        private void okButtonClick(object sender, EventArgs e)
        {
            IsOk = true;
            Done(this, EventArgs.Empty);
        }

        private void cancelButtonClick(object sender, EventArgs e)
        {
            IsOk = false;
            Done(this, EventArgs.Empty);
        }
    }
}
