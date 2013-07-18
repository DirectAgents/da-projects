using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Windows.Forms;

namespace LineCommander
{
    public partial class MainForm : Form
    {
        private bool selectionEnabled;

        public MainForm()
        {
            var catalog = new DirectoryCatalog(@".", "CakeExtracter.exe");
            var composistionContainer = new CompositionContainer(catalog);
            composistionContainer.ComposeParts(this);

            InitializeComponent();

            foreach (var consoleCommand in this.Commands)
            {
                var commandRow = dataSet1.Commands.AddCommandsRow(consoleCommand.Command);
                foreach (var item in consoleCommand.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance))
                {
                    var commandParametersRow = dataSet1.CommandParameters.AddCommandParametersRow(commandRow, item.Name, "");
                }
            }

            this.Shown += Form1_Shown;
        }

        void Form1_Shown(object sender, EventArgs e)
        {
            this.commandsDataGridView.ClearSelection();
            this.selectionEnabled = true;
        }

        [ImportMany]
        public IEnumerable<CakeExtracter.Common.ConsoleCommand> Commands { get; set; }

        private void commandsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
        }
    }
}
