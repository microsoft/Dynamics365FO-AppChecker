{
    // This is a generic style document. Use it to specify how to render your graphs on the
    // graph drawing surface. You should modify this file to show what is important to your
    // analysis: If you are looking to analyse a particular parameter, bind that parameter 
    // to a visual parameter, like color or size. Modify the prompt sizes and what is shown
    // when the user hovers over the nodes and edges in the graph etc.
    // You will need the description of the vis.js rendering library; you will find those 
    // here: https://visjs.github.io/vis-network/docs/network/
    //    Options: Describe general behavior: https://visjs.github.io/vis-network/docs/network/#options
    //    Nodes: https://visjs.github.io/vis-network/docs/network/nodes.html
    //    Edges: https://visjs.github.io/vis-network/docs/network/edges.html


    // The createGraphNode function is used to generate a vis.js node from the node data in
    // the provided parameter. Map the visual characteristics that you want your nodes to 
    // display to the data in the nodes.
    function createGraphNode(node) {
        //if (node.labels[0] == 'Class') {
        //    var image = "Resources/Class_16x.svg";
        //    return { id: node.id, value: node.properties.WMC, label: node.properties.Name, title: node.properties.$indegree, color: "red", shape: 'image', 'image': image }
        //}
        //else if (node.labels[0] == 'Interface') {
        //    var image = "Resources/Interface_16x.svg";
        //    return { id: node.id, label: node.properties.Name, title: node.properties.Name, shape: 'image', 'image': image }
        //}
        //else if (node.labels[0] == 'Method') {
        //    // Find the correct image to use, depending on the method visibility
        //    var image = "Method_16x.svg";
        //    if (node.properties.Visibility == 'private')
        //        image = "MethodPrivate_16x.svg";
        //    else if (node.properties.Visibility == 'protected')
        //        image = "MethodProtect_16x.svg";
        //    else if (node.properties.Visibility == 'internal')
        //        image = "MethodFriend_16x.svg";
        //    else if (node.properties.Visibility == 'final')
        //        image = "MethodSealed_16x.svg";
        //    return { id: node.id, value: node.properties.LOC, label: node.properties.Name, title: node.properties.Name, shape: 'image', image: 'Resources/' + image, color: "green" }
        //}

        // Minimal implementation.
        return { id: node.id, label: node.properties.Name, title: node.properties.Name };
    }

    // The createGraphEdge function is used to generate a vis.js edge from the edge data in
    // the provided parameter. Map the visual characteristics that you want your edges to 
    // display to the data in the edges.
    function createGraphEdge(edge) {
        //if (edge.type == 'CALLS') {
        //    return { id: edge.id, from: edge.from, to: edge.to, value: edge.properties.Count, label: edge.type, title: 'Calls: ' + edge.properties.Count, color: { color: 'darkgrey' } };
        //}
        //else {
        //    return { id: edge.id, from: edge.from, to: edge.to, label: edge.type };
        //}

        // Minimal implementation.
        return { id: edge.id, from: edge.from, to: edge.to };
    },

    // Provide options that govern other aspects of the graph rendering.
    function options() {
        return {
            interaction: { hover: true, selectConnectedEdges: false },
            manipulation: {
                enabled: false, // true enables adding nodes to the graph
            },
            nodes: {
                size: 10, // For nodes that do not have specific size or where no value attribute is provided
                font: { strokeWidth: 0, bold: false }, // strokeWidth is the amount of space around the text in nodes or edges.
                shape: 'dot',
                scaling: {
                    min: 10, max: 30,
                    label: { // Make sure font size is in this range.
                        min: 8, max: 24
                    }
                }
            },
            edges: {
                arrows: "to",
                shadow: false,
                smooth: true,
                font: { strokeWidth: 0, opacity: 1.0 },
                scaling: {
                    min: 2, 'max': 12, 'label': { 'enabled': true, 'min': 9, 'max': 14 }
                }
            }
        }
    }
}