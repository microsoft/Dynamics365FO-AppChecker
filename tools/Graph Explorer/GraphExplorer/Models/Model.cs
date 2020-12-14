// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SocratexGraphExplorer.Models
{
    public class Model : INotifyPropertyChanged
    {
        public string WebRootPath { private set; get; }

        /// <summary>
        /// The driver for accessing the graph database.
        /// </summary>
        private IDriver Driver { get; set; }

        /// <summary>
        /// Backing field for the nodes shown in the graph
        /// </summary>
        private HashSet<long> nodesShown;

        /// <summary>
        /// The nodes shown on the canvas. An event is raised when the value changes.
        /// </summary>
        public HashSet<long> NodesShown
        {
            get { return this.nodesShown;  }
            set 
            { 
                this.nodesShown = value;
                this.OnPropertyChanged(nameof(NodesShown));
            }
        }

        private List<IRecord> queryResults;
        public List<IRecord> QueryResults
        {
            get { return this.queryResults; }
            set
            {
                this.queryResults = value;
                this.OnPropertyChanged(nameof(QueryResults));
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
                var scriptTextStream = assembly.GetManifestResourceStream("SocratexGraphExplorer.Resources.Script.html");

                using var reader = new StreamReader(scriptTextStream);
                return reader.ReadToEnd();
            }
        }

        public string StyleDocumentSource
        {
            get { return Properties.Settings.Default.Configuration; }
            set
            {
                if (value != Properties.Settings.Default.Configuration)
                {
                    Properties.Settings.Default.Configuration = value;
                    this.OnPropertyChanged(nameof(StyleDocumentSource));
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string errorMessage = "";
        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set
            {
                this.errorMessage = value;
                this.OnPropertyChanged(nameof(ErrorMessage));
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

        private (int, int) editorPosition;
        public (int, int) EditorPosition
        {
            get { return this.editorPosition;  }
            set
            {
                this.editorPosition = value;
                this.OnPropertyChanged(nameof(EditorPosition));
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
                this.OnPropertyChanged(nameof(ConnectionString));
                this.OnPropertyChanged(nameof(Username));
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
                this.OnPropertyChanged(nameof(ConnectionString));
                this.OnPropertyChanged(nameof(Server));
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
                this.OnPropertyChanged(nameof(Port));
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
                    this.OnPropertyChanged(nameof(QueryFontSize));
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
                OnPropertyChanged(nameof(ConnectResultNodes));
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
                OnPropertyChanged(nameof(ShowLineNumbers));
            }
        }

        public string QueryFont => Properties.Settings.Default.QueryFont;

        public IDriver CreateNeo4jDriver(string password)
        {
            this.Driver = GraphDatabase.Driver(string.Format("bolt://{0}:{1}", this.Server, this.Port), AuthTokens.Basic(this.Username, password));
            return this.Driver;
        }

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
                    this.OnPropertyChanged(nameof(IsDarkMode));
                }
            }
        }

        public Model()
        {
            Properties.Settings.Default.PropertyChanged += SettingsChanged;

            // Create the directory where the web artifacts live.
            this.WebRootPath = CreateTemporaryDirectory();

            // Copy the web page to the web root:
            var script = this.Source;
            File.WriteAllText(Path.Combine(this.WebRootPath, "Script.html"), script);

            // Copy the configuration file into the web root directory.
            var configFileName = Path.Combine(this.WebRootPath, "Config.js");
            File.WriteAllText(configFileName, this.StyleDocumentSource);

            var assembly = Assembly.GetExecutingAssembly();

            // Copy all the embedded resources optionally used for graph adornments
            var resourcesPath = Path.Combine(this.WebRootPath, "Resources");
            Directory.CreateDirectory(resourcesPath);

            var resourcePrefix = "SocratexGraphExplorer.Resources.SourcecodeSymbols";
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

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public static string GenerateJSON(List<IRecord> records)
        {
            var nodes = new Dictionary<long,object>();
            var edges = new Dictionary<long, object>();
            var values = new List<object>();

            var inDegrees = new Dictionary<long, long>();  // Map from nodeId onto in degree
            var outDegrees = new Dictionary<long, long>(); // Map from nodeId onto out degree

            void GenerateRelationship(IRelationship relationship)
            {
                if (!edges.ContainsKey(relationship.Id))
                {
                    var edge = new Dictionary<string, object>
                    {
                        ["id"] = relationship.Id,
                        ["type"] = relationship.Type,
                        ["from"] = relationship.StartNodeId,
                        ["to"] = relationship.EndNodeId
                    };

                    if (outDegrees.ContainsKey(relationship.StartNodeId))
                        outDegrees[relationship.StartNodeId] += 1;
                    else
                        outDegrees[relationship.StartNodeId] = 1;

                    if (inDegrees.ContainsKey(relationship.EndNodeId))
                        inDegrees[relationship.EndNodeId] += 1;
                    else
                        inDegrees[relationship.EndNodeId] = 1;

                    var props = new Dictionary<string, object>();
                    foreach (var kvp in relationship.Properties.OrderBy(p => p.Key))
                    {
                        props[kvp.Key] = kvp.Value;
                    }
                    edge["properties"] = props;

                    edges[relationship.Id] = edge;
                }
            }

            void GeneratePath(IPath value)
            {
                // Extract the nodes and the bpath between them
                GenerateNode(value.Start);
                GenerateNode(value.End);

                foreach (var relationship in value.Relationships)
                {
                    GenerateRelationship(relationship);
                }
            }

            void GenerateNode(INode node)
            {
                if (!nodes.ContainsKey(node.Id))
                {
                    var n = new Dictionary<string, object>
                    {
                        ["id"] = node.Id,
                        ["labels"] = node.Labels.ToArray()
                    };

                    var props = new Dictionary<string, object>();
                    foreach (var kvp in node.Properties.OrderBy(p => p.Key))
                    {
                        props[kvp.Key] = kvp.Value;
                    }

                    n["properties"] = props;
                    nodes[node.Id] = n;
                }
            }

            void GenerateList(List<object> l)
            {
                //var v = new List<object>();
                //// TODO. Something is wrong here.
                //foreach (var element in l)
                //{
                //    Generate(element);
                //}
                values.Add(l);
            }

            void GenerateObject(object o)
            {
                if (o != null)
                    values.Add(o);
            }

            void Generate(object value)
            {
                if (value is IPath)
                    GeneratePath(value as IPath);
                else if (value is INode)
                    GenerateNode(value as INode);
                else if (value is IRelationship)
                    GenerateRelationship(value as IRelationship);
                else if (value is List<object>)
                    GenerateList(value as List<object>);
                else
                    GenerateObject(value);
            }

            foreach (var record in records)
            {
                var kvps = record.Values;

                foreach (var kvp in kvps)
                {
                    Generate(kvp.Value);
                }
            }


            foreach (var nodeId in nodes.Keys)
            {
                var node = nodes[nodeId] as Dictionary<string, object>;
                var nodeProperties = node["properties"] as Dictionary<string, object>;

                if (inDegrees.ContainsKey(nodeId))
                {
                    nodeProperties["$indegree"] = inDegrees[nodeId];
                }
                else
                {
                    nodeProperties["$indegree"] = 0;
                }

                if (outDegrees.ContainsKey(nodeId))
                {
                    nodeProperties["$outdegree"] = outDegrees[nodeId];
                }
                else
                {
                    nodeProperties["$outdegree"] = 0;
                }
            }

            var result = new Dictionary<string, object>
            {
                ["nodes"] = nodes.Values,
                ["edges"] = edges.Values,
                ["values"] = values
            };

            var serialized = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            return serialized;
        }

        /// <summary>
        /// Generate a HTML text representation from the data returned
        /// from Neo4j.
        /// </summary>
        /// <param name="records">The records fetched from neo4j</param>
        /// <returns>The HTML representation of the records in human readable form.</returns>
        public static string GenerateHtml(List<IRecord> records)
        {
            static void GeneratePath(StringBuilder b, int indent, IPath value)
            {
                var indentString = new string(' ', indent);
                b.AppendLine(indentString + "{");
                b.Append(indentString + "  start:");
                GenerateNode(b, indent + 2, value.Start);
                b.Append(indentString + "  end:");
                GenerateNode(b, indent + 2, value.End);
                b.AppendLine(indentString + "  segments: [");

                var firstSegment = true;
                foreach(var relation in value.Relationships)
                {
                    if (!firstSegment)
                        b.AppendLine(",");
                    else
                        firstSegment = false;

                    b.AppendLine(indentString + "    {");
                    b.AppendLine(indentString + "      start: " + relation.StartNodeId.ToString() + ",");
                    b.AppendLine(indentString + "      relationship: {");
                    b.AppendLine(indentString + "        id: " + relation.Id.ToString() + ",");
                    b.AppendLine(indentString + "        type: " + relation.Type + ",");
                    b.AppendLine(indentString + "        start: " + relation.StartNodeId.ToString() + ",");
                    b.AppendLine(indentString + "        end: " + relation.EndNodeId.ToString() + ",");
                    b.AppendLine(indentString + "        properties: {");

                    var first = true;
                    foreach (var prop in relation.Properties.OrderBy(p => p.Key))
                    {
                        if (!first)
                            b.AppendLine(",");
                        else
                            first = false;
                        b.Append(indentString + "          " + prop.Key + ": " + prop.Value.ToString());
                    }
                    b.AppendLine();
                    b.AppendLine(indentString + "        }");
                    b.AppendLine(indentString + "      },");
                    b.AppendLine(indentString + "      end: " + relation.EndNodeId.ToString());
                    b.Append("    }");
                }
                b.AppendLine();
                b.AppendLine(indentString + "  ],");
                b.AppendLine(indentString + "  length: " + value.Relationships.Count());
                b.AppendLine("}");
            }

            static void GenerateNode(StringBuilder b, int indent, INode node)
            {
                var indentString = new string(' ', indent);

                b.AppendLine(indentString + "{");
                b.AppendLine(indentString + "  id: " + node.Id.ToString() + ",");

                b.AppendLine(indentString + "  labels: [");
                bool first = true;
                foreach (var label in node.Labels)
                {
                    if (!first)
                        b.AppendLine(",");
                    else
                        first = false;
                    b.Append(indentString + "    " + label);
                }
                b.AppendLine();
                b.AppendLine(indentString + "  ],");

                b.AppendLine(indentString + "  properties: {");
                first = true;
                foreach (var prop in node.Properties.OrderBy(p => p.Key))
                {
                    if (!first)
                        b.AppendLine(",");
                    else
                        first = false;
                    b.Append(indentString + "    " + prop.Key + ": " + prop.Value.ToString());
                }
                b.AppendLine();
                b.AppendLine(indentString + "  }");

                b.AppendLine(indentString + "}");
            }

            static void GenerateRelationship(StringBuilder b, int indent, IRelationship relationship)
            {
                var indentString = new string(' ', indent);

                b.AppendLine(indentString + "{");
                b.AppendLine(indentString + "  id: " + relationship.Id.ToString() + ",");
                b.AppendLine(indentString + "  type: " + relationship.Type + ",");
                b.AppendLine(indentString + "  start: " + relationship.StartNodeId.ToString() + ", ");
                b.AppendLine(indentString + "  end: " + relationship.EndNodeId.ToString() + ", ");

                b.AppendLine(indentString + "  properties: {");
                var first = true;
                foreach (var prop in relationship.Properties.OrderBy(p => p.Key))
                {
                    if (!first)
                        b.AppendLine(",");
                    else
                        first = false;
                    b.Append(indentString + "    " + prop.Key + ": " + prop.Value.ToString());
                }
                b.AppendLine();
                b.AppendLine(indentString + "  }");
            }

            static void GenerateList(StringBuilder b, List<object> l)
            {
                bool first = true;
                b.Append("[");
                foreach (var element in l)
                {
                    if (!first)
                        b.Append(", ");

                    Generate(b, 0, element);
                    first = false;
                }
                b.Append("]");
            }

            static void GenerateString(StringBuilder b, string s)
            {
                b.Append(s);
            }

            static void Generate(StringBuilder b, int indent, object value)
            {
                if (value is IPath)
                    GeneratePath(b, indent, value as IPath);
                else if (value is INode)
                    GenerateNode(b, indent, value as INode);
                else if (value is IRelationship)
                    GenerateRelationship(b, indent, value as IRelationship);
                else if (value is List<object>)
                    GenerateList(b, value as List<object>);
                else
                    GenerateString(b, value.ToString());
            }

            var builder = new StringBuilder();
            builder.AppendLine("<html>");
            builder.AppendLine("<style>");
            builder.AppendLine("    table, th, td {");
            builder.AppendLine("        border: 1px solid black;");
            builder.AppendLine("        font-size: " + Properties.Settings.Default.TextResultsFontSize.ToString() + ";");
            builder.AppendLine("        border-collapse: collapse;");
            builder.AppendLine("    }");
            builder.AppendLine("</style>");
            builder.AppendLine("<body>");
            builder.AppendLine("<table style='width:100%;'>");

            builder.AppendLine("  <tr>");
            if (records.Any())
            {
                foreach (var heading in records.First().Keys)
                {
                    builder.AppendLine("    <th>" + heading + "</th>");
                }
            }
            builder.AppendLine("  </tr>");

            foreach (var record in records)
            {
                builder.AppendLine("  <tr>");

                var keys = record.Values.Keys.GetEnumerator();
                var values = record.Values.Values;

                foreach (var value in values)
                {
                    builder.AppendLine("    <td><pre>");

                    if (value != null)
                    {
                        Generate(builder, 0, value);

                        keys.MoveNext();
                        string key = keys.Current;
                    }
                    builder.AppendLine("    </pre></td>");
                }
                builder.AppendLine("  </tr>");
            }

            builder.AppendLine("</table>");
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");
            return builder.ToString();
        }
     
        public static HashSet<long> HarvestNodeIdsFromGraph(List<IRecord> records)
        {
            var result = new HashSet<long>();

            if (records == null || !records.Any())
                return result;

            App.Current.Dispatcher.Invoke(() =>
            {
                foreach (var record in records)
                {
                    foreach (var v in record.Values)
                    {
                        if (v.Value is IPath)
                        {
                            // There are two nodes connected by an edge:
                            var path = v.Value as IPath;
                            // The start and end can be the same, for self referenctial nodes.
                            result.Add(path.Start.Id);
                            result.Add(path.End.Id);
                        }
                        else if (v.Value is INode)
                        {
                            var n = v.Value as INode;
                            result.Add(n.Id);
                        }
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// Determines if the list of records can be rendered as a graph. This is true if
        /// there are no atomic values (like strings, ints, etc.) in the list of records.
        /// </summary>
        /// <param name="res">The list containing the records.</param>
        /// <returns>True if the records can be interpreted as as graph, and false otherwise.</returns>
        public static bool CanBeRenderedAsGraph(IList<IRecord> res)
        {
            return res.All(rec => rec.Values.All(v => v.Value is INode || v.Value is IPath || v.Value is IRelationship));
        }

        public Task<IResultCursor> ExecuteQueryAsync(string cypherSource, Dictionary<string, object> parameters = null)
        { 
            IAsyncSession session = this.Driver.AsyncSession();
            return session.RunAsync(cypherSource, parameters);
        }

            /// <summary>
            /// Execute the cypher string on the current connection. If the cypher is incorrect
            /// the error message is updated.
            /// </summary>
            /// <param name="cypherSource">The cypher source</param>
            /// <param name="parameters">Any parameters used in the source string</param>
            /// <returns>The list of results.</returns>
        public async Task<List<IRecord>> ExecuteCypherAsync(string cypherSource, IDictionary<string, object> parameters=null)
        {
            this.ErrorMessage = "Running query...";

            IAsyncSession session = this.Driver.AsyncSession();

            try
            {
                IResultCursor cursor = await session.RunAsync(cypherSource, parameters);
                List<IRecord> res = await cursor.ToListAsync();

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
            return null;
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
