using System;
using System.Windows.Forms;
namespace BarGraphColumn
{
    public class DataGridViewBarGraphColumn :
      DataGridViewColumn
    {
        public DataGridViewBarGraphColumn()
        {
            this.CellTemplate = new DataGridViewBarGraphCell();
            this.ReadOnly = true;
        }

        public long MaxValue;
        private bool needsRecalc = true;

        public void CalcMaxValue()
        {
            if (needsRecalc)
            {
                int colIndex = this.DisplayIndex;
                for (int rowIndex = 0;
                  rowIndex < this.DataGridView.Rows.Count;
                  rowIndex++)
                {
                    DataGridViewRow row =
                      this.DataGridView.Rows[rowIndex];
                    MaxValue = Math.Max(MaxValue,
                      Convert.ToInt64(row.Cells[colIndex].Value));
                }
                needsRecalc = false;
            }
        }
    }
}
