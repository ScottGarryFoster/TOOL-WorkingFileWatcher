using WorkingFileWatcher;

namespace WorkingFileWatcherTests
{
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

        [TestInitialize]
        public void Setup()
        {
            this.fileWatcher = new FileWatcher();
            this.fileWatcher.Start();
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
        public void AddFileToWatch_MovesAFileToAnotherDirectory_WhenItIsEditedTest()
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
            Thread.Sleep(OverOneSecondInMilliseconds);
            System.IO.File.WriteAllText(Path.Combine(sourceTempPath, testFile), expectedText);
            Thread.Sleep(OverOneSecondInMilliseconds);

            // Assert
            Assert.IsTrue(File.Exists(Path.Combine(destinationTempPath, testFile)));
            Assert.AreEqual(expectedText, File.ReadAllText(testFile));
        }

        [TestMethod]
        public void AddFileToWatch_MovesAFileUponASecondEdit_WhenItIsEditedTwiceTest()
        {
            // Arrange
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

            Thread.Sleep(OverOneSecondInMilliseconds);
            System.IO.File.WriteAllText(Path.Combine(sourceTempPath, testFile), secondText);
            Assert.IsTrue(File.Exists(Path.Combine(destinationTempPath, testFile)));

            // Act
            Thread.Sleep(OverOneSecondInMilliseconds);
            System.IO.File.WriteAllText(Path.Combine(sourceTempPath, testFile), expectedText);
            Thread.Sleep(OverOneSecondInMilliseconds);

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
        public void AddFileToWatch_DoesNotCopy_WhenFileExistsButOnlyAfterAFailedAddAttemptTest()
        {
            // Arrange
            string sourceTempPath = GetFreshTempDirectory();
            string destinationTempPath = GetFreshTempDirectory();
            this.directoriesToDelete.AddRange(new string[] { sourceTempPath, destinationTempPath });
            string testFile = "test.txt";

            this.fileWatcher.AddFileToWatch(Path.Combine(sourceTempPath, testFile), destinationTempPath);
            Assert.IsFalse(File.Exists(Path.Combine(destinationTempPath, testFile)));

            string intialText = "Start text";

            // Act
            Thread.Sleep(OverOneSecondInMilliseconds);
            System.IO.File.WriteAllText(Path.Combine(sourceTempPath, testFile), intialText);
            Thread.Sleep(OverOneSecondInMilliseconds);

            // Assert
            Assert.IsFalse(File.Exists(Path.Combine(destinationTempPath, testFile)));
        }

        [TestMethod]
        public void AddFileToWatch_CreatesDirectory_WhenDirectoryIsNotFoundTest()
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
            Thread.Sleep(OverOneSecondInMilliseconds);
            System.IO.File.WriteAllText(Path.Combine(sourceTempPath, testFile), expectedText);
            Thread.Sleep(OverOneSecondInMilliseconds);

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
    }
}