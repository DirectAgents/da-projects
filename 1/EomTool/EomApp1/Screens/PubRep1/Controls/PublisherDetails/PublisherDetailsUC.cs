using EomApp1.Screens.PubRep1.MVP.PublisherDetails;
using System;
using System.Windows.Forms;

namespace EomApp1.Screens.PubRep1.Controls.PublisherDetails
{
    public partial class PublisherDetailsUC : UserControl
    {
        NotesPresenter publisherNotesPresenter;
        AttachmentsPresenter publisherAttachmentsPresenter;
        public event EventHandler RelatedItemCountChanged;

        public PublisherDetailsUC()
        {
            InitializeComponent();
            this.publisherNotesPresenter = new NotesPresenter(publisherNotes1);
            this.publisherAttachmentsPresenter = new AttachmentsPresenter(publisherAttachments1);
            this.publisherNotes1.ItemCreated += publisherNotes1_NoteCreated;
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
                this.publisherAttachments1.SetPublisher(value);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
