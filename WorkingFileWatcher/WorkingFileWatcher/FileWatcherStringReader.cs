using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WorkingFileWatcher
{
    /// <summary>
    /// Converts a string representation of FileWatcher and DirectoryWatcher files to objects.
    /// This is one level below a file reader.
    /// </summary>
    public class FileWatcherStringReader : IFileWatcherStringReader
    {
        /// <summary>
        /// Extracts watchable files and directory from a parsable string.
        /// </summary>
        /// <param name="toParse">Information to parse. </param>
        /// <param name="files">Files extracted. </param>
        /// <returns>True means extracted. </returns>
        public bool ExtractFilesAndDirectoriesFromFile(string[] toParse, out List<WatchedFileInfo> files)
        {
            files = new List<WatchedFileInfo>();
            if (toParse == null || !toParse.Any())
            {
                return false;
            }

            XDocument document = new XDocument();
            try
            {
                string joined = string.Join(' ', toParse);
                document = XDocument.Parse(joined);
            }
            catch (Exception)
            {
                return false;
            }

            ExtractFiles(document, files);

            return true;
        }

        /// <summary>
        /// Extract the files portion of the document.
        /// </summary>
        /// <param name="document">Document to extract from. </param>
        /// <param name="files">Files extracted. </param>
        private void ExtractFiles(XDocument document, List<WatchedFileInfo> files)
        {
            XElement? filesNode = document.Root?.Descendants().Where(x => x.Name.ToString().ToLower() == "files").FirstOrDefault();
            if (filesNode != null)
            {
                IEnumerable<XElement?> fileNodes =
                    filesNode.Descendants().Where(x => x.Name.ToString().ToLower() == "file").ToList();
                foreach (XElement? fileNode in fileNodes)
                {
                    string currentFilepath = string.Empty;
                    string currentDestination = string.Empty;

                    XAttribute? filepathAttribute = fileNode?.Attributes()
                        .Where(x => x.Name.ToString().ToLower() == "filepath").FirstOrDefault();
                    if (filepathAttribute != null)
                    {
                        currentFilepath = filepathAttribute.Value;
                    }

                    XAttribute? destinationAttribute = fileNode?.Attributes()
                        .Where(x => x.Name.ToString().ToLower() == "destination").FirstOrDefault();
                    if (destinationAttribute != null)
                    {
                        currentDestination = destinationAttribute.Value;
                    }

                    if (!string.IsNullOrWhiteSpace(currentFilepath) &&
                        !string.IsNullOrWhiteSpace(currentDestination))
                    {
                        files.Add(new WatchedFileInfo()
                        {
                            FileName = currentFilepath,
                            Destination = currentDestination,
                        });
                    }
                }
            }
        }
    }
}
