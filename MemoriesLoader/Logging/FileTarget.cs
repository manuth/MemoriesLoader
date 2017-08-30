using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesLoader.Logging
{
    /// <summary>
    /// Represents a file-target for a <see cref="Logger"/>.
    /// </summary>
    public class FileTarget : StreamTarget
    {
        /// <summary>
        /// Gets the file to log to.
        /// </summary>
        public FileInfo FileInfo { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileTarget"/> class with a file-name.
        /// </summary>
        /// <param name="fileName">The name of the file to log to.</param>
        public FileTarget(string fileName) : base(File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
        {
            FileInfo = new FileInfo(fileName);
            Stream.Position = Stream.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileTarget"/> class with a <see cref="FileStream"/>.
        /// </summary>
        /// <param name="fileStream">The stream of the file to log to.</param>
        public FileTarget(FileStream fileStream) : base(fileStream)
        {
            FileInfo = new FileInfo(fileStream.Name);
        }
    }
}
