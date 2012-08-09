using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace EomApp1.Screens.Wpf
{
    public static class Utilities
    {
        public static void PlaceWpfControlIntoForm<T>(Form form) where T : System.Windows.Controls.UserControl, new()
        {
            form.Controls.Add(new ElementHost
            {
                Dock = DockStyle.Fill,
                Child = new T()
            });
        }
    }
}
