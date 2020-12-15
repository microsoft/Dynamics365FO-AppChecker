using System;
using System.Collections.Generic;
using System.Text;

namespace SocratexGraphExplorer.Common
{
    public interface IPluginMetadata
    {
        string Label { get; }
        string Version { get; }
    }
}
