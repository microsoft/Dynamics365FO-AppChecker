using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GraphExplorer.Common
{
    public interface INodeRenderer
    {
        void SelectNodeAsync(INode node);
    }
}
