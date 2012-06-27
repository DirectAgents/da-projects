using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Final2
{
    public class DataGridViewReadOnlyTextFillColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewReadOnlyTextFillColumn(string dataPropertyName, string headerText)
        {
            //AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            //FillWeight = 75F;
            DataPropertyName = dataPropertyName;
            HeaderText = headerText;
            Name = dataPropertyName + "Name";
            ReadOnly = true;
        }
    }
}
