using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

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
    }
}
