using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingFileWatcher
{
    /// <summary>
    /// Stores all known information about the file being watched.
    /// </summary>
    public class WatchedFileInfo
    {
        /// <summary>
        /// File name to watch.
        /// </summary>
        public string FileName;

        /// <summary>
        /// Destination Directory.
        /// </summary>
        public string Destination;

        /// <summary>
        /// Last time file updated.
        /// </summary>
        public DateTime LastUpdated;

        /// <summary>
        /// True means updated since we started watching.
        /// </summary>
        public bool EverUpdated;
    }
}
