using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;

namespace DevAssignment.IO
{
    /// <summary>
    /// Database result writer
    /// </summary>
    internal class DbResultWriter : IResultWriter
    {
        /// <summary>
        /// DynamoDB client
        /// </summary>
        private readonly AmazonDynamoDBClient _dynamoDbClient;

        /// <summary>
        /// Table name
        /// </summary>
        private readonly string _tableName;

        /// <summary>
        /// Initializes a new instance of result writer for a database
        /// </summary>
        /// <param name="accessKey">AWS access key</param>
        /// <param name="secretKey">AWS secret key</param>
        /// <param name="tableName">Table name</param>
        public DbResultWriter(string accessKey, string secretKey, string tableName)
        {
            // Create DynamoDB client
            _dynamoDbClient = new AmazonDynamoDBClient(accessKey, secretKey);
            _tableName = tableName;
        }

        /// <summary>
        /// Write the word frequencies to the database
        /// </summary>
        /// <param name="wordFrequencies">Word frequency dictionary</param>
        public void WriteResult(IDictionary<string, int> wordFrequencies)
        {
            // Create a list to store the PutRequests
            var putRequests = new List<WriteRequest>();

            // Create a PutRequest for each word frequency and put it in the list
            foreach (var kvp in wordFrequencies)
            {
                Console.WriteLine($"Writing word: {kvp.Key}, frequency: {kvp.Value}");
                var putRequest = new PutRequest
                {
                    Item = new Dictionary<string, AttributeValue>
                        {
                            { "Word", new AttributeValue { S = kvp.Key } },
                            { "Frequency", new AttributeValue { N = kvp.Value.ToString() } }
                        }
                };

                putRequests.Add(new WriteRequest { PutRequest = putRequest });
            }

            // Create a BatchWriteItemRequest
            var request = new BatchWriteItemRequest
            {
                RequestItems = new Dictionary<string, List<WriteRequest>>
                    {
                        { _tableName, putRequests }
                    }
            };

            // Write the word frequencies to the database
            _dynamoDbClient.BatchWriteItem(request);
        }

        /// <summary>
        /// Dispose the DynamoDB client
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine("Disposing AmazonDynamoDBClient");
            _dynamoDbClient?.Dispose();
        }
    }
}
