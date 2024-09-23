using DevAssignment.Exceptions;
using DevAssignment.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DevAssignment.Tests.IO
{
    /// <summary>
    /// Test class for the FileResultWriter class
    /// </summary>
    [TestClass]
    public class FileResultWriterTests
    {
        /// <summary>
        /// Test data for the TestWriteOutputFile method
        /// </summary>
        public static IEnumerable<object[]> WordFreqData
        {
            get
            {
                return new[]
                {
                    // two words with the same frequency and starting with the same letter at last two positions
                    new object[] {
                        new ConcurrentDictionary<string, int> {
                            ["hello"] = 2,
                            ["apple"] = 1,
                            ["again"] = 1
                        },
                        new string[] {
                            "hello,2",
                            "again,1",
                            "apple,1"
                        }
                    },
                    // two words with the same frequency and starting with different letters at first two positions
                    new object[] {
                        new ConcurrentDictionary<string, int> {
                            ["hello"] = 2,
                            ["apple"] = 3,
                            ["again"] = 3
                        },
                        new string[] {
                            "again,3",
                            "apple,3",
                            "hello,2"
                        }
                    },
                };
            }
        }

        /// <summary>
        /// Test writing to a file with an unsupported extension
        /// </summary>
        /// <param name="filePath"></param>
        [DataTestMethod]
        [DataRow("test.dat")]
        [DataRow("test.exe")]
        [ExpectedException(typeof(NotSupportedFormatException))]
        public void TestWriteUnsupportFile(string filePath)
        {
            _ = new FileResultWriter(filePath);
        }


        /// <summary>
        /// Test writing to a file with an exists file name
        /// </summary>
        [TestMethod]
        public void TestOverwriteSameFileName()
        {
            // Arrange
            var testOutput = "test_output_same.txt";
            File.WriteAllText(testOutput, "Hello World");

            ConcurrentDictionary<string, int> wordFrequencies = new ConcurrentDictionary<string, int>();

            try
            {
                // Act
                IResultWriter writer = new FileResultWriter(testOutput);
                writer.WriteResult(wordFrequencies);
            }
            catch (ArgumentException ex)
            {
                // Assert
                Assert.AreEqual("Output file already exists.", ex.Message);
            }
            finally
            {
                // Cleanup
                if (File.Exists(testOutput))
                {
                    File.Delete(testOutput);
                }
            }
        }

        /// <summary>
        /// Test output file has been sorted first by FREQUENCY and then by WORD
        /// </summary>
        [TestMethod]
        [DynamicData(nameof(WordFreqData))]
        public void TestWriteOutputFile(ConcurrentDictionary<string, int> wordFrequencies, string[] expected)
        {
            // Arrange
            var testOutput = "test_output.txt";
#if DEBUG
            // Start the stopwatch
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Measure initial memory usage
            long initialMemory = GC.GetTotalMemory(true);
#endif
            // Act
            IResultWriter writer = new FileResultWriter(testOutput);
            writer.WriteResult(wordFrequencies);
#if DEBUG
            // Stop the stopwatch
            stopwatch.Stop();

            // Measure final memory usage
            long finalMemory = GC.GetTotalMemory(true);

            // Calculate memory used
            long memoryUsed = finalMemory - initialMemory;

            // Display elapsed time and memory used
            Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Memory used: {memoryUsed} bytes");
#endif
            // Assert
            var lines = File.ReadAllLines(testOutput);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], lines[i]);
            }

            // Cleanup
            if (File.Exists(testOutput))
            {
                File.Delete(testOutput);
            }
        }
    }
}
