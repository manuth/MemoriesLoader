using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesLoader.Logging
{
    /// <summary>
    /// Provides the functionallity to log procedures.
    /// </summary>
    public partial class Logger
    {
        /// <summary>
        /// Gets or sets the targets to log to.
        /// </summary>
        public List<LogTarget> Targets { get; set; } = new List<LogTarget>();

        /// <summary>
        /// Logs a <paramref name="message"/> with the specified <see cref="LogLevel"/>.
        /// </summary>
        /// <param name="level">The <see cref="LogLevel"/> of the message.</param>
        /// <param name="message">The message to log.</param>
        public void Log(LogLevel level, string message)
        {
            Log(new LogMessage(level, message));
        }

        /// <summary>
        /// Logs a <see cref="LogMessage"/>.
        /// </summary>
        /// <param name="message">The <see cref="LogMessage"/> to log.</param>
        public void Log(LogMessage message)
        {
            foreach (LogTarget target in Targets)
            {
                target.Log(message);
            }
        }

        /// <summary>
        /// Logs an info-message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Info(string message)
        {
            Log(new LogMessage(LogLevel.Info, message));
        }

        /// <summary>
        /// Logs a warning.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Warn(string message)
        {
            Log(new LogMessage(LogLevel.Warning, message));
        }

        /// <summary>
        /// Logs a debug-message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Debug(string message)
        {
            Log(new LogMessage(LogLevel.Debug, message));
        }
    }
}
