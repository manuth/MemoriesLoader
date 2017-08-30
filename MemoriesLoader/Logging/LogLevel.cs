
using System;

namespace MemoriesLoader.Logging
{
    /// <summary>
    /// Defines constants to declare the level of a log-message.
    /// 
    /// This enumeration has a <see cref="FlagsAttribute"/> attribute that allows a bitwise combination of its member values.
    /// </summary>
    [Flags]
    public enum LogLevel
    {
        /// <summary>
        /// Undefined log-level.
        /// </summary>
        Undefined = -1,

        /// <summary>
        /// No log-level.
        /// </summary>
        None = 0,

        /// <summary>
        /// Log-level for information-messages.
        /// </summary>
        Info = 1,

        /// <summary>
        /// Log-level for warnings.
        /// </summary>
        Warning = 2,

        /// <summary>
        /// Log-level for debug-messages.
        /// </summary>
        Debug = 4,

        /// <summary>
        /// All log-levels.
        /// </summary>
        All = 7
    }
}