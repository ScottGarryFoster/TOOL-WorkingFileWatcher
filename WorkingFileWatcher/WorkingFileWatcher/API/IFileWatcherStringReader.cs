using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingFileWatcher
{
    /// <summary>
    /// Converts a string representation of FileWatcher and DirectoryWatcher files to objects.
    /// This is one level below a file reader.
    /// </summary>
    public interface IFileWatcherStringReader
    {
        /// <summary>
        /// Extracts watchable files and directory from a parsable string.
        /// </summary>
        /// <param name="toParse">Information to parse. </param>
        /// <param name="files">Files extracted. </param>
        /// <returns>True means extracted. </returns>
        bool ExtractFilesAndDirectoriesFromFile(string[] toParse, out List<WatchedFileInfo> files);
    }
}
