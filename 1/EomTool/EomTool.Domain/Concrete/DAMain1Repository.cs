﻿using System.Collections.Generic;
using System.Linq;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;

namespace EomTool.Domain.Concrete
{
    public class DAMain1Repository : IDAMain1Repository
    {
        DAMain1Entities db;
        public DAMain1Repository()
        {
            this.db = new DAMain1Entities();
        }

        private List<DADatabase> daDatabases;
        public List<DADatabase> DADatabases
        {
            get
            {
                if (this.daDatabases == null)
                {
                    this.daDatabases = db.DADatabases
                                         .OrderByDescending(c => c.effective_date)
                                         .ToList();
                }
                return this.daDatabases;
            }
        }

        // --- PublisherNotes ---

        public IQueryable<PublisherNote> PublisherNotes
        {
            get { return db.PublisherNotes; }
        }

        public IQueryable<PublisherNote> PublisherNotesForPublisher(string pubName)
        {
            return db.PublisherNotes.Where(n => n.publisher_name == pubName);
        }

        public void AddPublisherNote(string pubName, string note, string identity)
        {
            var pubNote = new PublisherNote()
            {
                note = note,
                added_by_system_user = identity,
                publisher_name = pubName
            };
            db.PublisherNotes.AddObject(pubNote);
            db.SaveChanges();
        }
    }
}
