// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XppReasoningWpf.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using XppReasoningWpf.Views;
    using BaseXInterface;

    class SubmittedQueriesViewModel
    {
        private readonly RelayCommand showResultsCommand;
        private readonly RelayCommand cancelQueryCommand;
        private const string jobDetailsQuery = "xquery jobs:list-details()";

        private readonly Model model;

        public ICommand ShowResultsCommand
        {
            get => this.showResultsCommand;
        }

        public ICommand CancelQueryCommand
        {
            get => this.cancelQueryCommand;
        }

        public string Title
        {
            get { return "Submitted queries on " + this.model.HostName + ":" + this.model.Username;  }
        }
        public ObservableCollection<SubmittedQueryDescriptor> SubmittedQueries => this.model.SubmittedQueries;

        /// <summary>
        /// Call the server to get the list of running jobs.
        /// </summary>
        private async Task GetJobsStatusAsync()
        {
            string result = "";
            using (var session = this.model.GetSession(""))
            {
                result = $"<Jobs>{await session.ExecuteAsync(jobDetailsQuery)}</Jobs>";
            }
            XDocument document = XDocument.Parse(result);

            // Build the list from the server:
            IList<SubmittedQueryDescriptor> serverList = new List<SubmittedQueryDescriptor>();
            foreach (var job in document.Document.XPathSelectElements("//job"))
            {
                var jobId = job.Attribute("id").Value;
                var state = job.Attribute("state").Value;
                var user = job.Attribute("user").Value;
                var duration = job.Attribute("duration").Value;
                string query;

                if (this.model.JobIdToQuery.TryGetValue(jobId, out string formattedQuery))
                {
                    query = formattedQuery;
                }
                else
                {
                    query = job.Value;
                }

                // Do not show the job that is getting the job details.
                if (string.Compare(query, jobDetailsQuery, true) == 0)
                {
                    continue;
                }
                serverList.Add(new SubmittedQueryDescriptor(jobId)
                {
                    Query = query,
                    Duration = XmlConvert.ToTimeSpan(duration),
                    User = user,
                    IsRunning = (string.Compare(state, "running") == 0),
                });
            }

            var displayList = this.model.SubmittedQueries;
            foreach (var descriptor in displayList)
                descriptor.Delete = true;

            foreach (var descriptor in serverList)
            {
                var displayed = displayList.Where(d => d.JobId == descriptor.JobId).FirstOrDefault();

                if (displayed != null)
                {
                    displayed.IsRunning = descriptor.IsRunning;
                    displayed.Duration = descriptor.Duration;
                    displayed.Delete = false;
                }
                else
                {
                    // There is nothing displayed for this item from the server.
                    displayList.Add(descriptor);
                }
            }

            // Now all the displayed elements have been updated with new values
            // and new ones have been inserted. Any displayed item that has the 
            // delete flag set to true is deleted:
            foreach (var descriptor in displayList.Where(d => d.Delete).ToArray())
            {
                displayList.Remove(descriptor);
                this.model.JobIdToQuery.Remove(descriptor.JobId);
            }
        }

        public async Task GetUpdatesAsync(Func<bool> isClosing)
        {
            while(!isClosing())
            {
                await this.GetJobsStatusAsync();
                await Task.Delay(Properties.Settings.Default.TimeBetweenQueryListUpdates);
            }
        }

        /// <summary>
        /// Show the results from the server for the given job Id
        /// </summary>
        /// <param name="jobId">The job id for which the result is wanted</param>
        /// <returns>The result as calculated on the server</returns>
        private async Task<string> ShowResult(string jobId)
        {
            // Get the result from the server
            using (var session = this.model.GetSession(""))
            {
                var result = await session.DoQueryAsync($"jobs:result('{jobId}')", null);
                this.model.QueryResult = result;
                return result;
            }
        }

        private async void DeleteJob(string jobId)
        {
            using (var session = this.model.GetSession(""))
            {
                var result = await session.DoQueryAsync($"jobs:stop('{jobId}')", null);
            }
        }

        public SubmittedQueriesViewModel(SubmittedQueriesWindow view, Model model)
        {
            this.model = model;

            this.showResultsCommand = new RelayCommand(
               p => {
                   var item = view.QueriesList.SelectedItems[0] as SubmittedQueryDescriptor;
                   var result = this.ShowResult(item.JobId);
               },

               p =>
               {
                   if (view.QueriesList.SelectedItems.Count == 1)
                   {
                       var item = view.QueriesList.SelectedItems[0] as SubmittedQueryDescriptor;
                       return !item.IsRunning;
                   }
                   else
                   {
                       return false;
                   }
               }
            );

            this.cancelQueryCommand = new RelayCommand(
               p => {
                   foreach (var item in view.QueriesList.SelectedItems)
                   {
                       var jobId = (item as SubmittedQueryDescriptor).JobId;
                       this.DeleteJob(jobId);
                   }
               },

               p => { return view.QueriesList.SelectedItems.Count >= 1; }
            );

        }

    }
}
