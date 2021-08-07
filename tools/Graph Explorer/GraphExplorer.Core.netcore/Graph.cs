﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace GraphExplorer.Core.netcore
{
    #region Nodes and Edges
    public class Edge
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public long From { get; set; }
        public long To { get; set; }

        public IDictionary<string, object> Properties { get; set; }

         public Edge()
        {
            this.Properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }
    }

    public class Node
    {
        public long Id { get; set; }
        public string[] Labels { get; set; }

        public IDictionary<string, object> Properties { get; set; }

        public Node()
        {
            this.Properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }
    }
    #endregion

    /// <summary>
    /// This is the representation of a graph. It contains nodes, edges and values.
    /// This representation does not have any dependencies to any backend.
    /// </summary>
    public class Graph
    {
        private IDictionary<long, Node> NodesDictionary { get; set; }
        private IDictionary<long, Edge> EdgesDictionary { get; set; }
        private IEnumerable<object> ValuesList { get; set; }

        public IEnumerable<Node> Nodes
        {
            get
            {
                return this.NodesDictionary.Values;
            }
            set
            {
                foreach (var n in value)
                {
                    this.NodesDictionary[n.Id] = n;
                }
            }
        }

        public IEnumerable<Edge> Edges
        {
            get
            {
                return this.EdgesDictionary.Values;
            }
            set
            {
                foreach (var e in value)
                {
                    this.EdgesDictionary[e.Id] = e;
                }
            }
        }

        public IEnumerable<object> Values => this.ValuesList;

        public Graph()
        {
            this.NodesDictionary = new Dictionary<long, Node>();
            this.EdgesDictionary = new Dictionary<long, Edge>();
            this.ValuesList = new List<object>();
        }

        public Graph (IDictionary<long, Node> nodeDictionary, IDictionary<long, Edge> edgeDictionary, IEnumerable<object> values)
        {
            this.NodesDictionary = nodeDictionary;
            this.EdgesDictionary = edgeDictionary;
            this.ValuesList = values;
        }

        /// <summary>
        /// Create a new graph like this one. 
        /// </summary>
        /// <returns>A new graph with the same nodes, edges and values as this one.</returns>
        public Graph Clone()
        {
            var nodes = new Dictionary<long, Node>();
            var edges = new Dictionary<long, Edge>();
            var values = new object[this.ValuesList.Count()];

            foreach (var node in this.Nodes)
            {
                nodes[node.Id] = node;
            }

            foreach (var edge in this.Edges)
            {
                edges[edge.Id] = edge;
            }

            int i = 0;
            foreach (var v in this.ValuesList)
            {
                values[i++] = v;
            }

            var result = new Graph
            {
                NodesDictionary = nodes,
                EdgesDictionary = edges,
                ValuesList = values
            };

            return result;
        }

        /// <summary>
        /// Merge this graph with an existing one. The result does not overwrite this
        /// graph value: A new graph is returned.
        /// </summary>
        /// <param name="g">The incoming graph.</param>
        /// <returns>A graph with the nodes and edges from this graph and the nodes
        /// and edges from the incoming graph. No duplicates are ever created.</returns>
        public Graph Merge(Graph g)
        {
            // Add all the nodes and edges that are not already in the graph
            var self = this.Clone();

            // Now add the information from the incoming graph.
            foreach (var node in g.Nodes)
            {
                if (!self.NodesDictionary.ContainsKey(node.Id))
                {
                    self.NodesDictionary[node.Id] = node;
                }
            }

            foreach (var edge in g.Edges)
            { 
                if (!self.EdgesDictionary.ContainsKey(edge.Id))
                {
                    self.EdgesDictionary[edge.Id] = edge;
                }
            }

            return self;
        }

        /// <summary>
        /// Get all the node ids in this graph
        /// </summary>
        /// <returns>The set of node ids in this graph.</returns>
        public HashSet<long> GetNodeIds()
        {
            return this.NodesDictionary.Keys.ToHashSet<long>();
        }

        /// <summary>
        /// Get the node designated by the given node id.
        /// </summary>
        /// <param name="nodeId">The node id that designates the node to get.</param>
        /// <returns>The designated node, or null if no node with the given id exists.</returns>
        public Node GetNode(long nodeId)
        {
            // Node node;
            if (this.NodesDictionary.TryGetValue(nodeId, out Node node))
            {
                return node;
            }
            return null;
        }

        /// <summary>
        /// Get the edge designated by the given id.
        /// </summary>
        /// <param name="edgeId">The edge id that designates the edge to get.</param>
        /// <returns>The designated edge, or null if no edge with the given id exists.</returns>
        public Edge GetEdge(long edgeId)
        {
            // Node node;
            if (this.EdgesDictionary.TryGetValue(edgeId, out Edge edge))
            {
                return edge;
            }
            return null;
        }

        /// <summary>
        /// Delete the given node and its incoming and outgoing edges
        /// </summary>
        /// <param name="nodeId">The node to delete.</param>
        public void DeleteNode(long nodeId)
        {
            if (this.NodesDictionary.ContainsKey(nodeId))
            {
                // Deal with all the edges:
                var edges = this.EdgesDictionary.Values.Where(e => e.From == nodeId || e.To == nodeId);
                foreach (var e in edges)
                {
                    this.EdgesDictionary.Remove(e.Id);
                }

                // Now delete the node itself:
                this.NodesDictionary.Remove(nodeId);
            }
        }

        /// <summary>
        /// Delete the given edge 
        /// </summary>
        /// <param name="edgeId">The node to delete.</param>
        public void DeleteEdge(long edgeId)
        {
            if (this.EdgesDictionary.ContainsKey(edgeId))
            {
                // Now delete the node itself:
                this.EdgesDictionary.Remove(edgeId);
            }
        }

        public string GenerateJSON()
        {
            var serialized = JsonConvert.SerializeObject(this, Formatting.Indented);
            return serialized;
        }

        /// <summary>
        /// Generate a HTML text representation from the data returned
        /// from Neo4j.
        /// </summary>
        /// <param name="records">The records fetched from neo4j</param>
        /// <returns>The HTML representation of the records in human readable form.</returns>
        public string GenerateHtml(int fontsize=12)
        {
            return "";
//            #region old
//            static void GeneratePath(StringBuilder b, int indent, IPath value)
//            {
//                var indentString = new string(' ', indent);
//                b.AppendLine(indentString + "{");
//                b.Append(indentString + "  start:");
//                GenerateNode(b, indent + 2, value.Start);
//                b.Append(indentString + "  end:");
//                GenerateNode(b, indent + 2, value.End);
//                b.AppendLine(indentString + "  segments: [");

//                var firstSegment = true;
//                foreach (var relation in value.Relationships)
//                {
//                    if (!firstSegment)
//                        b.AppendLine(",");
//                    else
//                        firstSegment = false;

//                    b.AppendLine(indentString + "    {");
//                    b.AppendLine(indentString + "      start: " + relation.StartNodeId.ToString() + ",");
//                    b.AppendLine(indentString + "      relationship: {");
//                    b.AppendLine(indentString + "        id: " + relation.Id.ToString() + ",");
//                    b.AppendLine(indentString + "        type: " + relation.Type + ",");
//                    b.AppendLine(indentString + "        start: " + relation.StartNodeId.ToString() + ",");
//                    b.AppendLine(indentString + "        end: " + relation.EndNodeId.ToString() + ",");
//                    b.AppendLine(indentString + "        properties: {");

//                    var first = true;
//                    foreach (var prop in relation.Properties.OrderBy(p => p.Key))
//                    {
//                        if (!first)
//                            b.AppendLine(",");
//                        else
//                            first = false;
//                        b.Append(indentString + "          " + prop.Key + ": " + prop.Value.ToString());
//                    }
//                    b.AppendLine();
//                    b.AppendLine(indentString + "        }");
//                    b.AppendLine(indentString + "      },");
//                    b.AppendLine(indentString + "      end: " + relation.EndNodeId.ToString());
//                    b.Append("    }");
//                }
//                b.AppendLine();
//                b.AppendLine(indentString + "  ],");
//                b.AppendLine(indentString + "  length: " + value.Relationships.Count());
//                b.AppendLine("}");
//            }

//            static void GenerateNode(StringBuilder b, int indent, INode node)
//            {
//                var indentString = new string(' ', indent);

//                b.AppendLine(indentString + "{");
//                b.AppendLine(indentString + "  id: " + node.Id.ToString() + ",");

//                b.AppendLine(indentString + "  labels: [");
//                bool first = true;
//                foreach (var label in node.Labels)
//                {
//                    if (!first)
//                        b.AppendLine(",");
//                    else
//                        first = false;
//                    b.Append(indentString + "    " + label);
//                }
//                b.AppendLine();
//                b.AppendLine(indentString + "  ],");

//                b.AppendLine(indentString + "  properties: {");
//                first = true;
//                foreach (var prop in node.Properties.OrderBy(p => p.Key))
//                {
//                    if (!first)
//                        b.AppendLine(",");
//                    else
//                        first = false;
//                    b.Append(indentString + "    " + prop.Key + ": " + prop.Value.ToString());
//                }
//                b.AppendLine();
//                b.AppendLine(indentString + "  }");

//                b.AppendLine(indentString + "}");
//            }

//            static void GenerateRelationship(StringBuilder b, int indent, IRelationship relationship)
//            {
//                var indentString = new string(' ', indent);

//                b.AppendLine(indentString + "{");
//                b.AppendLine(indentString + "  id: " + relationship.Id.ToString() + ",");
//                b.AppendLine(indentString + "  type: " + relationship.Type + ",");
//                b.AppendLine(indentString + "  start: " + relationship.StartNodeId.ToString() + ", ");
//                b.AppendLine(indentString + "  end: " + relationship.EndNodeId.ToString() + ", ");

//                b.AppendLine(indentString + "  properties: {");
//                var first = true;
//                foreach (var prop in relationship.Properties.OrderBy(p => p.Key))
//                {
//                    if (!first)
//                        b.AppendLine(",");
//                    else
//                        first = false;
//                    b.Append(indentString + "    " + prop.Key + ": " + prop.Value.ToString());
//                }
//                b.AppendLine();
//                b.AppendLine(indentString + "  }");
//            }

//            static void GenerateList(StringBuilder b, List<object> l)
//            {
//                bool first = true;
//                b.Append("[");
//                foreach (var element in l)
//                {
//                    if (!first)
//                        b.Append(", ");

//                    Generate(b, 0, element);
//                    first = false;
//                }
//                b.Append("]");
//            }

//            static void GenerateString(StringBuilder b, string s)
//            {
//                b.Append(s);
//            }

//            static void Generate(StringBuilder b, int indent, object value)
//{
//                if (value is IPath)
//                    GeneratePath(b, indent, value as IPath);
//                else if (value is INode)
//                    GenerateNode(b, indent, value as INode);
//                else if (value is IRelationship)
//                    GenerateRelationship(b, indent, value as IRelationship);
//                else if (value is List<object>)
//                    GenerateList(b, value as List<object>);
//                else
//                    GenerateString(b, value.ToString());
//            }
//            #endregion

//            var builder = new StringBuilder();
//            builder.AppendLine("<html>");
//            builder.AppendLine("<style>");
//            builder.AppendLine("    table, th, td {");
//            builder.AppendLine("        border: 1px solid black;");
//            builder.AppendLine("        font-size: " + fontsize.ToString() + ";");
//            builder.AppendLine("        border-collapse: collapse;");
//            builder.AppendLine("    }");
//            builder.AppendLine("</style>");
//            builder.AppendLine("<body>");
//            builder.AppendLine("<table style='width:100%;'>");

//            // Generate a row for each record
//            builder.AppendLine("  <tr>");
//            if (records.Any())
//            {
//                foreach (var heading in records.First().Keys)
//                {
//                    builder.AppendLine("    <th>" + heading + "</th>");
//                }
//            }
//            builder.AppendLine("  </tr>");

//            foreach (var record in records)
//            {
//                builder.AppendLine("  <tr>");

//                var keys = record.Values.Keys.GetEnumerator();
//                var values = record.Values.Values;

//                foreach (var value in values)
//                {
//                    builder.AppendLine("    <td><pre>");

//                    if (value != null)
//                    {
//                        StringBuilder b = new StringBuilder();
//                        Generate(b, 0, value);

//                        string valueString = b.ToString();
//                        builder.AppendLine(WebUtility.HtmlEncode(valueString));

//                        keys.MoveNext();
//                        string key = keys.Current;
//                    }
//                    builder.AppendLine("   </pre></td>");
//                }
//                builder.AppendLine("  </tr>");
//            }

//            builder.AppendLine("</table>");
//            builder.AppendLine("</body>");
//            builder.AppendLine("</html>");
//            return builder.ToString();
        }
    }
}
