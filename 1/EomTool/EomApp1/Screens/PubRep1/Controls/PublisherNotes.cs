using EomApp1.Screens.PubRep1.Views;
using EomTool.Domain.Entities;
using System;
using System.Windows.Forms;

namespace EomApp1.Screens.PubRep1.Controls
{
    public partial class PublisherNotes : UserControl, IPublisherNotesView
    {
        public PublisherNotes()
        {
            InitializeComponent();
        }

        #region External inteface (ouside of View)
        public event EventHandler NoteCreated;
        public void SetPublisher(string publisherName)
        {
            if (this.PublisherChangedEvent != null)
                this.PublisherChangedEvent(this, publisherName);
        }
        #endregion

        #region UI handlers
        void AddNoteClicked(object sender, EventArgs e)
        {
            if (this.AddPublisherNoteEvent != null)
            {
                var addPublisherNoteEventArgs = new AddPublisherNoteEventArgs();
                this.AddPublisherNoteEvent(this, addPublisherNoteEventArgs);
                if (addPublisherNoteEventArgs.Success)
                {
                    // The receiver of AddPublisherNoteEvent may set NoteCreated to true
                    if(this.NoteCreated != null)
                        this.NoteCreated(this, EventArgs.Empty);
                }
            }
        }
        #endregion

        #region IPublisherNotesView
        public void FillPublisherNotes(PubNote[] notes)
        {
            this.pubNoteBindingSource.Clear();
            foreach (var note in notes)
                this.pubNoteBindingSource.Add(note);
        }
        public event EventHandler<PublisherChangedEventArgs> PublisherChangedEvent;
        public event EventHandler<AddPublisherNoteEventArgs> AddPublisherNoteEvent;
        #endregion
    }
}
