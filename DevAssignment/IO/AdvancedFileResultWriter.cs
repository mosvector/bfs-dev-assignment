using DevAssignment.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevAssignment.IO
{
    internal class AdvancedFileResultWriter : IResultWriter
    {
        /// <summary>
        /// Output file path
        /// </summary>
        private readonly string _filePath;

        /// <summary>
        /// Initializes a new instance of result writer for a file
        /// </summary>
        /// <param name="filePath">Output file path</param>
        public AdvancedFileResultWriter(string filePath, bool overwrite = false)
        {
            // if overwrite existing file is not expected, throw an exception
            if (!overwrite && File.Exists(_filePath))
            {
                throw new ArgumentException("Output file already exists.", nameof(filePath));
            }

            // Check if the file path ends with .txt, if not throw an exception
            if (!filePath.EndsWith(".txt"))
            {
                throw new NotSupportedFormatException("Unsupported output.", ".txt");
            }

            _filePath = filePath;
        }

        /// <summary>
        /// Write the word frequencies to the output file
        /// </summary>
        /// <param name="wordFrequencies">Word frequencies dictionay</param>
        public void WriteResult(IDictionary<string, int> wordFrequencies)
        {
            // check if the word frequencies is null, if so throw an exception
            if (wordFrequencies == null)
            {
                throw new ArgumentNullException(nameof(wordFrequencies), "Word frequencies cannot be null.");
            }

            // Sort the words by frequency, then by word
            var sortedWords = wordFrequencies.AsParallel().OrderByDescending(pair => pair.Value).ThenBy(pair => pair.Key);

            try
            {
                // Write the words to the output file
                // Each line should be formatted as WORD, FREQUENCY.
                // WORD is a word from the input text and FREQUENCY is how often it was found in it
                using (var writer = new StreamWriter(_filePath))
                {
                    foreach (var pair in sortedWords)
                    {
                        writer.WriteLine($"{pair.Key},{pair.Value}");
                    }
                }
            }
            catch (IOException ex)
            {
                throw new ApplicationException("An error occurred while writing to the file.", ex);
            }
        }

        /// <summary>
        /// Dispose the object
        /// </summary>
        public void Dispose()
        {
        }
    }
}
