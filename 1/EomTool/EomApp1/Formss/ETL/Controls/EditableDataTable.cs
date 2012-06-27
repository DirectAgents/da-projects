using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EomApp1.Formss.ETL.Data;

namespace EomApp1.Formss.ETL.Controls
{
    public partial class EditableDataTable : UserControl
    {
        public class GoPressedEventArgs : EventArgs
        {
            public DataSet1.DTRestCallRow Row { get; set; }
        }

        public event EventHandler GoPressed;

        public string LoadFile { get; set; }

        public EditableDataTable()
        {
            InitializeComponent();
        }

        private void dTRestCallBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            dataSet1.DTRestCall.WriteXml(SavePath);
        }

        private void EditableDataTable_Load(object sender, EventArgs e)
        {
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            dataSet1.ReadXml(OpenPath);
        } 
        
        private string OpenPath
        {
            get
            {
                if (string.IsNullOrEmpty(LoadFile))
                {
                    var d = new OpenFileDialog();
                    d.ShowDialog();
                    string f = d.FileName;
                    return f;
                }
                else
                {
                    return LoadFile;
                }
            }
        }

        private string SavePath
        {
            get
            {
                var d = new SaveFileDialog();
                d.ShowDialog();
                string f = d.FileName;
                return f;
            }
        }

        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int col = e.ColumnIndex;
            if (DGV.Columns.IndexOf(LoadButtonCol) == col)
            {
                if (GoPressed != null)
                {
                    GoPressed(this, new GoPressedEventArgs { Row = _bindingSource.DataRowAtX<DataSet1.DTRestCallRow>(e.RowIndex) });
                }
            }
        }
    }
    static class MyExt
    {
        internal static T DataRowAtX<T>(this BindingSource bs, int i) where T : class
        {
            return ((DataRowView)bs[i]).Row as T;
        }
    }
}
