using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Final2
{
    public class DataGridViewReadOnlyMoneyColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewReadOnlyMoneyColumn(string dataPropertyName, string headerText)
        {
            DataPropertyName = dataPropertyName;
            HeaderText = headerText;
            Name = dataPropertyName + "Name";
            ReadOnly = true;
            Visible = true;
        }
    }
}