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
    using System.Windows.Threading;

    /// <summary>
    /// This is the model for the application. The views interact with the data
    /// stored herein, preferably through view models. 
    /// </summary>
    public class Model : INotifyPropertyChanged
    {
        public FileSystemWatcher Watcher { get; private set; }

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

        private ObservableCollection<QueryEntry> queries;
        public ObservableCollection<QueryEntry> Queries
        {
            get { return queries; }
            set
            {
                this.queries = value;
                this.OnPropertyChanged(nameof(Queries));
            }
        }

        private ObservableCollection<string> languages;
        public ObservableCollection<string> Languages
        {
            get { return languages; }
            set
            {
                this.languages = value;
                this.OnPropertyChanged(nameof(Languages));
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
            this.Server = new BaseXInterface.BaseXServer(server, port, username, password);
        }

        public void CloseConnectionToServer()
        {
            if (this.Server != null)
                this.Server.CloseConnection();

            this.Server = null;
        }
        public DatabaseSession GetSession(string database)
        {
            return this.Server.GetSession(database);
        }

        public bool IsServerOnline(string host, int port, string username, string password)
        {
            try
            {
                var session = new Session(host, port, username, password);
                session.Execute("show users");
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

        public (ObservableCollection<QueryEntry>, ObservableCollection<string>) PopulateQueries()
        {
            const string queriesSubDirectoryName = "Queries";
            var languages = new ObservableCollection<string>();
            var queries = new ObservableCollection<QueryEntry>();

            // Create the directory with queries and add some examples.
            // By definition the queries are in the user's documents folder:
            if (!Directory.Exists(this.QueriesDirectory))
            {
                Directory.CreateDirectory(this.QueriesDirectory);
            }

            var queriesDirectoryName = Path.Combine(this.QueriesDirectory, queriesSubDirectoryName);
            if (!Directory.Exists(queriesDirectoryName))
            {
                Directory.CreateDirectory(queriesDirectoryName);
                SampleQueries.AddQueries(this.QueriesDirectory);
            }

            // Load the content of all the queries in the designated directory into the model

            DirectoryInfo root = new DirectoryInfo(queriesDirectoryName);
            foreach (var directory in root.EnumerateDirectories())
            {
                // capture the directory name
                languages.Add(directory.Name);

                // Iterate over all the files in that directory:
                foreach (var file in directory.EnumerateFiles("*.xq"))
                {
                    queries.Add(new QueryEntry()
                    {
                        Path = file.FullName,
                        Description = this.GetDescription(file.FullName),
                        Language = directory.Name
                    });
                }
            }

            return (queries, languages);
        }

        private void QueryDirectoryChanged(object sender, FileSystemEventArgs e)
        {
            // In this simple solution, we reread all the files in the directory
            IList<QueryEntry> queries = new List<QueryEntry>();

            if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                // Check if we have one in the model and delete if so.
                var entry = this.Queries.Where(q => q.Path == e.Name).FirstOrDefault();
                if (entry != null)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        this.Queries.Remove(entry);
                    });
                }
            }
            else if (e.ChangeType == WatcherChangeTypes.Created)
            {
                // Do nothing. Everything is handled in the Changed handler, since it
                // is mot guaranteed that the file has been written yet when this is triggered.
            }
            else if (e.ChangeType == WatcherChangeTypes.Renamed)
            {
                var entry = this.Queries.Where(q => q.Path == e.Name).FirstOrDefault();
                if (entry != null)
                {
                    entry.Path = e.Name;
                }
            }
            else if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                // This event may happen when the file is created, or at any time after than
                // when an existing file is actually changed.
                var entry = this.Queries.Where(q => q.Path == e.Name).FirstOrDefault();
                if (entry == null)
                {
                    entry = new QueryEntry() { Path = e.Name, Description = this.GetDescription(e.FullPath) };
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        this.Queries.Add(entry);
                    });
                }
                else
                {
                    // It was already in the model, but it has changed, so go read the description again.
                    entry.Description = GetDescription(e.FullPath);
                }
            }
        }

        public Model()
        {
            (var queries, var languages) = this.PopulateQueries();
            this.Queries = queries;
            this.Languages = languages;

            // Calculate the directories ...
            foreach (var directory in queries.Select(q => Path.GetDirectoryName(q.Path)).Distinct())
            {
                this.Watcher = new FileSystemWatcher(directory, "*.xq");
                this.Watcher.Changed += new FileSystemEventHandler(QueryDirectoryChanged);
                this.Watcher.Created += new FileSystemEventHandler(QueryDirectoryChanged);
                this.Watcher.Deleted += new FileSystemEventHandler(QueryDirectoryChanged);
                this.Watcher.Renamed += new RenamedEventHandler(QueryDirectoryChanged);

                this.Watcher.EnableRaisingEvents = true;
            }

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
