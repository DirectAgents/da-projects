using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;
using System;
using System.Linq;

namespace EomTool.Domain.Concrete
{
    public class DAMain1Repository : IDAMain1Repository, IDisposable
    {
        DAMain1Entities db;
        public DAMain1Repository()
        {
            this.db = new DAMain1Entities();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        // --- EOM Databases ---

        public IQueryable<DADatabase> DADatabases
        {
            get { return db.DADatabases; }
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

        // --- Settings ---

        public Setting GetSetting(string name)
        {
            return db.Settings.FirstOrDefault(s => s.SettingName == name);
        }

        public string GetSettingValue(string name)
        {
            var setting = GetSetting(name);
            if (setting != null)
                return setting.SettingValue;
            else
                return null;
        }

        public int? GetSettingIntValue(string name)
        {
            var settingVal = GetSettingValue(name);
            int intVal;
            if (int.TryParse(settingVal, out intVal))
                return intVal;
            else
                return null;
        }

        public decimal? GetSettingDecimalValue(string name)
        {
            var settingVal = GetSettingValue(name);
            decimal decimalVal;
            if (decimal.TryParse(settingVal, out decimalVal))
                return decimalVal;
            else
                return null;
        }

        public void SaveSetting(string name, string value)
        {
            var setting = GetSetting(name);
            if (setting != null)
                setting.SettingValue = value;
            else
            {
                setting = new Setting()
                {
                    SettingId = MaxSettingId() + 1,
                    SettingName = name,
                    SettingValue = value
                };
                db.Settings.AddObject(setting);
            }
            db.SaveChanges();
        }

        public void SaveSetting(string name, int? value)
        {
            SaveSetting(name, value.ToString());
        }
        public void SaveSetting(string name, decimal? value)
        {
            SaveSetting(name, value.ToString());
        }

        private int MaxSettingId()
        {
            int maxSettingId = 0;
            if (db.Settings.Any())
                maxSettingId = db.Settings.Max(s => s.SettingId);
            return maxSettingId;
        }

        // ---

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    this.db.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
