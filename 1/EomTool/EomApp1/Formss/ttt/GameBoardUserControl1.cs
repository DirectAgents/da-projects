using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.ttt
{
    public partial class GameBoardUserControl1 : UserControl
    {
        public GameBoardUserControl1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var db = new TttDataClasses1DataContext();
            var state = from c in db.TttState1s
                        select c;
            Dictionary<int, int> dic = db.TttState1s.ToDictionary(c => c.id, c => c.state);
            gameSquareUserControl11.DisplayMode = (GameSquareUserControl1.Mode)dic[1];
            gameSquareUserControl12.DisplayMode = (GameSquareUserControl1.Mode)dic[2];
            gameSquareUserControl13.DisplayMode = (GameSquareUserControl1.Mode)dic[3];
            gameSquareUserControl14.DisplayMode = (GameSquareUserControl1.Mode)dic[4];
            gameSquareUserControl15.DisplayMode = (GameSquareUserControl1.Mode)dic[5];
            gameSquareUserControl16.DisplayMode = (GameSquareUserControl1.Mode)dic[6];
            gameSquareUserControl17.DisplayMode = (GameSquareUserControl1.Mode)dic[7];
            gameSquareUserControl18.DisplayMode = (GameSquareUserControl1.Mode)dic[8];
            gameSquareUserControl19.DisplayMode = (GameSquareUserControl1.Mode)dic[9];
        }
    }
}
