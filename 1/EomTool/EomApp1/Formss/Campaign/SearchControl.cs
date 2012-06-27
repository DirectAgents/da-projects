using System;
using System.Windows.Forms;
namespace EomApp1.Formss.Campaign
{
    public delegate void SearchTextChangedEventHandler(SearchControl searchControl, SearchTextChangedEventArgs searchTextChangedEventArgs);
    public delegate void AccountManagerSelectedEventHandler(SearchControl searchControl, AccountManagerSelectedEventArgs accountManagerSelectedEventArgs);
    public partial class SearchControl : UserControl
    {
        public event SearchTextChangedEventHandler SearchTextChanged;
        public event AccountManagerSelectedEventHandler AccountManagerSelected; 
        public SearchControl()
        {
            InitializeComponent();
        }
        public void ClearSearchText()
        {
            _searchTextBox.Clear();
        } 
        private void OnSearchTextChanged()
        {
            if (SearchTextChanged != null)
                SearchTextChanged(this, new SearchTextChangedEventArgs(_searchTextBox.Text));
        }
        private void OnAccountManagerSelected(int accountManagerID, string accountManagerName)
        {
            if (AccountManagerSelected != null)
                AccountManagerSelected(this, new AccountManagerSelectedEventArgs(accountManagerID, accountManagerName));
        }
        private bool CancelButtonVisible
        {
            get
            {
                return _cancelButton.Visible;
            }
            set
            {
                if (_cancelButton.Visible != value)
                    _cancelButton.Visible = value;
            }
        }
        #region UI Event Handlers
        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            OnSearchTextChanged();
            CancelButtonVisible = !string.IsNullOrWhiteSpace(_searchTextBox.Text);
        }
        private void CancelSearchButton_Click(object sender, EventArgs e)
        {
            ClearSearchText();
        }
        private void _searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                ClearSearchText();
        }
        private void _accountManagerLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var am = new SelectAccountManagerForm();
            am.ShowDialog(this);
            _accountManagerLink.Text = am.SelectedAccountManagerName;
            OnAccountManagerSelected(am.SelectedAccountManagerId, am.SelectedAccountManagerName);
        }
        #endregion
        #region Design
        public string SearchLabelText
        {
            get
            {
                return _searchLabel.Text;
            }
            set
            {
                _searchLabel.Text = value;
            }
        }
        #endregion
    }
}
