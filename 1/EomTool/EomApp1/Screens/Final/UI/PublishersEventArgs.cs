using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Screens.Final
{
    public class PublishersEventArgs : EventArgs
    {
        public PublishersEventArgs(int[] affIDs, string[] costCurrs)
        {
            this.AffIds = affIDs;
            this.CostCurrs = costCurrs;
        }
        public int[] AffIds { get; set; }
        public string[] CostCurrs { get; set; }
    }
}
