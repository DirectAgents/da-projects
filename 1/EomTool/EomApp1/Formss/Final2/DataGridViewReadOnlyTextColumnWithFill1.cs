using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Final2
{
    public class DataGridViewReadOnlyCurrencyColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewReadOnlyCurrencyColumn(string dataPropertyName, string headerText)
        {
            AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            DataPropertyName = dataPropertyName;
            HeaderText = headerText;
            Name = dataPropertyName + "Name";
            ReadOnly = true;
            Visible = true;
        }
    }
}
