using System;
using System.Linq;

namespace QBService
{
    public class Logger
    {
        public static string Input(string message, string parameters, string ticket)
        {
            using (var db = new LogModelContainer())
            {
                var entry = db.Entries.Add(new Entry
                {
                    Uid = Guid.NewGuid().ToString(),
                    Message = message,
                    Parameters = parameters,
                    Timestamp = DateTime.Now,
                    Ticket = ticket
                });
                db.SaveChanges();
                return entry.Uid;
            }
        }

        public static void Output(string uid, string document, string ticket)
        {
            using (var db = new LogModelContainer())
            {
                var entry = db.Entries.Single(c => c.Uid == uid);
                entry.Document = document;
                entry.Timestamp = DateTime.Now;
                entry.Ticket = ticket;
                db.SaveChanges();
            }
        }
    }
}