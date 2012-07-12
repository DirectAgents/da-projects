using System;
using System.Windows.Forms;

namespace EomApp1.Screens.Synch
{
    public partial class TargetSystem : UserControl
    {
        public TargetSystem()
        {
            InitializeComponent();
            this.radioButton1.CheckedChanged += new EventHandler(radioButton1_CheckedChanged);
            this.radioButton2.CheckedChanged += new EventHandler(radioButton2_CheckedChanged);
        }

        void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                this.OnTargetSystemChoiceChanged(TargetSystemChoice.DirectTrack);
            }
        }

        void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                this.OnTargetSystemChoiceChanged(TargetSystemChoice.Cake);
            }
        }

        private void OnTargetSystemChoiceChanged(TargetSystemChoice targetSystemChoice)
        {
            if (this.TargetSystemChoiceChanged != null)
            {
                this.TargetSystemChoiceChanged(this, new TargetSystemChoiceChangedEventArgs(targetSystemChoice));
            }
        }

        public TargetSystemChoice TargetSystemChoice
        {
            get
            {
                return this.radioButton1.Checked ? TargetSystemChoice.DirectTrack : TargetSystemChoice.Cake;
            }
            set
            {
                switch (value)
                {
                    case TargetSystemChoice.DirectTrack:
                        radioButton1.Checked = true;
                        this.OnTargetSystemChoiceChanged(Synch.TargetSystemChoice.DirectTrack);
                        break;
                    case TargetSystemChoice.Cake:
                        radioButton2.Checked = true;
                        this.OnTargetSystemChoiceChanged(Synch.TargetSystemChoice.Cake);
                        break;
                    default:
                        break;
                }
            }
        }

        public event EventHandler<TargetSystemChoiceChangedEventArgs> TargetSystemChoiceChanged;
    }
}
