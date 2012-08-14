using System;
using System.Drawing;
using System.Windows.Forms;

namespace EomAppControls.Filtering
{
    public class DataGridViewTextBoxFilterColumnHeaderCell : DataGridViewColumnHeaderCell
    {
        private DataGridViewColumnHeaderCell replacedHeaderCell;
        private TextBox filterTextBox = null;

        public DataGridViewTextBoxFilterColumnHeaderCell(DataGridViewColumnHeaderCell replacedHeaderCell)
        {
            this.Value = replacedHeaderCell.Value;
            this.replacedHeaderCell = replacedHeaderCell;
            this.FilterBoxBackColor = TextBox.DefaultBackColor;
            this.FilterBoxHasValueBackColor = Color.FromArgb(255, 255, 192);
        }

        protected override void Paint(
                                    System.Drawing.Graphics graphics,
                                    System.Drawing.Rectangle clipBounds,
                                    System.Drawing.Rectangle cellBounds,
                                    int rowIndex,
                                    DataGridViewElementStates dataGridViewElementState,
                                    object value,
                                    object formattedValue,
                                    string errorText,
                                    DataGridViewCellStyle cellStyle,
                                    DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                    DataGridViewPaintParts paintParts)
        {
            base.Paint(
                    graphics,
                    clipBounds,
                    cellBounds,
                    rowIndex,
                    dataGridViewElementState,
                    value,
                    formattedValue,
                    errorText,
                    cellStyle,
                    advancedBorderStyle,
                    paintParts);

            if ((paintParts & DataGridViewPaintParts.ContentBackground) == 0)
                return;

            SizeF valueSize = graphics.MeasureString(value as string, this.InheritedStyle.Font);
            int headerTextWidth = Convert.ToInt32(valueSize.Width);
            int left = cellBounds.X + headerTextWidth + 5;
            int width = cellBounds.Width - headerTextWidth - 20;
            int top = cellBounds.Top;
            int height = cellBounds.Height;

            if (width < 25)
            {
                if (filterTextBox != null)
                    filterTextBox.Visible = false;
                return;
            }
            else
                width = (width < 100) ? width : 100;

            if (filterTextBox == null)
                SetupFilter();
            else
                if (!filterTextBox.Visible)
                    filterTextBox.Visible = true;

            filterTextBox.Bounds = new Rectangle(left, top, width, height);
        }

        private void SetupFilter()
        {
            filterTextBox = new TextBox
            {
                BackColor = this.FilterBoxBackColor
            };
            filterTextBox.TextChanged += (s, e) => AdjustStyle();
            filterTextBox.TextChanged += (s, e) => OnFilterChanged();
            this.DataGridView.Controls.Add(filterTextBox);
        }

        private void OnFilterChanged()
        {
            if (FilterChanged != null)
                FilterChanged(this, new FilterChangedEventArgs(filterTextBox.Text));
        }

        private void AdjustStyle()
        {
            var xLabel = filterTextBox.Tag as Label;
            if (xLabel == null)
            {
                xLabel = new Label
                {
                    Text = "X",
                    Visible = false,
                    BackColor = Color.White,
                    ForeColor = Color.Red,
                    Padding = new Padding(0),
                    Margin = new Padding(0)
                };
                var graphics = xLabel.CreateGraphics();
                int xWidth = graphics.MeasureString("X", xLabel.Font).ToSize().Width;
                xLabel.Left = filterTextBox.ClientRectangle.Width - xWidth;
                xLabel.Click += (s, e) => filterTextBox.Clear();
                xLabel.Cursor = Cursors.Arrow;
                filterTextBox.Controls.Add(xLabel);
                filterTextBox.Tag = xLabel;
            }

            if (filterTextBox.Text.Length > 0)
            {
                filterTextBox.BackColor = this.FilterBoxHasValueBackColor;
                xLabel.Visible = true;
            }
            else
            {
                filterTextBox.BackColor = this.FilterBoxBackColor;
                xLabel.Visible = false;
            }
        }

        public Color FilterBoxHasValueBackColor { get; set; }
        public Color FilterBoxBackColor { get; set; }

        public event EventHandler<FilterChangedEventArgs> FilterChanged;
        public class FilterChangedEventArgs : EventArgs
        {
            public FilterChangedEventArgs(string filterText)
            {
                this.FilterText = filterText;
            }
            public string FilterText { get; set; }
        }
    }
}
