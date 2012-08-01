using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Screens.Final
{
    public class PublishersEventArgs : EventArgs
    {
        public PublishersEventArgs(int[] affIDs)
        {
            this.AffIds = affIDs;
        }
        public int[] AffIds { get; set; }
    }
}
