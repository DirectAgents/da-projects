using System;

namespace EomApp1.Formss.Synch
{
    public class TargetSystemChoiceChangedEventArgs : EventArgs
    {
        public TargetSystemChoiceChangedEventArgs(TargetSystemChoice targetSystemChoice)
        {
            this.TargetSystemChoice = targetSystemChoice;
        }

        public TargetSystemChoice TargetSystemChoice { get; set; }
    }
}
