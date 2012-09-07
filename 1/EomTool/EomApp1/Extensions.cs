using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Globalization;

namespace EomApp1
{
    public static class Extensions
    {
        public static IEnumerable<ToolStripMenuItem> TaggedToolStripMenuItems(this Form form)
        {
            foreach (var menuItem in from c in form.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                                     where c.FieldType == typeof(ToolStripMenuItem)
                                     let mi = (c.GetValue(form) as ToolStripMenuItem)
                                     where !string.IsNullOrWhiteSpace((string)mi.Tag)
                                     select mi)
                yield return menuItem;
        }

        public static bool CaseInsensitiveContains(this List<string> source, string toCheck)
        {
            var toCheckUpper = toCheck.ToUpper();
            return source.Any(c => c.ToUpper() == toCheckUpper);
        }

        public static string[] ToArray(this string s, params char[] seps)
        {
            return s.Split(seps.Length > 0 ? seps : new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static T[] ToArray<T>(this string s, params char[] seps)
        {
            if (typeof(T) == typeof(int))
            {
                return s.Split(seps.Length > 0 ? seps : new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(id => int.Parse(id))
                            .Cast<T>()
                            .ToArray();
            }
            else throw new Exception("cannot convert to " + typeof(T).Name);
        }

        public static IEnumerable<ToolStripItem> DisabledMenus(this MenuStrip menuStrip)
        {
            foreach (ToolStripMenuItem item in menuStrip.Items)
                if (item.DropDownItems.Cast<ToolStripItem>().All(c => !c.Enabled))
                    yield return item;
        }

        public static void LaunchForm<T>(this Form parentForm) where T : Form, new()
        {
            var form = Application.OpenForms.OfType<T>().FirstOrDefault();
            if (form != null)
            {
                if (form.WindowState == FormWindowState.Minimized)
                    form.WindowState = FormWindowState.Normal;
                form.BringToFront();
            }
            else
            {
                form = new T();
                form.Show(parentForm);
            }
        }

        public static string SumString<T>(this IEnumerable<IGrouping<string, T>> group, Func<T, decimal> func)
        {
            // TODO: put in common
            var map = new Dictionary<string, string>();
            map.Add("USD", "en-us");
            map.Add("GBP", "en-gb");
            map.Add("EUR", "de-de");
            map.Add("AUD", "en-AU");
            //return string.Join(", ", group.Select(c => string.Format(CultureInfo.CreateSpecificCulture(map[c.Key]), "{0:C} ({1})", c.Sum(d => func(d)), c.Key)));
            return string.Join(", ", group.Select(c => string.Format(CultureInfo.CreateSpecificCulture(map[c.Key]), "{0:C}", c.Sum(d => func(d)))));
        }

        public static bool HasRows(this DataGridView grid)
        {
            return grid.Rows.Count > 0;
        }

        public static IEnumerable<DataGridViewRow> EnumerateRows(this DataGridView grid)
        {
            return grid.Rows.Cast<DataGridViewRow>();
        }

        public static void ForEachRow(this DataGridView grid, Action<DataGridViewRow> action)
        {
            if (grid.HasRows())
                foreach (var row in grid.EnumerateRows())
                    action(row);
        }

        public static void ForEachCellInColumn<T>(this DataGridView grid, int columnIndex, Action<T> action) where T : DataGridViewCell
        {
            grid.ForEachRow(row => action((T)row.Cells[columnIndex]));
        }

        public static T Value<T>(this DataGridViewColumn column, DataGridViewCellEventArgs cellEvent)
        {
            return (T)column.DataGridView[column.Index, cellEvent.RowIndex].Value;
        }

        public static int[] Indicies(this DataGridViewColumn[] dataGridViewColumns)
        {
            return dataGridViewColumns.Select(c => c.Index).ToArray();
        }
    }
}
