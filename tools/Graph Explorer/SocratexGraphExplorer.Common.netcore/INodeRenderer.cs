using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GraphExplorer.Core.netcore;

namespace GraphExplorer.Common
{
    public interface INodeRenderer
    {
        void SelectNodeAsync(Node node);
    }
}
