using System.ComponentModel.Composition;
using SocratexGraphExplorer.Common;

namespace SocratexGraphExplorer.XppPlugin
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
