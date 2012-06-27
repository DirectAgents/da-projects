using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Events
{
    class TextEventArgs : EventArgs
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
