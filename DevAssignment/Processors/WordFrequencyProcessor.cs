using DevAssignment.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DevAssignment.Processors
{
    /// <summary>
    /// Word frequency processor
    /// </summary>
    internal class WordFrequencyProcessor : ISourceProcessor, IDisposable
    {
        /// <summary>
        /// Source reader
        /// </summary>
        private readonly ISourceReader _reader;

        /// <summary>
        /// Initializes a new instance of the WordFrequencyProcessor
        /// </summary>
        /// <param name="reader">Source reader</param>
        /// <exception cref="ArgumentNullException"></exception>
        public WordFrequencyProcessor(ISourceReader reader) {
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        /// <summary>
        /// Read the source and return the word frequencies
        /// </summary>
        /// <returns>Word frequencies in ConcurrentDictionary</returns>
        public IDictionary<string, int> Process()
        {
            // Frequency of words, case-insensitive, as AsParallel() is used, ConcurrentDictionary is used
            var wordFrequencies = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            // Pattern to match words, \b is a word boundary
            var wordPattern = new Regex(@"\b\w+\b", RegexOptions.Compiled);

            // Use LINQ to read and count the words in parallel
            _reader.ReadLines().AsParallel().ForAll(line =>
            {
                // Find all the words in the line
                foreach (Match match in wordPattern.Matches(line))
                {
                    // Add the word to the dictionary, incrementing the count if it already exists
                    wordFrequencies.AddOrUpdate(match.Value.ToLower(), 1, (key, oldValue) => oldValue + 1);
                }
            });

            return wordFrequencies;

        }

        /// <summary>
        /// Dispose the processor
        /// </summary>
        public void Dispose()
        {
        }
    }
}
