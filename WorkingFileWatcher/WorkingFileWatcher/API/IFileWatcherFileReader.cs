using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingFileWatcher
{
    /// <summary>
    /// Reads File Watcher File
    /// </summary>
    public interface IFileWatcherFileReader
    {
        /// <summary>
        /// Extracts files to watch from file.
        /// </summary>
        /// <param name="filename">Filename to read. </param>
        /// <param name="files">Files extracted. </param>
        /// <returns>True means extracted. </returns>
        bool ExtractFilesAndDirectoriesFromFile(string filename, out List<WatchedFileInfo> files); 
    }
}
