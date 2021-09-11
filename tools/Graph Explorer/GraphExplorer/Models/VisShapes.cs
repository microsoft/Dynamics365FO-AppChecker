using System;
using System.Collections.Generic;
using System.Text;

namespace GraphExplorer.Models
{
    /// <summary>
    /// Enums describing the layout of vis.js nodes.
    /// See https://visjs.github.io/vis-network/docs/network/nodes.html
    /// </summary>
    enum VisShapes
    {
        // These draw the label inside
        Ellipse,
        Circle,
        Database,
        Box,
        Text,

        // These draw the label outside:
        Image,
        CircularImage,
        Diamond,
        Dot,
        Star,
        Triangle,
        TriangleDown,
        Hexagon,
        Square,
        Icon
    }
}
