using System;
using System.Collections.Generic;
using System.Text;

namespace SocratexGraphExplorer.Common
{
    public interface IEdgeRendererPlugin : IPlugin
    {
        IEdgeRenderer CreateRenderer(IModel model);
    }
}
