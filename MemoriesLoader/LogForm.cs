using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoriesLoader
{
    /// <summary>
    /// Provides a form with a <see cref="System.Windows.Forms.RichTextBox"/> to show log-messages.
    /// </summary>
    public partial class LogForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogForm"/> class.
        /// </summary>
        public LogForm()
        {
            InitializeComponent();
            CreateHandle();
        }

        /// <summary>
        /// Cancels closing the form if the Form-Closing's performed by the user.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="FormClosingEventArgs"/> that contains the event data.</param>
        private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender is Form form)
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    e.Cancel = true;
                    form.Hide();
                }
            }
        }

        /// <summary>
        /// Opens up the link that has been clicked.
        /// </summary>
        /// <param name="sender">The source of the object.</param>
        /// <param name="e">The <see cref="LinkClickedEventArgs"/> that contains the event data.</param>
        private void RichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }
    }
}
