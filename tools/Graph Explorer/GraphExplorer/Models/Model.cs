// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SocratexGraphExplorer.Models
{
    public class PropertyItem
    {
        public string Key { get; set;  }
        public string Value { get; set; }
    }

    public class Model : INotifyPropertyChanged
    {
        /// <summary>
        /// The driver for accessing the graph database.
        /// </summary>
        private IDriver Driver { get; set; }

        /// <summary>
        /// The collection of properties for a selected node or edge.
        /// </summary>
        public ObservableCollection<PropertyItem> NodeProperties { get; } = new ObservableCollection<PropertyItem>();

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

        private long selectedNode = 0;
        public long SelectedNode
        {
            get
            {
                return this.selectedNode;
            }
            set
            {
                if (this.selectedNode != value)
                {
                    this.selectedNode = value;
                    this.OnPropertyChanged(nameof(SelectedNode));
                }
            }
        }

        private long selectedEdge = 0;
        public long SelectedEdge
        { 
            get
            {
                return this.selectedEdge;
            }
            set
            {
                if (this.selectedEdge != value)
                {
                    this.selectedEdge = value;
                    this.OnPropertyChanged(nameof(SelectedEdge));
                }
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

        public HashSet<long> HarvestNodeIdsFromGraph(List<IRecord> records)
        {
            if (!records.Any())
                return null;

            var result = new HashSet<long>();
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
        /// Populate the list of ListView items containing node or edge properties.
        /// </summary>
        /// <param name="records"></param>
        public void GeneratePropertyNodeListView(List<IRecord> records)
        {
            if (!records.Any())
                return;

            App.Current.Dispatcher.Invoke(() =>
            {
                this.NodeProperties.Clear();
                var v = records[0];
                KeyValuePair<string, object> f = v.Values.FirstOrDefault();
                INode n = f.Value as INode;

                if (n != null)
                {
                    string labelString = string.Empty;
                    foreach (var label in n.Labels)
                    {
                        labelString = ":" + label;
                    }
                    this.NodeProperties.Add(new PropertyItem() { Key = "Label", Value = labelString });
                    this.NodeProperties.Add(new PropertyItem() { Key = "Id", Value = n.Id.ToString() });
                    foreach (var property in n.Properties)
                    {
                        this.NodeProperties.Add(new PropertyItem() { Key = property.Key, Value = property.Value.ToString() });
                    }
                }
                else
                {
                    IRelationship r = f.Value as IRelationship;

                    string typeString = ":" + r.Type;
                    this.NodeProperties.Add(new PropertyItem() { Key = "Type", Value = typeString });

                    foreach (var property in r.Properties)
                    {
                        this.NodeProperties.Add(new PropertyItem() { Key = property.Key, Value = property.Value.ToString() });
                    }
                }
            });
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

        public async Task<List<IRecord>> ExecuteCypherAsync(string cypherSource, Dictionary<string, object> parameters=null)
        {
            var session = this.Driver.AsyncSession();
            IResultCursor cursor;

            this.ErrorMessage = "";
            try
            {
                cursor = await session.RunAsync(cypherSource, parameters);
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
