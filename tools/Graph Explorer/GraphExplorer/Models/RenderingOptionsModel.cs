using System;
using System.Collections.Generic;
using System.Text;

namespace SocratexGraphExplorer.Models
{
    internal class RenderingOptionsModel
    {
        public IEnumerable<string> GetNodeLabels()
        {
            return new[] { "Banana", "kumquat", "Apple" };
        }

        public IEnumerable<string> GetEdgeLabels()
        {
            return new[] { "CALLS", "DERIVEDFROM", "METHODIN" };
        }
    }
}
