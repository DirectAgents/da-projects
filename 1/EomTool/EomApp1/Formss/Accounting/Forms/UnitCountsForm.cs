using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EomApp1.Formss.Accounting.Data;

namespace EomApp1.Formss.Accounting.Forms
{
    public partial class UnitCountsForm : Form
    {
        public UnitCountsForm(int pid, string cd, decimal rev, decimal cost)
        {
            this.Pid = pid;

            this.Cd = cd;

            this.Rev = rev;

            this.Cost = cost;

            InitializeComponent();

            Fill();

            UpdateTotal();
        }

        private void UpdateTotal()
        {
            var sum = unitCountDataSet.SelectNumUnitsByPidCdCostRev.Sum(c => c.num_units);

            label2.Text = sum.ToString();
        }

        private void Fill()
        {
            selectNumUnitsByPidCdCostRevTableAdapter.Fill(
                unitCountDataSet.SelectNumUnitsByPidCdCostRev,
                Pid,
                Cd,
                Rev,
                Cost);
        }

        private void Save(object sender, EventArgs e)
        {
            Validate();

            selectNumUnitsByPidCdCostRevBindingSource.EndEdit();

            var db = new AccountingDataClassesDataContext(true);

            unitCountDataSet
                .GetChanges(DataRowState.Modified).Tables[0].Rows
                .Cast<EomApp1.Formss.Accounting.Data.UnitCountDataSet.SelectNumUnitsByPidCdCostRevRow>().ToList()
                .ForEach(row =>
                {
                    db.Items.First(item => item.id == row.id).num_units = row.num_units;
                });

            db.SubmitChanges();

            Fill();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell.Value = "0";
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateTotal();
        }

        public int Pid { get; set; }

        public string Cd { get; set; }

        public decimal Rev { get; set; }

        public decimal Cost { get; set; }
    }
}
