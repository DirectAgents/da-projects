﻿using System;
using System.Windows.Forms;

namespace EomApp1.Screens.Final
{
    public partial class ConfirmationBox : Form
    {
        private readonly string _question;

        public ConfirmationBox(string question)
        {
            InitializeComponent();

            _question = question;

            // Wire up event so we know when to close the dialog.
            confirmActionUserControl1.Done += ConfirmActionUserControl1ChoiceSelected;
        }

        private void ConfirmActionUserControl1ChoiceSelected(object sender, EventArgs e)
        {
            Close();
        }

        private void ConfirmActionDialogLoad(object sender, EventArgs e)
        {
            SetDesktopLocation(MousePosition.X - 100, MousePosition.Y);
            Text = _question;
        }

        // pattern: instance version of functionality
        public bool ShowConfirmationModalDialog(out string notes)
        {
            ShowDialog(Form.ActiveForm);
            notes = confirmActionUserControl1.NotesText;
            return confirmActionUserControl1.IsOk;
        }

        /// <summary>
        /// intrinsically, functionality has
        ///     inputs
        ///     outputs
        ///     side effects
        /// for a static call
        ///     inputs are the in params
        ///     outputs are the out params and return type
        /// for an instance call
        ///     inputs are the in params and (potentially a subset of) object state
        ///     outputs are the out params and (potentially a subset of) object state
        /// </summary>
        /// <param name="message"></param>
        /// <param name="notes"></param>
        /// <returns></returns>
        public static bool ShowConfirmationModalDialog(string message, out string notes)
        {
            var dialog = new ConfirmationBox(message);
            return dialog.ShowConfirmationModalDialog(out notes);
        }

        public static bool Confirm(Form owner, string text, string caption)
        {
            var dialog = MessageBox.Show(text, caption, MessageBoxButtons.OKCancel);
            return (dialog == DialogResult.OK);
        }
    }
}