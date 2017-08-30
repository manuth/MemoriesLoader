using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesLoader.Logging
{
    /// <summary>
    /// Provides a target for a <see cref="Logger"/>.
    /// </summary>
    public abstract class LogTarget
    {
        /// <summary>
        /// Gets or sets the levels of the messages to log.
        /// </summary>
        public LogLevel Level { get; set; } = LogLevel.None;

        /// <summary>
        /// Gets or sets the formatter to format the log-messages.
        /// </summary>
        public LogFormatter Formatter { get; set; } = new LogFormatter();

        /// <summary>
        /// Logs a <see cref="LogMessage"/>.
        /// </summary>
        /// <param name="message">The <see cref="LogMessage"/> to log.</param>
        public virtual void Log(LogMessage message)
        {
            if (Level.HasFlag(message.Level))
            {
                LogInternal(Formatter.Format(message));
            }
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        protected abstract void LogInternal(string message);
    }
}
