using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GraphExplorer.Common
{
    public interface IModel
    {
        HashSet<long> NodesShown { get; set; }

        Task<List<IRecord>> ExecuteCypherAsync(string cypherSource, IDictionary<string, object> parameters = null);

        /// <summary>
        /// Add the nodes specified by the query and its parameters to the set of visible nodes.
        /// </summary>
        /// <param name="query">The query that produces the extra nodes to add.</param>
        /// <param name="parameters">The parameters mapping names used in the query onto values.</param>
        Task AddNodesAsync(string query, IDictionary<string, object> parameters);
    }
}
