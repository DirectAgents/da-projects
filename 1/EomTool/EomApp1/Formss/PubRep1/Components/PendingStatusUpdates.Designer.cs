namespace EomApp1.Formss.PubRep1.Components
{
    partial class PendingStatusUpdates
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.publisherReportDataSet11 = new EomApp1.Formss.PubRep1.Data.PublisherReportDataSet1();
            this.itemIDsForStatusChangeTableAdapter1 = new EomApp1.Formss.PubRep1.Data.PublisherReportDataSet1TableAdapters.ItemIDsForStatusChangeTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.publisherReportDataSet11)).BeginInit();
            // 
            // publisherReportDataSet11
            // 
            this.publisherReportDataSet11.DataSetName = "PublisherReportDataSet1";
            this.publisherReportDataSet11.EnforceConstraints = false;
            this.publisherReportDataSet11.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // itemIDsForStatusChangeTableAdapter1
            // 
            this.itemIDsForStatusChangeTableAdapter1.ClearBeforeFill = true;
            ((System.ComponentModel.ISupportInitialize)(this.publisherReportDataSet11)).EndInit();

        }

        #endregion

        private Data.PublisherReportDataSet1 publisherReportDataSet11;
        private Data.PublisherReportDataSet1TableAdapters.ItemIDsForStatusChangeTableAdapter itemIDsForStatusChangeTableAdapter1;
    }
}
