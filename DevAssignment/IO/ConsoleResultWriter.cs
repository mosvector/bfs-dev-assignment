using System;
using System.Collections.Generic;
using System.Linq;

namespace DevAssignment.IO
{
    /// <summary>
    /// Console result writer
    /// </summary>
    internal class ConsoleResultWriter : IResultWriter
    {
        /// <summary>
        /// Write the word frequencies to the console
        /// </summary>
        /// <param name="wordFrequencies"></param>
        public void WriteResult(IDictionary<string, int> wordFrequencies)
        {
            var sortedWords = wordFrequencies.AsParallel().OrderByDescending(pair => pair.Value).ThenBy(pair => pair.Key);
            // Write the words to the console
            // Each line should be formatted as WORD, FREQUENCY.
            // WORD is a word from the input text and FREQUENCY is how often it was found in it
            Console.WriteLine("WORD,FREQUENCY");
            Console.WriteLine("------------");
            foreach (var pair in sortedWords)
            {
                Console.WriteLine($"{pair.Key},{pair.Value}");
            }
        }

        public void Dispose()
        {
        }
    }
}
