using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingFileWatcher
{
    /// <summary>
    /// Reads File Watcher File.
    /// </summary>
    public class FileWatcherFileReader : IFileWatcherFileReader
    {

        private IFileWatcherStringReader fileWatcherStringReader;

        public FileWatcherFileReader(IFileWatcherStringReader reader)
        {
            this.fileWatcherStringReader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        /// <summary>
        /// Extracts files to watch from file.
        /// </summary>
        /// <param name="filename">Filename to read. </param>
        /// <param name="files">Files extracted. </param>
        /// <returns>True means extracted. </returns>
        public bool ExtractFilesAndDirectoriesFromFile(string filename, out List<WatchedFileInfo> files)
        {
            files = new List<WatchedFileInfo>();
            bool extracted = false;
            if(File.Exists(filename))
            {
                string[] fileContents = File.ReadAllLines(filename);
                extracted = this.fileWatcherStringReader.ExtractFilesAndDirectoriesFromFile(fileContents, out files);

            }

            return extracted;
        }
    }
}
