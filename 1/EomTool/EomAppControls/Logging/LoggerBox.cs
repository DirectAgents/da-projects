using System;
using System.Linq;
using System.Windows.Forms;
using DAgents.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mainn.Controls.Logging
{
    public partial class LoggerBox : UserControl, ILogger
    {
        public LoggerBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Wheather or not to display the primary logging area.
        /// </summary>
        /// <value></value>
        public bool ShowLogMessages
        {
            get;
            set;
        }

        /// <summary>
        /// Wheather or not to display the secondary logging area.
        /// </summary>
        /// <value></value>
        public bool ShowErrorMessages
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<IgnoreItem> IgnoreItems
        {
            get { return _ignoreErrorMessageIfContains; }
            set { _ignoreErrorMessageIfContains = value; }
        }
        List<IgnoreItem> _ignoreErrorMessageIfContains = new List<IgnoreItem>();

        #region ILogger Members

        public void Log(string message)
        {
            //if (ShouldIgnore(message, MessageType.Error))
            //{
            //    return;
            //}

            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate()
                {
                    Log(message);
                });
            }
            else
            {
                if (primaryRichTextBox.Lines.Length > 1000)
                {
                    primaryRichTextBox.Clear();
                    primaryRichTextBox.AppendText("cleared after 1000 messages...");
                }

                primaryRichTextBox.AppendText(message + "\n");
                primaryRichTextBox.ScrollToCaret();
            }
        }

        public void LogError(string message)
        {
            //if (ShouldIgnore(message, MessageType.Error))
            //{
            //    return;
            //}

            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate()
                {
                    LogError(message);
                });
            }
            else
            {
                if (primaryRichTextBox.Lines.Length > 1000)
                {
                    primaryRichTextBox.Clear();
                    primaryRichTextBox.AppendText("cleared after 1000 messages...");
                }

                secondaryRichTextBox.AppendText(message + "\n");
                secondaryRichTextBox.ScrollToCaret();
            }
        }

        private bool ShouldIgnore(string message, MessageType messageType)
        {
            Predicate<IgnoreItem> predicate = ignoreItem =>
            {
                bool result = false;
                if (ignoreItem.MessageType == messageType)
                {
                    switch (ignoreItem.MatchType)
                    {
                        case MatchType.Equals:
                            result = ignoreItem.Value == message;
                            break;
                        case MatchType.Contains:
                            result = message.Contains(ignoreItem.Value);
                            break;
                        default:
                            break;
                    }
                }
                return result;
            };
            var shouldIgnore = IgnoreItems.Exists(predicate);
            return shouldIgnore;
        }

        #endregion

        private void clearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            primaryRichTextBox.Clear();
        }

        private void LogBox_Load(object sender, EventArgs e)
        {
            primaryRichTextBox.Visible = ShowLogMessages;
            secondaryRichTextBox.Visible = ShowErrorMessages;
        }
    }

    public enum MatchType
    {
        Equals,
        Contains
    }

    public enum MessageType
    {
        Message,
        Error
    }

    public class IgnoreItem
    {
        public IgnoreItem(MatchType matchType, MessageType messageType, string value)
        {
            MatchType = matchType;
            MessageType = messageType;
            Value = value;
        }
        public MatchType MatchType { get; set; }
        public MessageType MessageType { get; set; }
        public string Value { get; set; }
    }
}
