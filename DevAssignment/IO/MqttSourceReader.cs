using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAssignment.IO
{
    /// <summary>
    /// Mqtt source reader
    /// </summary>
    internal class MqttSourceReader : ISourceReader
    {
        /// <summary>
        /// Queue to store messages
        /// </summary>
        private readonly BlockingCollection<string> _messageQueue;

        /// <summary>
        /// MQTT client options
        /// </summary>
        private readonly MqttClientOptions _options;

        /// <summary>
        /// stop receiving flag
        /// </summary>
        private bool _stopReceiving = false;

        /// <summary>
        /// MQTT client
        /// </summary>
        private readonly IMqttClient _client;

        /// <summary>
        /// Topic to subscribe
        /// </summary>
        private readonly string _topic;

        /// <summary>
        /// Initializes a new instance of source reader for MQTT
        /// </summary>
        /// <param name="server">MQTT server</param>
        /// <param name="port">MQTT port</param>
        /// <param name="clientId">MQTT client ID, optional</param>
        /// <param name="topic">MQTT topic</param>
        public MqttSourceReader(string server, int port, string clientId, string topic)
        {
            // Create message queue
            _messageQueue = new BlockingCollection<string>();

            // Configure MQTT client options
            MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder();

            if (string.IsNullOrEmpty(clientId))
            {
                builder.WithClientId(clientId);

            }
            _options = builder.WithTcpServer(server, port).Build();

            // Create MQTT client and store the topic
            _client = CreateMqttClient();
            _topic = topic;
        }

        /// <summary>
        /// Create MQTT client
        /// </summary>
        /// <returns></returns>
        private IMqttClient CreateMqttClient()
        {
            // Create MQTT client
            var factory = new MqttFactory();
            var client = factory.CreateMqttClient();

            // Subscribe to topic and consume messages
            client.ApplicationMessageReceivedAsync += OnMessage;

            return client;
        }

        /// <summary>
        /// Receive message from MQTT server
        /// </summary>
        /// <param name="e">Event from MQTT server</param>
        /// <returns></returns>
        private Task OnMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment.ToArray());
            Console.WriteLine($"Received application message: {message}");

            _messageQueue.Add(message);

            if (message == "")
            {
                _stopReceiving = true;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Read the source and return the lines
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ReadLines()
        {
            if (_client.IsConnected == false)
            {
                // Connect to MQTT server
                _client.ConnectAsync(_options).Wait();
            }

            Console.WriteLine($"Subscribing to topic: {_topic}");
            _client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(_topic).Build()).Wait();
            
            foreach (var message in _messageQueue.GetConsumingEnumerable())
            {
                // If the stop receiving flag is set, break the loop
                if (_stopReceiving)
                {
                    Console.WriteLine("Received empty string");
                    break;
                }

                yield return message;
            }

            Console.WriteLine("Stopped receiving messages");
            _client.UnsubscribeAsync(_topic).Wait();
        }

        /// <summary>
        /// Dispose the object
        /// </summary>
        public void Dispose()
        {
            _client.DisconnectAsync().Wait();
            Console.WriteLine("Disconnected from MQTT server");

            _client.Dispose();
        }
    }
}


 