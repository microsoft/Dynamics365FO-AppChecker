using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExplorer.Models
{
    public class IconKindGroup
    {
        public string Kind { get; }
        public string[] Aliases { get; }

        public IconKindGroup(IEnumerable<string> kinds)
        {
            if (kinds is null) 
                throw new ArgumentNullException(nameof(kinds));

            var allValues = kinds.ToList();

            if (!allValues.Any()) 
                throw new ArgumentException($"{nameof(kinds)} must contain at least one value");

            this.Kind = allValues.First();
            this.Aliases = allValues
                .OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase)
                .ToArray();
        }
    }
}
