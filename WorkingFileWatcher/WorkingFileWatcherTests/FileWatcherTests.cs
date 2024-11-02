using Moq;
using WorkingFileWatcher;

namespace WorkingFileWatcherTests
{
    /// <summary>
    /// Sometimes it's best to move 'Start()' as close to the
    /// first delay, or when you expect the Tasks to begin for the
    /// tests to actually have enough time to setup everything.
    /// </summary>
    [TestClass]
    public class FileWatcherTests
    {
        /// <summary>
        /// To kick the File watcher we need to wait more than a second.
        /// This is the number of milliseconds we will wait for this.
        /// </summary>
        private const int OverOneSecondInMilliseconds = 1100;

        private IFileWatcher fileWatcher;
        private List<string> directoriesToDelete;
        private Mock<IFileWatcherFileReader> mockFileWatcherFileReader;

        [TestInitialize]
        public void Setup()
        {
            this.mockFileWatcherFileReader = new Mock<IFileWatcherFileReader>();
            this.fileWatcher = new FileWatcher(this.mockFileWatcherFileReader.Object);

            this.directoriesToDelete = new List<string>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.fileWatcher.Stop();
            foreach (string dir in this.directoriesToDelete)
            {
                System.IO.Directory.Delete(dir, true);
            }
        }


        [TestMethod]
        public async Task AddFileToWatch_MovesAFileToAnotherDirectory_WhenItIsEditedTest()
        {
            // Arrange
            string sourceTempPath = GetFreshTempDirectory();
            string destinationTempPath = GetFreshTempDirectory();
            this.directoriesToDelete.AddRange(new string[] {sourceTempPath, destinationTempPath});

            string testFile = "test.txt";
            string intialText = "Start text";
            string expectedText = "End text";
            System.IO.File.AppendAllText(Path.Combine(sourceTempPath, testFile), intialText);
            
            this.fileWatcher.AddFileToWatch(Path.Combine(sourceTempPath, testFile), destinationTempPath);
            Assert.IsFalse(File.Exists(Path.Combine(destinationTempPath, testFile)));

            // Act
            this.fileWatcher.Start();
            await Task.Delay(OverOneSecondInMilliseconds);
            System.IO.File.WriteAllText(Path.Combine(sourceTempPath, testFile), expectedText);
            await Task.Delay(OverOneSecondInMilliseconds);

            // Assert
            Assert.IsTrue(File.Exists(Path.Combine(destinationTempPath, testFile)));
            Assert.AreEqual(expectedText, File.ReadAllText(testFile));
        }

        [TestMethod]
        public async Task AddFileToWatch_MovesAFileUponASecondEdit_WhenItIsEditedTwiceTest()
        {
            // Arrange
            this.fileWatcher.Start();
            await Task.Delay(OverOneSecondInMilliseconds);

            string sourceTempPath = GetFreshTempDirectory();
            string destinationTempPath = GetFreshTempDirectory();
            this.directoriesToDelete.AddRange(new string[] { sourceTempPath, destinationTempPath });

            string testFile = "test.txt";
            string intialText = "Start text";
            string secondText = "Second text";
            string expectedText = "End text";
            System.IO.File.AppendAllText(Path.Combine(sourceTempPath, testFile), intialText);

            this.fileWatcher.AddFileToWatch(Path.Combine(sourceTempPath, testFile), destinationTempPath);
            Assert.IsFalse(File.Exists(Path.Combine(destinationTempPath, testFile)));

            await Task.Delay(OverOneSecondInMilliseconds);
            System.IO.File.WriteAllText(Path.Combine(sourceTempPath, testFile), secondText);
            Assert.IsTrue(File.Exists(Path.Combine(destinationTempPath, testFile)));

            // Act
            await Task.Delay(OverOneSecondInMilliseconds);
            System.IO.File.WriteAllText(Path.Combine(sourceTempPath, testFile), expectedText);
            await Task.Delay(OverOneSecondInMilliseconds);

            // Assert
            Assert.IsTrue(File.Exists(Path.Combine(destinationTempPath, testFile)));
            Assert.AreEqual(expectedText, File.ReadAllText(testFile));
        }

