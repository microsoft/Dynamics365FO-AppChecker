using Azure;
using Azure.AI.OpenAI;
using Azure.Core;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
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

        //public static string AzureOpenAiModel = "chatGPT_GPT35-turbo-0301";
        public static string OpenAiModel
        {
            get
            {
                if (UsesAzureOpenAI)
                {
                    return "GPT-4-32K";
                }
                else
                {
                    // return "gpt-4-turbo-2024-04-09";
                    return "gpt-4o";
                }
            }
        }

        public static string ApiKey
        {
            get
            {
                if (false && UsesAzureOpenAI)
                {
                    return Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");
                }
                else
                {
                    return Environment.GetEnvironmentVariable("OpenAIBearerToken");
                }
            }
        }

        public static Lazy<IChatCompletionService> ChatCompletionService
        {
            get
            {
                if (UsesAzureOpenAI)
                {
                    return new Lazy<IChatCompletionService>(new AzureOpenAIChatCompletionService(
                         deploymentName: OpenAiModel,
                         openAIClient: OpenAIClient.Value));
                }
                else
                {
                    return new Lazy<IChatCompletionService>(new OpenAIChatCompletionService(
                        modelId: OpenAiModel,
                        apiKey: ApiKey)) ;
                }
            }
        }

        public static Lazy<OpenAIClient> OpenAIClient
        {
            get
            {
                if (UsesAzureOpenAI)
                {
                    string AzureOpenAiApiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");
                    string AzureOpenAiEndpoint = "https://openai-pva.openai.azure.com/";

                    return new Lazy<OpenAIClient>(new OpenAIClient(
                      new Uri(AzureOpenAiEndpoint),
                      new AzureKeyCredential(AzureOpenAiApiKey)));
                }
                else
                {
                    // private OpenAI account
                    string OpenAiApiKey = Environment.GetEnvironmentVariable("OpenAIBearerToken");
                    string OpenAIEndpoint = "https://api.openai.com/v1/chat/completions";

                    return new Lazy<OpenAIClient>(new OpenAIClient(
                        new Uri(OpenAIEndpoint),
                        new AzureKeyCredential(OpenAiApiKey)));
                }
            }
        }

    }
}
