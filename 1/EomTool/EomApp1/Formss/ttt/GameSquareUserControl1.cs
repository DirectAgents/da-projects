using System.Linq;
using System.Windows.Forms;

namespace EomApp1.Formss.ttt
{
    public partial class GameSquareUserControl1 : UserControl
    {
        public enum Mode
        {
            Empty = 1,
            X = 2,
            O = 3
        }
        public GameSquareUserControl1()
        {
            InitializeComponent();
        }
        private Mode _Mode = Mode.Empty;
        public Mode DisplayMode
        {
            get
            {
                return _Mode;
            }
            set
            {
                if (_Mode == value)
                {
                    return;
                }
                _Mode = value;
                switch (value)
                {
                    case Mode.X:
                        this.panel1.BackgroundImage = global::EomApp1.Properties.Resources.TttX;
                        break;
                    case Mode.O:
                        this.panel1.BackgroundImage = global::EomApp1.Properties.Resources.TttO;
                        break;
                    case Mode.Empty:
                        this.panel1.BackgroundImage = null;
                        break;
                    default:
                        break;
                }
            }
        }
        public int DisplayStateId { get; set; }

        private void panel1_MouseEnter(object sender, System.EventArgs e)
        {
        }

        private void panel1_MouseLeave(object sender, System.EventArgs e)
        {
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Tag.ToString() == "X")
            {
                SubmitDisplayMode(Mode.X);
            }
            else if (e.ClickedItem.Tag.ToString() == "O")
            {
                SubmitDisplayMode(Mode.O);
            }
        }

        private void SubmitDisplayMode(Mode mode)
        {
            var db = new TttDataClasses1DataContext();
            var x = (from c in db.TttState1s
                     where c.id == DisplayStateId
                     select c).First();
            x.state = (int)mode;
            db.SubmitChanges();
        }
    }
}
