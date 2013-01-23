using System;
using System.ComponentModel;
using System.Windows.Forms;
using DAgents.Common;
using System.Threading;

namespace QuickBooksService.Gui
{
    public partial class QuickBooksServiceGuiForm : Form, ILogger
    {
        enum State
        {
            Default,
            Running
        }

        State _currentState = State.Default;

        public QuickBooksServiceGuiForm()
        {
            InitializeComponent();
        }

        private void QuickBooksServiceGuiForm_Load(object sender, EventArgs e)
        { 
            _targetsCheckBoxList.CheckAll();
            Log("Ready");
        }

        /// <summary>
        /// When the user clicks "Go" s/he has selected either the US or INTL radio button
        /// and checked one or more targets in the check box list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _goButton_Click(object sender, EventArgs e)
        {
            EnterState(State.Running);
            backgroundWorker1.RunWorkerAsync();
        }

        bool CanGo
        {
            set
            {
                _goButton.Enabled = value;
            }
        }

        void EnterState(State toState)
        {
            switch (_currentState)
            {
                case State.Default:
                    switch (toState)
	                {
                        case State.Running:
                            _goButton.Disable();
                            break;
                        default:
                            throw new InvalidOperationException();
	                }
                    break;
                case State.Running:
                    switch (toState)
                    {
                        case State.Default:
                            _goButton.Enable();
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                    break;
                default:
                    break;
            }
            _currentState = toState;
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            bool result = false;
            try
            {
                var args = new QuickBooksServiceArgs(this);
                result = Run(QuickBooksServiceArgs.Command.Extract, args);
                if (result)
                {
                    result = Run(QuickBooksServiceArgs.Command.Load, args);
                }
            }
            finally
            {
                e.Result = result;
                EnterState(State.Default);
            }
        }

        bool Run(QuickBooksServiceArgs.Command command, QuickBooksServiceArgs args)
        {
            bool result;
            try
            {
                Log("calling qb service...");
                QuickBooksService.Boot.Main(new[] { command.ToString().ToLower(), args.Company, args.Targets }, this);
                Log("returned from qb service...");
                result = true;
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
                result = false;
            }
            return result;
        }

        class QuickBooksServiceArgs
        {
            internal enum Command
            {
                Extract,
                Load
            }

            QuickBooksServiceGuiForm _form;

            internal QuickBooksServiceArgs(QuickBooksServiceGuiForm form)
            {
                this._form = form;
            }

            internal string Company
            {
                get { return _form._usRadioButton.Checked ? "us" : "intl"; }
            }

            internal string Targets
            {
                get
                {
                    var checkBoxList = _form._targetsCheckBoxList;
                    var result = string.Join(",", checkBoxList.EnumerateChecked<string>());
                    return result;
                }
            }
        } 
        
        void _CheckedChanged(object sender, EventArgs e)
        {
            bool canGo = (_usRadioButton.Checked || _intlRadioButton.Checked);
            if (_goButton.Enabled != canGo)
            {
                _goButton.Enabled = canGo;
            }
        }

        #region ILogger Members
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
        public void LogError(string message)
        {
            Console.WriteLine(message);
        }
        #endregion
    }
}
