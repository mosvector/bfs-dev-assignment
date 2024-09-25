using DevAssignment.IO;
using DevAssignment.Processors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DevAssignment.Tests.Processors
{
    /// <summary>
    /// Test class for the WordFrequencyProcessor class
    /// </summary>
    [TestClass]
    public class WordFrequencyProcessorTests
    {
        /// <summary>
        /// Test the words are delimited by space, new lines, and punctuation marks.
        /// </summary>
        [TestMethod]
        public void TestWordsDelimiters()
        {
            // Arrange
            var testInput = $"test_input_delimiter.txt";
            using (StreamWriter writer = new StreamWriter(testInput))
            {
                writer.Write("Hello-world! Hello A, B and C; and D's *app?\n((Apple), {Broadridge}, [Hash], \"GitHub\", A/B, etc...) ok.");
            }

            try
            {
                // Act
                ISourceReader reader = new FileSourceReader(testInput);
                ISourceProcessor processor = new WordFrequencyProcessor(reader);
                var wordFrequencies = processor.Process();

                // Assert
                Assert.IsFalse(wordFrequencies.ContainsKey("hello-world")); // hyphen is delimiter
                Assert.AreEqual(2, wordFrequencies["Hello"]); // hyphen
                Assert.AreEqual(1, wordFrequencies["World"]); // exclamation mark
                Assert.AreEqual(1, wordFrequencies["world"]); // case-insensitive
                Assert.AreEqual(2, wordFrequencies["A"]); // 'A' and 'A/B' (slash)
                Assert.AreEqual(2, wordFrequencies["B"]); // 'B' and 'A/B' (slash)
                Assert.AreEqual(1, wordFrequencies["C"]); // semicolon
                Assert.AreEqual(1, wordFrequencies["D"]); // apostrophe
                Assert.AreEqual(1, wordFrequencies["Apple"]); // round brackets
                Assert.AreEqual(1, wordFrequencies["Broadridge"]); // braces
                Assert.AreEqual(1, wordFrequencies["Hash"]); // square brackets
                Assert.AreEqual(1, wordFrequencies["GitHub"]); // quotation marks
                Assert.AreEqual(1, wordFrequencies["etc"]); // ellipsis points
                Assert.AreEqual(1, wordFrequencies["s"]); // apostrophe
                Assert.AreEqual(1, wordFrequencies["app"]); // asterisk, question mark and newline
                Assert.AreEqual(1, wordFrequencies["ok"]); // space & full stop
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

        /// <summary>
        /// Test the performance of the ProcessFile method.
        /// </summary>
        /// <param name="numberOfLines">number of lines written to the test input file</param>
        [DataTestMethod]
        [DataRow(10)]
        [DataRow(1_000_000)]
        [DataRow(5_000_000)]
        public void TestProcessFile(int numberOfLines)
        {
            // Arrange
            var testInput = $"test_input_{numberOfLines}.txt";
            StringBuilder sb = new StringBuilder();
            using (StreamWriter writer = new StreamWriter(testInput))
            {
                for (int i = 0; i < numberOfLines; i++)
                {
                    writer.WriteLine(".NET Developer’s Test Assignment.");
                }
            }

            try
            {
                // Get the file info
                FileInfo fileInfo = new FileInfo(testInput);
                long fileSize = fileInfo.Length;

                Console.WriteLine($"File size: {fileSize} bytes");

#if DEBUG
                // Start the stopwatch
                Stopwatch stopwatch = Stopwatch.StartNew();

                // Measure initial memory usage
                long initialMemory = GC.GetTotalMemory(true);
#endif
                // Act
                ISourceReader reader = new FileSourceReader(testInput);
                ISourceProcessor processor = new WordFrequencyProcessor(reader);
                var wordFrequencies = processor.Process();
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
                Assert.AreEqual(1 * numberOfLines, wordFrequencies["net"]);
                Assert.AreEqual(1 * numberOfLines, wordFrequencies["developer"]);
                Assert.AreEqual(1 * numberOfLines, wordFrequencies["s"]);
                Assert.AreEqual(1 * numberOfLines, wordFrequencies["test"]);
                Assert.AreEqual(1 * numberOfLines, wordFrequencies["assignment"]);

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
