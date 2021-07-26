using System.ComponentModel.Composition;
using GraphExplorer.Common;

namespace GraphExplorer.XppPlugin
{
    [Export(typeof(INodeRendererPlugin))]
    [ExportMetadata("Label", "Table")]
    [ExportMetadata("Version", "1.0.0")]
    
    public class TableNodePlugin : INodeRendererPlugin
    {
        public INodeRenderer CreateRenderer(IModel model)
        {
            return new TableNodeRenderer(model);
        }
    }
}
