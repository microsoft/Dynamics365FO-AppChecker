using System.ComponentModel.Composition;
using GraphExplorer.Common;

namespace GraphExplorer.XppPlugin
{
    [Export(typeof(INodeRendererPlugin))]
    [ExportMetadata("Label", "ClassMember")]
    [ExportMetadata("Version", "1.0.0")]
    
    public class ClassMemberNodePlugin : INodeRendererPlugin
    {
        public INodeRenderer CreateRenderer(IModel model)
        {
            return new ClassMemberNodeRenderer(model);
        }
    }
}
