using EomApp1.Screens.PubRep1.Views;
using EomTool.Domain.Concrete;
using EomTool.Domain.Entities;
using System.Linq;
using System.Windows.Forms;

namespace EomApp1.Screens.PubRep1.Presenters
{
    public class PublisherNotesPresenter
    {
        IPublisherNotesView view;
        string publisherName;

        public PublisherNotesPresenter(IPublisherNotesView view)
        {
            this.view = view;
            this.view.PublisherChangedEvent += view_PublisherChangedEvent;
            this.view.AddPublisherNoteEvent += view_AddPublisherNoteEvent;
        }

        void view_PublisherChangedEvent(object sender, PublisherChangedEventArgs e)
        {
            this.publisherName = e.PublisherName;

            using (var context = new EomEntities(EomAppCommon.EomAppSettings.ConnStr, false))
            using (var repository = new PublisherRelatedItemsRepository(context))
            {
                var notes = repository.Notes(this.publisherName);
                this.view.FillPublisherNotes(notes.ToArray());
            }
        }

        void view_AddPublisherNoteEvent(object sender, AddPublisherNoteEventArgs e)
        {
            var noteDialog = new Forms.NoteDialog();
            // todo: get rid of cast used to get parent form
            var dialogResult = UI.MaskedDialog.ShowDialog((view as EomApp1.Screens.PubRep1.Controls.PublisherNotes).ParentForm, noteDialog);
            if (dialogResult == DialogResult.OK)
            {
                using (var context = new EomEntities(EomAppCommon.EomAppSettings.ConnStr, false))
                using (var repository = new PublisherRelatedItemsRepository(context))
                {
                    repository.AddNote(
                        this.publisherName,
                        DAgents.Common.WindowsIdentityHelper.GetWindowsIdentityNameLower(),
                        noteDialog.NoteText);

                    context.SaveChanges();
                    e.Success = true;

                    var notes = repository.Notes(this.publisherName);
                    this.view.FillPublisherNotes(notes.ToArray());
                }
            }
        }
    }
}
