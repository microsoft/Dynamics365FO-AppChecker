// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocratexGraphExplorer.Models
{
    public class Model : INotifyPropertyChanged
    {
        /// <summary>
        /// The driver for accessing the graph database.
        /// </summary>
        private IDriver Driver { get; set; }

        /// <summary>
        /// Backing field for the nodes shown in the graph
        /// </summary>
        private HashSet<long> nodesShown;

        /// <summary>
        /// The nodes shown on the canvas. A property is raised when the value changes.
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

        public string Source
        {
            get {
                var s = Properties.Settings.Default.Configuration;

                // Inject connection information into the configuration.
                var source = s
                    .Replace("{Server}", this.Server)
                    .Replace("{Port}", this.Port.ToString())
                    .Replace("{Username}", this.Username)
                    .Replace("{Password}", this.Password);

                return source;
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

        public string Password { get; set; }

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

        public string QueryFont => Properties.Settings.Default.QueryFont;

        public IDriver CreateNeo4jDriver(string password)
        {
            this.Driver = GraphDatabase.Driver(string.Format("bolt://{0}:{1}", this.Server, this.Port), AuthTokens.Basic(this.Username, password));
            return this.Driver;
        }

        public Model()
        {
            Properties.Settings.Default.PropertyChanged += SettingsChanged;
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
                using (IDriver driver = GraphDatabase.Driver(string.Format("bolt://{0}:{1}", server, port), AuthTokens.Basic(username, password)))
                {
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
            }
            catch (Exception )
            {
                return false;
            }
        }

        /// <summary>
        /// Generate a HTML text representation from the data returned
        /// from Neo4j.
        /// </summary>
        /// <param name="records">The records fetched from neo4j</param>
        /// <returns>The HTML representation of the records in human readable form.</returns>
        public static string GenerateHtml(List<IRecord> records)
        {
            void GeneratePath(StringBuilder b, int indent, IPath value)
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

            void GenerateNode(StringBuilder b, int indent, INode node)
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

            void GenerateRelationship(StringBuilder b, int indent, IRelationship relationship)
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

            void GenerateList(StringBuilder b, List<object> l)
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

            void GenerateString(StringBuilder b, string s)
            {
                b.Append(s);
            }

            void Generate(StringBuilder b, int indent, object value)
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
     
        public static string CommaSeparatedString(IEnumerable<long> set)
        {
            return string.Join(",", set);
        }

        public static HashSet<long> HarvestNodeIdsFromGraph(List<IRecord> records)
        {
            var result = new HashSet<long>();

            if (!records.Any())
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
        /// Execute the cypher string on the current connection. If the cypher is incorrect
        /// the error message is updated.
        /// </summary>
        /// <param name="cypherSource">The cypher source</param>
        /// <param name="parameters">Any parameters used in the source string</param>
        /// <returns>The list of results.</returns>
        public async Task<List<IRecord>> ExecuteCypherAsync(string cypherSource, Dictionary<string, object> parameters=null)
        {
            var session = this.Driver.AsyncSession();

            this.ErrorMessage = "Running query...";
            try
            {
                IResultCursor cursor = await session.RunAsync(cypherSource, parameters);
                var res = await cursor.ToListAsync();

                this.ErrorMessage = "Done.";
                return res;
            }
            catch (Exception e)
            {
                this.ErrorMessage = e.Message;
            }
            return null;
        }
    }
}
