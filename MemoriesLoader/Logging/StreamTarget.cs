using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesLoader.Logging
{
    /// <summary>
    /// Represents a stream-target for a <see cref="Logger"/>.
    /// </summary>
    public class StreamTarget : LogTarget
    {
        /// <summary>
        /// Gets the encoding to write to the stream.
        /// </summary>
        public Encoding Encoding { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="System.IO.Stream"/> to log to.
        /// </summary>
        public Stream Stream { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamTarget"/> class with a stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to log to.</param>
        public StreamTarget(Stream stream)
        {
            Encoding = Encoding.UTF8;
            Stream = stream;
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        protected override void LogInternal(string message)
        {
            using (BinaryWriter writer = new BinaryWriter(Stream, Encoding, true))
            {
                writer.Write(Encoding.GetBytes(message + Environment.NewLine));
                Stream.Flush();
            }
        }
    }
}
