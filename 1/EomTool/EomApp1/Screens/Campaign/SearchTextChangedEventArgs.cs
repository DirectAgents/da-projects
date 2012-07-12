using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Screens.Campaign
{
    public class SearchTextChangedEventArgs : EventArgs
    {
        public SearchTextChangedEventArgs(string searchText)
        {
            this.SearchText = searchText;
        }
        public string SearchText
        {
            get;
            set;
        }
    }
}
