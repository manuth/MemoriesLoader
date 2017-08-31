using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesLoader.Logging
{
    /// <summary>
    /// Provides the functionallity to format <see cref="LogMessage"/>s.
    /// </summary>
    public class LogFormatter
    {
        /// <summary>
        /// Gets or sets the <see cref="Func{LogMessage, string}"/> that is used to format <see cref="LogMessage"/>s.
        /// </summary>
        public Func<LogMessage, string> Formatter { get; set; } = new Func<LogMessage, string>(message =>
        {
              return $"{message.Time.ToString()}: {message.Level.ToString().PadRight(5)} {message.Description}";
        });

        /// <summary>
        /// Formats a <see cref="LogMessage"/> to a string.
        /// </summary>
        /// <param name="message">The <see cref="LogMessage"/> to format.</param>
        /// <returns>A string representing the <see cref="LogMessage"/>.</returns>
        public string Format(LogMessage message)
        {
            return Formatter(message);
        }
    }
}
