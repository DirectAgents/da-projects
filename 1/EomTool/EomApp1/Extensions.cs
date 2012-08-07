using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System;

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
            foreach (var item in source)
            {
                if (item.ToUpper() == toCheck.ToUpper())
                    return true;
            }
            return false;
        }
    }
}
