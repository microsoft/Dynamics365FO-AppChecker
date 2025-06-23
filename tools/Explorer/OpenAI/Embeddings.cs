using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Azure;
//using Azure.AI.OpenAI;
//using OpenAI.Embeddings;
using System.Numerics;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using System.ClientModel;
using System.Security.Cryptography.Pkcs;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using OpenAI.Embeddings;
using Azure.AI.OpenAI;
using Azure;

#nullable enable
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
        public async Task<ReadOnlyMemory<float>> CreateEmbeddingsAsync(params string[] query)
        {
            if (!query.Any())
            {
                throw new ArgumentException("The query must contain one or more strings.", nameof(query));
            }

            var embeddingGenerationOptions = new EmbeddingGenerationOptions
            {
                Dimensions = this.dimensions,
            };

            var embeddings = await client.GenerateEmbeddingsAsync(query, embeddingGenerationOptions);


            // For now, return the first vector because there is only one query to create an embedding for.
            var result = embeddings.Value.First();
            return result.ToFloats();

            //foreach (var e in embeddings.Value)
            //{
            //    ReadOnlyMemory<float> vector = e.ToFloats();
            //}

            //return embeddings;

        }

        public async Task<string> GetMatchingEmbeddingAsync(string query, int numberOfResults)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException("The query must not be empty.", nameof(query));
            }

            if (numberOfResults <= 0)
            {
                throw new ArgumentException("The number of results must be greater than 0.", nameof(numberOfResults));
            }

            string endpoint = "https://sourceinformationrepository.search.windows.net";
            string indexName = "vector-1744504987774";
            string apiKey = Secrets.AzureAiSearchApiKey;

            var searchClient = new SearchClient(
                new Uri(endpoint),
                indexName,
                new AzureKeyCredential(apiKey));

            var queryVector = await this.CreateEmbeddingsAsync(query);

            // Vector representation of search query
            //float[] queryVector = { 0.876f, -0.342f, 0.654f, -0.211f };

            var searchOptions = new SearchOptions
            {
                Size = numberOfResults, // Return top N nearest neighbors.
                Select = { "title" },
                VectorSearch = new VectorSearchOptions
                {
                    Queries = { new VectorizedQuery(queryVector)
                        {
                            KNearestNeighborsCount = numberOfResults,
                            Fields = { "text_vector" }, // Specify the field containing the vectorized source
                        }
                    }
                }
            };

            // Run the search
            var response = await searchClient.SearchAsync<SearchDocument>("*", searchOptions);

            Console.WriteLine("Semantic Search Results:");
            string results = string.Empty;
            await foreach (var result in response.Value.GetResultsAsync())
            {
                results += $"Score: {result.Score}\n Key: {result.Document.First().Key} Value: {result.Document.First().Value}";
                // Console.WriteLine($"Source: {result.Document["Source"]}");
            }

            return results;
        }
    }
}
