using System.ComponentModel.Composition;
using GraphExplorer.Common;

namespace GraphExplorer.XppPlugin
{
    [Export(typeof(INodeRendererPlugin))]
    [ExportMetadata("Label", "Form")]
    [ExportMetadata("Version", "1.0.0")]
    
    public class FormNodePlugin : INodeRendererPlugin
    {
        public INodeRenderer CreateRenderer(IModel model)
        {
            return new FormNodeRenderer(model);
        }
    }
}
