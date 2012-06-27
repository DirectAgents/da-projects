using System.Windows.Forms;
using DGVColumnSelector;
using EomApp1.Formss.Common;
using System.Drawing;

namespace EomApp1.Formss.PubRep1.Controls
{
    public partial class AccountingView : UserControl
    {
        public void Initialize()
        {
            _gridView.RowHeadersVisible = true;
            _gridView.AllowUserToResizeRows = false;
            _gridView.AutoSize = true;
            _gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            _gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            _gridView.MultiSelect = false;
            _gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataGridViewColumnSelector cs = new DataGridViewColumnSelector();
            cs.DataGridView = _gridView;
            cs.MaxHeight = 800;
            cs.Width = 150;
        }
         
        public AccountingView()
        {
            InitializeComponent();
        }

        public string Publisher
        {
            set
            {
                _tableAdapter.FillByPublisher(_dataSet.AccountingDetailsWithMargin, value);
                _gridView.ClearSelection();
            }
        }

        //25% +    is green
        //15%-25% is yellow
        //<= 15% is red 
        private void _gridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int ci = e.ColumnIndex;
            int ri = e.RowIndex;
            if (ci == PercentMargin.Index)
            {
                if ((decimal)_gridView[ci, ri].Value < 15)
                {
                    _gridView[ci, ri].Style.BackColor = Color.Red;
                }
                else if ((decimal)_gridView[ci, ri].Value < 25)
                {
                    _gridView[ci, ri].Style.BackColor = Color.Yellow;
                }
                else
                {
                    _gridView[ci, ri].Style.BackColor = Color.Green;
                }
            }
        }

        private void toolStripButton1_Click(object sender, System.EventArgs e)
        {

        }
    }
}
