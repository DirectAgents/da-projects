using EomTool.Domain.Entities;
using System;

namespace EomApp1.Screens.PubRep1.Views
{
    public interface IPublisherNotesView
    {
        void FillPublisherNotes(PubNote[] items);
        event EventHandler<PublisherChangedEventArgs> PublisherChangedEvent;
        event EventHandler<AddPublisherNoteEventArgs> AddPublisherNoteEvent;
    }

    public class PublisherChangedEventArgs : EventArgs
    {
        public static implicit operator PublisherChangedEventArgs(string publisherName)
        {
            return new PublisherChangedEventArgs { PublisherName = publisherName };
        }

        public string PublisherName { get; private set; }
    }

    public class AddPublisherNoteEventArgs : EventArgs
    {
        public bool Success { get; set; }
    }
}
