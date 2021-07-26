using System.ComponentModel.Composition;
using GraphExplorer.Common;

namespace GraphExplorer.XppPlugin
{
    [Export(typeof(INodeRendererPlugin))]
    [ExportMetadata("Label", "Method")]
    [ExportMetadata("Version", "1.0.0")]
    
    public class MethodNodePlugin : INodeRendererPlugin
    {
        public INodeRenderer CreateRenderer(IModel model)
        {
            return new MethodNodeRenderer(model);
        }
    }
}
