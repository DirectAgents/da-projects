using EomApp1.Screens.PubRep1.MVP.PublisherDetails;
using System;
using System.Windows.Forms;

namespace EomApp1.Screens.PubRep1.Controls.PublisherDetails
{
    public partial class AttachmentsUC : UserControl, IAttachments
    {
        public AttachmentsUC()
        {
            InitializeComponent();
        }

        public event MessageEvent ItemCreated; // TODO/NOTE: this member is not part of an interface

        public void SetPublisher(string publisherName) 
        {
            SelectionChanged.Notify(publisherName);
        }

        void AddAttachmentClicked(object sender, EventArgs e)
        {
            if (Add.Notify())
                ItemCreated.Notify();
        }

        private void OpenAttachmentClicked(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var dataGridView = (DataGridView)sender;
                if (e.ColumnIndex == nameDataGridViewTextBoxColumn.Index)
                {
                    int attachmentID = (int)dataGridView1[idDataGridViewTextBoxColumn.Index, e.RowIndex].Value;
                    OpenAttachment.Notify(attachmentID);
                }
            }
        }

        public void Fill(EomTool.Domain.Entities.PubAttachment[] attachments)
        {
            this.pubAttachmentBindingSource.Clear();
            foreach (var attachement in attachments)
            {
                this.pubAttachmentBindingSource.Add(attachement);
            }
        }

        public event MessageEvent<string> SelectionChanged;
        public event MessageEvent Add;
        public event MessageEvent<int> OpenAttachment;
    }
}
