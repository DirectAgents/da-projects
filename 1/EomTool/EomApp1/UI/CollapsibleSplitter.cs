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
    public partial class CollapsibleSplitter : SplitContainer
    {
        public CollapsibleSplitter()
        {
            InitializeComponent();
        }

        public bool IsVertical
        {
            set
            {
                if (value)
                {
                    Orientation = Orientation.Vertical;
                    pictureBox1.Visible = true;
                    pictureBox2.Visible = false;
                }
                else
                {
                    Orientation = Orientation.Horizontal;
                    pictureBox1.Visible = false;
                    pictureBox2.Visible = true;
                }
            }
            get
            {
                return 
                    (Orientation.Vertical == Orientation) 
                    ? true 
                    : false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Flip();
        }

        private void Flip()
        {
            Panel1Collapsed = !Panel1Collapsed;

            if (Panel1Collapsed)
            {
                pictureBox1.Image = EomApp1.Properties.Resources.RightArrow;
            }
            else
            {
                pictureBox1.Image = EomApp1.Properties.Resources.LeftArrow;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Flip();
        }
    }
}
