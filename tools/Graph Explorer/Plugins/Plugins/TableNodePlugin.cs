using System.ComponentModel.Composition;
using SocratexGraphExplorer.Common;

namespace SocratexGraphExplorer.XppPlugin
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
