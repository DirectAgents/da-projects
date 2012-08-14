using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DAgents.Common
{
    public class BindingSourceFilter
    {
        public BindingSourceFilter(BindingSource bindingSource)
        {
            this.BindingSource = bindingSource;
        }

        public void SetColumnFilter(string columnName, string filterText)
        {
            if (filterText.Length > 0)
                this.ColumnFilters[columnName] = filterText;
            else if (this.ColumnFilters.ContainsKey(columnName))
                this.ColumnFilters.Remove(columnName);

            this.BindingSource.Filter = this.FilterText;
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
                return sb.ToString();
            }
        }

        public BindingSource BindingSource { get; set; }

        Dictionary<string, string> columnFilters = new Dictionary<string, string>();
        public Dictionary<string, string> ColumnFilters { get { return columnFilters; } }
    }
}
