// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        public void CreateNeo4jDriver(string password)
        {
            this.Driver = GraphDatabase.Driver(string.Format("bolt://{0}:{1}", this.Server, this.Port), AuthTokens.Basic(this.Username, password));
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

                    var session = driver.AsyncSession();
                    var cursor = await session.RunAsync("match(c) return count(c)");
                    var list = await cursor.ToListAsync();
                    return list != null && list.Any();
                }
            }
            catch (Exception )
            {
                return false;
            }
        }


 
        //private string GenerateValue(string key, object value)
        //{
        //    if (value is List<object>)
        //    {
        //        var retval = "[";
        //        bool first = true;
        //        foreach (var v in value as List<object>)
        //        {
        //            if (!first)
        //                retval += ", ";
        //            retval += GenerateValue("", v);
        //            first = false;
        //        }
        //        return retval + "]";
        //    }
        //    else if (value is string)
        //    {
        //        return (string)value;
        //    }
        //    else if (value is long)
        //    {
        //        return ((long)value).ToString();
        //    }
        //    else if (value is double)
        //    {
        //        return ((double)value).ToString();
        //    }
        //    return "Unknown type: " + value.GetType().FullName ;
        //}

        //private string GenerateText(List<IRecord> records)
        //{
        //    var retval = "";
        //    foreach (var record in records)
        //    {
        //        var keys = record.Values.Keys.GetEnumerator();
        //        var values = record.Values.Values;

        //        foreach (var value in values)
        //        {
        //            keys.MoveNext();
        //            string key = keys.Current;
        //        }
        //    }

        //    return retval;
        //}

        //private Graph GenerateGraph(List<IRecord> records)
        //{
        //    Graph result = new Graph();

        //    IDictionary<long, INode> nodes = new Dictionary<long, INode>();
        //    IDictionary<long, IRelationship> relationships = new Dictionary<long, IRelationship>();

        //    this.currentColor = 0;

        //    foreach (var record in records)
        //    {
        //        var keys = record.Values.Keys.GetEnumerator();
        //        var values = record.Values.Values;

        //        foreach (var value in values)
        //        {
        //            keys.MoveNext();
        //            string key = keys.Current;

        //            if (value is INode)
        //            {
        //                INode node = value as INode;
        //                if (!nodes.ContainsKey(node.Id))
        //                    nodes.Add(node.Id, node);
        //            }
        //            else if (value is IRelationship)
        //            {
        //                IRelationship relationship = value as IRelationship;
        //                if (!relationships.ContainsKey(relationship.Id))
        //                    relationships.Add(relationship.Id, relationship);
        //            }
        //            else if (value is IPath)
        //            {
        //                IPath path = value as IPath;
        //                foreach (var node in path.Nodes)
        //                {
        //                    INode n = node as INode;
        //                    if (!nodes.ContainsKey(n.Id))
        //                        nodes.Add(n.Id, n);
        //                }

        //                foreach (var relationship in path.Relationships)
        //                {
        //                    IRelationship r = relationship as IRelationship;
        //                    if (!relationships.ContainsKey(r.Id))
        //                        relationships.Add(r.Id, r);
        //                }
        //            }
        //            else
        //            {
        //                this.GenerateValue(key, value);
        //            }
        //        }
        //    }

        //    IDictionary<long, GraphVertex> verticesById = new Dictionary<long, GraphVertex>();

        //    foreach (var node in nodes)
        //    {
        //        var labels = node.Value.Labels.ToArray();
        //        var vertex = new GraphVertex(node.Key, labels, node.Value.Properties);
        //        verticesById.Add(node.Key, vertex);

        //        if (!VertexColors.ContainsKey(vertex.Labels.First()))
        //        {
        //            VertexColors[vertex.Labels.First()] = colors[currentColor];
        //            currentColor += 1;
        //            currentColor = currentColor % colors.Length;
        //        }
        //        vertex.Color = VertexColors[vertex.Labels.First()];

        //        result.AddVertex(vertex);
        //    }
        //    foreach (var relationship in relationships)
        //    {
        //        var sourceNodeId = relationship.Value.StartNodeId;
        //        var targetNodeId = relationship.Value.EndNodeId;
        //        var edge = new GraphEdge(verticesById[sourceNodeId], verticesById[targetNodeId], relationship.Value.Type);

        //        result.AddEdge(edge);
        //    }

        //    return result;
        //}

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

            this.ErrorMessage = "";
            try
            {
                IResultCursor cursor = await session.RunAsync(cypherSource, parameters);
                return await cursor.ToListAsync();
            }
            catch (Exception e)
            {
                this.ErrorMessage = e.Message;
            }
            return null;
        }
    }
}
