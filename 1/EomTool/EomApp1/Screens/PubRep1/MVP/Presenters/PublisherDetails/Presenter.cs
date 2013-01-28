using EomTool.Domain.Concrete;
using EomTool.Domain.Entities;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EomApp1.Screens.PubRep1.MVP.PublisherDetails
{
    public abstract class Presenter<TView, TItem, TKey> where TView : IView<TItem, TKey>
    {
        protected TKey SelectedValue { get; private set; }
        protected TView View { get; set; }

        public Presenter(TView view)
        {
            View = (TView)view;
            View.SelectionChanged += OnSelectionChangedInternal;
            View.Add += OnAdd;
        }

        void OnSelectionChangedInternal(TKey selectedValue)
        {
            this.SelectedValue = selectedValue;
            OnSelectionChanged();
        }

        protected abstract void OnSelectionChanged();
        protected abstract bool OnAdd();
    }

    public class NotesPresenter : Presenter<INotes, PubNote, string>
    {
        Forms.NoteDialog noteDialog;

        public NotesPresenter(INotes view)
            : base(view)
        {
        }

        protected override void OnSelectionChanged()
        {
            Fill();
        }

        protected override bool OnAdd()
        {
            if (GetNote() && SaveNote())
            {
                Fill();
                return true;
            }
            return false;
        }

        void Fill()
        {
            using (var context = new EomEntities(EomAppCommon.EomAppSettings.ConnStr, false))
            using (var repository = new PublisherRelatedItemsRepository(context))
            {
                View.Fill(repository.Notes(SelectedValue).ToArray());
            }
        }

        bool GetNote()
        {
            this.noteDialog = new Forms.NoteDialog();

            // TODO: create view
            var dialogResult = UI.MaskedDialog.ShowDialog(
                                    (View as EomApp1.Screens.PubRep1.Controls.PublisherDetails.NotesUC).ParentForm,
                                    this.noteDialog);

            return (dialogResult == DialogResult.OK);
        }

        bool SaveNote()
        {
            try
            {
                using (var context = new EomEntities(EomAppCommon.EomAppSettings.ConnStr, false))
                using (var repository = new PublisherRelatedItemsRepository(context))
                {
                    repository.AddNote(SelectedValue, DAgents.Common.WindowsIdentityHelper.GetWindowsIdentityNameLower(), noteDialog.NoteText);
                    context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }

    public class AttachmentsPresenter : Presenter<IAttachments, PubAttachment, string>
    {
        OpenFileDialog openFileDialog;

        public AttachmentsPresenter(IAttachments view)
            : base(view)
        {
            View.OpenAttachment += OpenAttachment;
        }

        void OpenAttachment(int attachmentID)
        {
            using (var context = new EomEntities(EomAppCommon.EomAppSettings.ConnStr, false))
            using (var repository = new PublisherRelatedItemsRepository(context))
            {
                var attachment = repository.AttachmentContentById(attachmentID);
                string filePath = null;
                try
                {
                    filePath = WriteContentToTempFile(attachment);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing attachment content to temporary file: " + ex.Message);
                }
                if (filePath != null)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(filePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error opening attachment: " + ex.Message);
                    }
                }
            }
        }

        static string WriteContentToTempFile(PubAttachment attachment)
        {
            // Create a temp directory unique to the attachment id
            DirectoryInfo di = new DirectoryInfo(System.IO.Path.GetTempPath())
                                    .CreateSubdirectory("EomTool")
                                    .CreateSubdirectory("PubRepScreen")
                                    .CreateSubdirectory("PubAttachments")
                                    .CreateSubdirectory(attachment.id.ToString());

            // Write the binary content into the temp file
            string filePath = di.FullName + "\\" + attachment.name;
            File.WriteAllBytes(filePath, attachment.binary_content);
            return filePath;
        }

        protected override void OnSelectionChanged()
        {
            Fill();
        }

        protected override bool OnAdd()
        {
            if (GetAttachment() && SaveAttachment())
            {
                Fill();
                return true;
            }
            return false;
        }

        void Fill()
        {
            using (var context = new EomEntities(EomAppCommon.EomAppSettings.ConnStr, false))
            using (var repository = new PublisherRelatedItemsRepository(context))
                View.Fill(repository.Attachments(SelectedValue).ToArray());
        }

        bool GetAttachment()
        {
            this.openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 3;
            openFileDialog.Multiselect = false; // TODO: support multiselect
            openFileDialog.RestoreDirectory = true;
            return (this.openFileDialog.ShowDialog() == DialogResult.OK);
        }

        bool SaveAttachment()
        {
            try
            {
                Stream stream = this.openFileDialog.OpenFile();
                if (stream != null)
                {
                    using (stream)
                    using (var context = new EomEntities(EomAppCommon.EomAppSettings.ConnStr, false))
                    using (var repository = new PublisherRelatedItemsRepository(context))
                    {
                        repository.AddAttachment(
                            this.SelectedValue,
                            this.openFileDialog.FileName.Split('\\').Last(),
                            "attached " + DateTime.Now.ToString() + " by " + DAgents.Common.WindowsIdentityHelper.GetWindowsIdentityNameLower(),
                            ReadToEnd(stream));

                        context.SaveChanges();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk: " + ex.Message);
                return false;
            }
            return false;
        }

        static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;
            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }
            try
            {
                byte[] readBuffer = new byte[4096];
                int totalBytesRead = 0;
                int bytesRead;
                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;
                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }
                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}