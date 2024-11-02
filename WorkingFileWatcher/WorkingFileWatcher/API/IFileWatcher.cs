using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingFileWatcher
{
    /// <summary>
    /// Watches certain files and directories and copies them to other locations.
    /// </summary>
    public interface IFileWatcher
    {
        /// <summary>
        /// Adds the given file to watch.
        /// The application will be watched and then copied to the given directory on edit.
        /// </summary>
        /// <param name="path">Path to copy. </param>
        /// <param name="directory">Directory to copy to. </param>
        /// <returns>True means could add path and directory. </returns>
        bool AddFileToWatch(string path, string directory);

        /// <summary>
        /// Add files or directories using a filewatcher file.
        /// </summary>
        /// <param name="path">File path to a filewatcher file. </param>
        /// <returns>True means added. </returns>
        bool AddFilesAndDirectoryFromFile(string path);

        /// <summary>
        /// Run the File Watcher.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the File Watcher.
        /// </summary>
        void Stop();
    }
}
