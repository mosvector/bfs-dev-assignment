using DevAssignment.IO;
using DevAssignment.Processors;
using System;
using System.IO;

namespace DevAssignment
{
    /// <summary>
    /// Implement a Windows console application that can process an input text file and create an
    /// output file containing the list of all words in the input file and their frequencies.
    /// </summary>
    class Program
    {
        static int Main(string[] args)
        {
            // Get the program name
            string programName = AppDomain.CurrentDomain.FriendlyName;

            // Check the command-line arguments
            if (args.Length != 2)
            {
                Console.WriteLine($"Usage: {programName} <Input File Path> <Output File Path>");
                return -2;
            }

            // Get the input and output file paths
            // The name of the input file is the first command-line parameter.
            // The name of the output file is the second command-line parameter.
            var inputFilePath = args[0];
            var outputFilePath = args[1];

            // Check if the input file exists
            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine("Input file does not exist.");
                return -1;
            }

            Console.WriteLine($"Processing file {inputFilePath}");

            try
            {
                // Process the input file and write the output file                
                ISourceReader reader = new FileSourceReader(inputFilePath);
                ISourceProcessor processor = new WordFrequencyProcessor(reader);
                IResultWriter writer = new FileResultWriter(outputFilePath);

                var wordFrequencies = processor.Process();
                writer.WriteResult(wordFrequencies);

                Console.WriteLine($"Processed successfully and results have written to {outputFilePath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return -3;
            }

            return 0;
        }
    }
}
