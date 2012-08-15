using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System;

namespace EomApp1.Screens.Extra
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

            Apply();
        }

        public void Apply()
        {
            string filter = this.FilterText;

            if (this.BindingSource.Filter != filter)
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

                // Do not filter edited rows
                string editedFilter = string.Empty;
                if (this.EditedIds.Count > 0)
                    editedFilter = "id=" + String.Join(" OR id=", this.EditedIds);

                // Do not filter newly added rows
                if (filter.Length > 0)
                {
                    filter += " OR id < 0";
                    if (editedFilter.Length > 0)
                        filter += " OR " + editedFilter;
                }

                return filter;
            }
        }

        public BindingSource BindingSource { get; set; }

        Dictionary<string, string> columnFilters = new Dictionary<string, string>();
        public Dictionary<string, string> ColumnFilters { get { return columnFilters; } }

        ISet<int> editedIds = new HashSet<int>();
        public ISet<int> EditedIds
        {
            get { return editedIds; }
            set { editedIds = value; }
        }
    }
}
