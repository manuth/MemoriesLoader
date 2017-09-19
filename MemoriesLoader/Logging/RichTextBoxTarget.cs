using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoriesLoader.Logging
{
    /// <summary>
    /// Represents a textbox-target for a <see cref="Logger"/>.
    /// </summary>
    public class RichTextBoxTarget : LogTarget
    {
        /// <summary>
        /// The <see cref="RichTextBox"/> to log to.
        /// </summary>
        private RichTextBox richTextBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="RichTextBoxTarget"/> class.
        /// </summary>
        /// <param name="richTextBox">The <see cref="RichTextBox"/> to write the log to.</param>
        public RichTextBoxTarget(RichTextBox richTextBox)
        {
            this.richTextBox = richTextBox;
        }

        /// <summary>
        /// Logs a <see cref="LogMessage"/>.
        /// </summary>
        /// <param name="message">The <see cref="LogMessage"/> to log.</param>
        public override void Log(LogMessage message)
        {
            MethodInvoker action = new MethodInvoker(
                () =>
                {
                    base.Log(message);
                });

            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        protected override void LogInternal(LogMessage message)
        {
            Color messageColor;
            Color originalColor = richTextBox.SelectionColor;

            switch (message.Level)
            {
                case LogLevel.Debug:
                    messageColor = Color.Yellow;
                    break;
                case LogLevel.Warning:
                    messageColor = Color.Red;
                    break;
                default:
                    messageColor = richTextBox.SelectionColor;
                    break;
            }

            richTextBox.SelectionColor = messageColor;
            richTextBox.AppendText(Formatter.Format(message));
            richTextBox.ScrollToCaret();
            richTextBox.SelectionColor = originalColor;
            richTextBox.AppendText(Environment.NewLine);
        }
    }
}
