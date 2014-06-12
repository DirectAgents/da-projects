using EomApp1.Screens.PubRep1.MVP.PublisherDetails;
using EomTool.Domain40.Entities;
using System;
using System.Windows.Forms;

namespace EomApp1.Screens.PubRep1.Controls.PublisherDetails
{
    public partial class NotesUC : UserControl, INotes
    {
        public NotesUC()
        {
            InitializeComponent();
        }

        #region External inteface (ouside of View)
        public event EventHandler ItemCreated;
        public void SetPublisher(string publisherName)
        {
            if (this.SelectionChanged != null)
                this.SelectionChanged(publisherName);
        }
        #endregion

        #region UI handlers
        void AddNoteClicked(object sender, EventArgs e)
        {
            if (this.Add != null)
                if (this.Add() && this.ItemCreated != null)
                    this.ItemCreated(this, EventArgs.Empty);
        }
        #endregion

        #region IPublisherNotesView
        public void Fill(PubNote[] notes)
        {
            this.pubNoteBindingSource.Clear();
            foreach (var note in notes)
                this.pubNoteBindingSource.Add(note);
        }
        public event MessageEvent<string> SelectionChanged;
        public event MessageEvent Add;
        #endregion
    }
}
