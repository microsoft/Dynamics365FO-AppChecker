using System;
using System.Collections.Generic;
using System.Text;

namespace GraphExplorer.Models
{
    /// <summary>
    /// This class describes a neo4j database.
    /// </summary>
    public class DatabaseDescriptor
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string CurrentStatus { get; set; }
        public bool IsDefault { get; set; }

}
}
