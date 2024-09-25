using DevAssignment.IO;
using DevAssignment.Models;
using DevAssignment.Processors;
using DevAssignment.Utils;
using System;

namespace DevAssignment
{
    /// <summary>
    /// Factory class to create the source reader, processor and writer
    /// </summary>
    internal static class Factory
    {
        /// <summary>
        /// Create the source reader based on the options
        /// </summary>
        /// <param name="options">Options from command line parser</param>
        /// <returns>Return ISourceReader</returns>
        /// <exception cref="ArgumentException"></exception>
        internal static ISourceReader CreateSourceReader(Options options)
        {
            if (options.FromMqtt)
            {
                // Get the MQTT server, port, client ID and topic from the configuration
                var mqttServer = ConfigHelper.GetConfigValue("MqttServer");
                var mqttPort = int.Parse(ConfigHelper.GetConfigValue("MqttPort"));
                var clientId = ConfigHelper.GetConfigValue("MqttClientId");
                var topic = ConfigHelper.GetConfigValue("MqttTopic");

                return new MqttSourceReader(mqttServer, mqttPort, clientId, topic);
            }
            else if (options.FromConsole)
            {
                return new ConsoleSourceReader();
            }
            else if (false == string.IsNullOrEmpty(options.FromFile))
            {
                return new FileSourceReader(options.FromFile);
            }

            throw new ArgumentException($"No supported source found.");
        }

        /// <summary>
        /// Create the result writer based on the options
        /// </summary>
        /// <param name="options">Options from command line parser</param>
        /// <returns>Returns IResultWriter</returns>
        /// <exception cref="ArgumentException"></exception>
        internal static IResultWriter CreateResultWriter(Options options)
        {
            if (options.ToDatabase)
            {
                // Get the access key and secret key from the configuration
                var accessKey = ConfigHelper.GetConfigValue("DBAccessKey");
                var secretKey = ConfigHelper.GetConfigValue("DBSecretKey");

                return new DbResultWriter(accessKey, secretKey, "WordFrequency");

            }
            else if (false == string.IsNullOrEmpty(options.ToFile))
            {
                return new FileResultWriter(options.ToFile);
            }
            else if (options.ToConsole)
            {
                return new ConsoleResultWriter();
            }

            throw new ArgumentException($"No supported target found.");
        }

        /// <summary>
        /// Create the source processor based on the options
        /// </summary>
        /// <param name="options">Options from command line parser</param>
        /// <param name="reader">ISourceReader for read source</param>
        /// <returns>Returns ISourceProcessor</returns>
        internal static ISourceProcessor CreateSourceProcessor(Options options, ISourceReader reader)
        {
            if (options.PartitionSize < 1)
            {
                throw new ArgumentException("Partition size should be greater than 0");
            }
            
            if (options.PartitionSize == 1)
            {
                return new WordFrequencyProcessor(reader);
            }

            return new PartitionWordFrequencyProcessor(reader, options.PartitionSize);
        }
    }
}
