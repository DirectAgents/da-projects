using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineCommander
{
    public partial class MainForm : Form
    {
        [ImportMany]
        public IEnumerable<CakeExtracter.Common.ConsoleCommand> Commands { get; set; }

        private TypeParser typeParser;

        public MainForm()
        {
            ComposeThisObject();

            InitializeComponent();

            FillData();

            CakeExtracter.Logger.Instance = new ConsoleLogger();

            this.typeParser = new TypeParser();
        }

        private void ComposeThisObject()
        {
            var catalog = new DirectoryCatalog(@".", "CakeExtracter.dll");
            var composistionContainer = new CompositionContainer(catalog);
            composistionContainer.ComposeParts(this);
        }

        private void FillData()
        {
            foreach (var consoleCommand in this.Commands)
            {
                var commandRow = dataSet1.Commands.AddCommandsRow(consoleCommand.Command, consoleCommand.GetType().FullName);
                foreach (var item in consoleCommand.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance))
                {
                    var commandParametersRow = dataSet1.CommandParameters.AddCommandParametersRow(commandRow, item.Name, "", item.PropertyType.FullName);
                }
            }
        }

        // Click Run
        private void toolStripButton1_RunClicked(object sender, EventArgs e)
        {
            ExecuteSelectedConsoleCommand();
        }

        private void ExecuteSelectedConsoleCommand()
        {
            var commandsRow = GetCommandsRow();
            var consoleCommand = GetConsoleCommand(commandsRow);
            StartConsoleCommandTask(consoleCommand);
        }

        private static void StartConsoleCommandTask(CakeExtracter.Common.ConsoleCommand consoleCommand)
        {
            var task = new Task(() => consoleCommand.Run(null));
            task.Start();
        }

        private CakeExtracter.Common.ConsoleCommand GetConsoleCommand(DataSet1.CommandsRow commandsRow)
        {
            var consoleCommand = Commands.Single(c => c.GetType().FullName == commandsRow.CommandType);
            var consoleCommandType = consoleCommand.GetType();
            foreach (var commandParametersRow in commandsRow.GetCommandParametersRows())
            {
                var valueToSet = this.typeParser.Parse(commandParametersRow.ParameterType, commandParametersRow.ParameterValue);
                consoleCommandType.GetProperty(commandParametersRow.ParameterName).SetValue(consoleCommand, valueToSet);
            }
            return consoleCommand;
        }

        private DataSet1.CommandsRow GetCommandsRow()
        {
            var selectedRowView = (DataRowView)commandsBindingSource.Current;
            var commandsRow = (DataSet1.CommandsRow)selectedRowView.Row;
            return commandsRow;
        }

        // Click Save
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            commandParametersDataGridView.EndEdit();
            dataSet1.WriteXml("LineCommanderData.xml");
        }

        // Click Load
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            dataSet1.Clear();
            dataSet1.ReadXml("LineCommanderData.xml");
        }
    }
}
