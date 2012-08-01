using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomToolDatabaseUpdater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.dADatabaseTableAdapter.Fill(this.dataSet1.DADatabase);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var backup = new DatabaseBackup(SelectedRow.connection_string);
            backup.CopyToTest();
        }

        public DataSet1.DADatabaseRow SelectedRow
        {
            get { return (dADatabaseBindingSource.Current as DataRowView).Row as DataSet1.DADatabaseRow; }
        }
    }
}
