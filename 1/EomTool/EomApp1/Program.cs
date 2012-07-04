using System;
using System.Windows.Forms;

namespace EomApp1
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                EomAppCommon.EomAppSettings.ConnStr = Properties.Settings.Default.DADatabaseR1ConnectionString;
                Application.Run(new EOMForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
    }
}