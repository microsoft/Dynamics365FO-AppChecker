using System;
using System.Collections.Generic;
using System.Text;

namespace GraphExplorer.Common
{
    public interface IEdgeRendererPlugin : IPlugin
    {
        IEdgeRenderer CreateRenderer(IModel model);
    }
}
