using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Final2
{
    public class DataGridViewReadOnlyButtonColumn : DataGridViewButtonColumn
    {
        DataGridViewCellStyle style = new DataGridViewCellStyle();
        public DataGridViewReadOnlyButtonColumn(string headerText)
        {
            FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            HeaderText = headerText;
            Name = headerText + "Name";
            Text = headerText;
            ReadOnly = true;
            UseColumnTextForButtonValue = true;
        }
    }
}