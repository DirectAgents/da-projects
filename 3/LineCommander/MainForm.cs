using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CakeExtracter.Bootstrappers;
using Quartz;
using Quartz.Impl;

namespace LineCommander
{
    public partial class MainForm : Form
    {
        [ImportMany]
        private IEnumerable<IBootstrapper> bootstrappers;

        [ImportMany]
        public IEnumerable<CakeExtracter.Common.ConsoleCommand> Commands { get; set; }

        private TypeParser typeParser;

        public MainForm()
        {
            InitializeComponent();
            this.Shown += MainForm_Shown;
            this.typeParser = new TypeParser();
        }

        void MainForm_Shown(object sender, EventArgs e)
        {
            ComposeThisObject();
            bootstrappers.ToList().ForEach(c => c.Run());
            FillData();
        }

        private void ComposeThisObject()
        {
            var catalog = new DirectoryCatalog(@".", "CakeExtracter.dll");
            var composistionContainer = new CompositionContainer(catalog);
            composistionContainer.ComposeParts(this);
        }

        private void FillData()
        {
            // TODO: the scheduler should be created by IOC and injected
            var schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFactory.GetScheduler();
            scheduler.Start();

            // Wrap all the commands with a ScheduledCommand
            this.Commands = this.Commands.Select(c => new CakeExtracter.Commands.ScheduledCommand(scheduler, c));

            foreach (var consoleCommand in this.Commands)
            {
                var commandRow = dataSet1.Commands.AddCommandsRow(consoleCommand.Command);
                foreach (var item in consoleCommand.GetArgumentProperties())
                {
                    dataSet1.CommandParameters.AddCommandParametersRow(commandRow, item.Name, "", item.PropertyType.FullName);
                }
            }
        }

        //
        // Click Run
        //
        private void toolStripButton1_RunClicked(object sender, EventArgs e)
        {
            // Make sure changes are flushed to the dataset
            commandParametersDataGridView.EndEdit();
            commandParametersBindingSource.EndEdit();

            ExecuteSelectedConsoleCommand();
        }

        private void ExecuteSelectedConsoleCommand()
        {
            var commandsRow = GetCommandsRow();
            var consoleCommand = GetConsoleCommand(commandsRow);
            Task.Factory.StartNew(() =>
            {
                var runningCommandRow = runtimeDataSet.RunningCommands.AddRunningCommandsRow(Guid.NewGuid(), consoleCommand.OneLineDescription, "Running");

                consoleCommand.Run(null);

                runningCommandRow.RunState = "Not Running";
            });
        }

        private CakeExtracter.Common.ConsoleCommand GetConsoleCommand(DataSet1.CommandsRow commandsRow)
        {
            var consoleCommand = Commands.Single(c => c.Command == commandsRow.CommandName);
            foreach (var commandParametersRow in commandsRow.GetCommandParametersRows())
            {
                if (!string.IsNullOrWhiteSpace(commandParametersRow.ParameterValue))
                {
                    var valueToSet = this.typeParser.Parse(commandParametersRow.ParameterType, commandParametersRow.ParameterValue);
                    if (!consoleCommand.TrySetProperty(commandParametersRow.ParameterName, valueToSet))
                    {
                        MessageBox.Show("Could not set property: " + commandParametersRow.ParameterName);
                    }
                }
            }
            return consoleCommand;
        }

        private DataSet1.CommandsRow GetCommandsRow()
        {
            var selectedRowView = (DataRowView)commandsBindingSource.Current;
            var commandsRow = (DataSet1.CommandsRow)selectedRowView.Row;
            return commandsRow;
        }

        //
        // Click Save
        //
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            // Make sure changes are flushed to the dataset
            commandParametersDataGridView.EndEdit();
            commandParametersBindingSource.EndEdit();

            // Show the save dialog
            var fd = new SaveFileDialog();
            var fdr = fd.ShowDialog();
            if (fdr == System.Windows.Forms.DialogResult.OK)
            {
                var saveFilePath = fd.FileName;

                // Save the file
                dataSet1.WriteXml(saveFilePath);

                // Store path in user settings and save them
                LineCommander.Properties.Settings.Default.SaveFilePath = saveFilePath;
                LineCommander.Properties.Settings.Default.Save();
            }
        }

        //
        // Click Load
        //
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog();
            fd.FileName = LineCommander.Properties.Settings.Default.SaveFilePath;
            var fdr = fd.ShowDialog();
            if (fdr == System.Windows.Forms.DialogResult.OK)
            {
                dataSet1.Clear();
                dataSet1.ReadXml(fd.FileName);
            }
        }
    }
}
