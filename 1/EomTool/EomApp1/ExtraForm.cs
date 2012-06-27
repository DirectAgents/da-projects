using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1
{
    public partial class ExtraForm : Form
    {
        public ExtraForm()
        {
            InitializeComponent();
        }

        private void extraBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.extraBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.extra2DataSet);

        }

        private void ExtraForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'extra2DataSet.Extra' table. You can move, or remove it, as needed.
            this.eXTableAdapt.Fill(this.extra2DataSet.Extra);

        }
    }
}
