// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Neo4j.Driver;
using GraphExplorer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GraphExplorer.Core.netcore;
using unvell.ReoGrid.IO.OpenXML.Schema;

namespace GraphExplorer.Models
{
    public class Model : IModel, INotifyPropertyChanged
    {
        public string WebRootPath { private set; get; }

        /// <summary>
        /// Backing field for the nodes shown in the graph
        /// </summary>
        // private HashSet<long> nodesShown;

        ///// <summary>
        ///// The nodes shown on the canvas. An event is raised when the value changes.
        ///// </summary>
        //public HashSet<long> NodesShown
        //{
        //    get { return this.nodesShown;  }
        //    set 
        //    { 
        //        this.nodesShown = value;
        //        this.OnPropertyChanged(nameof(this.NodesShown));
        //    }
        //}


        private Graph graph = new Graph();

        /// <summary>
        /// This is the rendered graph.
        /// </summary>
        public Graph Graph
        {
            get => this.graph;
            set
            {
                // if (value != this.graph)
                {
                    this.graph = value;
                    this.OnPropertyChanged(nameof(this.Graph));
                }
            }
        }

        /// <summary>
        /// This event is triggered when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Returns the Uri of the file containing the html source of the graph surface.
        /// </summary>
        public Uri ScriptUri {  get { return new Uri(Path.Combine(this.WebRootPath, "Script.html")); } }

        private string Source
        {
            get {
                var assembly = Assembly.GetExecutingAssembly();
                var scriptTextStream = assembly.GetManifestResourceStream("GraphExplorer.Resources.Script.html");

                using var reader = new StreamReader(scriptTextStream);
                return reader.ReadToEnd();
            }
        }

        public string StyleDocumentSource
        {
            get 
            { 
                return Properties.Settings.Default.Configuration; 
            }
            set
            {
                if (value != Properties.Settings.Default.Configuration)
                {
                    Properties.Settings.Default.Configuration = value;
                    this.OnPropertyChanged(nameof(this.StyleDocumentSource));
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // TODO move this to the view model. It does not really belong here.
        private string errorMessage = "";
        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set
            {
                this.errorMessage = value;
                this.OnPropertyChanged(nameof(this.ErrorMessage));
            }
        }
        
        // TODO move this to the view model. It does not really belong here.
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
                    this.OnPropertyChanged(nameof(this.CaretPositionString));
                }
            }
        }

        // TODO Move this to the view model. It does not really belong here.
        private (int, int) editorPosition;
        public (int, int) EditorPosition
        {
            get { return this.editorPosition;  }
            set
            {
                this.editorPosition = value;
                this.OnPropertyChanged(nameof(this.EditorPosition));
            }
        }

        public string Username
        {
            get
            {
                return Properties.Settings.Default.Username;
            }
            set
            {
                Properties.Settings.Default.Username = value;
                this.OnPropertyChanged(nameof(this.ConnectionString));
                this.OnPropertyChanged(nameof(this.Username));
            }
        }

        public string Server
        {
            get
            {
                return Properties.Settings.Default.Server;
            }
            set
            {
                Properties.Settings.Default.Server = value;
                this.OnPropertyChanged(nameof(this.ConnectionString));
                this.OnPropertyChanged(nameof(this.Server));
            }
        }

        public int Port
        {
            get
            {
                return Properties.Settings.Default.Port;
            }
            set
            {
                Properties.Settings.Default.Port = value;
                this.OnPropertyChanged(nameof(this.Port));
            }
        }

        /// <summary>
        /// Set or get the query editor font size.
        /// </summary>
        public int QueryFontSize
        {
            get 
            {
                return Properties.Settings.Default.QueryFontSize;
            }

            set { 
                if (value != Properties.Settings.Default.QueryFontSize)
                {
                    Properties.Settings.Default.QueryFontSize = value;
                    this.OnPropertyChanged(nameof(this.QueryFontSize));
                }
            }
        }

