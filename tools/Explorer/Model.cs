// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace XppReasoningWpf
{
    using BaseXInterface;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    /// <summary>
    /// This is the model for the application. The views interact with the data
    /// stored herein, preferably through view models. 
    /// </summary>
    public class Model : INotifyPropertyChanged
    {
        private readonly IDictionary<string, string> jobIdToQuery = new Dictionary<string, string>();

        /// <summary>
        /// This event is triggered when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Username
        {
            get {
                return Properties.Settings.Default.Username;
            }
            set
            {
                Properties.Settings.Default.Username = value;
                this.OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get; set;
        }

        public IDictionary<string, string> JobIdToQuery => this.jobIdToQuery;
  
        private readonly ObservableCollection<SubmittedQueryDescriptor> submittedQueries = new ObservableCollection<SubmittedQueryDescriptor>();
        public ObservableCollection<SubmittedQueryDescriptor> SubmittedQueries
        {
            get { return submittedQueries; }
        }

        private string hostName = string.Empty;
        public string HostName
        {
            get { return this.hostName; }
            set
            {
                this.hostName = value;
                this.OnPropertyChanged(nameof(HostName));
            }
        }
        /// <summary>
        /// The connection to the BaseX server;
        /// </summary>
        public BaseXInterface.BaseXServer Server { get; private set; }

        private string status = string.Empty;
        public string Status
        {
            get { return this.status;  }
            set
            {
                if (this.status != value)
                {
                    this.status = value;
                    this.OnPropertyChanged(nameof(Status));
                }
            }
        }

        /// <summary>
        /// Return a human readable string designating the server and database to 
        /// which the system is currently connected. This property should be updated
        /// whenever any of the constituent parts change.
        /// </summary>
        public string ConnectionString
        {
            get {
                var connectionInfo = this.Username + "@" + this.HostName;

                if (this.SelectedDatabase != null)
                    connectionInfo += ":" + this.SelectedDatabase.Name;

                return connectionInfo;
            }
        }
        private string caretPositionString = string.Empty;
        public string CaretPositionString
        {
            get
            {
                return this.caretPositionString;
            }
            set
            {
                if (this.caretPositionString != value)
                {
                    this.caretPositionString = value;
                    this.OnPropertyChanged(nameof(CaretPositionString));
                }
            }
        }

        private Database selectedDatabase;
        public Database SelectedDatabase
        {
            get { return this.selectedDatabase; }
            set
            {
                if (this.selectedDatabase != value && value != null && !string.IsNullOrEmpty(value.Name))
                {
                    this.selectedDatabase = value;
                    var dbs = new ObservableCollection<Database>();

                    foreach (var db in this.Databases)
                    {
                        var newdb = db;
                        newdb.IsCurrent = db == value;
                        dbs.Add(newdb);
                    }

                    this.Databases = dbs;

                    this.OnPropertyChanged(nameof(SelectedDatabase));
                }
            }
        }

        private ObservableCollection<BaseXInterface.Database> databases;
        public ObservableCollection<Database> Databases
        {
            get { return this.databases; }
            set
            {
                this.databases = value;
                this.OnPropertyChanged(nameof(Databases));
            }
        }

        private string queryResult;

        public string QueryResult
        {
            get { return queryResult; }
            set
            {
                if (this.queryResult != value)
                {
                    this.queryResult = value;
                    this.OnPropertyChanged(nameof(QueryResult));
                }
            }
        }

        /// <summary>
        /// Gets the directory hosting the queries.
        /// </summary>
        public string QueriesDirectory
        {
            get
            {
                var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var queriesDirectory = string.Format(Properties.Settings.Default.QueriesPath, documentsFolder);
                return queriesDirectory;
            }
        }

        public delegate void TickEventHandler(object sender, EventArgs e);
        public event TickEventHandler Tick;

        public void CreateServer(string server, int port, string username, string password)
        {
            this.Server = new BaseXServer(server, port, username, password);
            this.Server.DatabaseOpening += (databaseName) => { this.OnPropertyChanged("DatabaseOpening"); };
            this.Server.DatabaseOpened += (databaseName) => { this.OnPropertyChanged("DatabaseOpened"); };
        }

        public async Task CloseConnectionToServerAsync()
        {
            if (this.Server != null)
                await this.Server.CloseConnectionAsync();

            this.Server = null;
        }
        public async Task<DatabaseSession> GetSessionAsync(string database)
        {
            return await this.Server.GetSessionAsync(database);
        }

        public async Task<bool>  IsServerOnlineAsync(string host, int port, string username, string password)
        {
            try
            {
                var session = new Session(host, port, username, password);
                await session.ExecuteAsync("show users");
                session.Close();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get a description of the query by parsing the content looking for the 
        /// first comments in (: :)
        /// </summary>
        /// <param name="fileName">The query file to extract the description from,</param>
        /// <returns>The content of the first comment.</returns>
        private string GetDescription(string fileName)
        {
            try
            {
                var content = File.ReadAllText(fileName);
                content = content.TrimStart(new[] { ' ', '\n' });
                Regex r = new Regex(@"\(:(?'Content'.*):\)", RegexOptions.Multiline);
                Match m = r.Match(content);
                if (m.Success)
                {
                    return m.Groups["Content"].Value;
                }
            }
            catch (Exception)
            {
                // Do not do anything for now
            }
            return "*** No description found ***";
        }

        public Model()
        {
            // Create the ticks that are triggered every 2 seconds.
            // These are used to trigger source folding etc.
            DispatcherTimer tickTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(2)
            };

            tickTimer.Tick += (object sender, EventArgs a) =>
            {
                this.Tick?.Invoke(sender, a);
            };

            tickTimer.Start();

        }
    }
}
