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
        /// <param name="richTextBox"></param>
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
                    Color messageColor;

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

                    Color originalColor = richTextBox.SelectionColor;
                    richTextBox.SelectionColor = messageColor;
                    base.Log(message);
                    richTextBox.SelectionColor = originalColor;
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
        protected override void LogInternal(string message)
        {
            richTextBox.AppendText(message + Environment.NewLine);
            richTextBox.ScrollToCaret();
        }
    }
}
