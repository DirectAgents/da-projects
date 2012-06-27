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
    public partial class SourceForm : Form
    {
        public SourceForm()
        {
            InitializeComponent();
        }

        private void sourceBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.sourceBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSetForSource);

        }

        private void SourceForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSetForSource.Source' table. You can move, or remove it, as needed.
            this.sourceTableAdapter.Fill(this.dataSetForSource.Source);

        }
    }
}
