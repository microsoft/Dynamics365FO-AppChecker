using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using GraphExplorer.Core.netcore;

namespace GraphExplorer.Models
{
    internal static class Neo4jDatabase
    {
        private static IDriver Driver { get; set; }

        public static DatabaseDescriptor Database { get; set; }

        public static void CreateDriver(string username, string password, string server, int port)
        {
            Driver = GraphDatabase.Driver(string.Format("bolt://{0}:{1}", server, port), AuthTokens.Basic(username, password));
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
                foreach (var relation in value.Relationships)
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
                        StringBuilder b = new StringBuilder();
                        Generate(b, 0, value);

                        string valueString = b.ToString();
                        builder.AppendLine(WebUtility.HtmlEncode(valueString));

                        keys.MoveNext();
                        string key = keys.Current;
                    }
                    builder.AppendLine("   </pre></td>");
                }
                builder.AppendLine("  </tr>");
            }

            builder.AppendLine("</table>");
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");

            return builder.ToString();
        }

        public static string GenerateJSON(List<IRecord> records)
        {
            var graph = GenerateGraph(records);
            return graph.GenerateJSON();
        }

        /// <summary>
        /// Generate a graph instance from the list of records provided by the
        /// Neo4j database.
        /// </summary>
        /// <param name="records">The incoming records</param>
        /// <returns>A graph instance with the nodes, edges and values provided by
        /// the neo4j graph database.</returns>
        public static Graph GenerateGraph(List<IRecord> records)
        {
            var nodes = new Dictionary<long, Node>();
            var edges = new Dictionary<long, Edge>();
            var values = new List<object>();

            var inDegrees = new Dictionary<long, long>();  // Map from nodeId onto in degree
            var outDegrees = new Dictionary<long, long>(); // Map from nodeId onto out degree

            void GenerateRelationship(IRelationship relationship)
            {
                if (!edges.ContainsKey(relationship.Id))
                {
                    var edge = new Edge
                    {
                        Id = relationship.Id,
                        Type = relationship.Type,
                        From = relationship.StartNodeId,
                        To = relationship.EndNodeId
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
                    
                    edge.Properties = props;

                    edges[relationship.Id] = edge;
                }
            }

            void GeneratePath(IPath value)
            {
                // Extract the nodes and the path between them
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
                    var n = new Node
                    {
                        Id = node.Id,
                        Labels = node.Labels.ToArray()
                    };

                    var props = new Dictionary<string, object>();
                    foreach (var kvp in node.Properties.OrderBy(p => p.Key))
                    {
                        props[kvp.Key] = kvp.Value;
                    }

                    n.Properties = props;
                    nodes[node.Id] = n;
                }
            }

            void GenerateList(List<object> l)
            {
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

            // Now calculate the indegree and outdegree of the nodes and add them
            // to the properties. Use '$' to dismbiguate from any properties that
            // the user may have used.
            foreach (var nodeId in nodes.Keys)
            {
                var node = nodes[nodeId]; // as Dictionary<string, object>;
                var nodeProperties = node.Properties; // ["properties"] as Dictionary<string, object>;

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

            var result = new Graph(nodes, edges, values);
            return result;
        }
        
        public static async Task<Graph> GetNodeAsync(long nodeId)
        {
            var query = "match (n) where id(n) = $nodeId return n";
            var result = await ExecuteCypherQueryListAsync(query, new Dictionary<string, object>() { { "nodeId", nodeId } });

            return GenerateGraph(result);
        }

        /// <summary>
        /// Generates the graph of the outgoing edges (and the nodes at the other end of
        /// those edges) for the node specified by its unique id.
        /// </summary>
        /// <param name="nodeId">The Id of the node for which the outgoing edges is required.</param>
        /// <returns>The graph with the outgoing edges, their nodes and the incoming node.</returns>
        public static async Task<Graph> GetOutgoingEdges(long nodeId)
        {
            var query = "match (n) -[r]-> (q) where id(n) = $nodeId return *";
            var result = await ExecuteCypherQueryListAsync(query, new Dictionary<string, object>() { { "nodeId", nodeId } });

            return GenerateGraph(result);
        }

        public static async Task<Graph> GetIncomingEdges(long nodeId)
        {
            var query = "match (n) <-[r]- (q) where id(n) = $nodeId return *";
            var result = await ExecuteCypherQueryListAsync(query, new Dictionary<string, object>() { { "nodeId", nodeId } });

            return GenerateGraph(result);
        }

        /// <summary>
        /// Get the names of the nodes in the current database.
        /// </summary>
        /// <returns>The list of the node names.</returns>
        public static async Task<IEnumerable<string>> GetNodeLabels()
        {
            var result = await ExecuteCypherQueryListAsync(@"
CALL db.labels() YIELD label
RETURN { name: 'labels', data: COLLECT(label)[..1000]} AS result", null);

            KeyValuePair<string, object> f = result[0].Values.FirstOrDefault();

            var dictionary = f.Value as Dictionary<string, object>;
            var names = dictionary["data"] as List<object>;

            return names.Select(q => q as string).OrderBy(q => q);
        }

        /// <summary>
        /// Get the databases known by Neo4j
        /// </summary>
        /// <returns>The list of database descriptors for the databases known by Neo4j</returns>
        public static async Task<IEnumerable<DatabaseDescriptor>> GetDatabases()
        {
            IAsyncSession session = Driver.AsyncSession(o => o.WithDatabase("system"));
            var queryResult = await session.RunAsync("SHOW DATABASES");
            var list = await queryResult.ToListAsync();

            var result = new List<DatabaseDescriptor>();

            foreach (var record in list)
            {
                var database = new DatabaseDescriptor
                {
                    Name = record.Values["name"] as string,
                    Address = record.Values["address"] as string,
                    IsDefault = (bool)record.Values["default"],
                    CurrentStatus = record.Values["currentStatus"] as string
                };
                result.Add(database);
            }

            return result;
        }

        public static async Task<List<IRecord>> ExecuteCypherQueryListAsync(string cypherSource, IDictionary<string, object> parameters = null)
        {
            // TODO this should not be public, since it pollutes the code with the Neo4j specific type.
            // All usage should be replaced with the graph based APIs.
            IAsyncSession session = Database != null && !string.IsNullOrEmpty(Database.Name)
                ? Driver.AsyncSession(o => o.WithDatabase(Database.Name))
                : Driver.AsyncSession();

            var transaction = await session.BeginTransactionAsync();
            var cursor = await transaction.RunAsync(cypherSource, parameters);
            var result = await cursor.ToListAsync();

            await transaction.CommitAsync();

            return result;
        }

        public static async Task<Graph> GetImplicitEdgesAsync(Graph g)
        {
            var query = @"
                match (n) where id(n) in $nodeIds optional match (n) -[r]- (m) 
                where id(n) in $nodeIds and id(m) in $nodeIds 
                return n,m,r";

            return await ExecuteQueryGraphAsync(query, new Dictionary<string, object>() { { "nodeIds", g.Nodes.Select(n => n.Id).ToArray() } });
        }

        public static async Task<Graph> ExecuteQueryGraphAsync(string source, IDictionary<string, object> parameters = null)
        {
            var records = await ExecuteCypherQueryListAsync(source, parameters);
            var graph = GenerateGraph(records);
            return graph;
        }

        public static async Task<(Graph,string)> ExecuteQueryGraphAndHtmlAsync(string source, IDictionary<string, object> parameters = null)
        {
            var records = await ExecuteCypherQueryListAsync(source, parameters);
            var graph = GenerateGraph(records);
            var html = GenerateHtml(records);
            return (graph, html);
        }
    }
}
