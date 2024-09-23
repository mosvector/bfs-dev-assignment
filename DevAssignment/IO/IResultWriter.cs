using System.Collections.Generic;

namespace DevAssignment.IO
{
    /// <summary>
    /// Interface for writing the result
    /// </summary>
    internal interface IResultWriter
    {
        /// <summary>
        /// Write the word frequencies to the output
        /// </summary>
        /// <param name="wordFrequencies"></param>
        void WriteResult(IDictionary<string, int> wordFrequencies);
    }
}
