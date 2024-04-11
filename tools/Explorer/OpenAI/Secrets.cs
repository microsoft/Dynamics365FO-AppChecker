using Azure;
using Azure.AI.OpenAI;
using Azure.Core;
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
        private static string AzureOpenAiApiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");
        private static string AzureOpenAiEndpoint = "https://openai-pva.openai.azure.com/";
        //public static string AzureOpenAiModel = "chatGPT_GPT35-turbo-0301";
        public static string AzureOpenAiModel = "GPT-4-32K";

        public static Lazy<OpenAIClient> OpenAIClient => new Lazy<OpenAIClient>(new OpenAIClient(
              new Uri(AzureOpenAiEndpoint),
              new AzureKeyCredential(AzureOpenAiApiKey)));


    }
}
