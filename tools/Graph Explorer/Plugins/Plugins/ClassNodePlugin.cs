using System.ComponentModel.Composition;
using GraphExplorer.Common;

namespace GraphExplorer.XppPlugin
{
    [Export(typeof(INodeRendererPlugin))]
    [ExportMetadata("Label", "Class")]
    [ExportMetadata("Version", "1.0.0")]
    
    public class ClassNodePlugin : INodeRendererPlugin
    {
        public INodeRenderer CreateRenderer(IModel model)
        {
            return new ClassNodeRenderer(model);
        }
    }
}