        [TestMethod]
        public void AddFileToWatch_ReturnFalse_WhenFileDoesNotExistTest()
        {
            // Arrange
            string sourceTempPath = GetFreshTempDirectory();
            string destinationTempPath = GetFreshTempDirectory();
            this.directoriesToDelete.AddRange(new string[] { sourceTempPath });
            string testFile = "test.txt";

            // Act
            bool actual = this.fileWatcher.AddFileToWatch(Path.Combine(sourceTempPath, testFile), destinationTempPath);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void AddFileToWatch_ReturnTrue_WhenFileExistsTest()
        {
            // Arrange
            string sourceTempPath = GetFreshTempDirectory();
            string destinationTempPath = GetFreshTempDirectory();
            this.directoriesToDelete.AddRange(new string[] { sourceTempPath });

            string testFile = "test.txt";
            string intialText = "Start text";
            System.IO.File.AppendAllText(Path.Combine(sourceTempPath, testFile), intialText);

            // Act
            bool actual = this.fileWatcher.AddFileToWatch(Path.Combine(sourceTempPath, testFile), destinationTempPath);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public async Task AddFileToWatch_DoesNotCopy_WhenFileExistsButOnlyAfterAFailedAddAttemptTest()
        {
            // Arrange
            this.fileWatcher.Start();

            string sourceTempPath = GetFreshTempDirectory();
            string destinationTempPath = GetFreshTempDirectory();
            this.directoriesToDelete.AddRange(new string[] { sourceTempPath, destinationTempPath });
            string testFile = "test.txt";

            this.fileWatcher.AddFileToWatch(Path.Combine(sourceTempPath, testFile), destinationTempPath);
            Assert.IsFalse(File.Exists(Path.Combine(destinationTempPath, testFile)));

            string intialText = "Start text";

            // Act
            await Task.Delay(OverOneSecondInMilliseconds);
            System.IO.File.WriteAllText(Path.Combine(sourceTempPath, testFile), intialText);
            await Task.Delay(OverOneSecondInMilliseconds);

            // Assert
            Assert.IsFalse(File.Exists(Path.Combine(destinationTempPath, testFile)));
        }

        [TestMethod]
        public async Task AddFileToWatch_CreatesDirectory_WhenDirectoryIsNotFoundTest()
        {
            // Arrange
            string sourceTempPath = GetFreshTempDirectory();
            string destinationTempPath = GetFreshTempDirectory(doCreate: false);
            this.directoriesToDelete.AddRange(new string[] { sourceTempPath, destinationTempPath });

            string testFile = "test.txt";
            string intialText = "Start text";
            string expectedText = "End text";
            System.IO.File.AppendAllText(Path.Combine(sourceTempPath, testFile), intialText);

            this.fileWatcher.AddFileToWatch(Path.Combine(sourceTempPath, testFile), destinationTempPath);

            // Act
            this.fileWatcher.Start();
            await Task.Delay(OverOneSecondInMilliseconds);
            System.IO.File.WriteAllText(Path.Combine(sourceTempPath, testFile), expectedText);
            await Task.Delay(OverOneSecondInMilliseconds);

            // Assert
            Assert.IsTrue(Directory.Exists(destinationTempPath));
            Assert.IsTrue(File.Exists(Path.Combine(destinationTempPath, testFile)));
            Assert.AreEqual(expectedText, File.ReadAllText(testFile));
        }

        private string GetFreshTempDirectory(bool doCreate = true)
        {
            string potentialPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            if(Directory.Exists(potentialPath) || File.Exists(potentialPath))
            {
                return GetFreshTempDirectory();
            }

            Directory.CreateDirectory(potentialPath);
            return potentialPath;
        }

        #region AddFilesAndDirectoryFromFile

        [TestMethod]
        public async Task AddFilesAndDirectoryFromFile_WatchesFile_WhenFileIsReadTest()
        {
            // Arrange

            string sourceTempPath = GetFreshTempDirectory();
            string destinationTempPath = GetFreshTempDirectory();
            this.directoriesToDelete.AddRange(new string[] { sourceTempPath, destinationTempPath });

            string testFile = "test.txt";
            string intialText = "Start text";
            string expectedText = "End text";
            System.IO.File.AppendAllText(Path.Combine(sourceTempPath, testFile), intialText);

            string givenFilePath = "givenPath";
            List<WatchedFileInfo> expected = new()
            {
                new WatchedFileInfo {
                    FileName = Path.Combine(sourceTempPath, testFile),
                    Destination = destinationTempPath
                }
            };
            this.mockFileWatcherFileReader
                .Setup(p => p.ExtractFilesAndDirectoriesFromFile
                    (givenFilePath, out expected)).Returns(true);

            this.fileWatcher.AddFilesAndDirectoryFromFile(givenFilePath);
            Assert.IsFalse(File.Exists(Path.Combine(destinationTempPath, testFile)));
            this.fileWatcher.Start();

            // Act
            await Task.Delay(OverOneSecondInMilliseconds);
            System.IO.File.WriteAllText(Path.Combine(sourceTempPath, testFile), expectedText);
            await Task.Delay(OverOneSecondInMilliseconds);

            // Assert
            Assert.IsTrue(File.Exists(Path.Combine(destinationTempPath, testFile)));
            Assert.AreEqual(expectedText, File.ReadAllText(testFile));
        }

        [TestMethod]
        public void AddFilesAndDirectoryFromFile_ReturnsTrue_WhenFileIsReadTest()
        {
            // Arrange
            string givenFilePath = "givenPath";
            List<WatchedFileInfo> expected = new();
            this.mockFileWatcherFileReader
                .Setup(p => p.ExtractFilesAndDirectoriesFromFile
                    (givenFilePath, out expected)).Returns(true);

            // Act
            bool actual = this.fileWatcher.AddFilesAndDirectoryFromFile(givenFilePath);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void AddFilesAndDirectoryFromFile_ReturnsFalse_WhenFileIsNotReadTest()
        {
            // Arrange
            string givenFilePath = "givenPath";
            List<WatchedFileInfo> expected = new();
            this.mockFileWatcherFileReader
                .Setup(p => p.ExtractFilesAndDirectoriesFromFile
                    (givenFilePath, out expected)).Returns(false);

            // Act
            bool actual = this.fileWatcher.AddFilesAndDirectoryFromFile(givenFilePath);

            // Assert
            Assert.IsFalse(actual);
        }

        #endregion
    }
}