        /// <summary>
        /// Get the connection string that is shown in the title bar.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                var connectionInfo = this.Username + "@" + this.Server;
                return connectionInfo;
            }
        }

        public bool ConnectResultNodes
        {
            get
            {
                return Properties.Settings.Default.ConnectResultNodes;
            }
            set
            {
                Properties.Settings.Default.ConnectResultNodes = value;
                this.OnPropertyChanged(nameof(this.ConnectResultNodes));
            }
        }

        public bool ShowNavigationButtons
        {
            get
            {
                return Properties.Settings.Default.ShowNavigationButtons;
            }
            set
            {
                Properties.Settings.Default.ShowNavigationButtons = value;
                this.OnPropertyChanged(nameof(this.ShowNavigationButtons));
            }
        }

        public bool AllowKeyboardNavigation
        {
            get
            {
                return Properties.Settings.Default.AllowKeyboardNavigation;
            }
            set
            {
                Properties.Settings.Default.AllowKeyboardNavigation = value;
                this.OnPropertyChanged(nameof(this.AllowKeyboardNavigation));
            }
        }

        public bool ShowLineNumbers
        {
            get 
            {
                return Properties.Settings.Default.ShowLineNumbers;
            }
            set 
            {
                Properties.Settings.Default.ShowLineNumbers = value;
                this.OnPropertyChanged(nameof(this.ShowLineNumbers));
            }
        }

        public string QueryFont => Properties.Settings.Default.QueryFont;

        /// <summary>
        /// Specifies whether the UI is rendered in dark or light mode.
        /// </summary>
        public bool IsDarkMode
        {
            get { return Properties.Settings.Default.DarkMode; }
            set {
                if (value != Properties.Settings.Default.DarkMode)
                {
                    Properties.Settings.Default.DarkMode = value;
                    this.OnPropertyChanged(nameof(this.IsDarkMode));
                }
            }
        }

        private static string ExtractSource(string assemblyPath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(assemblyPath);
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public Model()
        {
            Properties.Settings.Default.PropertyChanged += this.SettingsChanged;

            // Create the directory where the web artifacts live.
            this.WebRootPath = CreateTemporaryDirectory();

            // Copy the web page to the web root:
            var script = this.Source;
            File.WriteAllText(Path.Combine(this.WebRootPath, "Script.html"), script);

            // Copy the configuration file into the web root directory.
            var configFileName = Path.Combine(this.WebRootPath, "Config.js");
            File.WriteAllText(configFileName, this.StyleDocumentSource);

            var resourcesPath = Path.Combine(this.WebRootPath, "Resources");
            Directory.CreateDirectory(resourcesPath);

            // Copy some icons that are needed for menus etc. The names are artifacts in the Resources fork
            var artifacts = new[] { "eye-off.svg", "eye-off-outline.svg", "ray-end-arrow.svg", 
                "ray-start-arrow.svg", "ray-start.svg", "ray-start-vertex-end.svg", "ray-end.svg", "ray-vertex.svg",
                "Contextual.js", "Contextual.theme.css", "Contextual.css" };
            string resourcePrefix = "GraphExplorer.Resources.";

            foreach (var artifact in artifacts)
            {
                var source = ExtractSource(resourcePrefix + artifact);
                File.WriteAllText(Path.Combine(this.WebRootPath, artifact), source);
            }

            // Copy all the embedded resources optionally used for graph adornments
            resourcePrefix = "GraphExplorer.Resources.SourcecodeSymbols";

            var assembly = Assembly.GetExecutingAssembly();
            foreach (var resource in assembly.GetManifestResourceNames())
            {
                // Skip names outside of your desired subfolder
                if (!resource.StartsWith(resourcePrefix))
                {
                    continue;
                }
                using Stream input = assembly.GetManifestResourceStream(resource);
                using Stream output = File.Create(Path.Combine(resourcesPath, resource.Substring(resourcePrefix.Length + 1)));
                input.CopyTo(output);
            }
        }

        private static string CreateTemporaryDirectory()
        {
            string path = Path.GetRandomFileName();
            var directory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), path));

            return directory.ToString();
        }
        /// <summary>
        /// Called when any of the properties are changed. We choose to save the 
        /// values every time, so changes survive crashes and unexpected closedowns.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        private void SettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            // Save all the user's settings
            Properties.Settings.Default.Save();
        }

        public bool IsDebugMode
        {
            get
            {
#if (DEBUG)
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Try to determine if the Neo4j server designated by the parameters provided, is online.
        /// </summary>
        /// <param name="server">The URL of the server running the neo4j server.</param>
        /// <param name="port">The port number.</param>
        /// <param name="username">The user name</param>
        /// <param name="password">The password</param>
        /// <returns></returns>
        public async Task<bool> IsServerOnlineAsync(string server, int port, string username, string password)
        {
            if (string.IsNullOrEmpty(server))
                return false;

            if (port < 0)
                return false;

            try
            {
                using IDriver driver = GraphDatabase.Driver(string.Format("bolt://{0}:{1}", server, port), AuthTokens.Basic(username, password));
                if (driver == null)
                    return false;

                try
                {
                    await driver.VerifyConnectivityAsync();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            catch (Exception )
            {
                return false;
            }
        }


     
        //public static HashSet<long> HarvestNodeIdsFromGraph(List<IRecord> records)
        //{
        //    var result = new HashSet<long>();

        //    if (records == null || !records.Any())
        //        return result;

        //    App.Current.Dispatcher.Invoke(() =>
        //    {
        //        foreach (var record in records)
        //        {
        //            foreach (var v in record.Values)
        //            {
        //                if (v.Value is IPath)
        //                {
        //                    // There are two nodes connected by an edge:
        //                    var path = v.Value as IPath;
        //                    // The start and end can be the same, for self referenctial nodes.
        //                    result.Add(path.Start.Id);
        //                    result.Add(path.End.Id);
        //                }
        //                else if (v.Value is INode)
        //                {
        //                    var n = v.Value as INode;
        //                    result.Add(n.Id);
        //                }
        //            }
        //        }
        //    });
        //    return result;
        //}

        /// <summary>
        /// Determines if the list of records can be rendered as a graph. This is true if
        /// there are no atomic values (like strings, ints, lists, etc.) in the list of records.
        /// </summary>
        /// <param name="res">The list containing the records.</param>
        /// <returns>True if the records can be interpreted as as graph, and false otherwise.</returns>
        public static bool CanBeRenderedAsGraph(Graph g)
        {
            return !g.Values.Any();
        }

        /// <summary>
        /// Execute the cypher string on the current connection. If the cypher is incorrect
        /// the error message is updated. This method will update the status line when the
        /// query has executed.
        /// </summary>
        /// <param name="cypherSource">The cypher source</param>
        /// <param name="parameters">Any parameters used in the source string</param>
        /// <returns>The list of results.</returns>
        public async Task<(Graph, string)> ExecuteCypherAsync(string cypherSource, IDictionary<string, object> parameters=null)
        {
            this.ErrorMessage = "Running query...";

            try
            {
                var res = await Neo4jDatabase.ExecuteQueryGraphAndHtmlAsync(cypherSource, parameters);

                this.ErrorMessage = "Done.";
                return res;
            }
            catch (Neo4jException e)
            {
                if (e.Code == "Neo.ClientError.Statement.SyntaxError")
                {
                    // Auxiliary information is passed in the Message(!)
                    // Get the line and column information
                    //  (line 1, column 9 (offset: 8))
                    Regex rx = new Regex(@"(.+)\(line\s+(\d+),\s+column\s+(\d+)");
                    var m = rx.Match(e.Message);
                    if (m.Success)
                    {
                        var lineNo = int.Parse(m.Groups[2].Value);
                        var columnNo = int.Parse(m.Groups[3].Value);
                        var errorString = m.Groups[1].Value;

                        this.ErrorMessage = errorString;
                        this.EditorPosition = (lineNo, columnNo);
                    }
                    else
                    {
                        this.ErrorMessage = e.Message;
                    }
                }
                else
                {
                    this.ErrorMessage = e.Message;
                }
            }
            return (null,"");
        }

        // TODO: remove this.
        public async Task AddNodesAsync(string query, IDictionary<string, object> parameters)
        {
            //var queryResult = await this.ExecuteCypherAsync(query, parameters);
            //var result = Model.HarvestNodeIdsFromGraph(queryResult);

            //if (result != null && result.Any())
            //{
            //    var nodes = this.NodesShown;
            //    nodes.UnionWith(result);
            //    this.NodesShown = nodes;
            //}
        }

        /// <summary>
        /// Called when the application closes down. Do any cleanup here.
        /// </summary>
        public void Close()
        {
            Directory.Delete(this.WebRootPath, recursive: true);
        }
    }
}
