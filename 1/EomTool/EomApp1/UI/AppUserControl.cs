using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.UI
{
    public partial class AppUserControl : UserControl
    {
        public AppUserControl()
        {
            this.ControlAdded += new ControlEventHandler(DirectAgentsUserControl1_ControlAdded);
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        void DirectAgentsUserControl1_ControlAdded(object sender, ControlEventArgs e)
        {
            //e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
        }


        // 1
        private void DirectAgentsUserControl1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }

        // 2
        private void DirectAgentsUserControl1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        // 3
        private void DirectAgentsUserControl1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
