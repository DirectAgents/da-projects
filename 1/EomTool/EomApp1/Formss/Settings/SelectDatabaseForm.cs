using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Settings
{
    public partial class SelectDatabaseForm : Form
    {
        public SelectDatabaseForm(string selectedDatabaseName)
        {
            SelectedDatabaseName = selectedDatabaseName;
            InitializeComponent();
        }

        private void dADatabaseBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.dADatabaseBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.settingsDataSet1);
        }

        private void SelectDatabaseForm_Load(object sender, EventArgs e)
        {
            this.dADatabaseTableAdapter.Fill(this.settingsDataSet1.DADatabase);
            foreach (DataGridViewRow row in dADatabaseDataGridView.Rows)
            {
                DataGridViewCell cell = row.Cells["colName"];
                if ((string)cell.Value == SelectedDatabaseName)
                {
                    dADatabaseDataGridView.CurrentCell = cell;
                }
            }
        }

        public event EventHandler SelectedARow;

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SelectedDatabaseName = (string)dADatabaseDataGridView.CurrentRow.Cells["colName"].Value;

            if (SelectedARow != null)
            {
                SelectedARow(this, EventArgs.Empty);
            }
        }

        public string SelectedDatabaseName { get; set; }
    }
}
