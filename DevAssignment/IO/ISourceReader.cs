﻿using System.Collections.Generic;

namespace DevAssignment.IO
{
    /// <summary>
    /// Interface for reading the source
    /// </summary>
    internal interface ISourceReader
    {
        /// <summary>
        /// Process the source and return the word frequencies
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> ReadLines();
    }
}
