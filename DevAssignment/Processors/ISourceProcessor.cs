using System.Collections.Generic;

namespace DevAssignment.Processors
{
    /// <summary>
    /// Interfce for processing the source
    /// </summary>
    internal interface ISourceProcessor
    {
        /// <summary>
        /// Read the source and return the word frequencies
        /// </summary>
        /// <returns>Word frequencies</returns>
        IDictionary<string, int> Process();
    }
}
