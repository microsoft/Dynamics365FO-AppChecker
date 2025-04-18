using Azure;
using Azure.AI.OpenAI;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenAI.Chat;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XppReasoningWpf.OpenAI
{
    /// <summary>
    /// This class does the evaluation of the AI (for now: OpenAI) prompts.
    /// </summary>
    internal class PromptEvaluator
    {
        /// <summary>
        /// This is the system prompt that is used to inform the AI model about what to do when presented 
        /// with some X++ code.
        /// </summary>
        // public string ManipulateSystemPrompt = System.IO.File.ReadAllText("ManipulateSystemPrompt.txt");

        private string systemPrompt = string.Empty;

        //private ChatHistory history = null;
        private IList<ChatMessage> chatHistory = null;

        private AzureOpenAIClient chatCompletionClient;

        public PromptEvaluator(string systemPrompt)
        {
            this.systemPrompt = systemPrompt;
        }

        public async Task<(string, TimeSpan)> EvaluatePromptAsync(string query)
        {
            const int MaxQueryLength = 5000;
            // TODO: Remove:
            if (query.Length > MaxQueryLength)
            {
                query = query.Substring(0, MaxQueryLength);
            }


            if (systemPrompt.Length == 0)
            {
                throw new ArgumentException("The system prompt must be set.");
            }

            if (string.IsNullOrEmpty(query))
            {
                return (string.Empty, TimeSpan.Zero);
            }

            //// We need the chatGPT instance and the history
            //if (this.history == null)
            //{
            //    // This will insert the system prompt into the chat history.
            //    this.history = new ChatHistory(this.systemPrompt);
            //}

            if (this.chatHistory == null)
            {
                this.chatHistory = new List<ChatMessage>();
                this.chatHistory.Add(new SystemChatMessage(this.systemPrompt));
            }

            if (chatCompletionClient == null)
            {
                this.chatCompletionClient = Secrets.OpenAIClient.Value;
            }

            this.chatHistory.Add (new UserChatMessage(query));
            //this.history.AddUserMessage(query);

            var timer = Stopwatch.StartNew();
            ClientResult<ChatCompletion> response = await this.chatCompletionClient
                .GetChatClient(Secrets.DeploymentName)
                .CompleteChatAsync(this.chatHistory);

            timer.Stop();

            string result = "";
            foreach (var item in response.Value.Content)
            {
                result += item.Text;
            }

            this.chatHistory.Add(new AssistantChatMessage(result));
            //history.Add(result);

            return (result , timer.Elapsed);
        }
    }
}
