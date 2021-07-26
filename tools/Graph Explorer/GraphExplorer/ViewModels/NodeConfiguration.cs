using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
namespace GraphExplorer.ViewModels
{
    /// <summary>
    /// Class defining properties relating to a node
    /// </summary>
    public class NodeConfiguration 
    {
        /// <summary>
        ///  The node type
        /// </summary>
        public string Selection { get; set; }

        public string Color { get; set; }

        public int Size { get; set; }
    }
}
