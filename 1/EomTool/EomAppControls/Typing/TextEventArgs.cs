using System;

namespace EomApp1.Events
{
    public class TextEventArgs : EventArgs
    {
        private string trackedText;
        public TextEventArgs(string s)
        {
            this.trackedText = s;
        }
        public string Text
        {
            get
            {
                return this.trackedText;
            }
        }
    }
}
