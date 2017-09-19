using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesLoader.Logging
{
    /// <summary>
    /// Represents a console-target for a <see cref="Logger"/>.
    /// </summary>
    public class ConsoleTarget : LogTarget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleTarget"/> class.
        /// </summary>
        public ConsoleTarget()
        {
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        protected override void LogInternal(LogMessage message)
        {
            Console.WriteLine(Formatter.Format(message));
        }
    }
}
