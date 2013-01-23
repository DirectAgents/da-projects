using EomTool.Domain.Entities;
using System;
using System.Linq;

namespace EomTool.Domain.Concrete
{
    public class PublisherRelatedItemsRepository : IDisposable
    {
        EomEntities context;

        public PublisherRelatedItemsRepository(EomEntities context)
        {
            this.context = context;
        }

        public IQueryable<PubNote> Notes(string publisherName)
        {
            var result = this.context.PubNotes.Where(c => c.publisher_name == publisherName);
            return result;
        }

        public void AddNote(string publisherName, string authorIdentity, string noteText)
        {
            var note = new PubNote()
            {
                note = noteText,
                created = DateTime.Now,
                publisher_name = publisherName,
                added_by_system_user = authorIdentity
            };
            context.PubNotes.AddObject(note);
        }

        public void Dispose()
        {
        }
    }
}
