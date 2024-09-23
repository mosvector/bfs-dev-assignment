using DevAssignment.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using System.Linq;
using DevAssignment.Exceptions;

namespace DevAssignment.Tests.IO
{
    /// <summary>
    /// Test class for the FileSourceReader class
    /// </summary>
    [TestClass]
    public class FileSourceReaderTests
    {
        /// <summary>
        /// Test reading lines from null file path and empty file path
        /// </summary>
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("    ")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReadLinesWhenFilePathIsNullOrEmpty(string filePath)
        {
            _ = new FileSourceReader(filePath);
        }

        /// <summary>
        /// Test reading lines from unsupported file
        /// </summary>
        /// <param name="filePath"></param>
        [DataTestMethod]
        [DataRow("test.dat")]
        [DataRow("test.exe")]
        [ExpectedException(typeof(NotSupportedFormatException))]
        public void TestReadUnsupportFile(string filePath)
        {
            _ = new FileSourceReader(filePath);
        }

        /// <summary>
        /// Test reading lines from non-existent file
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestReadLinesWhenFileDoesNotExist()
        {
            _ = new FileSourceReader("nonexistentfile.txt");
        }

        [TestMethod]
        public void TestReadFile()
        {
            // Arrange
            var testInput = $"test_input_readfile.txt";
            using (StreamWriter writer = new StreamWriter(testInput))
            {
                writer.Write("Hello World 1!\n");
                writer.Write("Hello World 2!\n");
                writer.Write("Hello World 3!");
            }

            try
            {
                // Act
                ISourceReader reader = new FileSourceReader(testInput);
                var lines = reader.ReadLines().ToList();

                // Assert
                Assert.AreEqual(3, lines.Count);
                Assert.AreEqual("Hello World 1!", lines[0]);
                Assert.AreEqual("Hello World 2!", lines[1]);
                Assert.AreEqual("Hello World 3!", lines[2]);
            }
            finally
            {
                // Cleanup
                if (File.Exists(testInput))
                {
                    File.Delete(testInput);
                }
            }
        }
    }
}