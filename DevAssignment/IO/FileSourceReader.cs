using DevAssignment.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace DevAssignment.IO
{
    /// <summary>
    /// Reading the source from a file
    /// </summary>
    internal class FileSourceReader : ISourceReader
    {
        /// <summary>
        /// Input file path
        /// </summary>
        private readonly string _filePath;

        /// <summary>
        /// Initializes a new instance of source reader for a file 
        /// </summary>
        /// <param name="filePath">Input file path</param>
        public FileSourceReader(string filePath)
        {
            // Check if the file path is null or empty, if so throw an exception
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException("File path cannot be null or empty.", nameof(filePath));
            }

            if (!filePath.EndsWith(".txt"))
            {
                throw new NotSupportedFormatException("Unsupported input.", ".txt");
            }

            if (File.Exists(filePath) == false)
            {
                throw new FileNotFoundException("File does not exist.", filePath);
            }

            _filePath = filePath;
        }

        /// <summary>
        /// Read the source file and return the lines
        /// </summary>
        /// <returns>A iterator of lines</returns>
        public IEnumerable<string> ReadLines()
        {
            // Read the file line by line and use yield return to return each line
            using (StreamReader reader = new StreamReader(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
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
