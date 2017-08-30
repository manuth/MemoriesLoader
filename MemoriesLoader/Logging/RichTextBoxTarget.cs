using System;
using System.Collections.Generic;
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
        /// Logs a message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        protected override void LogInternal(string message)
        {
            Action action = () =>
            {
                richTextBox.AppendText(message + Environment.NewLine);
                richTextBox.ScrollToCaret();
            };

            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke(action);
            }
            else
            {
                action();
            }
        }
    }
}
