using System;
using System.Collections.Generic;
using System.Text;

namespace GraphExplorer.Common
{
    public interface INodeRendererPlugin : IPlugin
    {
        INodeRenderer CreateRenderer(IModel model);
    }
}
