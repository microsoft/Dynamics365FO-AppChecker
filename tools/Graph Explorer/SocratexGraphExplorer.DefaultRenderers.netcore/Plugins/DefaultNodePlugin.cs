using System.ComponentModel.Composition;
using GraphExplorer.Common;

namespace GraphExplorer.DefaultsPlugin
{
    [Export(typeof(INodeRendererPlugin))]
    [ExportMetadata("Label", "()")]
    [ExportMetadata("Version", "1.0.0")]
    
    public class DefaultNodePlugin : INodeRendererPlugin
    {
        public INodeRenderer CreateRenderer(IModel model)
        {
            return new DefaultNodeRenderer(model);
        }
    }
}
