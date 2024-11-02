using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkingFileWatcher;

namespace WorkingFileWatcherTests
{
    [TestClass]
    public class FileWatcherStringReaderTests
    {
        private IFileWatcherStringReader fileWatcherStringReader;

        [TestInitialize]
        public void Setup()
        {
            this.fileWatcherStringReader = new FileWatcherStringReader();
        }

        [TestMethod]
        public void ExtractFilesAndDirectoriesFromFile_ReturnsFalse_WhenToParseIsNullTest()
        {
            // Arrange
            string[] given = null;

            // Act
            bool actual = this.fileWatcherStringReader.ExtractFilesAndDirectoriesFromFile(given, out _);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ExtractFilesAndDirectoriesFromFile_ReturnsFalse_WhenGivenIsEmptyTest()
        {
            // Arrange
            string[] given = Array.Empty<string>();

            // Act
            bool actual = this.fileWatcherStringReader.ExtractFilesAndDirectoriesFromFile(given, out _);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ExtractFilesAndDirectoriesFromFile_FilesExtracts_WhenFileIsAddedDirectlyTest()
        {
            // Arrange
            WatchedFileInfo expected = new() { FileName = "filepath.txt", Destination = "directory" };
            string[] given = new[]
            {
                "<FileWatcher>",
                    "<Files>",
                        $"<File filepath=\"{expected.FileName}\" destination=\"{expected.Destination}\"/>",
                    "</Files>",
                "</FileWatcher>"
            };
            List<WatchedFileInfo> extracted = new();

            // Act
            this.fileWatcherStringReader.ExtractFilesAndDirectoriesFromFile(given, out extracted);

            // Assert
            Assert.IsNotNull(extracted);
            Assert.IsTrue(extracted.Any());
            Assert.AreEqual(expected.FileName, extracted[0].FileName);
            Assert.AreEqual(expected.Destination, extracted[0].Destination);
        }

        [TestMethod]
        public void ExtractFilesAndDirectoriesFromFile_FilesExtractsWithMultiple_WhenFileIsAddedDirectlyTest()
        {
            // Arrange
            WatchedFileInfo expected = new() { FileName = "filepath.txt", Destination = "directory" };
            WatchedFileInfo expected2 = new() { FileName = "filepath2.txt", Destination = "directory2" };
            string[] given = new[]
            {
                "<FileWatcher>",
                    "<Files>",
                        $"<File filepath=\"{expected.FileName}\" destination=\"{expected.Destination}\"/>",
                        $"<File filepath=\"{expected2.FileName}\" destination=\"{expected2.Destination}\"/>",
                    "</Files>",
                "</FileWatcher>"
            };
            List<WatchedFileInfo> extracted = new();

            // Act
            this.fileWatcherStringReader.ExtractFilesAndDirectoriesFromFile(given, out extracted);

            // Assert
            Assert.IsNotNull(extracted);
            Assert.AreEqual(2, extracted.Count());
            
            Assert.AreEqual(expected.FileName, extracted[0].FileName);
            Assert.AreEqual(expected.Destination, extracted[0].Destination);
            Assert.AreEqual(expected2.FileName, extracted[1].FileName);
            Assert.AreEqual(expected2.Destination, extracted[1].Destination);
        }

        [TestMethod]
        public void ExtractFilesAndDirectoriesFromFile_ReturnsTrue_WhenFileIsAddedCorrectlyTest()
        {
            // Arrange
            WatchedFileInfo expected = new() { FileName = "filepath.txt", Destination = "directory" };
            string[] given = new[]
            {
                "<FileWatcher>",
                    "<Files>",
                        $"<File filepath=\"{expected.FileName}\" destination=\"{expected.Destination}\"/>",
                    "</Files>",
                "</FileWatcher>"
            };
            List<WatchedFileInfo> extracted = new();

            // Act
            bool actual = this.fileWatcherStringReader.ExtractFilesAndDirectoriesFromFile(given, out extracted);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void ExtractFilesAndDirectoriesFromFile_ReturnsFalse_WhenFileHasAnXmlErrorTest()
        {
            // Arrange
            WatchedFileInfo expected = new() { FileName = "filepath.txt", Destination = "directory" };
            string[] given = new[]
            {
                "[FileWatcher>",
                    "<Files>",
                        $"<File filepath=\"{expected.FileName}\" destination=\"{expected.Destination}\"/>",
                    "</Files>",
                "</FileWatcher>"
            };
            List<WatchedFileInfo> extracted = new();

            // Act
            bool actual = this.fileWatcherStringReader.ExtractFilesAndDirectoriesFromFile(given, out extracted);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ExtractFilesAndDirectoriesFromFile_ReturnsTrueButNotTheFile_WhenFileNameIsNotFoundTest()
        {
            // Arrange
            WatchedFileInfo expected = new() { FileName = "filepath.txt", Destination = "directory" };
            string[] given = new[]
            {
                "<FileWatcher>",
                    "<Files>",
                        $"<File destination=\"{expected.Destination}\"/>",
                    "</Files>",
                "</FileWatcher>"
            };
            List<WatchedFileInfo> extracted = new();

            // Act
            bool actual = this.fileWatcherStringReader.ExtractFilesAndDirectoriesFromFile(given, out extracted);

            // Assert
            Assert.IsTrue(actual);
            Assert.IsFalse(extracted.Any());
        }

        [TestMethod]
        public void ExtractFilesAndDirectoriesFromFile_ReturnsTrueButNotTheFile_WhenFileNameIsWhitespaceTest()
        {
            // Arrange
            WatchedFileInfo expected = new() { FileName = "filepath.txt", Destination = "directory" };
            string[] given = new[]
            {
                "<FileWatcher>",
                    "<Files>",
                        $"<File filepath=\"  \" destination=\"{expected.Destination}\"/>",
                    "</Files>",
                "</FileWatcher>"
            };
            List<WatchedFileInfo> extracted = new();

            // Act
            bool actual = this.fileWatcherStringReader.ExtractFilesAndDirectoriesFromFile(given, out extracted);

            // Assert
            Assert.IsTrue(actual);
            Assert.IsFalse(extracted.Any());
        }

        [TestMethod]
        public void ExtractFilesAndDirectoriesFromFile_ReturnsTrueButNotTheFile_WhenDestinationIsNotFoundTest()
        {
            // Arrange
            WatchedFileInfo expected = new() { FileName = "filepath.txt", Destination = "directory" };
            string[] given = new[]
            {
                "<FileWatcher>",
                    "<Files>",
                        $"<File filepath=\"{expected.FileName}\"/>",
                    "</Files>",
                "</FileWatcher>"
            };
            List<WatchedFileInfo> extracted = new();

            // Act
            bool actual = this.fileWatcherStringReader.ExtractFilesAndDirectoriesFromFile(given, out extracted);

            // Assert
            Assert.IsTrue(actual);
            Assert.IsFalse(extracted.Any());
        }

        [TestMethod]
        public void ExtractFilesAndDirectoriesFromFile_ReturnsTrueButNotTheFile_WhenDestinationIsWhitespaceTest()
        {
            // Arrange
            WatchedFileInfo expected = new() { FileName = "filepath.txt", Destination = "directory" };
            string[] given = new[]
            {
                "<FileWatcher>",
                    "<Files>",
                        $"<File filepath=\"{expected.FileName}\" destination=\"  \"/>",
                    "</Files>",
                "</FileWatcher>"
            };
            List<WatchedFileInfo> extracted = new();

            // Act
            bool actual = this.fileWatcherStringReader.ExtractFilesAndDirectoriesFromFile(given, out extracted);

            // Assert
            Assert.IsTrue(actual);
            Assert.IsFalse(extracted.Any());
        }
    }
}
