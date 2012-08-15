using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Screens.Extra
{
    public class BindingSourceFilter
    {
        public BindingSourceFilter(BindingSource bindingSource, DataGridView dataGridView)
        {
            this.BindingSource = bindingSource;
            this.DataGridView = dataGridView;
        }

        public void SetColumnFilter(string columnName, string filterText)
        {
            if (filterText.Length > 0)
                this.ColumnFilters[columnName] = filterText;
            else if (this.ColumnFilters.ContainsKey(columnName))
                this.ColumnFilters.Remove(columnName);

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string filter = this.FilterText;

            // if there is a filter, the datagridview is readonly to prevent magically disappearing rows
            DataGridView.ReadOnly = !string.IsNullOrWhiteSpace(filter);
            this.BindingSource.Filter = filter;
        }

        public string this[string columnName]
        {
            set { this.SetColumnFilter(columnName, value); }
        }

        public string FilterText
        {
            get
            {
                var sb = new StringBuilder();

                foreach (var item in this.ColumnFilters.OrderBy(c => c.Key))
                {
                    if (sb.Length > 0)
                        sb.Append(" AND ");
                    sb.AppendFormat("{0} LIKE '%{1}%'", item.Key, item.Value);
                }

                string filter = sb.ToString();

                // Never filter newly added rows
                if (filter.Length > 0)
                    filter += " OR id < 0";

                return filter;
            }
        }

        public BindingSource BindingSource { get; set; }

        Dictionary<string, string> columnFilters = new Dictionary<string, string>();
        public Dictionary<string, string> ColumnFilters { get { return columnFilters; } }

        public DataGridView DataGridView { get; set; }
    }
}
