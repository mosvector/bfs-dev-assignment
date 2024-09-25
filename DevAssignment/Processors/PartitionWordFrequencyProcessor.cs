using DevAssignment.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DevAssignment.Processors
{
    /// <summary>
    /// Word frequency processor that partitions the file content and processes in parallel
    /// </summary>
    internal class PartitionWordFrequencyProcessor : ISourceProcessor
    {
        /// <summary>
        /// Source reader
        /// </summary>
        private readonly ISourceReader _reader;

        /// <summary>
        /// Partition size
        /// </summary>
        private readonly int _partitionSize;

        /// <summary>
        /// Initializes a new instance of the PartitionWordFrequencyProcessor
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="partitionSize"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PartitionWordFrequencyProcessor(ISourceReader reader, int partitionSize = 1000)
        {
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _partitionSize = partitionSize;  // Number of lines to process per partition
        }

        /// <summary>
        /// Clean up resources
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Process the source and return the word frequencies
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, int> Process()
        {
            var wordFrequencies = new ConcurrentDictionary<string, int>();
            var wordPattern = new Regex(@"\b\w+\b", RegexOptions.Compiled);

            // Partition the file content and process in parallel
            var partitions = PartitionFileLines(_reader, _partitionSize);

            // Parallel processing of each partition
            Parallel.ForEach(partitions, partition =>
            {
                foreach (var line in partition)
                {
                    var words = wordPattern.Matches(line).Cast<Match>().Select(m => m.Value);
                    foreach (var word in words)
                    {
                        // Increment word frequency in a thread-safe manner
                        wordFrequencies.AddOrUpdate(word, 1, (key, count) => count + 1);
                    }
                }
            });

            return wordFrequencies;
        }

        /// <summary>
        /// Helper method to partition file lines
        /// </summary>
        /// <param name="sourceReader"></param>
        /// <param name="partitionSize"></param>
        /// <returns></returns>
        private static IEnumerable<List<string>> PartitionFileLines(ISourceReader sourceReader, int partitionSize)
        {
            List<string> partition = new List<string>(partitionSize);

            // Read file line by line
            foreach (var line in sourceReader.ReadLines())
            {
                partition.Add(line);

                // Yield partition when it reaches the specified size
                if (partition.Count >= partitionSize)
                {
                    yield return partition;
                    partition = new List<string>(partitionSize);
                }
            }

            // Yield remaining partition if there are any remaining lines
            if (partition.Count > 0)
            {
                yield return partition;
            }
        }
    }
}
