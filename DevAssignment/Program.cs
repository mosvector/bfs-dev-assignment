using CommandLine;
using DevAssignment.Models;
using System;

namespace DevAssignment
{

    /// <summary>
    /// Implement a Windows console application that can process an input text file and create an
    /// output file containing the list of all words in the input file and their frequencies.
    /// </summary>
    partial class Program
    {
        /// <summary>
        /// Entry point of the application
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        static int RunOptions(Options options)
        {
            try
            {
                // Check if the input and output options are selected
                if (options.IsOnlyOneInputOptionSelected() == false)
                {
                    throw new ArgumentException("Please select one input option.");
                }

                if (options.IsOnlyOneOutputOptionSelected() == false)
                {
                    throw new ArgumentException("Please select one output option.");
                }

                // Create the source reader, processor and result writer based on the options
                using (var reader = Factory.CreateSourceReader(options))
                using (var processor = Factory.CreateSourceProcessor(options, reader))
                using (var writer = Factory.CreateResultWriter(options))
                {
                    var wordFrequencies = processor.Process();
                    writer.WriteResult(wordFrequencies);
                }

                Console.WriteLine($"Processed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

                if (options.Verbose)
                {
                    Console.WriteLine(ex.StackTrace);
                }

                return 1;
            }

            return 0;
        }

        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                  (Options opts) => RunOptions(opts),
                  errs => 1);
        }
    }
}
