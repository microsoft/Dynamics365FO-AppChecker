using GraphExplorer.Common;
using System.ComponentModel.Composition;

namespace GraphExplorer.DefaultsPlugin
{
    [Export(typeof(IEdgeRendererPlugin))]
    [ExportMetadata("Label", "()")]
    [ExportMetadata("Version", "1.0.0")]

    public class DefaultEdgePlugin : IEdgeRendererPlugin
    {
        public IEdgeRenderer CreateRenderer(IModel model)
        {
            return new DefaultEdgeRenderer(model);
        }
    }
}
