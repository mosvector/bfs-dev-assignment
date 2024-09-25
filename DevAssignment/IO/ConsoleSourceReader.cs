using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevAssignment.IO
{
    /// <summary>
    /// Read the source from the console
    /// </summary>
    internal class ConsoleSourceReader : ISourceReader
    {
        /// <summary>
        /// Queue to store messages
        /// </summary>
        private readonly BlockingCollection<string> _messageQueue;

        /// <summary>
        /// stop receiving flag
        /// </summary>
        private bool _stopReceiving = false;

        public ConsoleSourceReader()
        {
            _messageQueue = new BlockingCollection<string>();
        }

        /// <summary>
        /// Read the input from the console and add it to the message queue
        /// </summary>
        private void ReadInput()
        {
            while (false == _stopReceiving)
            {
                Console.WriteLine("Enter a line of text (or press Enter to finish):");
                var input = Console.ReadLine();
                _messageQueue.Add(input);

                if (input == "")
                {
                    _stopReceiving = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Read the source from the console and return the lines
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ReadLines()
        {
            // Reset the flag
            _stopReceiving = false;

            // Create a task to read the input
            Task task = Task.Run(() => ReadInput());

            // Read the input from the message queue
            foreach (var message in _messageQueue.GetConsumingEnumerable())
            {
                // If the stop receiving flag is set, break the loop
                if (_stopReceiving)
                {
                    Console.WriteLine("Input completed");
                    break;
                }

                yield return message;
            }

            task.Wait();
        }

        /// <summary>
        /// Clean up the object
        /// </summary>
        public void Dispose()
        {
            _stopReceiving = true;
        }        
    }
}
