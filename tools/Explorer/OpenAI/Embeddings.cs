using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Embeddings;
using System.Numerics;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using System.ClientModel;
using System.Security.Cryptography.Pkcs;

namespace XppReasoningWpf.OpenAI
{
    internal class Embeddings
    {
        private EmbeddingClient client;
        private int dimensions;

        public Embeddings(int dimensions = 1000)
        {
            this.dimensions = dimensions;

            string? apiKey = Environment.GetEnvironmentVariable("AzureOpenAIApiKey");

            var endpoint = "https://xppcopilotopenai.openai.azure.com/";
            var endpointUri = new Uri(endpoint);
            var model = "text-embedding-3-large";
            var deploymentName = "text-embedding-3-large";

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(deploymentName))
            {
                throw new ArgumentException("Please set the apiKey and endpoint variables.");
            }

            AzureOpenAIClient openAIClient = new(
                new Uri(endpoint),
                new System.ClientModel.ApiKeyCredential(apiKey));

            client = openAIClient.GetEmbeddingClient(model);
            this.dimensions = dimensions;
        }

        /// <summary>
        /// Use the embedding generator model to get an embeddng for the provided string.
        /// </summary>
        /// <param name="query">The string to calculate an embedding for.</param>
        /// <returns>The embedding for the provided string.</returns>
        private ClientResult<OpenAIEmbeddingCollection> CreateEmbeddings(params string[] query)
        {
            if (!query.Any())
            {
                throw new ArgumentException("The query must contain one or more strings.", nameof(query));
            }

            var embeddingGenerationOptions = new EmbeddingGenerationOptions
            {
                Dimensions = this.dimensions,
            };

            var embeddings = client.GenerateEmbeddings(query, embeddingGenerationOptions);
            return embeddings;
            //embeddings.Value.ToList().ForEach(e =>
            //{
            //    ReadOnlyMemory<float> vector = e.ToFloats();
            //    int length = vector.Length;
            //    System.Console.Write($"data[{e.Index}]: length={length}, ");
            //    System.Console.Write($"[{vector.Span[0]}, {vector.Span[1]}, ..., ");
            //    System.Console.WriteLine($"{vector.Span[length - 2]}, {vector.Span[length - 1]}]");
            //});
        }

        public void GetMatchingEmbedding(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException("The query must not be empty.", nameof(query));
            }
        }
    }
}
