using Castle.Core.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkingFileWatcher;

namespace WorkingFileWatcherTests
{
    [TestClass]
    public class FileWatcherFileReaderTests
    {
        private List<string> deleteDirectories;
        private IFileWatcherFileReader fileWatcherFileReader;
        private Mock<IFileWatcherStringReader> mockFileWatcherStringReader;

        [TestInitialize]
        public void Setup()
        {
            this.deleteDirectories = new List<string>();
            this.mockFileWatcherStringReader = new Mock<IFileWatcherStringReader>();
            this.fileWatcherFileReader = new FileWatcherFileReader(this.mockFileWatcherStringReader.Object);
        }

        [TestCleanup]
        public void Teardown()
        {
            foreach(string dir in this.deleteDirectories)
            {
                System.IO.Directory.Delete(dir, true);
            }
        }

        [TestMethod]
        public void ExtractFilesAndDirectoriesFromFile_CallsStringReaderWithFileContents_WhenFileExistsTest()
        {
            // Arrange
            string newDirectory = GetFreshTempDirectory();
            this.deleteDirectories.Add(newDirectory);
            System.IO.File.AppendAllText(Path.Combine(newDirectory, "test.txt"), "content");

            var expected = new List<WatchedFileInfo>() { new WatchedFileInfo() { FileName = "content" } };
            this.mockFileWatcherStringReader
                .Setup(p => p.ExtractFilesAndDirectoriesFromFile
                    (It.IsAny<string[]>(), out expected)).Returns(true);

            // Act
            bool answer = this.fileWatcherFileReader
                .ExtractFilesAndDirectoriesFromFile(Path.Combine(newDirectory, "test.txt"), out List<WatchedFileInfo> actual);

            // Assert
            Assert.IsTrue(answer);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExtractFilesAndDirectoriesFromFile_ReturnsFalse_WhenFileDoesNotExistTest()
        {
            // Arrange
            string newDirectory = GetFreshTempDirectory();
            
            var expected = new List<WatchedFileInfo>();
            this.mockFileWatcherStringReader
                .Verify(p => p.ExtractFilesAndDirectoriesFromFile
                    (It.IsAny<string[]>(), out expected), Times.Never);

            // Act
            bool answer = this.fileWatcherFileReader
                .ExtractFilesAndDirectoriesFromFile(Path.Combine(newDirectory, "test.txt"), 
                    out List<WatchedFileInfo> actual);

            // Assert
            Assert.IsFalse(answer);
            this.mockFileWatcherStringReader.Verify();
        }

        private string GetFreshTempDirectory(bool doCreate = true)
        {
            string potentialPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            if (Directory.Exists(potentialPath) || File.Exists(potentialPath))
            {
                return GetFreshTempDirectory();
            }

            Directory.CreateDirectory(potentialPath);
            return potentialPath;
        }
    }
}
