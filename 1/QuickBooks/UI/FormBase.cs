using System;
using System.Windows.Forms;

namespace QuickBooks.UI.WinForms
{
    public class FormBase : Form
    {
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            foreach (Control control in this.Controls)
            {
                if (control is UserControlBase)
                {
                    var ucb = (UserControlBase)control;
                    ucb.OnShown();
                }
            }
        }
    }
}
