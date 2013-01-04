using System.Collections.Generic;
using System.Linq;
using EomTool.Domain.Entities;

namespace EomTool.Domain.Abstract
{
    public interface IDAMain1Repository
    {
        List<DADatabase> DADatabases { get; }

        IQueryable<PublisherNote> PublisherNotes { get; }
        IQueryable<PublisherNote> PublisherNotesForPublisher(string pubName);
        void AddPublisherNote(string pubName, string note, string identity);
    }
}
