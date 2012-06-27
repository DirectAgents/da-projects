using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EomApp1.Formss.AB.Views;
using EomApp1.Formss.AB.Presenters;

namespace EomApp1.Formss.AB.Forms
{
    public partial class AB : Form, IABView
    {
        private ABPresenter _presenter;
        public AB()
        {
            InitializeComponent();
        }

        private void AB_Load(object sender, EventArgs e)
        {
            _presenter = new ABPresenter();
            _presenter.Init(this);
            _advertisersTree.ExpandAll();
        }

        #region IABView Members

        List<Data.Advertiser> IABView.Advertisers
        {
            set 
            {
                TreeNode root = _advertisersTree.TopNode;
                foreach (var item in value)
                {
                    root.Nodes.Add(item.Name);
                }
            }
        }

        //List<Data.ABItem> IABView.ABItems
        //{
        //    set
        //    {
        //        aBItemBindingSource.Clear();
        //        value.ForEach(c => aBItemBindingSource.Add(c));
        //    }
        //}
        List<Data.ABItem> IABView.ABItems
        {
            set
            {
                aBItemBindingSource.Clear();
                value.ForEach(c => aBItemBindingSource.Add(c));
            }
        }

        bool IABView.ConvertToTargetCurrency
        {
            get
            {
                return _convertCurrencyMenuItem.Checked;
            }
        }

        decimal IABView.Total
        {
            set
            {
                label1.Text = value.ToString("N2");
            }
        }

        #endregion

        private void _advertisersTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            HandleSelection();
        }

        private void HandleSelection()
        {
            label1.Text = "";
            if ((string)_advertisersTree.SelectedNode.Tag == "Root") return;
            string selectedAdvertiser = _advertisersTree.SelectedNode.Text;
            _presenter.SelectedAdvertiser = selectedAdvertiser;
        }

        private void _convertCurrencyMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            HandleSelection();
        }

        private void editStartingBalancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AdvertiserStartingBalances()).ShowDialog();
        }
    }
}
