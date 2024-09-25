using CommandLine;

namespace DevAssignment.Models
{
    /// <summary>
    /// Options for the application
    /// </summary>
    internal class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option("from-file", Required = false, HelpText = "Input from file", Group = "input")]
        public string FromFile { get; set; }

        [Option("from-mqtt", Required = false, HelpText = "Input from mqtt", Group = "input")]
        public bool FromMqtt { get; set; }

        [Option("from-console", Required = false, HelpText = "Input from console", Group = "input")]
        public bool FromConsole { get; set; }

        [Option("to-db", Required = false, HelpText = "Output to DynamoDB", Group = "output")]
        public bool ToDatabase { get; set; }

        [Option("to-console", Required = false, HelpText = "Output to console", Group = "output")]
        public bool ToConsole { get; set; }

        [Option("to-file", Required = false, HelpText = "Output to file", Group = "output")]
        public string ToFile { get; set; }

        [Option("partition-size", Required = false, HelpText = "Partition size", Default = 1)]
        public int PartitionSize { get; set; }

        /// <summary>
        /// Check if one of the output options is selected
        /// </summary>
        /// <returns></returns>
        public bool IsOnlyOneOutputOptionSelected()
        {
            int count = 0;
            if (ToDatabase)
                count++;
            if (ToConsole)
                count++;
            if (!string.IsNullOrEmpty(ToFile))
                count++;

            return count == 1;
        }

        /// <summary>
        /// Check if one of the input options is selected
        /// </summary>
        /// <returns></returns>
        public bool IsOnlyOneInputOptionSelected()
        {
            int count = 0;
            if (!string.IsNullOrEmpty(FromFile))
                count++;
            if (FromMqtt)
                count++;
            if (FromConsole)
                count++;

            return count == 1;
        }
    }
}
