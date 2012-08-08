using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EomAppControls
{
    public class ExtendedDataGridView : DataGridView
    {
        private Dictionary<DataGridViewColumn, DataGridViewColumnHeaderCell> originalHeaderCells = new Dictionary<DataGridViewColumn, DataGridViewColumnHeaderCell>();
        private bool columnFiltersAdded = false;

        protected override void OnBindingContextChanged(EventArgs e)
        {
            base.OnBindingContextChanged(e);
            if (ColumnSelectorEnabled)
                new DataGridViewColumnSelector
                {
                    DataGridView = this,
                    MaxHeight = 800,
                    Width = 150
                };
        }

        protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
        {
            base.OnDataBindingComplete(e);
            if (ColumnFiltersEnabled)
            {
                AddColumnFilters();
            }
        }

        public void AddColumnFilters()
        {
            if (this.DataSource != null)
            {
                foreach (DataGridViewColumn column in this.Columns)
                {
                    if (IsTextBox(column))
                    {
                        if (NeedsFilter(column))
                        {
                            ConvertToFilter(column);
                        }
                    }
                    else
                    {
                        if (column.SortMode != DataGridViewColumnSortMode.Automatic)
                        {
                            column.SortMode = DataGridViewColumnSortMode.Automatic;
                        }
                    }
                }
            }
            this.columnFiltersAdded = true;
        }

        public void RemoveColumnFilters()
        {
            foreach (var item in this.originalHeaderCells)
            {
                item.Key.HeaderCell = item.Value;
            }
            this.originalHeaderCells.Clear();
            this.columnFiltersAdded = false;
        }

        public void ToggleColumnFilters()
        {
            if (this.columnFiltersAdded)
            {
                RemoveColumnFilters();
            }
            else
            {
                AddColumnFilters();
            }
        }

        private void ConvertToFilter(DataGridViewColumn column)
        {
            this.originalHeaderCells[column] = column.HeaderCell;
            column.HeaderCell = new DataGridViewAutoFilter.DataGridViewAutoFilterColumnHeaderCell(column.HeaderCell);
        }

        private bool NeedsFilter(DataGridViewColumn column)
        {
            return column.HeaderCell.GetType() != typeof(DataGridViewAutoFilter.DataGridViewAutoFilterColumnHeaderCell);
        }

        private bool IsTextBox(DataGridViewColumn column)
        {
            return column.CellType == typeof(DataGridViewTextBoxCell);
        }

        public bool ColumnSelectorEnabled { get; set; }

        public bool ColumnFiltersEnabled { get; set; }
    }
}
