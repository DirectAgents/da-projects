using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EomApp1.UI;

namespace EomApp1.Formss.Payment.Controls
{
    public partial class AdvertiserListUC : UserControl
    {
        public AdvertiserListUC()
        {
            InitializeComponent();
        }

        internal void Fill()
        {
            advertiserTableAdapter.Fill(paymentsDataSet.Advertiser);
        }
    }
}
