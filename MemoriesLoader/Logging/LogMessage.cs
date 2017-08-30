using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesLoader.Logging
{
    /// <summary>
    /// Represents a log-message.
    /// </summary>
    public class LogMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class with a
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public LogMessage(LogLevel level, string message)
        {
            Level = level;
            Description = message;
        }

        /// <summary>
        /// Gets or sets the level of the log-message.
        /// </summary>
        public LogLevel Level { get; set; } = LogLevel.Undefined;

        /// <summary>
        /// Gets the time of the creation of the message.
        /// </summary>
        public DateTime Time { get; protected set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the description of the message.
        /// </summary>
        public string Description { get; set; } = String.Empty;
    }
}
