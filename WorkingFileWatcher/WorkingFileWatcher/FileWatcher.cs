using System.IO;

namespace WorkingFileWatcher
{
    /// <summary>
    /// Watches certain files and directories and copies them to other locations.
    /// </summary>
    public class FileWatcher : IFileWatcher
    {
        /// <summary>
        /// True means File watcher is running.
        /// </summary>
        private bool isRunning;

        /// <summary>
        /// List of Files to watch.
        /// </summary>
        private List<WatchedFileInfo> filesToWatch;

        /// <summary>
        /// Reads File Watcher File.
        /// </summary>
        private readonly IFileWatcherFileReader fileWatcherFileReader;

        public FileWatcher(IFileWatcherFileReader fileWatcherFileReader)
        {
            this.fileWatcherFileReader = fileWatcherFileReader ?? 
                throw new ArgumentNullException(nameof(fileWatcherFileReader));
            this.filesToWatch = new List<WatchedFileInfo>();
        }

        /// <summary>
        /// Adds the given file to watch.
        /// The application will be watched and then copied to the given directory on edit.
        /// </summary>
        /// <param name="path">Path to copy. </param>
        /// <param name="directory">Directory to copy to. </param>
        /// <returns>True means could add path and directory. </returns>
        public bool AddFileToWatch(string path, string directory)
        {
            if(!File.Exists(path))
            {
                return false;
            }

            var watchedFileInfo = new WatchedFileInfo();
            watchedFileInfo.FileName = path;
            watchedFileInfo.Destination = directory;
            watchedFileInfo.EverUpdated = false;
            this.filesToWatch.Add(watchedFileInfo);

            return true;
        }

        /// <summary>
        /// Add files or directories using a filewatcher file.
        /// </summary>
        /// <param name="path">File path to a filewatcher file. </param>
        /// <returns>True means added. </returns>
        public bool AddFilesAndDirectoryFromFile(string path)
        {
            bool extracted = this.fileWatcherFileReader
                .ExtractFilesAndDirectoriesFromFile(
                    path, out List<WatchedFileInfo> files);
            if(extracted)
            {
                this.filesToWatch.AddRange(files);
            }

            return extracted;
        }

        /// <summary>
        /// Run the File Watcher.
        /// </summary>
        public void Start()
        {
            this.isRunning = true;
            LookForChanges();
        }

        /// <summary>
        /// Stops the File Watcher.
        /// </summary>
        public void Stop()
        {
            this.isRunning = false;
        }

        /// <summary>
        /// Main loop to look for changes.
        /// </summary>
        private async Task LookForChanges()
        {
            await Task.Run(() =>
            {
                while (isRunning)
                {
                    foreach(WatchedFileInfo file in this.filesToWatch)
                    {
                        if(!File.Exists(file.FileName))
                        {
                            continue;
                        }

                        DateTime lastModified = File.GetLastWriteTime(file.FileName);
                        if(!file.EverUpdated && lastModified < DateTime.Now)
                        {
                            CopyFile(file);
                        }
                        // This is not tested yet
                        else if(lastModified > file.LastUpdated)
                        {
                            CopyFile(file);
                        }
                    }
                    Task.Delay(1000);
                }
            });
        }

        /// <summary>
        /// Copies file to destination.
        /// </summary>
        /// <param name="watchedFileInfo">Stores all known information about the file being watched. </param>
        private void CopyFile(WatchedFileInfo watchedFileInfo)
        {
            string from = watchedFileInfo.FileName;
            string to = Path.Combine(watchedFileInfo.Destination, Path.GetFileName(watchedFileInfo.FileName));
            File.Copy(from, to, true);
            watchedFileInfo.LastUpdated = File.GetLastWriteTime(watchedFileInfo.FileName);
            watchedFileInfo.EverUpdated = true;
        }
    }
}
