//using Azure;
//using Azure.AI.OpenAI;
//using Azure.Core;
//using Microsoft.SemanticKernel.ChatCompletion;
//using Microsoft.SemanticKernel.Connectors.OpenAI;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XppReasoningWpf
{
    static class Secrets
    {
        /// <summary>
        /// Determines whether the Azure OpenAI API should be used.
        /// </summary>
        public static bool UsesAzureOpenAI => false;

        public static string OpenAiModel
        {
            get
            {
                return "GPT-4o";
            }
        }

        public static string ApiKey
        {
            get
            {
                return Environment.GetEnvironmentVariable("AzureOpenAIApiKey");
            }
        }

        public static string DeploymentName => "gpt-4o";

        public static string AzureAiSearchApiKey => Environment.GetEnvironmentVariable("AzureAiSearchApiKey");

        public static Lazy<Azure.AI.OpenAI.AzureOpenAIClient> OpenAIClient
        {
            get
            {
                string AzureOpenAiApiKey = Environment.GetEnvironmentVariable("AzureOpenAIApiKey");
                string AzureOpenAiEndpoint = Environment.GetEnvironmentVariable("AzureOpenAIEndpoint");

                return new Lazy<Azure.AI.OpenAI.AzureOpenAIClient>(new Azure.AI.OpenAI.AzureOpenAIClient(
                    new Uri(AzureOpenAiEndpoint),
                    new AzureKeyCredential(AzureOpenAiApiKey)));
            }
        }

    }
}
