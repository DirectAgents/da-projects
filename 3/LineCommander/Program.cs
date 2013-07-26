using System;
using System.Windows.Forms;

namespace LineCommander
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ConsoleHelper.InitWin32Console();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run(new MainForm());
        }
    }
}
