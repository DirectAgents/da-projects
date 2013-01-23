using System.Collections.Generic;
using System.Windows.Forms;

namespace QuickBooksService.Gui
{
    static class ControlExtensions
    {
        // ----------------------------------------------
        // Control
        // ----------------------------------------------        

        public static void Enable(this Control control)
        {
            control.Enabled = true;
        }

        public static void Disable(this Control control)
        {
            control.Enabled = false;
        }

        // ----------------------------------------------        
        // CheckedListBox
        // ----------------------------------------------        

        public static void CheckAll(this CheckedListBox checkedListBox)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemChecked(i, true);
            }
        }

        public static IEnumerable<T> EnumerateChecked<T>(this CheckedListBox checkedListBox)
        {
            foreach (var item in checkedListBox.CheckedItems)
            {
                yield return (T)item;
            }
        }
    }
}
