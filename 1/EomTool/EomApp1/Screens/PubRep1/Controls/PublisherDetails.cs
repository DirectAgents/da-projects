using EomApp1.Screens.PubRep1.Presenters;
using System;
using System.Windows.Forms;

namespace EomApp1.Screens.PubRep1.Controls
{
    public partial class PublisherDetails : UserControl
    {
        PublisherNotesPresenter publisherNotesPresenter;
        public event EventHandler RelatedItemCountChanged;

        public PublisherDetails()
        {
            InitializeComponent();
            this.publisherNotesPresenter = new PublisherNotesPresenter(publisherNotes1);
            this.publisherNotes1.NoteCreated += publisherNotes1_NoteCreated;
        }

        void publisherNotes1_NoteCreated(object sender, EventArgs e)
        {
            if (this.RelatedItemCountChanged != null)
            {
                this.RelatedItemCountChanged(this, EventArgs.Empty);
            }
        }

        public string Publisher
        {
            set
            {
                this.publisherNotes1.SetPublisher(value);               
            }
        }
    }
}
