using System;
using System.Windows.Forms;

namespace EomApp1.Screens.Wpf
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Utilities.PlaceWpfControlIntoForm<EomApp.Controls.Wpf.Cone>(this);
        }
    }
}
