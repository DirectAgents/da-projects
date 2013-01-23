using System;
using System.Windows.Forms;

namespace QuickBooks.UI
{
    public class UserControlBase : UserControl
    {
        public void OnShown()
        {
            foreach (Control control in this.Controls)
            {
                if (control is UserControlBase)
                {
                    var ucb = (UserControlBase)control;
                    ucb.OnShown();
                }
            }
            if (this.Shown != null)
            {
                Shown(this, EventArgs.Empty);
            }
        }

        public event EventHandler Shown;
    }
}
