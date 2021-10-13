using GraphExplorer.Core.netcore;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GraphExplorer.Common
{
    public interface IEdgeRenderer
    {
        void SelectEdgeAsync(Edge edge);
    }
}
